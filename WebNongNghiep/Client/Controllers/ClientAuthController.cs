using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView;
using WebNongNghiep.Client.ModelView.MessageMailView;
using WebNongNghiep.Client.ModelView.PasswordResetView;
using WebNongNghiep.Client.Services;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.ModelView.UserView;

namespace WebNongNghiep.Client.Controllers
{
   
    [ApiController]
    public class ClientAuthController : Controller
    {
        private  UserManager<User> _userManager;
        private IClientAuthServices _authServices;
        private RoleManager<IdentityRole> _roleManager;
        private IClientEmailSenderServices _emailSender;

        public ClientAuthController(IClientAuthServices authServices, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IClientEmailSenderServices emailSender
            )
            
        {
            _authServices = authServices;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
         
        }
        [HttpPost]
        [Route("/api/auth/Register")]
        public async Task<IActionResult> Register(Cl_UserDetails userDetails)
        {
            try
            {
                var identityUser = new User()
                {
                    UserName = userDetails.UserName,
                    Email = userDetails.Email,
                    Address = userDetails.Address,
                    PhoneNumber = userDetails.PhoneNumber,
                };


                bool checkRoleUser = await _roleManager.RoleExistsAsync("User");
                if (!checkRoleUser)
                {
                    var role = new IdentityRole();
                    role.Name = "User";
                    await _roleManager.CreateAsync(role);
                }

                var checkUserExist = await _userManager.FindByNameAsync(userDetails.UserName);
                var checkUserEmailExist = await _userManager.FindByEmailAsync(userDetails.Email);

                if (checkUserExist != null || checkUserEmailExist != null)
                {
                    return new BadRequestObjectResult(new { Message = "Tài khoản đã tồn tại" });
                }


                var result = await _userManager.CreateAsync(identityUser, userDetails.Password);            
                //Email Confirm
                //
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(identityUser, "User");
                    return Ok(new { Message = "Đăng kí thành công!" });
                }
                return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra" });
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [HttpGet]
        [Route("/auth")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return View("Error");

                var result = await _userManager.ConfirmEmailAsync(user, token);
                return Ok(new { Message = "Xác nhận thành công" });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }

        [HttpPost]
        [Route("api/auth/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(Cl_ForgotPasswordView view)
        {
            try
            {
                if (view.Email == null)
                {
                    return new BadRequestObjectResult(new { Message = "Email không được để trống" });
                }
                var user = await _userManager.FindByEmailAsync(view.Email);
                if (user == null)
                {
                    return new BadRequestObjectResult(new { Message = "Email chưa được đăng kí trong hệ thống. Vui lòng thử lại" });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("ResetPassword", "ClientAuth", new { token, email = user.Email }, Request.Scheme,"localhost:3000");
                var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
                await _emailSender.SendEmailAsync(message);
                return Ok(new { Message = "Email đặt lại mật khẩu đã được gửi vào email của bạn. Vui lòng kiểm tra" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }

        [HttpGet]
        [Route("/auth/ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new Cl_ResetPasswordView { Token = token, Email = email };
            return Ok().SetCookiesResetPwd(Response, model.Token);
        }
       

        [HttpPost]
        [Route("/auth/ResetPassword")]
        public async Task<IActionResult> ResetPassword(Cl_ResetPasswordView resetPwdView)
        {
            var user = await _userManager.FindByEmailAsync(resetPwdView.Email);
            if (user == null)
            {
                return new BadRequestObjectResult(new { Message = "Mật khẩu không được để trống" });
            }
            await _userManager.ResetPasswordAsync(user, resetPwdView.Token, resetPwdView.Password);
            return new BadRequestObjectResult(new { Message = "Cập nhật mật khẩu thành công" });
        }

        [HttpPost]
        [Route("/api/auth/Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {
            try
            {

                if (!ModelState.IsValid || credentials == null)
                {
                    return new BadRequestObjectResult(new { Message = "Vui lòng nhập tên tài khoản và mật khẩu" });
                }

                var identityUser = await _userManager.FindByNameAsync(credentials.UserName);

                
                if (identityUser == null)
                {
                    return new BadRequestObjectResult(new { Message = "Sai tên tài khoản" });
                }

                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return new BadRequestObjectResult(new { Message = "Sai mật khẩu" });
                }

                var roles = await _userManager.GetRolesAsync(identityUser);
                if (roles[0] != "User")
                {
                    return new BadRequestObjectResult(new { Message = "Đăng nhập thất bại" });
                }


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, identityUser.Email),
                    new Claim(ClaimTypes.Name, identityUser.UserName),
                    new Claim(ClaimTypes.Role, roles[0])

                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity));

                var userToReturn = new Cl_UserToReturn
                {
                    UserId = identityUser.Id,
                    UserName = identityUser.UserName,
                    Address = identityUser.Address,
                    PhoneNumber = identityUser.PhoneNumber,
                    Email = identityUser.Email,
                    Roles = roles[0],
                    Message = "Đăng nhập thành công"
                };
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        

        [HttpPost]
        [Route("/api/auth/Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok(new { Message = "Bạn đã đăng xuất" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

    }

}
