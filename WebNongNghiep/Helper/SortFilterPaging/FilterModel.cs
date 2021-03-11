using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Helper.SortFilterPaging
{
    public class FilterModel
    {
        public string filter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
 
        public FilterModel()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public FilterModel(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
