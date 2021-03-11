using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.FilterParamsView;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientGetFilterParamsServices
    {
        Task<IEnumerable<Cl_Company>> GetListCompany();
        Task<IEnumerable<Cl_Weight>> GetListWeight();

    }
}
