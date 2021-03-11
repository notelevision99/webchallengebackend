using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Fop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Database;
using WebNongNghiep.Helper;
using WebNongNghiep.InterfaceService;
using WebNongNghiep.Models;
using WebNongNghiep.ModelView;

namespace WebNongNghiep.Services
{
    public class ProductServices : IProductServices
    {
        private readonly MasterData _db;
        public ProductServices(MasterData db)
        {
            _db = db;           
        }
        public async Task<(IEnumerable<ProductForList>, int)> GetListProduct(IFopRequest request)
        {
            var totalCountProduct = _db.Products.Count();
            var (listProducts, totalCount) =  _db.Products.Include(p=>p.Category).Select(p => new ProductForList
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,
                Price =(int)p.Price,
                PhotoUrl = p.Photos.First().Url,  
                Weight = p.Weight,
                Company = p.Company,
                TotalCount = totalCountProduct
            }).ApplyFop(request);
          
            return (await listProducts.ToListAsync(), totalCount);
        }
        


        public async Task<Product> GetProductForUpdate(int id)
        {
            var product = await  _db.Products
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(u => u.Id == id);              // 1

            if (product == null)
                return null;
            return product;         
        }

        public async Task<ProductForDetail> GetProductDetail(int id)
        {
            var product = await _db.Products
                   .Include(p => p.Photos)
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(u => u.Id == id);             
            var photosToReturn = new List<PhotoForDetail>();
            if (product == null)
                return null;
            if (product.Photos.Count > 0)
            {
               
                foreach (var photo in product.Photos)                                 
                {
                    var productPhoto = new PhotoForDetail
                    {
                        Id = photo.Id,
                        Url = photo.Url,
                        Description = photo.Description,
                        DateAdded = photo.DateAdded,
                        IsMain = photo.IsMain
                    };

                    photosToReturn.Add(productPhoto);
                }

                var productForReturn = new ProductForDetail                                
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    Price = product.Price,                  
                    Description = product.Description,
                    ProductDetails = product.ProductDetails,
                    Weight = product.Weight,
                    Company = product.Company,
                    Photos = photosToReturn,
                };
                return productForReturn;
            }
            return new ProductForDetail
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.CategoryName,
                Price = product.Price,
                //PhotoUrl = product.Photos.FirstOrDefault(p => p.IsMain).Url,               
                Description = product.Description,
                ProductDetails = product.ProductDetails,
                Photos = null
            };

        }

        public async Task<ProductForDetail> CreateProduct(ProductForCreation productDto)
        {
            var checkUrlSeoExist = _db.Products.Where(p => p.UrlSeo == productDto.UrlSeo);
            if(checkUrlSeoExist == null)
            {
                //add product in db     
                Product productToReturn = new Product
                {
                    Id = productDto.Id,
                    ProductName = productDto.ProductName,
                    Price = productDto.Price,
                    CategoryId = productDto.CategoryId,
                    PhotoUrl = productDto.PhotoUrl,
                    Description = productDto.Description,
                    ProductDetails = productDto.ProductDetails,
                    Weight = productDto.Weight,
                    Company = productDto.Company,
                    UrlSeo = productDto.UrlSeo                  
                };
                _db.Add(productToReturn);
                await _db.SaveChangesAsync();

                return new ProductForDetail
                {
                    Id = productToReturn.Id,
                    ProductName = productToReturn.ProductName,
                    CategoryId = productToReturn.CategoryId,
                    Price = productToReturn.Price,
                    Description = productToReturn.Description,
                    ProductDetails = productToReturn.ProductDetails,
                    Weight = productToReturn.Weight,
                    Company = productToReturn.Company,
                    UrlSeo = productToReturn.UrlSeo,
                    Photos = null

                };
            }
            return null;
            

        }
        public async Task<int> UpdateProduct(int id, ProductForCreation productDto)
        {
            
            var product = await _db.Products
                   .Include(p => p.Photos)
                   .FirstOrDefaultAsync(u => u.Id == id);              // 1

            if (product == null)
                return 0;
            if (productDto != null)
            {
                product.ProductName = productDto.ProductName;
                product.CategoryId = productDto.CategoryId;
                product.Price = productDto.Price;
                product.Description = productDto.Description;
                product.ProductDetails = productDto.ProductDetails;
                product.PhotoUrl = productDto.PhotoUrl;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<string> DeleteProduct(int id)
        {
            var produdctToDelete = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(produdctToDelete != null)
            {
                _db.Products.Remove(produdctToDelete);
                 await _db.SaveChangesAsync();
            }
            return "Xoá thành công!";
        }
    }
}
