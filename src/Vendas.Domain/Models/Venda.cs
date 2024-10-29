using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendas.Core.DomainObjects;

namespace Vendas.Domain.Models
{

    public class Venda : Entity, IAggregateRoot
    {
        public string Numero { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public Guid FilialId { get; set; }
        public List<ItemVenda> Itens { get; set; }
        public bool Cancelado { get; set; }

        //EF Rel
        public virtual Cliente Cliente { get; set; }
        public virtual Filial Filial { get; set; }

        public void AtualizarVenda() => DataAtualizacao = DateTime.UtcNow;

        public void CancelarVenda()
        {
            Cancelado = true;
            DataAtualizacao = DateTime.UtcNow;
            Cliente = null;
            Filial = null;
            Itens = null;
        }
    }
}
