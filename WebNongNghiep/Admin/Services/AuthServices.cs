using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.UserView;
using WebNongNghiep.Database;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.UserView;

namespace WebNongNghiep.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthServices(SignInManager<User> signInManager,
                        UserManager<User> userManager,
                        RoleManager<IdentityRole> roleManager){       
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<UserToReturn> Register(UserDetails userDto)
        {
            var identityUser = new User() { UserName = userDto.UserName, Email = userDto.Email, PhoneNumber = userDto.PhoneNumber };
            var checkEmailExist = userManager.FindByEmailAsync(userDto.Email);

            bool checkRoleUser = await roleManager.RoleExistsAsync("Admin");
            if (!checkRoleUser)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }
            if(checkEmailExist == null)
            {
                var checkUserExist = await userManager.FindByNameAsync(userDto.UserName);

                if (checkUserExist != null)
                {
                    return new UserToReturn
                    {
                        Message = "Tài khoản đã tồn tại"
                    };
                }

                await userManager.CreateAsync(identityUser, userDto.Password);
                await userManager.AddToRoleAsync(identityUser, "Admin");
                return new UserToReturn
                {
                    UserName = identityUser.UserName,
                    Email = identityUser.Email,
                    PhoneNumber = identityUser.PhoneNumber,
                    Roles = "Admin",
                    Message = "Đăng kí thành công"
                };
            }
            return new UserToReturn
            {
                Message = "Email đã tồn tại vui lòng nhập email khác"
            };
        }
            
        public async Task<IEnumerable<UserToReturn>> GetListAdmins()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = new List<UserToReturn>();
            
            foreach(var user in users)
            {
                var roleNames = await userManager.GetRolesAsync(user);
                if(roleNames[0] == "Admin")
                {
                    var userDto = new UserToReturn
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = roleNames[0]
                    };
                    
                    userDtos.Add(userDto);
                }             
            }
            return userDtos;
        }

        public async Task<UserToReturn> GetUserById(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return new UserToReturn
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };

        }

        public async Task<string> UpdateUser(string userId, UserDetails userViewInfo)
        {
            var userForUpdate = await userManager.FindByIdAsync(userId);

            if(userViewInfo.UserName != null)
            {
                userForUpdate.UserName = userViewInfo.UserName;
            }
            if(userViewInfo.Email != null)
            {
                userForUpdate.Email = userViewInfo.Email;
            }
            if(userViewInfo.PhoneNumber != null)
            {
                userForUpdate.PhoneNumber = userViewInfo.PhoneNumber;
            }
            if(userViewInfo.Address != null)
            {
                userForUpdate.Address = userViewInfo.Address;
            }
      
                 
            var result = await userManager.UpdateAsync(userForUpdate);
            if(!result.Succeeded)
            {
                return "Cập nhật không thành công!";
            }
            return "Cập nhật thành công";
        }
        

        public async Task<string> DeleteUser(string id)
        {
            if(id == null)
            {
                return "Không tìm thấy sản phẩm";
            }
            var userToDelete = await userManager.FindByIdAsync(id.ToString());
            var result = await userManager.DeleteAsync(userToDelete);
            if(result == null)
            {
                return "Xoá không thành công";
            }
            return "Xoá thành công";

        }

        
    }
}
