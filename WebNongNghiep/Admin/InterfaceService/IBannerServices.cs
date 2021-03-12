using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.BannerView;

namespace WebNongNghiep.Admin.InterfaceService
{
    public interface IBannerServices
    {
        Task<int> UploadBanner(int orderId,int width, int height, BannerToUpLoad bannerPhoto);
        Task<IEnumerable<BannerForList>> GetBanners();
        Task<int> DeleteBanner(int id);
    }
}
