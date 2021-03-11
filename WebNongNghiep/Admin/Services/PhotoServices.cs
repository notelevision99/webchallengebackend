using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.PhotoView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.Models;

namespace WebNongNghiep.Services
{
    public class PhotoServices : IPhotoService
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private readonly MasterData _db;

        public PhotoServices(IOptions<CloudinarySettings> cloudinaryConfig, MasterData db)
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
        public async Task<string> AddPhotoForProduct(int productId, PhotoForCreation photoDto)
        {

            Photo photo;
            var product = await _db                                   //  1.
               .Products
               .Where(u => u.Id == productId)
               .FirstOrDefaultAsync();
            if(photoDto.File != null)
            {
                foreach (var file in photoDto.File)
                {
                    //  2.

                    var uploadResult = new ImageUploadResult();                 //  3.

                    if (file.Length > 0)                                        //  4.
                    {

                        using (var stream = file.OpenReadStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(file.Name, stream),
                                Transformation = new Transformation()           //  *
                                                .Width(500).Height(500)
                                                .Crop("fill")
                                                .Gravity("face")
                            };

                            uploadResult = _cloudinary.Upload(uploadParams);    //  5.
                        }
                    }
                    photoDto.Url = uploadResult.Uri.ToString();                 //  4. (cont'd)
                    photoDto.PublicId = uploadResult.PublicId;
                    photo = new Photo
                    {
                        Url = photoDto.Url,
                        Description = photoDto.Description,
                        DateAdded = photoDto.DateAdded,
                        PublicId = photoDto.PublicId
                    };
                    product.Photos.Add(photo);
                    await SaveAll();
                }
            }

            

            //return new  PhotoForReturn
            //{
            //    Id = photo.Id,
            //    Url = photo.Url,
            //    Description = photo.Description,
            //    DateAdded = photo.DateAdded,
            //    IsMain = photo.IsMain,
            //    PublicId = photo.PublicId,
            //};
            return "Them thanh cong";
        }

        public async Task<bool> SaveAll()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<int> AddPhotoForBlog(int blogId, PhotoBlogForCreation photoDto)
        {
            if(photoDto.File == null)
            {
                return 0;
            }
            PhotoBlog photo;
            var blog = await _db                                   //  1.
               .Blogs
               .Where(u => u.BlogId == blogId)
               .FirstOrDefaultAsync();
            var uploadResult = new ImageUploadResult();


            if (photoDto.File.Length > 0)                                        //  4.
            {
                using (var stream = photoDto.File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(photoDto.File.Name, stream),
                        Transformation = new Transformation()           //  *
                                        .Width(500).Height(500)
                                        .Crop("fill")
                                        .Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);    //  5.
                }
            }
            photoDto.Url = uploadResult.Uri.ToString();                 //  4. (cont'd)
            photoDto.PublicId = uploadResult.PublicId;
            photo = new PhotoBlog
            {
                BlogId = blogId,
                Url = photoDto.Url,
                Description = photoDto.Description,
                DateAdded = photoDto.DateAdded,
                PublicId = photoDto.PublicId
            };
            blog.PhotoBlog = photo;
            await SaveAll();
            return 1;
        }
        
        public async Task<PhotoForReturn> GetPhoto(int id)
        {
            var photo = await _db.Photos.FirstOrDefaultAsync(p => p.Id == id);

            var photoForReturn = new PhotoForReturn
            {
                Id = photo.Id,
                Url = photo.Url,
                Description = photo.Description,
                DateAdded = photo.DateAdded,
                IsMain = photo.IsMain,
                PublicId = photo.PublicId,
            };
            return photoForReturn;
        }
        public async Task<string> DeletePhoto(int id)
        {
            var photo = await _db.Photos.FirstOrDefaultAsync(p => p.Id == id);
             _cloudinary.DeleteResources(photo.PublicId);

            _db.Photos.Remove(photo);
            await _db.SaveChangesAsync();
            return "Xóa thành công";
        }
        public async Task<int> DeletePhotoBlog(int id)
        {
            if(id == 0)
            {
                return 0;
            }    
            var photoToDelete = await _db.PhotoBlogs.FirstOrDefaultAsync(p => p.Id == id);
            if(photoToDelete == null)
            {
                return 1;
            }
            _db.PhotoBlogs.Remove(photoToDelete);
            await _db.SaveChangesAsync();
            return 1;
        }
    }
    
}
