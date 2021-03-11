using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Admin.ModelView.UserView
{
    public class UserToReturn
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
    }
}
