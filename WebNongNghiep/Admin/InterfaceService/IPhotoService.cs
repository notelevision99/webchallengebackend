using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.PhotoView;
using WebNongNghiep.Models;

namespace WebNongNghiep.InterfaceService
{
    public interface IPhotoService
    {
        Task<string>  AddPhotoForProduct(int productId, PhotoForCreation photoDto);
        Task<int> AddPhotoForBlog(int blogId, PhotoBlogForCreation photoDto);
        Task<bool> SaveAll();
        Task<PhotoForReturn> GetPhoto(int id);
        Task<string> DeletePhoto(int id);
        Task<int> DeletePhotoBlog(int id);
    }
}
