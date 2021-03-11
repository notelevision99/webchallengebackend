using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Client.ModelView.PasswordResetView
{
    public class Cl_ResetPasswordView
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
