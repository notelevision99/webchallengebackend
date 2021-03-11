using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.BlogView;
using WebNongNghiep.Helper;

namespace WebNongNghiep.Client.Controllers
{
    [Route("/api")]
    public class ClientBlogController : Controller
    {
        private readonly IClientBlogServices _blogServices;
        public ClientBlogController(IClientBlogServices blogServices)
        {
            _blogServices = blogServices;
        }
        [HttpGet("blogs/categoryblogs/{blogCategoryId}")]
        public async Task<IActionResult> GetBlogsByCateId(int blogCategoryId, [FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<Cl_BlogForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (blogsReturn, totalCount) = await _blogServices.GetBlogsByCateId(blogCategoryId, fopRequest);
                if (blogsReturn.Count() == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Tin tức hiện đang trống. Chúng tôi sẽ cập nhật trong thời gian sớm nhất" });
                }
                var response = new PagedResult<IEnumerable<Cl_BlogForList>>((blogsReturn), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }          
        }
        [HttpGet("blogs/{blogId}")]
        public async Task<IActionResult> GetOrderById(int blogId)
        {
            try
            {
                var result = await _blogServices.GetBlogById(blogId);
                if(result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy tin này. Vui lòng thử lại" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
    }
}
