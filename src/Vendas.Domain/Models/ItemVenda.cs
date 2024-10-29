using System;
using Vendas.Core.DomainObjects;

namespace Vendas.Domain.Models
{
    public class ItemVenda : Entity
    {
        public Guid VendaId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotalItem => (ValorUnitario * Quantidade) - Desconto;

        //EF Rel
        public virtual Produto Produto { get; set; }
    }
}
