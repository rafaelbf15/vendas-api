namespace Vendas.API.ViewModels
{
    using System;

    public class ItemVendaViewModel
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; } // Descrição do Produto
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotalItem { get; set; }
    }

}
