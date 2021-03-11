using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.BlogView;
using WebNongNghiep.Helper;

namespace WebNongNghiep.Admin.Controllers
{
    [Route("/admin/api/blogs")]
    [ApiController]
    public class BlogController : Controller
    {
        private IBlogServices _blogServices;
        public BlogController(IBlogServices blogServices)
        {
            _blogServices = blogServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogForCreation blogDto)
        {
            try
            {
                var result = await _blogServices.CreateBlog(blogDto);
                if (result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra. Vui lòng kiểm tra thông tin trước khi đăng" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
         
        }
        [HttpGet("categoryblogs/{blogCategoryId}")]
        public async Task<IActionResult> GetBlogs(int blogCategoryId,[FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<BlogForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (blogsByCateId, totalCount) = await _blogServices.GetBlogs(blogCategoryId, fopRequest);
                if(blogsByCateId == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm tháy tin tức nào!" });
                }
                var response = new PagedResult<IEnumerable<BlogForList>>((blogsByCateId), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }
        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            try
            {
                var result = await _blogServices.GetBlogById(blogId);
                if(result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy tin tức " });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId,BlogForCreation blogDto)
        {
            try
            {
                var result = await _blogServices.UpdateBlog(blogId, blogDto);
                if(result == -1)
                {
                    return new BadRequestObjectResult(new { Message = "Vui lòng thay đổi thông tin của blog" });
                }
                if(result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi khi cập nhật. Vui lòng thử lại" });
                }
                return Ok(new { Message = "Cập nhật tin tức thành công" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpDelete("{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            try
            {
                var result = await _blogServices.DeleteBlog(blogId);
                if (result == -1)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy id của blog. Vui lòng thử lại" });
                }
                if (result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy blog. Vui lòng thử lại" });
                }
                return Ok(new { Message = "Xoá tin thành công!" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }         
        }

    }
}
