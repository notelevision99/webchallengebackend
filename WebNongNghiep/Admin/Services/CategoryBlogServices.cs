using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.CategoryBlogView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Admin.Services
{
    public class CategoryBlogServices : ICategoryBlogServices
    {
        MasterData _db;
        public CategoryBlogServices(MasterData db)
        {
            _db = db;
        }
        public async Task<int> AddCategoryBlog(CategoryBlogForCreation cateDto)
        {
            var checkcategoryExist = await _db.CategoryBlogs.Where(p => p.CategoryBlogName == cateDto.CategoryBlogName).FirstOrDefaultAsync();
            if (checkcategoryExist == null)
            {
                CategoryBlog category = new CategoryBlog
                {
                    CategoryBlogName = cateDto.CategoryBlogName
                };
                _db.CategoryBlogs.Add(category);
                await _db.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<int> DeleteCategoryBlog(int categoryBlogId)
        {
            if(categoryBlogId == 0)
            {
                return 0;
            }
            var categoryBlogForDelete = await _db.CategoryBlogs.FirstOrDefaultAsync(p => p.CategoryBlogId == categoryBlogId);
            _db.CategoryBlogs.Remove(categoryBlogForDelete);
            await _db.SaveChangesAsync();
            return 1;
        }

        public async Task<IEnumerable<CategoryBlogForList>> GetCategoriesBlog()
        {
            var categoriesBlog = _db.CategoryBlogs.Select(p => new CategoryBlogForList
            {
                CategoryBlogId = p.CategoryBlogId,
                CategoryBlogName = p.CategoryBlogName
            });
            return  categoriesBlog;
        }
        
    }
}
