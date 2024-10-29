using Vendas.Core.Messages;
using System.Threading.Tasks;

namespace Vendas.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task PublishDomainEvent<T>(T notificacao) where T : DomainEvent;

    }
}
