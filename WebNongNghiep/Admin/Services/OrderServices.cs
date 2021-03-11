using Fop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.OrderView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Admin.Services
{
    public class OrderServices : IOrderServices
    {
        MasterData _db;
        public OrderServices(MasterData db)
        {
            _db = db;
        }
        public async Task<(IEnumerable<OrderForList>, int)> GetOrders(IFopRequest request)
        {

            var (orders, totalCount) = _db.Orders
                .Include(p => p.User)
                .Select(p => new OrderForList
                {
                    OrderId = p.OrderId,
                    DateOrder = p.DateOrder,
                    ShipAddress = p.ShipAddress,
                    UserName = p.User.UserName,
                    PhoneNumber = p.User.PhoneNumber
                }).ApplyFop(request);
            return (await orders.ToListAsync(),totalCount);
        }
        public async Task<OrderForDetails> GetOrderById(int orderId)
        {
            var orderDetail =  _db.OrderDetails
                .Include(p => p.Product)
                .Include(p => p.Order)
                .Include(p => p.Order.User)
                
                .FirstOrDefault(p => p.OrderId == orderId);
            if (orderDetail == null)
            {
                return null;
            }
            //Get TotalPrice
            
            //Get List Product In Order
            var items = _db.OrderDetails
                .Include(p => p.Product)
                .Where(p => p.OrderId == orderId)
                .Select(p => new ItemForList
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.ProductName,
                    PhotoUrl = p.Product.Photos.First().Url,
                    Price = (int)p.Product.Price,
                    Quantity = p.Quantity,
                    TotalPrice = (int)p.Product.Price * p.Quantity
                    
                }).ToList();
            int totalPrice = 0;
            foreach(var item in items)
            {
                totalPrice = totalPrice + item.TotalPrice;
            }
                
            return new OrderForDetails
            {
                Id = orderDetail.ID,
                UserName = orderDetail.Order.User.UserName,
                DateOrder = orderDetail.Order.DateOrder.Date,
                ShipAddress = orderDetail.Order.ShipAddress,
                ShipCity = orderDetail.Order.ShipCity,
                ShipProvince = orderDetail.Order.ShipProvince,
                PhoneNumber = orderDetail.Order.User.PhoneNumber,
                Email = orderDetail.Order.User.Email,
                UserId = orderDetail.Order.User.Id,              
                Items = items,
                TotalPrice = totalPrice

            };
            
        }
        //request include filter paging
        public async Task<(IEnumerable<OrderForList>,int)> GetOrdersByUserId(string userId, IFopRequest request)
        {
            var (orders,totalCount) = _db.Orders
                        .Include(p => p.User)
                        .Where(p => p.User.Id == userId)
                        .Select(p => new OrderForList
                        {
                            OrderId = p.OrderId,
                            DateOrder = p.DateOrder.Date,
                            ShipAddress = p.ShipAddress,
                            UserName = p.User.UserName,
                            PhoneNumber = p.User.PhoneNumber
                        }).ApplyFop(request);
            return (await orders.ToListAsync(), totalCount);                    
        }

        public async Task<int> DeleteOrder(int orderId)
        {
            if(orderId == 0)
            {
                return 0;
            }
            var orderToDelete = _db.Orders.FirstOrDefault(p => p.OrderId == orderId);
            _db.Orders.Remove(orderToDelete);
            await _db.SaveChangesAsync();
            return 1;
        }
        
        
        
    }
}
