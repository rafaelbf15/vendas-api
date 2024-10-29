using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Vendas.API.ViewModels;
using Vendas.Tests.Config;

namespace Vendas.Tests
{
    [TestCaseOrderer(ordererTypeName: "Vendas.Tests.Config.PriorityOrderer",
    ordererAssemblyName: "Vendas.Tests")]
    [CollectionDefinition(nameof(VendasApiIntegrationTestFixture))]
    public class VendasApiIntegrationTests : IClassFixture<VendasApiIntegrationTestFixture>
    {
        private VendasApiIntegrationTestFixture _fixture;
        private readonly HttpClient _client;
        
        public VendasApiIntegrationTests(VendasApiIntegrationTestFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.CreateClient();
            _client.DefaultRequestHeaders.Add("ApiKey", "9a28dcafffd34fd68a7e4f505b08c82d");
        }

        [Fact(DisplayName = "Cadastrar nova venda com sucesso"), TestPriority(1)]
        public async Task PostVenda_DeveCadastrarVendaComSucesso()
        {
            var vendaData = new
            {
                Id = _fixture.VendaId,
                Numero = "V123",
                DataVenda = DateTime.Now,
                _fixture.ClienteId,
                _fixture.FilialId,
                ValorTotal = 200,
                Itens = new[]
                {
                    new { _fixture.ProdutoId, Quantidade = 2, ValorUnitario = 100, Desconto = 0 }
                }
            };

            var response = await _client.PostAsJsonAsync("/api/v1/vendas", vendaData);

            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Atualizar venda existente"), TestPriority(2)]
        public async Task PutVenda_DeveAtualizarVendaComSucesso()
        {
            var vendaData = new
            {
                Id = _fixture.VendaId,
                Numero = "V123",
                DataVenda = DateTime.Now,
                _fixture.ClienteId,
                _fixture.FilialId,
                ValorTotal = 2000,
                Itens = new[]
                 {
                    new { _fixture.ProdutoId, Quantidade = 2, ValorUnitario = 1000, Desconto = 0 }
                }
            };

            var response = await _client.PutAsJsonAsync($"/api/v1/vendas/{_fixture.VendaId}", vendaData);

            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Cancelar venda existente"), TestPriority(3)]
        public async Task DeleteVenda_DeveCancelarVendaComSucesso()
        {
            var response = await _client.DeleteAsync($"/api/v1/vendas/{_fixture.VendaId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
