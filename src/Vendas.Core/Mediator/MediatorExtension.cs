using Vendas.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vendas.Core.Helpers;

namespace Vendas.Core.Mediator
{
    public static class MediatorExtension
    {
        public static async Task PublishEvents(this IMediatorHandler mediator, DbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications.IsAny());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublishDomainEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        public static async Task PublishEvents(this IMediatorHandler mediator, Entity entity)
        {
            if (entity.Notifications.IsAny())
            {
                var domainEvents = entity.Notifications.ToList();

                entity.ClearEvents();

                var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublishDomainEvent(domainEvent);
                });

                await Task.WhenAll(tasks);
            }
        }
    }
}
