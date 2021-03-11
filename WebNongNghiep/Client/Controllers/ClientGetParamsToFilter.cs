using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebNongNghiep.Client.InterfaceService;
namespace WebNongNghiep.Client.Controllers
{
    [Route("/api/getfilterparams")]
    public class ClientGetParamsToFilter : Controller
    {
        private IClientGetFilterParamsServices _servicesGetFilterParams;
        public ClientGetParamsToFilter(IClientGetFilterParamsServices servicesGetFilterParams)
        {
            _servicesGetFilterParams = servicesGetFilterParams;
        }
        [HttpGet("getcompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await _servicesGetFilterParams.GetListCompany();
            if(result == null)
            {
                return new BadRequestObjectResult(new { Message = "Có lỗi khi lấy thông tin nhà sản xuất sản phẩm" });
            }
            return Ok(result);
        }
        [HttpGet("getweights")]
        public async Task<IActionResult> GetWeights()
        {
            var result = await _servicesGetFilterParams.GetListWeight();
            if(result == null)
            {
                return new BadRequestObjectResult(new { Message = "Có lỗi khi lấy thông tin trọng lượng sản phẩm " });
            }
            return Ok(result);
        }
        
    }
}
