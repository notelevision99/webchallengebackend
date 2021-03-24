using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.BlogView;
using Microsoft.EntityFrameworkCore;
using WebNongNghiep.Database;
using WebNongNghiep.Models;

namespace WebNongNghiep.Admin.Services
{
    public class BlogServices : IBlogServices
    {
        MasterData _db;
        public BlogServices(MasterData db)
        {
            _db = db;
        }
        public async Task<BlogResponseForCreationPhoto> CreateBlog(BlogForCreation blogView)
        {
            var checkUrlSeoBlogExist = _db.Blogs.FirstOrDefault(p => p.UrlSeoBlog == blogView.UrlSeoBlog);
            var list = checkUrlSeoBlogExist;
            if (blogView == null)
            {
                return null;
            }
            if(list == null)
            {
                Blog blogToReturn = new Blog
                {
                    Title = blogView.Title,
                    ShortDescription = blogView.ShortDescription,
                    Content = blogView.Content,
                    CreatedDate = blogView.CreatedDate,
                    CategoryBlogId = blogView.BlogCategoryId,
                    UrlSeoBlog = blogView.UrlSeoBlog,
                };
                _db.Blogs.Add(blogToReturn);
                await _db.SaveChangesAsync();
                return new BlogResponseForCreationPhoto
                {
                    BlogId = blogToReturn.BlogId,
                };
            }
            return null;
                
            
        
            
        }

        public async Task<(IEnumerable<BlogForList>,int)> GetBlogs(int blogCategoryId, IFopRequest request)
        {

            var (blogs,totalCount) = _db.Blogs
                                .Include(p => p.CategoryBlog)
                                .Include(p => p.PhotoBlog)
                                .Where(p => p.CategoryBlogId == blogCategoryId)
                                .Select(p => new BlogForList
                                {
                                    BlogId = p.BlogId,
                                    Title = p.Title,
                                    ShortDescription = p.ShortDescription,
                                    BlogCategoryId = p.CategoryBlogId,
                                    BlogCategoryName = p.CategoryBlog.CategoryBlogName,
                                    UrlSeoBlog = p.UrlSeoBlog,
                                    PhotoUrl = p.PhotoBlog.Url                                   
                                }).ApplyFop(request);
            return (await blogs.ToListAsync(), totalCount);
        }

        public async Task<BlogForDetails> GetBlogById(int blogId)
        {
            
            var blog = await _db.Blogs
                .Include(p => p.CategoryBlog)
                .Include(p => p.PhotoBlog)
                .FirstOrDefaultAsync(p => p.BlogId == blogId);
            //Trường hợp blog chưa có hình ảnh đại diên
            if(blog.PhotoBlog == null)
            {
                return new BlogForDetails
                {
                    BlogId = blog.BlogId,
                    BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                    BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                    Title = blog.Title,
                    ShortDescription = blog.ShortDescription,
                    Content = blog.Content,
                    UrlSeoBlog = blog.UrlSeoBlog,
                    Photo = null
                };
            }
            //Trường hợp blog có đầy đủ thông tin
            var photo = await _db.PhotoBlogs
                    .Include(p => p.Blog)
                    .FirstAsync(p => p.BlogId == blogId);
            var photoReturn = new PhotoForDetail
            {
                Id = photo.Id,
                Url = photo.Url,
                Description = photo.Description,
                DateAdded = photo.DateAdded,
                IsMain = photo.IsMain
            };
            return new BlogForDetails
            {
                BlogId = blog.BlogId,
                BlogCategoryId = blog.CategoryBlog.CategoryBlogId,
                BlogCategoryName = blog.CategoryBlog.CategoryBlogName,
                Title = blog.Title,
                ShortDescription = blog.ShortDescription,
                Content = blog.Content,
                UrlSeoBlog = blog.UrlSeoBlog,
                Photo = photoReturn
            };
            
        }
        public async Task<int> UpdateBlog(int blogId, BlogForCreation blogDto)
        {
            if(blogId == 0)
            {
                return 0;
            }
            var blogForUpdate =await _db.Blogs.FirstAsync(p => p.BlogId == blogId);
            if(blogForUpdate == null)
            {
                return -1;
            }
            if(blogDto.Title != null)
            {
                blogForUpdate.Title = blogDto.Title;
            }
            if (blogDto.ShortDescription != null)
            {
                blogForUpdate.ShortDescription = blogDto.ShortDescription;
            }
            if(blogDto.Content != null)
            {
                blogForUpdate.Content = blogDto.Content;
            }
            if (blogDto.UrlSeoBlog != null)
            {
                blogForUpdate.UrlSeoBlog = blogDto.UrlSeoBlog;
            }
            if (blogDto.BlogCategoryId != 0)
            {
                blogForUpdate.CategoryBlogId = blogDto.BlogCategoryId;
            }
            await _db.SaveChangesAsync();
            return 1;
        }
        public async Task<int> DeleteBlog(int blogId)
        {
            if(blogId == 0)
            {
                return 0;
            }
            var blogToDelete = await _db.Blogs.FirstOrDefaultAsync(p => p.BlogId == blogId);
            if(blogToDelete == null)
            {
                return -1;
            }
            _db.Blogs.Remove(blogToDelete);
            await _db.SaveChangesAsync();
            return 1;
        }
        

    }
}
