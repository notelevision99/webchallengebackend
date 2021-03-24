using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
namespace WebNongNghiep.Client.Controllers
{
    [Route("/api/blogcategories")]
    public class ClientBlogCategoriesController : Controller
    {
        private readonly IClientBlogCategoriesServices _blogCategoriesServices;
        public ClientBlogCategoriesController(IClientBlogCategoriesServices blogCategoriesServices)
        {
            _blogCategoriesServices = blogCategoriesServices;
        }
        [HttpGet("getlist")]
        public async Task<IActionResult> GetBlogCategoriesList()
        {
            try
            {
                var result = await _blogCategoriesServices.GetBlogCategoriesLists();
                if (result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra, không tìm thấy dữ liệu" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }      
        }
        [HttpGet("getblogcategoriestake4")]
        public async Task<IActionResult> GetBlogCategoriesTake4()
        {
            try
            {
                var result = await _blogCategoriesServices.GetBlogsCateogriesListForHeader();
                if(result == null)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra, không tìm thấy dữ liệu" });
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
