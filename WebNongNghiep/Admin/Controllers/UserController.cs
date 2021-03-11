using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Database;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.ModelView;

namespace WebNongNghiep.Admin.Controllers
{
    [Route("/admin/api/users")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthServices _authServices;
        public UserController(IAuthServices authServices, UserManager<User> userManager)
        {
            _authServices = authServices;
            this.userManager = userManager;
        }
        [HttpGet("admin")]
        public async Task<IActionResult> GetListAdmin()
        {
            try
            {
                var listAdminsReturn = await _authServices.GetListAdmins();
                if (listAdminsReturn == null)
                {
                    return NotFound();
                }
                return Ok(listAdminsReturn);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
          
            }
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _authServices.GetUserById(userId);
            if (user == null)
            {
                return new BadRequestObjectResult(new { Message = "Có lỗi khi tìm thông tin user" });
            }
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDetails userToUpdate)
        {
            var user = await _authServices.UpdateUser(userId, userToUpdate);
            if (user == null)
            {
                return new BadRequestObjectResult(new { Message = "Có lỗi khi cập nhật thông tin user" });
            }
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if(id == null)
            {
                return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm" });
            }
            var resultDelete = await _authServices.DeleteUser(id);
            if(resultDelete == null)
            {
                return new BadRequestObjectResult(new { Message = "Xoá không thành công" });
            }
            return Ok(new { Message = "Xóa thành công" });
        }

        
    }
}
