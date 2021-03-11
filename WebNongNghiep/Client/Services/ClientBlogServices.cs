using Fop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.BlogView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Client.Services
{
    public class ClientBlogServices : IClientBlogServices
    {
        MasterData _db;
        public ClientBlogServices(MasterData db)
        {
            _db = db;
        }
        public async Task<Cl_BlogForDetails> GetBlogById(int blogId)
        {             
            var blog = await _db.Blogs
                .Include(p => p.PhotoBlog)
                .FirstOrDefaultAsync(p => p.BlogId == blogId);
            if(blog.PhotoBlog == null)
            {
                return new Cl_BlogForDetails
                {
                    BlogId = blog.BlogId,
                    CreatedDate = blog.CreatedDate,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    Content = blog.Content,
                    PhotoUrl = null
                };
            }
            return new Cl_BlogForDetails {
                BlogId = blog.BlogId,
                CreatedDate = blog.CreatedDate,
                Title = blog.Title,
                ShortDescription = blog.ShortDescription,
                Content = blog.Content,
                PhotoUrl = blog.PhotoBlog.Url
            };
        }

        public async Task<(IEnumerable<Cl_BlogForList>,int)> GetBlogsByCateId(int blogCategoryId, IFopRequest request)
        {
            var (blogs, totalCount) = _db.Blogs
                                     .Include(p => p.CategoryBlog)
                                     .Include(p => p.PhotoBlog)
                                     .Where(p => p.CategoryBlogId == blogCategoryId)
                                     .Select(p => new Cl_BlogForList
                                     {
                                         BlogId = p.BlogId,
                                         Title = p.Title,
                                         ShortDescription = p.ShortDescription,
                                         BlogCategoryId = p.CategoryBlog.CategoryBlogId,
                                         BlogCategoryName = p.CategoryBlog.CategoryBlogName,
                                         PhotoUrl = p.PhotoBlog.Url
                                     }).ApplyFop(request);
            return (await blogs.ToListAsync(), totalCount);
        }
    }
}
