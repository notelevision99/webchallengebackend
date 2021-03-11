using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fop;
using Fop.FopExpression;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.OrderView;
using WebNongNghiep.Helper;

namespace WebNongNghiep.Client.Controllers
{
    [Route("/api/orders")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize(Roles = "User")]
    [ApiController]
    public class ClientOrderController : Controller
    {
        private readonly IClientOrderServices _orderServices;
        public ClientOrderController(IClientOrderServices orderServices)
        {
            _orderServices = orderServices;
        }   
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Cl_OrderForCreation orderView)      
        {
            try
            {
                var result = await _orderServices.CreateOrder(orderView);
                if (result == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy thông tin nhập vào. Vui lòng kiểm tra lại" });
                }
                if (result == -1)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy sản phẩm trong đơn hàng. Vui lòng kiểm tra lại" });
                }
                return Ok(new { Message = "Đặt hàng thành công!" });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString()});

            }
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId,[FromQuery] FopQuery request)
        {
            try
            {
                var fopRequest = FopExpressionBuilder<Cl_OrderForList>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
                var (ordersToReturn, totalCount) = await _orderServices.GetOrdersByUserId(userId, fopRequest);
                if (ordersToReturn.Count() == 0)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy đơn hàng nào" });
                }
                var response = new PagedResult<IEnumerable<Cl_OrderForList>>((ordersToReturn), totalCount, request.PageNumber, request.PageSize); ;
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
                if (order == null)
                {
                    return new BadRequestObjectResult(new { Message = "Không tìm thấy id đơn hàng. Vui lòng thử lại" });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Message = ex.Message.ToString() });
            }
           
        }

    }
}
