using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.BlogView;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientBlogServices
    {
       Task<(IEnumerable<Cl_BlogForList>,int)> GetBlogsByCateId(int blogCategoryId, IFopRequest request);
       Task<(Cl_BlogForDetails, IEnumerable<Cl_BlogForList>)> GetBlogById(int blogId);
       Task<(Cl_BlogForDetails, IEnumerable<Cl_BlogForList>)> GetBlogByUrlSeoBlog(string urlSeoBlog);
       Task<(IEnumerable<Cl_BlogForList>, int)> GetBlogsByUrlSeoCategoryBlog(string urlSeoCateogryBlog, IFopRequest request);
    }
}
