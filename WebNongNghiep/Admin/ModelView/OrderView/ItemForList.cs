﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.OrderView
{
    public class ItemForList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PhotoUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
