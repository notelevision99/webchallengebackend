using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.CategoryView;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientCategoryServices
    {
        Task<IEnumerable<Cl_CategoryToReturn>> GetListCategories();
        Task<(IEnumerable<Cl_ProductForList>, int)> GetProductsByCateId(int cateId, FilterModel feature_hash);
        
 
    }
}
