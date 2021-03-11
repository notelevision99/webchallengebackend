using Fop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Admin.ModelView.OrderView;

namespace WebNongNghiep.Admin.InterfaceService
{
    public interface IOrderServices
    {
        Task<(IEnumerable<OrderForList>, int)> GetOrders(IFopRequest request);
        Task<OrderForDetails> GetOrderById(int id);
        Task<(IEnumerable<OrderForList>, int)> GetOrdersByUserId(string userId, IFopRequest request);
        Task<int> DeleteOrder(int id);
    }
}
