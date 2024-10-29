using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vendas.Core.Communication;
using Vendas.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Vendas.Core.Options;

namespace Vendas.API.Controllers
{

    [ApiController]
    public class MainController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly INotificator _notificator;
        protected readonly Guid userId;
        protected readonly AppSettingsConfig _appSettings;

        public MainController(IMapper autoMapper,
                              INotificator notificator,
                              IOptions<AppSettingsConfig> appSettings
            )
        {
            _mapper = autoMapper;
            _notificator = notificator;
            _appSettings = appSettings.Value;
        }

        protected List<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            AssignNotifications();

            if (OperationIsValid())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ResponseResult response)
        {
            ResponseHasErrors(response);

            return CustomResponse();
        }

        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response == null || !response.Errors.Messages.Any()) return false;

            foreach (var message in response.Errors.Messages)
            {
                AddProcessingError(message);
            }

            return true;
        }

        protected bool OperationIsValid()
        {
            return !Errors.Any();
        }

        protected void AddProcessingError(string error)
        {
            Errors.Add(error);
        }

        protected void ClearProcessingErrors()
        {
            Errors.Clear();
        }

        protected void NotifyError(string message)
        {
            _notificator.Handle(new Notification(message));
        }

        private void AssignNotifications()
        {
            if (_notificator.HasNotifications())
            {
                Errors.AddRange(_notificator.GetNotifications().Select(n => n.Message));
            }
        }
    }
}
