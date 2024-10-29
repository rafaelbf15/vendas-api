using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Services;

namespace Vendas.Domain.Events
{
    public class VendasEventHandler : INotificationHandler<CompraCriadaEvent>,
                                      INotificationHandler<CompraAlteradaEvent>,
                                      INotificationHandler<CompraCanceladaEvent>
    {
        private readonly IVendasRepository _vendasRepository;
        private readonly RabbitMQService _rabbitMQService;

        public VendasEventHandler(IVendasRepository vendasRepository, RabbitMQService rabbitMQService)
        {
            _vendasRepository = vendasRepository;
            _rabbitMQService = rabbitMQService;
        }

        public async Task Handle(CompraCriadaEvent message, CancellationToken cancellationToken)
        {
            var venda = await _vendasRepository.ObterVendaPorId(message.VendaId);

            _rabbitMQService.PublishEvent(venda, _rabbitMQService._settings.CompraCriadaQueue);

        }

        public async Task Handle(CompraAlteradaEvent message, CancellationToken cancellationToken)
        {
            var venda = await _vendasRepository.ObterVendaPorId(message.VendaId);

            _rabbitMQService.PublishEvent(venda, _rabbitMQService._settings.CompraAlteradaQueue);
        }

        public async Task Handle(CompraCanceladaEvent message, CancellationToken cancellationToken)
        {
            var venda = await _vendasRepository.ObterVendaPorId(message.VendaId);

            _rabbitMQService.PublishEvent(venda, _rabbitMQService._settings.CompraCanceladaQueue);
        }
    }
}
