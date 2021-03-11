using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.BlogView
{
    public class BlogForCreation
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogForCreation()
        {
            CreatedDate = DateTime.Now;
        }

    }
}
