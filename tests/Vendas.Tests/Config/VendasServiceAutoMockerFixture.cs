using Bogus;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using Vendas.Domain.Models;
using Vendas.Domain.Services;

namespace Vendas.Tests.Config
{
    public class VendasServiceAutoMockerFixture : IDisposable
    {
        public VendasService VendasService;
        public AutoMocker Mocker;

        public Venda GerarVenda()
        {
            var itens = GerarItensVenda(3); // Gerar três itens de venda válidos
            var cliente = GerarCliente();
            var filial = GerarFilial();

            var valorTotal = itens.Sum(i => i.ValorUnitario * i.Quantidade - i.Desconto);

            var venda = new Faker<Venda>("pt_BR")
                .CustomInstantiator(f => new Venda
                {
                    Id = Guid.NewGuid(),
                    Numero = "V123",
                    DataVenda = DateTime.UtcNow,
                    ClienteId = cliente.Id,
                    FilialId = filial.Id,
                    ValorTotal = valorTotal,
                    Itens = itens,
                    Cliente = cliente,
                    Filial = filial
                });

            return venda;
        }

        public List<ItemVenda> GerarItensVenda(int quantidade)
        {
            var item = new Faker<ItemVenda>("pt_BR")
                .CustomInstantiator(f => new ItemVenda
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = Guid.NewGuid(),
                    Quantidade = f.Random.Int(1, 10),
                    ValorUnitario = f.Finance.Amount(10, 100),
                    Desconto = f.Random.Decimal(0, 10)
                });

            return item.Generate(quantidade);
        }

        public Cliente GerarCliente()
        {
            return new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente
                {
                    Id = Guid.NewGuid(),
                    Nome = f.Name.FullName()
                });
        }

        public Filial GerarFilial()
        {
            return new Faker<Filial>("pt_BR")
                .CustomInstantiator(f => new Filial
                {
                    Id = Guid.NewGuid(),
                    Nome = f.Company.CompanyName()
                });
        }

        public VendasService ObterVendasService()
        {
            Mocker = new AutoMocker();
            VendasService = Mocker.CreateInstance<VendasService>();
            return VendasService;
        }

        public void Dispose()
        {
           
        }
    }
}
