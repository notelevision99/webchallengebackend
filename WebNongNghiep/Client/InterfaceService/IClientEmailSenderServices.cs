using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebNongNghiep.Client.ModelView.MessageMailView;

namespace WebNongNghiep.Client.InterfaceService
{
    public interface IClientEmailSenderServices
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);

    }
}
