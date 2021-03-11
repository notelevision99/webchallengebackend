using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
         
        [Required]
        public DateTime DateOrder { get; set; }
      
        [Required]
        public string ShipAddress { get; set; }

        [Required]
        public string ShipCity { get; set; }

        [Required]
        public string ShipProvince { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
   
    }
}
