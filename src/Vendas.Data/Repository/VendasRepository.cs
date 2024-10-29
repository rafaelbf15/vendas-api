using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.DataObjects;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Models;
using Vendas.Infra.Context;

namespace Vendas.Data.Repository
{
    public class VendasRepository : IVendasRepository
    {
        private readonly VendasDbContext _context;

        public VendasRepository(VendasDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void CadastrarVenda(Venda venda)
        {
            _context.Vendas.Add(venda);
        }

        public void AtualizarVenda(Venda venda)
        {
            _context.Vendas.Update(venda);
        }

        public async Task<Venda> ObterVendaPorId(Guid id)
        {
            return await _context.Vendas.AsNoTracking()
                .Include(x => x.Itens).ThenInclude(x => x.Produto)
                .Include(x => x.Cliente)
                .Include(x => x.Filial)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Venda>> ObterVendasPorClienteId(Guid clienteId)
        {
            return await _context.Vendas.AsNoTracking()
                .Include(x => x.Itens).ThenInclude(x => x.Produto)
                .Include(x => x.Cliente)
                .Include(x => x.Filial)
                .Where(x => x.ClienteId == clienteId)
                .ToListAsync();
        }

        public void Dispose() => _context?.Dispose();
        
    }
}
