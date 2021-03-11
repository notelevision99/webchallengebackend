using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView;
using WebNongNghiep.Client.ModelView.MessageMailView;
using WebNongNghiep.Database;
using WebNongNghiep.ModelView;

namespace WebNongNghiep.Client.Services
{
    public class ClientAuthServices : IClientAuthServices
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
   

        public ClientAuthServices(SignInManager<User> signInManager,
                        UserManager<User> userManager,
                        RoleManager<IdentityRole> roleManager
                       )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
          

        }
        //public async Task<Cl_UserToReturn> Register(Cl_UserDetails userDto)
        //{
           

           

        //    return new Cl_UserToReturn
        //    {
        //        UserName = identityUser.UserName,
        //        Email = identityUser.Email,
        //        Roles = "User",
        //        Message = "Đăng kí thành công"

        //    };

        //}

        public async Task<IEnumerable<Cl_UserToReturn>> GetListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var userDtos = new List<Cl_UserToReturn>();

            foreach (var user in users)
            {
                var roleNames = await userManager.GetRolesAsync(user);
                if (roleNames[0] == "User")
                {
                    var userDto = new Cl_UserToReturn
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = roleNames[0]
                    };

                    userDtos.Add(userDto);
                }
            }
            return userDtos;
        }
    }
}
