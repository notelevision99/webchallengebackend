using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        [StringLength(5000)]
        public string Title { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
  
        public int CategoryBlogId { get; set; }
        public virtual PhotoBlog PhotoBlog { get; set; }
        public virtual CategoryBlog CategoryBlog { get; set; }

    }
}
