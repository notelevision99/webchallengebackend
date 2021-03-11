using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientAuthServices
    {
        //Task<Cl_UserToReturn> Register(Cl_UserDetails userDetailsDto);
        Task<IEnumerable<Cl_UserToReturn>> GetListUsers();
    }
}
