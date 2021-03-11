using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Admin.InterfaceService;
using WebNongNghiep.Admin.ModelView.OrderView;
using WebNongNghiep.Helper;

namespace WebNongNghiep.Admin.Controllers
{
    [Route("/admin/api/orders")]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<OrderForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (ordersToReturn, totalCount) = await _orderServices.GetOrders(fopRequest);
                if (ordersToReturn.Count() == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy đơn hàng nào" });
                }
                var response = new PagedResult<IEnumerable<OrderForList>>((ordersToReturn), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }

        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderServices.GetOrderById(orderId);
                if(order == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy đơn hàng này. Vui lòng thử lại" });
                }
                return Ok(order);

            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() }); 
            }
        }
        [HttpGet("users/{userId}")]
     
        public async Task<IActionResult> GetOrdersByUserId(string userId, [FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<OrderForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (ordersToReturn, totalCount) = await _orderServices.GetOrdersByUserId(userId,fopRequest);
                if (ordersToReturn.Count() == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy đơn hàng nào" });
                }
                var response = new PagedResult<IEnumerable<OrderForList>>((ordersToReturn), totalCount, request.PageNumber, request.PageSize); ;
                return Ok(response);
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var result = await _orderServices.DeleteOrder(orderId);
                if (result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy mã đơn hàng. Vui lòng thử lại" });
                }
                return Ok(new { Message = "Xóa đơn hàng thành công!" });
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }
    }
}
