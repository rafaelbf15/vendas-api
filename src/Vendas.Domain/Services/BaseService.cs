using FluentValidation;
using FluentValidation.Results;
using Vendas.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vendas.Core.Services
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;

        protected BaseService(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string message)
        {
            _notificator.Handle(new Notification(message));
        }

        protected bool ExecValidation<TV, TE>(TV validation = null, TE entity = null) where TV : AbstractValidator<TE> where TE : class
        {
            if (validation == null || entity == null) return false;

            var validator = validation.Validate(entity);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}
