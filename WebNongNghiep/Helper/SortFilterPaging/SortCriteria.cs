using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Helper.SortFilterPaging
{
    public enum SortCriteria
    {
        [Description("Relevance")]
        Relevance = 0,
        [Description("Price: Low to High")]
        PriceLowToHigh = 1,
        [Description("Price: High to Low")]
        PriceHighToLow = 2

    }
}
