using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(5000)]
        [Required]
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string PhotoUrl { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [StringLength(10000)]
        public string ProductDetails { get; set; }

        public string Company { get; set; }

        public float Weight { get; set; }
        [Required]
        [StringLength(5000)]
        public string UrlSeo { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public Product()
        {
            Photos = new Collection<Photo>();
        }

    }
}
