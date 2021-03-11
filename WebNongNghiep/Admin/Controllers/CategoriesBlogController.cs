using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.CategoryBlogView;

namespace WebNongNghiep.Admin.Controllers
{
    [Route("/admin/api/categoriesblog")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoriesBlogController : Controller
    {
        
        private readonly ICategoryBlogServices _categoryBlogServices;
        public CategoriesBlogController(ICategoryBlogServices categoryBlogServices)
        {
            _categoryBlogServices = categoryBlogServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryBlog(CategoryBlogForCreation cateDto)
        {
            try
            {
                var result = await _categoryBlogServices.AddCategoryBlog(cateDto);
                if (result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra, vui lòng thử lại" });
                }
                return Ok(new { Message = "Tạo thành công" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoriesBlog()
        {
            try
            {
                var result = await _categoryBlogServices.GetCategoriesBlog();
                if(result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy danh mục bài viết" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpDelete("{categoryBlogId}")]
        public async Task<IActionResult> DeleteBlog(int categoryBlogId)
        {
            try
            {
                var result = await _categoryBlogServices.DeleteCategoryBlog(categoryBlogId);
                if(result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy id để xóa. Vui lòng thử lại" });
                }
                return Ok(new { Message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        
      
        
    }
}
