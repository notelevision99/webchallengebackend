using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Models
{
    public class PhotoForCreation
    {
        public string Url { get; set; }
        public List<IFormFile> File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoForCreation()
        {
            DateAdded = DateTime.Now;
        }
    }
}
