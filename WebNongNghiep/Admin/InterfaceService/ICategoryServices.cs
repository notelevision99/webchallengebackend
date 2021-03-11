using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.CategoryView;

namespace WebNongNghiep.InterfaceService
{
    public interface ICategoryServices
    {
        Task<CategoryForReturn> CreateCategory(CategoryForCreation cateDto);
        Task<CategoryForReturn> GetCategoryById(int id);
        Task<CategoryForReturn> UpdateCategory(int id,CategoryForCreation cateDto);
        Task<(IEnumerable<ProductForList>, int)> GetListProductsByCateId(int id, IFopRequest fop);
        Task<IEnumerable<CategoryForReturn>> GetListCategories();
        Task<string> DeleteCategory(int id);
    }
}
