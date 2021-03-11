using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Admin.ModelView.PhotoView;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.Models;

namespace WebNongNghiep.Controllers
{
    
    [Route("/admin/api")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    public class PhotosController : Controller
    {
       
        private readonly IPhotoService _photoService;
        private readonly IProductServices _productServices;

        public PhotosController(IPhotoService photoService, IProductServices productServices)
        {
            _productServices = productServices;
            _photoService = photoService;
        }

        [HttpPost("products/{productId}/photos")]
        
        public async Task<IActionResult> AddPhotoForProduct(int productId, PhotoForCreation photoDto)
        {
            try
            {
                var product = await _productServices.GetProductForUpdate(productId);

                if (product == null)
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy user" });
                var photoForReturn = await _photoService.AddPhotoForProduct(productId, photoDto);
                if (photoForReturn == null)
                    return new BadRequestObjectResult(new { Message = "Tải hình ảnh lên không thành công" });
                return Ok(photoForReturn);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }
        [HttpPost("blogs/{blogId}/photos")]
        public async Task<IActionResult> AddPhotoForBlog(int blogId,PhotoBlogForCreation photoDto)
        {
            try
            {
                var result = await _photoService.AddPhotoForBlog(blogId, photoDto);
                if(result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy hình ảnh tải lên. Vui lòng kiểm tra lại!" });
                }
                return Ok(new { Message = "Đăng hình ảnh thành công!" });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpGet("products/{productId}/photos/{id}")]
        public IActionResult GetPhoto(int id)
        {
            try
            {
                var photo = _photoService.GetPhoto(id);
                if (photo == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy hình ảnh" });
                }
                return Ok(photo);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }
        [HttpDelete("products/{productId}/photos/{id}")]
      
        public async Task<IActionResult> DeletePhoto(int id)     
        {
            try
            {
                var photoToDelete = await _photoService.DeletePhoto(id);
                if (photoToDelete == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm để xóa" });
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }
        [HttpDelete("blogs/photos/{id}")]

        public async Task<IActionResult> DeletePhotoBlog(int id)
        {
            try
            {
                var photoToDelete = await _photoService.DeletePhotoBlog(id);
                if (photoToDelete == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm để xóa" });
                }
                return Ok();
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }
    }
}
