using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Database;

namespace WebNongNghiep.Models
{
    public class ProductForCreation
    {

        public int Id { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ProductDetails { get; set; }

        public float Weight { get; set; }

        public string Company { get; set; }

        public string UrlSeo { get; set; }

        public string PhotoUrl { get; set; }



    }
}
