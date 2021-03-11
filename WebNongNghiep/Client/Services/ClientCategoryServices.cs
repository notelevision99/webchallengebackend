using Fop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.CategoryView;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Database;
using WebNongNghiep.Helper.SortFilterPaging;

namespace WebNongNghiep.Client.Services
{
    public class ClientCategoryServices : IClientCategoryServices
    {
        private MasterData _db;
        public ClientCategoryServices(MasterData db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Cl_CategoryToReturn>> GetListCategories()
        {
            var listCategories = _db.Categories.Select(p => new Cl_CategoryToReturn
            {
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName
            });
            return await listCategories.ToListAsync();
        }

        public async Task<(IEnumerable<Cl_ProductForList>, int)> GetProductsByCateName(string cateName, FilterModel features_hash)
        {

            SearchParameters seacrhParameters = new SearchParameters();
            string[] ListQuery;
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

                .BuildProductxCateid(cateName, _db, features_hash);
                return (searchQuery.Item1, searchQuery.Item2);
            }

            else
            {
                var countProductsByCateId = _db.Products
                    .Include(p=>p.Category)
                    .Where(p => p.Category.CategoryName == cateName)
                    .Count();
                var productsByCateId = _db.Products.Include(p => p.Category).Where(p => p.Category.CategoryName == cateName)
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
                var query = new SearchBuilder().BuildProductxCateid(cateName, _db, features_hash);
                return (query.Item1, query.Item2);
            }


        }


    }
}
