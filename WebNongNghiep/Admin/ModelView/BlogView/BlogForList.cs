using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.BlogView
{
    public class BlogForList
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int BlogCategoryId { get; set; }
        public string BlogCategoryName { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
