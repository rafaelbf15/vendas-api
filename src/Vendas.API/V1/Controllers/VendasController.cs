using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Vendas.API.Controllers;
using Vendas.API.ViewModels;
using Vendas.Core.Notifications;
using Vendas.Core.Options;
using Vendas.Domain.Interfaces;
using Vendas.Domain.Models;
using Vendas.Domain.Services;
using Vendas.Core.Helpers;

namespace Vendas.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize("ApiKeyOrBearer")]
    public class VendasController : MainController
    {
        private readonly IVendasRepository _vendasRepository;
        private readonly IVendasService _vendasService;

        public VendasController(
            IMapper autoMapper, 
            INotificator notificator, 
            IOptions<AppSettingsConfig> appSettings, 
            IVendasRepository vendasRepository,
            IVendasService vendasService
            ) : base(autoMapper, notificator, appSettings)
        {
            _vendasRepository = vendasRepository;
            _vendasService = vendasService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarVenda([FromBody] VendaViewModel vendaViewModel)
        {
            

            try
            {
                if (!ModelState.IsValid)
                {
                    NotifyError("Erro ao cadastrar venda, tente novamente ou contate o suporte");
                    return CustomResponse();
                }
                var venda = _mapper.Map<Venda>(vendaViewModel);
                if (!await _vendasService.CadastrarVenda(venda))
                {
                    NotifyError("Erro ao cadastrar a venda.");
                    return CustomResponse(vendaViewModel);
                }

                return CustomResponse();
            }
            catch (Exception ex)
            {

                throw;
            }

            
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarVenda(Guid id, [FromBody] VendaViewModel vendaViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyError("Erro ao atualizar venda, tente novamente ou contate o suporte");
                return CustomResponse();
            }

            var vendaExistente = await _vendasRepository.ObterVendaPorId(id);
            if (vendaExistente == null)
            {
                NotifyError("Venda não encontrada, tente novamente ou contate o suporte");
                return CustomResponse();
            }

            var venda = _mapper.Map(vendaViewModel, vendaExistente);

            if (!await _vendasService.AtualizarVenda(venda))
            {
                NotifyError("Erro ao atualizar a venda.");
                return CustomResponse(vendaViewModel);
            }

            return CustomResponse();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> CancelarVenda(Guid id)
        {
            if (!ModelState.IsValid)
            {
                NotifyError("Erro ao cancelar venda, tente novamente ou contate o suporte");
                return CustomResponse();
            }

            var venda = await _vendasRepository.ObterVendaPorId(id);
            if (venda == null)
            {
                NotifyError("Venda não encontrada, tente novamente ou contate o suporte");
                return CustomResponse();
            }

            if (!await _vendasService.CancelarVenda(venda))
            {
                NotifyError("Erro ao cancelar a venda.");
                return CustomResponse();
            }

            return CustomResponse();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterVendaPorId(Guid id)
        {
            var venda = await _vendasRepository.ObterVendaPorId(id);
            if (venda == null) return CustomResponse(); 

            var vendaViewModel = _mapper.Map<VendaViewModel>(venda);
            return CustomResponse(vendaViewModel);
        }

        [HttpGet("cliente/{clienteId:guid}")]
        public async Task<IActionResult> ObterVendasPorClienteId(Guid clienteId)
        {
            var vendas = await _vendasRepository.ObterVendasPorClienteId(clienteId);
            if (!vendas.IsAny()) return CustomResponse();

            var vendasViewModel = _mapper.Map<IEnumerable<VendaViewModel>>(vendas);
            return CustomResponse(vendasViewModel);
        }
    }
}
