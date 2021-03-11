using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }
        public int OrderId { get; set; }    
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
}
