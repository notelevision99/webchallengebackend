using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientProductServices
    {
        Task<IEnumerable<Cl_ProductForList>> GetProductsPopular();
        Task<(Cl_ProductForList, IEnumerable<Cl_ProductForList>)> GetProductDetails(string urlSeo);
        Task<(IEnumerable<Cl_ProductForList>, int)> GetProducts(FilterModel features_hash);

      
    }
}
