using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.BannerView
{
    public class BannerForList
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
