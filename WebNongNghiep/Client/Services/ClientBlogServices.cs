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
        public async Task<(Cl_BlogForDetails, IEnumerable<Cl_BlogForList>)> GetBlogById(int blogId)
        {             
            var blog = await _db.Blogs
                .Include(p => p.PhotoBlog)
                .Include(p => p.CategoryBlog)
                .FirstOrDefaultAsync(p => p.BlogId == blogId);

            var blogsRelated = _db.Blogs.Include(p => p.CategoryBlog)
                            .Where(p => p.CategoryBlogId == blog.CategoryBlogId)
                            .Select(p => new Cl_BlogForList
                            {
                                BlogId = p.BlogId,
                                Title = p.Title,
                                ShortDescription = p.ShortDescription,
                                BlogCategoryId = p.CategoryBlog.CategoryBlogId,
                                BlogCategoryName = p.CategoryBlog.CategoryBlogName,
                                UrlSeoBlog = p.UrlSeoBlog,
                                UrlSeoCategoryBlog = p.CategoryBlog.UrlSeoCategoryBlog,
                                PhotoUrl = p.PhotoBlog.Url
                            }).Take(12).ToList();



            if (blog.PhotoBlog == null)
            {
                var blogForReturnNoPhoto = new Cl_BlogForDetails
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                    BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                    UrlSeoBlog = blog.UrlSeoBlog,
                    UrlSeoCategoryBlog = blog.CategoryBlog.UrlSeoCategoryBlog,
                    Content = blog.Content,
                    PhotoUrl = null
                };
                return (blogForReturnNoPhoto, blogsRelated);
            }
            else
            {
                var blogForReturnHasPhoto = new Cl_BlogForDetails
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                    BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                    Content = blog.Content,
                    UrlSeoBlog = blog.UrlSeoBlog,
                    UrlSeoCategoryBlog = blog.CategoryBlog.UrlSeoCategoryBlog,
                    PhotoUrl = blog.PhotoBlog.Url
                };
                return (blogForReturnHasPhoto, blogsRelated);
            }
  
        }

        public async Task<(Cl_BlogForDetails, IEnumerable<Cl_BlogForList>)> GetBlogByUrlSeoBlog(string urlSeoBlog)
        {
            var blog = await _db.Blogs
                .Include(p => p.PhotoBlog)
                .Include(p => p.CategoryBlog)
                .FirstOrDefaultAsync(p => p.UrlSeoBlog == urlSeoBlog);

            var blogsRelated = _db.Blogs.Include(p => p.CategoryBlog)
                            .Where(p => p.CategoryBlogId == blog.CategoryBlogId)
                            .Select(p => new Cl_BlogForList
                            {
                                BlogId = p.BlogId,
                                Title = p.Title,
                                ShortDescription = p.ShortDescription,
                                BlogCategoryId = p.CategoryBlog.CategoryBlogId,
                                BlogCategoryName = p.CategoryBlog.CategoryBlogName,
                                UrlSeoBlog = p.UrlSeoBlog,
                                UrlSeoCategoryBlog = p.CategoryBlog.UrlSeoCategoryBlog,
                                PhotoUrl = p.PhotoBlog.Url
                            }).Take(12).ToList();
            if (blog.PhotoBlog == null)
            {
                var blogForReturnNoPhoto = new Cl_BlogForDetails
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                    BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                    UrlSeoBlog = blog.UrlSeoBlog,
                    UrlSeoCategoryBlog = blog.CategoryBlog.UrlSeoCategoryBlog,
                    Content = blog.Content,
                    PhotoUrl = null
                };
                return (blogForReturnNoPhoto, blogsRelated);
            }
            else
            {
                var blogForReturnHasPhoto = new Cl_BlogForDetails
                {
                    BlogId = blog.BlogId,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                    BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                    Content = blog.Content,
                    UrlSeoBlog = blog.UrlSeoBlog,
                    UrlSeoCategoryBlog = blog.CategoryBlog.UrlSeoCategoryBlog,
                    PhotoUrl = blog.PhotoBlog.Url
                };
                return (blogForReturnHasPhoto, blogsRelated);
            }

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
                                         UrlSeoBlog = p.UrlSeoBlog,
                                         UrlSeoCategoryBlog = p.CategoryBlog.UrlSeoCategoryBlog,
                                         PhotoUrl = p.PhotoBlog.Url
                                     }).ApplyFop(request);
            return (await blogs.ToListAsync(), totalCount);
        }

        public async Task<(IEnumerable<Cl_BlogForList>, int)> GetBlogsByUrlSeoCategoryBlog(string urlSeoCategoryBlog, IFopRequest request)
        {
            var (blogs, totalCount) = _db.Blogs
                                     .Include(p => p.CategoryBlog)
                                     .Include(p => p.PhotoBlog)
                                     .Where(p => p.CategoryBlog.UrlSeoCategoryBlog == urlSeoCategoryBlog)
                                     .Select(p => new Cl_BlogForList
                                     {
                                         BlogId = p.BlogId,
                                         Title = p.Title,
                                         ShortDescription = p.ShortDescription,
                                         BlogCategoryId = p.CategoryBlog.CategoryBlogId,
                                         BlogCategoryName = p.CategoryBlog.CategoryBlogName,                                     
                                         UrlSeoBlog = p.UrlSeoBlog,
                                         UrlSeoCategoryBlog = p.CategoryBlog.UrlSeoCategoryBlog,
                                         PhotoUrl = p.PhotoBlog.Url
                                     }).ApplyFop(request);
            return (await blogs.ToListAsync(), totalCount);
        }


    }
}
