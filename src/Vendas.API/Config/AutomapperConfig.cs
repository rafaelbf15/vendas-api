using AutoMapper;
using Vendas.API.ViewModels;
using Vendas.Domain.Models;

namespace Vendas.API.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Venda, VendaViewModel>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.NomeFilial, opt => opt.MapFrom(src => src.Filial.Nome));

            CreateMap<VendaViewModel, Venda>();

            CreateMap<ItemVenda, ItemVendaViewModel>()
                .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto.Nome))
                .ForMember(dest => dest.ValorTotalItem, opt => opt.MapFrom(src => (src.ValorUnitario * src.Quantidade) - src.Desconto));

            CreateMap<ItemVendaViewModel, ItemVenda>();

            CreateMap<Produto, ProdutoViewModel>().ReverseMap();

            CreateMap<Filial, FilialViewModel>().ReverseMap();

            CreateMap<Cliente, ClienteViewModel>().ReverseMap();

        }
    }
}
