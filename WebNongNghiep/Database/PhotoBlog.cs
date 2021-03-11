using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class PhotoBlog
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; }

        public int BlogId { get; set; }

        public string PublicId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
