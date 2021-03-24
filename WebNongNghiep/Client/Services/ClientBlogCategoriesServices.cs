using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.BlogCategoriesView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Client.Services
{
    public class ClientBlogCategoriesServices : IClientBlogCategoriesServices
    {
        MasterData _db;
        public ClientBlogCategoriesServices(MasterData db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Cl_BlogCategoriesList>> GetBlogCategoriesLists()
        {
            var blogCategoriesList = _db.CategoryBlogs.Select(p => new Cl_BlogCategoriesList
            {
                CategoryBlogId = p.CategoryBlogId,
                BlogCategoriesName = p.CategoryBlogName,
                UrlSeoCategoryBlog = p.UrlSeoCategoryBlog
                
            }).ToList();
            return  blogCategoriesList;

        }

        public async Task<IEnumerable<Cl_BlogCategoriesList>> GetBlogsCateogriesListForHeader()
        {
            var list = _db.CategoryBlogs.Select(p => new Cl_BlogCategoriesList
            {
                CategoryBlogId = p.CategoryBlogId,
                BlogCategoriesName = p.CategoryBlogName
            }).Take(4).ToList();
            return list;
        }
    }
}
