using Microsoft.EntityFrameworkCore;
using PredicateExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using WebNongNghiep.Client.ModelView.ProductView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Helper.SortFilterPaging
{
    public class SearchBuilder
    {
        private ISearchParameters _searchParameters;
        public SearchBuilder() : this(new SearchParameters()) { }
        public SearchBuilder(ISearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
        }
        public SearchBuilder SetSearchTerm(string searchTerm)
        {
            _searchParameters.SearchTerm = searchTerm;
            return this;
        }
        public SearchBuilder SetCompany(List<string> company)
        {
            _searchParameters.Company = company;
            return this;
        }
        public SearchBuilder SetSortBy(SortCriteria sortby)
        {
            _searchParameters.SortBy = sortby;
            return this;
        }
        public SearchBuilder SetPriceHigh(int pricehigh)
        {
            _searchParameters.PriceHigh = pricehigh;
            return this;
        }
        public SearchBuilder SetPriceLow(int pricelow)
        {
            _searchParameters.PriceLow = pricelow;
            return this;
        }
        public SearchBuilder SetWeight(List<float> weight)
        {
            _searchParameters.Weight = weight;
            return this;
        }

        //build for getproductsbycateid
        public (IEnumerable<Cl_ProductForList>, int) BuildProductxCateid(string cateName, MasterData entities, FilterModel filterModel)
        {
            //if filter         
            var predicate = PredicateExtensions.PredicateExtensions.Begin<Product>();

            if (!String.IsNullOrEmpty(_searchParameters.SearchTerm))
            {
                predicate = predicate.And(e => e.ProductName.Contains(_searchParameters.SearchTerm));

            }
            if (_searchParameters.PriceLow > 0 && _searchParameters.PriceHigh > 0)
            {
                predicate = predicate.And(e => e.Price >= _searchParameters.PriceLow && e.Price <= _searchParameters.PriceHigh);
            }
            if (_searchParameters.Company.Count > 0)
            {
                var temppredicate = PredicateExtensions.PredicateExtensions.Begin<Product>();
                foreach (var item in _searchParameters.Company)
                {
                    temppredicate = temppredicate.Or(e => e.Company == item);

                }
                predicate = predicate.And(temppredicate);
            }
            if (_searchParameters.Weight.Count > 0)
            {
                var temppredicate = PredicateExtensions.PredicateExtensions.Begin<Product>();
                foreach (var item in _searchParameters.Weight)
                {
                    temppredicate = temppredicate.Or(e => e.Weight == item);

                }
                predicate = predicate.And(temppredicate);
            }
            //Nếu filter và search != null
            if (predicate.Body.ToString() != "False")
            {
                var recordsByCateId = entities.Products
                    .Include(p => p.Category)              
                    .Where(p => p.Category.CategoryName == cateName);

                var countRecordsResult = recordsByCateId.Where(predicate).Count();

                var recordsResult = recordsByCateId.Where(predicate)
                   .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                   .Take(filterModel.PageSize)
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

                
                var recordsResultToSort = recordsByCateId.Where(predicate);
                switch (_searchParameters.SortBy)
                {
                    case SortCriteria.Relevance:
                        break;
                    case SortCriteria.PriceLowToHigh:
                        recordsResult = recordsResultToSort.OrderBy(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                               .Take(filterModel.PageSize)
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
                        break;

                    case SortCriteria.PriceHighToLow:
                        recordsResult = recordsResultToSort.OrderByDescending(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                              .Take(filterModel.PageSize)
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
                        break;
                    default:
                        break;
                }
                return (recordsResult, countRecordsResult);

            }
            //Nếu null chỉ phân trang vào sort
            else
            {
                var recordsByCateId = entities
                    .Products
                    .Include(p => p.Category)
                    .Where(p => p.Category.CategoryName == cateName);

                var countRecordsResult = recordsByCateId.Count();

                var recordsResult = recordsByCateId
                   .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                   .Take(filterModel.PageSize)
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

                //if sort => sort before paging
                var recordsResultToSort = recordsByCateId;
                switch (_searchParameters.SortBy)
                {
                    case SortCriteria.Relevance:
                        break;
                    case SortCriteria.PriceLowToHigh:
                        recordsResult = recordsResultToSort.OrderBy(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                                .Take(filterModel.PageSize)
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
                        break;

                    case SortCriteria.PriceHighToLow:
                        recordsResult = recordsResultToSort.OrderByDescending(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                              .Take(filterModel.PageSize)
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
                        break;
                    default:
                        break;
                }
                return (recordsResult, countRecordsResult);
            }


        }
        //build for getListProducts
        public (IEnumerable<Cl_ProductForList>, int) BuildProducts(MasterData entities, FilterModel filterModel)
        {
            var predicate = PredicateExtensions.PredicateExtensions.Begin<Product>();
            if (!String.IsNullOrEmpty(_searchParameters.SearchTerm))
            {
                predicate = predicate.And(e => e.ProductName.Contains(_searchParameters.SearchTerm));

            }
            if (_searchParameters.PriceLow > 0 && _searchParameters.PriceHigh > 0)
            {
                predicate = predicate.And(e => e.Price >= _searchParameters.PriceLow && e.Price <= _searchParameters.PriceHigh);
            }
            if (_searchParameters.Company.Count > 0)
            {
                var temppredicate = PredicateExtensions.PredicateExtensions.Begin<Product>();
                foreach (var item in _searchParameters.Company)
                {
                    temppredicate = temppredicate.Or(e => e.Company == item);

                }
                predicate = predicate.And(temppredicate);
            }
            if (_searchParameters.Weight.Count > 0)
            {
                var temppredicate = PredicateExtensions.PredicateExtensions.Begin<Product>();
                foreach (var item in _searchParameters.Weight)
                {
                    temppredicate = temppredicate.Or(e => e.Weight == item);

                }
                predicate = predicate.And(temppredicate);
            }
            //Lấy danh sách sản phẩm đã phân trang sẵn
            //Nếu có filter và search
            if (predicate.Body.ToString() != "False")
            {
                var records = entities.Products;

                var countRecordsResult = records.Where(predicate).Count();

                var recordsResult = records.Where(predicate)
                   .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                   .Take(filterModel.PageSize)
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


                var recordsResultToSort = records.Where(predicate);
                switch (_searchParameters.SortBy)
                {
                    case SortCriteria.Relevance:
                        break;
                    case SortCriteria.PriceLowToHigh:
                        recordsResult = recordsResultToSort.OrderBy(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                                .Take(filterModel.PageSize)
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
                        break;

                    case SortCriteria.PriceHighToLow:
                        recordsResult = recordsResultToSort.OrderByDescending(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                              .Take(filterModel.PageSize)
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
                        break;
                    default:
                        break;
                }
                return (recordsResult, countRecordsResult);

            }
            //Nếu không có filter và search thì mặc định là phân trang và sort
            else
            {
                var records = entities.Products;
                    

                var countRecordsResult = records.Count();

                var recordsResult = records
                   .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                   .Take(filterModel.PageSize)
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

                //if sort => sort before paging
                var recordsResultToSort = records;
                switch (_searchParameters.SortBy)
                {
                    case SortCriteria.Relevance:
                        break;
                    case SortCriteria.PriceLowToHigh:
                        recordsResult = recordsResultToSort.OrderBy(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                                .Take(filterModel.PageSize)
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
                        break;

                    case SortCriteria.PriceHighToLow:
                        recordsResult = recordsResultToSort.OrderByDescending(p => p.Price)
                                              .Skip((filterModel.PageNumber - 1) * filterModel.PageSize)
                                              .Take(filterModel.PageSize)
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
                        break;
                    default:
                        break;
                }
                return (recordsResult, countRecordsResult);
            }
        }
    }
}
