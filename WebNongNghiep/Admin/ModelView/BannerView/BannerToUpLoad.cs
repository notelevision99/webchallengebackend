using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.BannerView
{
    public class BannerToUpLoad
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public BannerToUpLoad()
        {
            DateAdded = DateTime.Now;
        }
    }
}
