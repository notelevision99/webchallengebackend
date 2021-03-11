using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.Services
{
    public class ClientProductServices : IClientProductServices
    {
        MasterData _db;
        public ClientProductServices(MasterData db)
        {
            _db = db;
        }
        public async Task<(IEnumerable<Cl_ProductForList>, int)> GetProducts(FilterModel features_hash)
        {
            SearchParameters seacrhParameters = new SearchParameters();
            string[] ListQuery;
            //Lấy danh sách sản phẩm (services) 
            //Nếu có filter và search
            if (features_hash.filter != null)
            {
                ListQuery = features_hash.filter.Split('_');
                foreach (var item in ListQuery)
                {
                    string typeQuery = item.Split('-')[0];
                    string QueryData = item.Split('-')[1];
                    switch (typeQuery)
                    {
                        case "1":
                            seacrhParameters.SearchTerm = QueryData;

                            break;
                        case "2":
                            seacrhParameters.Company.Add(QueryData);

                            break;

                        case "3":
                            seacrhParameters.PriceLow = int.Parse(QueryData);

                            break;
                        case "4":
                            seacrhParameters.PriceHigh = int.Parse(QueryData);

                            break;
                        case "5":
                            seacrhParameters.Weight.Add(float.Parse(QueryData));

                            break;

                        case "6":
                            switch (QueryData)
                            {

                                case "1":
                                    seacrhParameters.SortBy = SortCriteria.PriceHighToLow;

                                    break;
                                case "2":
                                    seacrhParameters.SortBy = SortCriteria.PriceLowToHigh;

                                    break;
                                default:
                                    seacrhParameters.SortBy = SortCriteria.Relevance;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                var searchQuery = new SearchBuilder().
                SetSearchTerm(seacrhParameters.SearchTerm).//1
                SetCompany(seacrhParameters.Company).//2

                SetPriceLow(seacrhParameters.PriceLow).//3
                SetPriceHigh(seacrhParameters.PriceHigh).//4
                SetWeight(seacrhParameters.Weight).
                SetSortBy(seacrhParameters.SortBy)//15

                .BuildProducts(_db, features_hash);
                return (searchQuery.Item1, searchQuery.Item2);
            }
            //nếu không có filter và search mặc định sẽ chỉ phân trang và chỉ có thể sort
            else
            {
                var countProductsByCateId = _db.Products.Count();
                var productsByCateId = _db.Products.Include(p => p.Category)
                .Select(p => new Cl_ProductForList
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    Price = (int)p.Price,
                    Company = p.Company,
                    Weight = p.Weight,
                    UrlSeo = p.UrlSeo,
                    PhotoUrl = p.Photos.First().Url,
                });
                var query = new SearchBuilder().BuildProducts(_db, features_hash);
                return (query.Item1, query.Item2);
            }
        }

  


    public async Task<IEnumerable<Cl_ProductForList>> GetProductsPopular()
        {
            var productsPopular =  _db.Products.Take(12).Select(p => new Cl_ProductForList
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,
                Price = (int)p.Price,
                Company = p.Company,
                Weight = p.Weight,
                UrlSeo = p.UrlSeo,
                PhotoUrl = p.Photos.First().Url,
            });
            return productsPopular;
        }
        //Get Product By CategoryId and Product Relate
        public async Task<(Cl_ProductForList,IEnumerable<Cl_ProductForList>)> GetProductDetails(string urlSeo)
         {          
            var productDetails = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Photos)
                .Where(p => p.UrlSeo == urlSeo).FirstOrDefaultAsync();
      
            var productToReturn = new Cl_ProductForList
            {
                Id = productDetails.Id,
                ProductName = productDetails.ProductName,
                CategoryId = productDetails.CategoryId,
                CategoryName = productDetails.Category.CategoryName,
                Price = (int)productDetails.Price,
                Company = productDetails.Company,
                Weight = productDetails.Weight,
                Description = productDetails.Description,
                ProductDetails = productDetails.ProductDetails,
                UrlSeo = productDetails.UrlSeo,
                PhotoUrl = productDetails.Photos.First().Url
            };
            var productsRelated = _db.Products.Include(p => p.Category).Where(p => p.CategoryId == productDetails.CategoryId)
                .Select(p => new Cl_ProductForList
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName,
                    Price = (int)p.Price,
                    Company = p.Company,
                    Weight = p.Weight,
                    Description = p.Description,          
                    UrlSeo = p.UrlSeo,
                    PhotoUrl = p.Photos.First().Url
                }).Take(12).ToList();        
            return (productToReturn, productsRelated);
        }
    }
}
