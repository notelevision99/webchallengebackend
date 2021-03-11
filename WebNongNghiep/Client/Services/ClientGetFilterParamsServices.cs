using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.InterfaceService;
using WebNongNghiep.Client.ModelView.FilterParamsView;
using WebNongNghiep.Database;

namespace WebNongNghiep.Client.Services
{
    public class ClientGetFilterParamsServices : IClientGetFilterParamsServices
    {
        MasterData _db;
        public ClientGetFilterParamsServices(MasterData db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Cl_Company>> GetListCompany()
        {
            var listCompany =  _db.Products.GroupBy(p => p.Company)
                .Select(p => new Cl_Company { 
                    Company = p.Key
            });
            return listCompany;
        }
        public async Task<IEnumerable<Cl_Weight>> GetListWeight()
        {
            var listWeight = _db.Products.GroupBy(p => p.Weight)
                .Select(p => new Cl_Weight
                {
                    Weight = p.Key
                });
            return listWeight;
        }

        
    }
}
