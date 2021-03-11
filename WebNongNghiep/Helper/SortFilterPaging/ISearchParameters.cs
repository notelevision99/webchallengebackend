using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Helper.SortFilterPaging
{
    public interface ISearchParameters
    {
        string SearchTerm { get; set; }
        List<string> Company { get; set; }
        SortCriteria SortBy { get; set; }
        List<float> Weight { get; set; }
        int PriceLow { get; set; }
        int PriceHigh { get; set; }


    }
}
