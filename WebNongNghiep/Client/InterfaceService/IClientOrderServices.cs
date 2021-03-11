using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.OrderView;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientOrderServices
    {
        Task<int> CreateOrder(Cl_OrderForCreation orderView);
        //Task<int> GetOrdersByCusId(string id);
        Task<(IEnumerable<Cl_OrderForList>, int)> GetOrdersByUserId(string userId, IFopRequest request);
        Task<Cl_OrderForDetails> GetOrderById(int orderId);
        
    }
}
