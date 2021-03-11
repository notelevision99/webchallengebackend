using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Models;

namespace WebNongNghiep.Client.ModelView.ProductView
{
    public class Cl_ProductForList
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Price { get; set; }

        public int TotalCount { get; set; }

        public string PhotoUrl { get; set; }

        public string Company { get; set; }

        public float Weight { get; set; }

        public string ProductDetails { get; set; }

        public string Description { get; set; }

        public string UrlSeo { get; set; }

        public ICollection<PhotoForDetail> Photos { get; set; }
    }
}
