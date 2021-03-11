using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.Models;
using WebNongNghiep.ModelView;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebNongNghiep.Controllers
{
    [Route("/admin/api/products")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles ="Admin")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IProductServices _productServices;
        public ProductsController(IPhotoService photoService, IProductServices productServices)
        {
            _photoService = photoService;
            _productServices = productServices;
        }
        
        [HttpGet]       
        public async Task<IActionResult> GetListProducts([FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<ProductForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

                var (productToReturn, totalCount) = await _productServices.GetListProduct(fopRequest);
                var response = new PagedResult<IEnumerable<ProductForList>>((productToReturn), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }
        
        // GET: api/<ProductsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var productToReturn = await _productServices.GetProductDetail(id);
                if (productToReturn == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm" });
                   
                }
                return Ok(productToReturn);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() }); 
            }
           
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductForCreation productDto)
        {
            try
            {
                var product = await _productServices.CreateProduct(productDto);
                if (product == null)
                {
                    return new BadRequestObjectResult(new { Message = "Tạo sản phẩm thất bại" });

                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductForCreation productDto)
        {
            try
            {
                var productToReturn = await _productServices.UpdateProduct(id, productDto);
                if (productToReturn == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Có lỗi khi cập nhật sản phẩm" });
                }
                return Ok(new { Message = "Cập nhật sản phẩm thành công!"});
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }
      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var productToDetele = await _productServices.DeleteProduct(id);
                if (productToDetele == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm để xóa" });
                }
                return Ok("Xóa thành công!");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
         
         
        }
    }
}
