using Moq;
using System.Linq;
using System.Threading.Tasks;
using Vendas.Core.Notifications;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Services;
using Vendas.Tests.Config;

namespace Vendas.Tests
{
    [CollectionDefinition(nameof(VendasServiceAutoMockerFixture))]
    public class VendasServiceAutoMockerFixtureTests : ICollectionFixture<VendasServiceAutoMockerFixture>
    {
    }

    [Collection(nameof(VendasServiceAutoMockerFixture))]
    public class VendasServiceTests
    {
        private readonly VendasServiceAutoMockerFixture _vendasServiceFixture;
        private readonly VendasService _vendasService;

        public VendasServiceTests(VendasServiceAutoMockerFixture vendasServiceFixture)
        {
            _vendasServiceFixture = vendasServiceFixture;
            _vendasService = _vendasServiceFixture.ObterVendasService();
        }

        [Fact(DisplayName = "Cadastrar Venda com Sucesso")]
        [Trait("Categoria", "Vendas Service")]
        public async Task VendasService_CadastrarVenda_DeveCadastrarComSucesso()
        {
            // Arrange
            var venda = _vendasServiceFixture.GerarVenda();

            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>()
                .Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _vendasService.CadastrarVenda(venda);

            // Assert
            Assert.True(result);
            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>().Verify(r => r.CadastrarVenda(venda), Times.Once);
        }


        [Fact(DisplayName = "Alterar Venda com Sucesso")]
        [Trait("Categoria", "Vendas Service")]
        public async Task VendasService_AlterarVenda_DeveRemoverItemERecalcularValorTotal()
        {
            // Arrange
            var venda = _vendasServiceFixture.GerarVenda();

            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>()
                .Setup(r => r.ObterVendaPorId(venda.Id)).ReturnsAsync(venda);

            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>()
                .Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            var valorTotalOriginal = venda.ValorTotal;

            var itemRemovido = venda.Itens.First();
            venda.Itens.Remove(itemRemovido);
            venda.ValorTotal = venda.Itens.Sum(i => (i.ValorUnitario * i.Quantidade) - i.Desconto);

            // Act
            var result = await _vendasService.AtualizarVenda(venda);

            // Assert
            Assert.True(result);
            Assert.Equal(venda.ValorTotal, venda.Itens.Sum(i => (i.ValorUnitario * i.Quantidade) - i.Desconto));
            Assert.NotEqual(valorTotalOriginal, venda.ValorTotal); 
            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>().Verify(r => r.AtualizarVenda(venda), Times.Once);
            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }



        [Fact(DisplayName = "Cancelar Venda com Sucesso")]
        [Trait("Categoria", "Vendas Service")]
        public async Task VendasService_CancelarVenda_DeveCancelarComSucesso()
        {
            // Arrange
            var venda = _vendasServiceFixture.GerarVenda();
            venda.Cancelado = false;

            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>()
                .Setup(r => r.ObterVendaPorId(venda.Id)).ReturnsAsync(venda);
            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>()
                .Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _vendasService.CancelarVenda(venda);

            // Assert
            Assert.True(result);
            _vendasServiceFixture.Mocker.GetMock<IVendasRepository>().Verify(r => r.AtualizarVenda(venda), Times.Once);
        }
    }
}
