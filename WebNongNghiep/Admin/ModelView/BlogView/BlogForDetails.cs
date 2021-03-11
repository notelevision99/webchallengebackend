using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Models;

namespace WebNongNghiep.Admin.ModelView.BlogView
{
    public class BlogForDetails
    {
        public int BlogId { get; set; }
        public int BlogCategoryId { get; set; }
        public string BlogCategoryName { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public PhotoForDetail Photo { get; set; }
    }
}
