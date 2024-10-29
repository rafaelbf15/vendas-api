using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Validation
{
    using FluentValidation;
    using System;
    using Vendas.Domain.Models;

    public class ItemVendaValidation : AbstractValidator<ItemVenda>
    {
        public ItemVendaValidation()
        {
            RuleFor(i => i.ProdutoId)
                .NotEmpty().WithMessage("Cada item de venda deve estar associado a um produto.");

            RuleFor(i => i.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade de cada item de venda deve ser maior que zero.");

            RuleFor(i => i.ValorUnitario)
                .GreaterThan(0).WithMessage("O valor unitário de cada item de venda deve ser maior que zero.");

            RuleFor(i => i.Desconto)
                .GreaterThanOrEqualTo(0).WithMessage("O desconto em um item de venda não pode ser negativo.");
        }
    }

}
