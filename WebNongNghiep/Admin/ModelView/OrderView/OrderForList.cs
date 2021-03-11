using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.OrderView
{
    public class OrderForList
    {
        public int OrderId { get; set; }

        public DateTime DateOrder { get; set; }

        public string ShipAddress { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }



    }
}
