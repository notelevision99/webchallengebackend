using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Helper.SortFilterPaging
{
    public class SearchParameters : ISearchParameters
    {
        public SearchParameters()
        {
            SearchTerm = String.Empty;
            Company = new List<string>();
            SortBy = SortCriteria.Relevance;
            PriceLow = 0;
            PriceHigh = 0;
            Weight = new List<float>();
        }
        public string SearchTerm { get; set; }
        public List<string> Company  { get; set; }
        public SortCriteria SortBy { get; set; }
        public int PriceLow { get; set; }
        public int PriceHigh { get; set; }
        public List<float> Weight { get; set; }

    }
}
