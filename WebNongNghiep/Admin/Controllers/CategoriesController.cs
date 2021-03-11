using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Helper;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.CategoryView;
using static System.Net.WebRequestMethods;

namespace WebNongNghiep.Controllers
{
    [Route("/admin/api/categories")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryServices _categoryServices;
        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryForCreation cateDto)
        {
            try
            {
                var category = await _categoryServices.CreateCategory(cateDto);
                if (category == null)
                {
                    return new BadRequestObjectResult(new { Message = "Tạo danh mục sản phẩm không thành công" });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetListCategories()
        {
            try
            {
                var listCategories = await _categoryServices.GetListCategories();
                if (listCategories == null)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi khi tìm danh mục sản phẩm" });
                }
                return Ok(listCategories);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryServices.GetCategoryById(id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }

        [HttpGet("getproductsbycateid/{cateId}")]
        public async Task<IActionResult> GetListProductByCateId(int cateId, [FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<ProductForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (listProductsByCateId, totalCount) = await _categoryServices.GetListProductsByCateId(cateId, fopRequest);
                var response = new PagedResult<IEnumerable<ProductForList>>((listProductsByCateId), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryForCreation cateDto)
        {
            try
            {
                var categoryToUpdate = await _categoryServices.UpdateCategory(id, cateDto);
                return Ok(categoryToUpdate);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });

            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var categoryToUpdate = await _categoryServices.DeleteCategory(id);
                if (categoryToUpdate != null)
                {
                    return Ok("Xóa thành công");
                }
                return BadRequest("Failed to delete category");
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }

    }
}
