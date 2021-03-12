using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.BannerView;

namespace WebNongNghiep.Admin.Controllers
{
    [Route("/admin/api/banners")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]

    public class BannerController : Controller
    {
        private IBannerServices _bannerServices;
        public BannerController(IBannerServices bannerServices)
        {
            _bannerServices = bannerServices;
        }
        [HttpPost("{orderId}/{width}/{height}")]
        public async Task<IActionResult> CreateBanner(int orderId, int width, int height, BannerToUpLoad bannerPhoto)
        {
            try
            {
                var banner = await _bannerServices.UploadBanner(orderId, width, height, bannerPhoto);
                if (banner == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Thêm thất bại" });
                }
                if(banner == -1)
                {
                    return new BadRequestObjectResult(new { Message = "Banner ở vị trí này đã tồn tại, vui lòng thử lại" });
                }
                return Ok(new { Message = "Upload ảnh thành công" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> GetBanners()
        {
            try
            {
                var banners = await _bannerServices.GetBanners();
                if (banners == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy banners" });
                }
                return Ok(banners);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                var result = await _bannerServices.DeleteBanner(id);
                if(result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy banner, vui lòng thử lại" });
                }
                return Ok(new { Message = "Xóa banner thành công" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
    }
}
