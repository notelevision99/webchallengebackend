using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.Controllers
{
    [Route("/api/products")]
    [ApiController]
    
    public class ClientProductController : Controller
    {
        private IClientProductServices _servicesProduct;
        public ClientProductController(IClientProductServices servicesProduct)
        {
            _servicesProduct = servicesProduct;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]FilterModel filterModel)
        {
            try
            {
                var products = await _servicesProduct.GetProducts(filterModel);
                if(products == (null,0))
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm!" });
                }
                return Ok(products);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpGet("getproductspopular")]
        public async Task<IActionResult> GetProductsPopular()
        {
            try
            {
                var products = await _servicesProduct.GetProductsPopular();
                if(products == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm" });
                }
                return Ok(products);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet("{urlSeo}")]
        public async Task<IActionResult> GetProductDetails(string urlSeo)
        {
            try
            {
                var result = await _servicesProduct.GetProductDetails(urlSeo);
                if(result == (null, null))
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm" });                 
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
