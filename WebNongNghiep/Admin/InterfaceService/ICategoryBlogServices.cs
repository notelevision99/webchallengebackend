using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.CategoryBlogView;

namespace WebNongNghiep.Admin.InterfaceService
{
    public interface ICategoryBlogServices
    {
        Task<int> AddCategoryBlog(CategoryBlogForCreation cateDto);
        Task<IEnumerable<CategoryBlogForList>> GetCategoriesBlog();
        Task<int> DeleteCategoryBlog(int id);
    }
}
