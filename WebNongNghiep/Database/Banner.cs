using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
       
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }
  
        public string PublicId { get; set; }
    }
}
