using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Client.ModelView.OrderView
{
    public class Cl_OrderForCreation
    {     
        public DateTime DateOrder { get; set; }
  
        public string ShipAddress { get; set; }
     
        public string ShipCity { get; set; }

        public string ShipProvince { get; set; }
      
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<Cl_ItemToCreation> Items { get; set; }

        public Cl_OrderForCreation()
        {
            DateOrder = DateTime.Now;
        }
    }
}
