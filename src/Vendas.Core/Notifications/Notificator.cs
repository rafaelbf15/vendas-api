﻿using Vendas.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vendas.Core.Notifications
{
    public class Notificator : INotificator
    {
        private List<Notification> _notifications;
        public Notificator()
        {
            _notifications = new List<Notification>();
        }
        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotifications()
        {
            return _notifications.IsAny();   
        }
    }
}
