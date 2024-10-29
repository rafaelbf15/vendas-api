using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Domain.Models;

namespace Vendas.Domain.Interfaces
{
    public interface IVendasService
    {
        Task<bool> CadastrarVenda(Venda venda);
        Task<bool> AtualizarVenda(Venda venda);
        Task<bool> CancelarVenda(Venda venda);
    }
}
