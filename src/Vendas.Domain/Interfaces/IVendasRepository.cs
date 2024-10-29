using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.DataObjects;
using Vendas.Domain.Models;

namespace Vendas.Domain.Interfaces
{
    public interface IVendasRepository : IRepository<Venda>
    {
        void CadastrarVenda(Venda venda);
        void AtualizarVenda(Venda venda);
        Task<Venda> ObterVendaPorId(Guid id);
        Task<IEnumerable<Venda>> ObterVendasPorClienteId(Guid clienteId);
    }
}
