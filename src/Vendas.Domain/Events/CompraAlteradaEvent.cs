using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.Messages;

namespace Vendas.Domain.Events
{
    public class CompraAlteradaEvent : DomainEvent
    {
        public Guid VendaId { get; set; }

        public CompraAlteradaEvent(Guid aggregateId) : base(aggregateId)
        {
            VendaId = aggregateId;
        }
    }
}
