using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.Messages;

namespace Vendas.Domain.Events
{
    public class CompraCriadaEvent : DomainEvent
    {
        public Guid VendaId { get; set; }

        public CompraCriadaEvent(Guid aggregateId) : base(aggregateId)
        {
            VendaId = aggregateId;
        }
    }
}
