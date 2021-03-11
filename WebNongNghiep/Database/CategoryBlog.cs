using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Database
{
    public class CategoryBlog
    {
        [Key]
        public int CategoryBlogId { get; set; }
        public string CategoryBlogName { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
