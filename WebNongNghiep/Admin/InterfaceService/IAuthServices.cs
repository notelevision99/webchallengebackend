using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.UserView;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.UserView;

namespace WebNongNghiep.InterfaceService
{
    public interface IAuthServices
    {
        //Task<string> Login(LoginCredentials loginDto);
        Task<UserToReturn> Register(UserDetails userDetailsDto);
        Task<string> DeleteUser(string id);
        Task<IEnumerable<UserToReturn>> GetListAdmins();
        Task<UserToReturn> GetUserById(string userId);
        Task<string> UpdateUser(string userId, UserDetails userToUpdate);
    }
}
