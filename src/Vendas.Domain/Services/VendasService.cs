using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.Notifications;
using Vendas.Core.Services;
using Vendas.Domain.Events;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Models;
using Vendas.Domain.Validation;

namespace Vendas.Domain.Services
{
    public class VendasService : BaseService, IVendasService
    {
        private readonly IVendasRepository _repository;

        public VendasService(IVendasRepository vendasRepository, INotificator notificator) : base(notificator)
        {
            _repository = vendasRepository;
        }

        public async Task<bool> CadastrarVenda(Venda venda)
        {
            if (!ExecValidation(new VendaValidation(), venda)) return false;

            _repository.CadastrarVenda(venda);

            venda.AddEvent(new CompraCriadaEvent(venda.Id));

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> AtualizarVenda(Venda venda)
        {
            if (!ExecValidation(new VendaValidation(), venda)) return false;

            if (venda.Cancelado)
            {
                Notify("Não é possível atualizar uma venda que já foi cancelada.");
                return false;
            }

            venda.AtualizarVenda();

            _repository.AtualizarVenda(venda);

            venda.AddEvent(new CompraAlteradaEvent(venda.Id));

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> CancelarVenda(Venda venda)
        {
            if (venda.Cancelado)
            {
                Notify("A venda já está cancelada.");
                return false;
            }

            if (venda.DataVenda < DateTime.Now.AddDays(-15))
            {
                Notify("Não é permitido cancelar uma venda concluída há mais de 15 dias.");
                return false;
            }

            venda.CancelarVenda();

            _repository.AtualizarVenda(venda);

            venda.AddEvent(new CompraCanceladaEvent(venda.Id));

            return await _repository.UnitOfWork.Commit();
        }

    }

}
