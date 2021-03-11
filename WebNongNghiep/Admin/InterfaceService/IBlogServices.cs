using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.BlogView;

namespace WebNongNghiep.Admin.InterfaceService
{
    public interface IBlogServices
    {
        Task<BlogResponseForCreationPhoto> CreateBlog(BlogForCreation blogView);
        Task<(IEnumerable<BlogForList>,int)> GetBlogs(int blogCategoryId,IFopRequest request);
        Task<int> DeleteBlog(int blogId);
        Task<int> UpdateBlog(int blogId, BlogForCreation blogDto);
        Task<BlogForDetails> GetBlogById(int blogId);
    }
}
