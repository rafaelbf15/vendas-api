using System;
using System.Collections.Generic;
using System.Text;

namespace Vendas.Core.Notifications
{
    public interface INotificator
    {
        bool HasNotifications();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
