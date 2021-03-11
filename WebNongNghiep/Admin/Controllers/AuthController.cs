using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using WebNongNghiep.Admin.ModelView.UserView;
using WebNongNghiep.Database;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.UserView;

namespace WebNongNghiep.Controllers
{
    [Route("/admin/api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthServices _authServices;
        public AuthController(IAuthServices authServices, UserManager<User> userManager)
        {
            _authServices = authServices;
            this.userManager = userManager;
        }
        //[HttpPost]
        //[Route("Register")]
        //public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        //{
        //    try
        //    {
        //        if (userDetails == null || userDetails.UserName == null
        //        || userDetails.Email == null || userDetails.Password == null)
        //        {
        //            return new BadRequestObjectResult(new { Message = "Đăng kí thất bại" });
        //        }
        //        var registerUser = await _authServices.Register(userDetails);
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var user = new UserDetails
        //        {
        //            UserName = registerUser.UserName,
        //            Email = registerUser.Email,
        //            Message = registerUser.Message,
        //            Address = registerUser.Address,
        //            PhoneNumber = registerUser.PhoneNumber
                    
        //        };
        //        return Ok(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }


        //}

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
        {

            try
            {
                if (!ModelState.IsValid || credentials == null)
                {
                    return new BadRequestObjectResult(new { Message = "Đăng nhập thất bại" });
                }

                var identityUser = await userManager.FindByNameAsync(credentials.UserName);
                if(identityUser == null)
                {
                    return new BadRequestObjectResult(new { Message = "Sai tên tài khoản" });
                }
               

                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return new BadRequestObjectResult(new { Message = "Sai mật khẩu" });
                }

                var roles = await userManager.GetRolesAsync(identityUser);
                if (roles[0] != "Admin")
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

                var userToReturn = new UserToReturn
                {
                    Id = identityUser.Id,
                    UserName = identityUser.UserName,
                    Address = identityUser.Address,
                    PhoneNumber = identityUser.PhoneNumber,
                    Email = identityUser.Email,
                    Roles = roles[0],
                    Message = "Đăng nhập thành công"


                };
                return Ok(userToReturn);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }   

        

        [HttpPost]
        [Route("Logout")]
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
