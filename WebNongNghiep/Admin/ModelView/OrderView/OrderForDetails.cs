using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.OrderView
{
    public class OrderForDetails
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime DateOrder { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipProvince { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public int TotalPrice { get; set; }

        public List<ItemForList> Items { get; set; }
    }
}
