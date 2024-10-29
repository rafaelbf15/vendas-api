namespace Vendas.API.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class VendaViewModel
    {
        public Guid Id { get; set; }
        public string Numero { get; set; }
        public DateTime DataVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string NomeCliente { get; set; } // Descrição do Cliente
        public Guid FilialId { get; set; }
        public string NomeFilial { get; set; } // Descrição da Filial
        public decimal ValorTotal { get; set; }
        public bool Cancelado { get; set; }
        public List<ItemVendaViewModel> Itens { get; set; } // Lista de itens
    }

}
