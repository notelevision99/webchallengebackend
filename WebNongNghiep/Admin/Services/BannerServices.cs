using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.BannerView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;

namespace WebNongNghiep.Admin.Services
{
    public class BannerServices : IBannerServices
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly MasterData _db;

        public BannerServices(IOptions<CloudinarySettings> cloudinaryConfig, MasterData db)
        {
            _db = db;

            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }
        public async Task<int> UploadBanner(int orderId, BannerToUpLoad bannerPhoto)
        {
            Banner banner;
            var bannerCheckExist = await _db.Banners.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if(bannerCheckExist == null)
            {
                if (bannerPhoto.File != null)
                {
                    //  2.

                    var uploadResult = new ImageUploadResult();                 //  3.

                    if (bannerPhoto.File.Length > 0)                                        //  4.
                    {

                        using (var stream = bannerPhoto.File.OpenReadStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(bannerPhoto.File.Name, stream),
                                Transformation = new Transformation()           //  *
                                                .Width(728).Height(200)
                                                .Crop("fill")
                                                .Gravity("face")
                            };

                            uploadResult = _cloudinary.Upload(uploadParams);    //  5.
                        }
                    }
                    bannerPhoto.Url = uploadResult.Uri.ToString();                 //  4. (cont'd)
                    bannerPhoto.PublicId = uploadResult.PublicId;
                    banner = new Banner
                    {
                        PhotoUrl = bannerPhoto.Url,
                        OrderId = orderId,
                        Description = bannerPhoto.Description,
                        DateAdded = bannerPhoto.DateAdded,
                        PublicId = bannerPhoto.PublicId
                    };
                    _db.Banners.Add(banner);
                    await _db.SaveChangesAsync();
                    return 1;
                }
                return 0;
            }
            return -1;
            

        }
        public async Task<IEnumerable<BannerForList>> GetBanners()
        {
            var banners =  _db.Banners.Select(p => new BannerForList
            {
                Id = p.Id,
                OrderId = p.OrderId,
                Url = p.PhotoUrl,
                PublicId = p.PublicId
            }).ToList();
            return banners;
            
        }
        public async Task<int> DeleteBanner(int id)
        {
            var bannerToDelete = await _db.Banners.FirstOrDefaultAsync(p => p.Id == id);
            if(bannerToDelete == null)
            {
                return 0;
            }
            _db.Banners.Remove(bannerToDelete);
            await _db.SaveChangesAsync();
            return 1;
        }
    }
}
