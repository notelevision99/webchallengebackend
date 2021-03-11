using Fop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Database;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.ModelView;
using WebNongNghiep.ModelView.CategoryView;

namespace WebNongNghiep.Services
{
    public class CategoryServices : ICategoryServices
    {
        private MasterData _db;
        public CategoryServices(MasterData db)
        {
            _db = db;
        }

        public async Task<CategoryForReturn> CreateCategory(CategoryForCreation cateDto)
        {
            var checkcategoryExist = await _db.Categories.Where(p => p.CategoryName == cateDto.CategoryName).FirstOrDefaultAsync();
            if(checkcategoryExist == null)
            {
                Category category = new Category
                {
                    CategoryName = cateDto.CategoryName
                };
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return new CategoryForReturn
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName
                };
            }
            else
            {
                return null;
            }
           
        }

        public async Task<CategoryForReturn> GetCategoryById(int cateId)
        {
            var category = await  _db.Categories.FirstOrDefaultAsync(p => p.CategoryId == cateId);
            return new CategoryForReturn
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<IEnumerable<CategoryForReturn>> GetListCategories()
        {
            var listCategories =  _db.Categories.Select(p => new CategoryForReturn
            {
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName
            });
            return await listCategories.ToListAsync();
        }

        
        public async Task<CategoryForReturn> UpdateCategory(int cateId, CategoryForCreation cateDto)
        {
            var category = await _db.Categories
                   .Include(p => p.Products)
                   .FirstOrDefaultAsync(u => u.CategoryId == cateId);              // 1

            if (category == null)
                return null;
            if (cateDto != null)
            {
                category.CategoryName = cateDto.CategoryName;
               
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
            }
            return new CategoryForReturn
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        public async Task<(IEnumerable<ProductForList>,int)> GetListProductsByCateId(int cateId, IFopRequest request)
        {
            var totalCountProduct = _db.Products.Where(p => p.CategoryId == cateId).Count();
           var (listProductsByCateId,totalCount) =  _db.Products.Include(p => p.Category)
                                        .Where(p => p.CategoryId == cateId)
                                        .Select(p => new ProductForList {
                                            Id = p.Id,
                                            ProductName = p.ProductName,
                                            CategoryId = p.CategoryId,
                                            CategoryName = p.Category.CategoryName,
                                            Price = (int)p.Price,
                                            PhotoUrl = p.Photos.First().Url,
                                            TotalCount = totalCountProduct
                                        }).ApplyFop(request);
            return (await listProductsByCateId.ToListAsync(),totalCount);        
        }

        public async Task<string> DeleteCategory(int id)
        {
            var categoryToDelete = await _db.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
            if(categoryToDelete != null)
            {
                _db.Categories.Remove(categoryToDelete);
                await _db.SaveChangesAsync();
            }
            return "Xóa thành công";
        }
    }
}
