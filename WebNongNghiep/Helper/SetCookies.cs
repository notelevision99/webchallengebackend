using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNongNghiep.Helper
{
    public static class SetCookies
    {
        public static ActionResult SetCookiesResetPwd(this ActionResult result, Microsoft.AspNetCore.Http.HttpResponse response, string text)
        {
            response.Cookies.Append(
                "Urs_CfmPwd",
                text,
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/"
                }
            );

            return result;
        }
    }

}
