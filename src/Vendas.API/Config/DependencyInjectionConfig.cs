using Microsoft.Extensions.DependencyInjection;
using Vendas.Core.Notifications;
using Microsoft.AspNetCore.Http;
using Vendas.Infra.Context;
using Vendas.Data.Repository;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Services;
using MediatR;
using Vendas.Core.Mediator;
using Vendas.Domain.Events;

namespace Vendas.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //Auth
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Contexts
            services.AddScoped<VendasDbContext>();

            //Repository
            services.AddScoped<IVendasRepository, VendasRepository>();

            // Services
            services.AddSingleton<RabbitMQService>();
            services.AddScoped<IVendasService, VendasService>();

            // Notifications
            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<CompraCriadaEvent>, VendasEventHandler>();
            services.AddScoped<INotificationHandler<CompraAlteradaEvent>, VendasEventHandler>();
            services.AddScoped<INotificationHandler<CompraCanceladaEvent>, VendasEventHandler>();
        }
    }
}
