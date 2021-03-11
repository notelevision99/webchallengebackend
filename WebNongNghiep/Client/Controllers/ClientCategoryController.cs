using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.Controllers
{
    [Route("/api/categories")]
    public class ClientCategoryController : Controller
    {
        private IClientCategoryServices _categoryServices;
    
        public ClientCategoryController(IClientCategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet()]
        public async Task<IActionResult> GetListCategories()
        {
            try
            {
                var listCategories = await _categoryServices.GetListCategories();
                if (listCategories == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy loại sản phẩm" });
                }
                return Ok(listCategories);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }    
        [HttpGet("getproductsbycatename/{cateName}")]      
        public async Task<IActionResult> GetProductsByCateId(string cateName,[FromQuery] FilterModel filterParams)
        {
            
            try
            {             
                var result = await _categoryServices.GetProductsByCateName(cateName, filterParams);
                if (result == (null,0))
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi xảy ra khi tìm kiếm sản phẩm" });
                }
                             
                return Ok(result);

            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
                      
        }

        

    }
}
