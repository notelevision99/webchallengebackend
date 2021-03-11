using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Client.ModelView.BlogView
{
    public class Cl_BlogForDetails
    {
        public int BlogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string PhotoUrl { get; set; }
    }
}
