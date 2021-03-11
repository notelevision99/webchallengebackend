using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Database;
using WebNongNghiep.Models;
using WebNongNghiep.ModelView;

namespace WebNongNghiep.InterfaceService
{
    public interface IProductServices
    {
        Task<Product> GetProductForUpdate(int id);
        Task<ProductForDetail> CreateProduct(ProductForCreation productDto);
        Task<ProductForDetail> GetProductDetail(int id);
        Task<int> UpdateProduct(int id, ProductForCreation productDto);
        Task<(IEnumerable<ProductForList>, int)> GetListProduct(IFopRequest request);
        Task<string> DeleteProduct(int id);

    }
}
