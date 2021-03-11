using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Microsoft.EntityFrameworkCore;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.OrderView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Client.Services
{
    public class ClientOrderServices : IClientOrderServices
    {
        MasterData _db;
        public ClientOrderServices(MasterData db)
        {
            _db = db;
        }
        public async Task<int> CreateOrder(Cl_OrderForCreation orderView)
        {
            if (orderView == null)
            {
                return 0;
            }

            var order = new Order
            {
                DateOrder = orderView.DateOrder,
                ShipAddress = orderView.ShipAddress,
                ShipCity = orderView.ShipCity,
                ShipProvince = orderView.ShipProvince,
                UserId = orderView.UserId,
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (Cl_ItemToCreation item in orderView.Items)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                _db.OrderDetails.Add(orderDetail);
                await _db.SaveChangesAsync();
            }
            return 1;

            
        }
        public async Task<(IEnumerable<Cl_OrderForList>,int)> GetOrdersByUserId(string userId, IFopRequest request)
        {
            var (orders, totalCount) = _db.Orders
                .Include(p => p.User)
                .Where(p => p.User.Id == userId)
                .Select(p => new Cl_OrderForList
                {
                    OrderId = p.OrderId,
                    DateOrder = p.DateOrder.Date,
                    ShipAddress = p.ShipAddress,
                    UserName = p.User.UserName,
                    PhoneNumber = p.User.PhoneNumber
                }).ApplyFop(request);
            return (await orders.ToListAsync(), totalCount);
                
        }
        public async Task<Cl_OrderForDetails> GetOrderById(int orderId)
        {
            var orderDetail = _db.OrderDetails
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
                .Select(p => new Cl_ItemForList
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.ProductName,
                    PhotoUrl = p.Product.Photos.First().Url,
                    Price = (int)p.Product.Price,
                    Quantity = p.Quantity,
                    TotalPrice = (int)p.Product.Price * p.Quantity

                }).ToList();
            int totalPrice = 0;
            foreach (var item in items)
            {
                totalPrice = totalPrice + item.TotalPrice;
            }

            return new Cl_OrderForDetails
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
       
       

    }
}
