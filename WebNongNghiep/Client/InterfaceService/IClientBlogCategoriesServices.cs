using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.BlogCategoriesView;
namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientBlogCategoriesServices
    {
        Task<IEnumerable<Cl_BlogCategoriesList>> GetBlogCategoriesLists();
        Task<IEnumerable<Cl_BlogCategoriesList>> GetBlogsCateogriesListForHeader();
    }
}
