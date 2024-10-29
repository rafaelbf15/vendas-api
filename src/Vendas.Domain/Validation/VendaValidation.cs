using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendas.Domain.Validation
{
    using FluentValidation;
    using System;
    using System.Linq;
    using Vendas.Domain.Models;

    public class VendaValidation : AbstractValidator<Venda>
    {
        public VendaValidation()
        {
            RuleFor(v => v.Numero)
                .NotEmpty().WithMessage("O número da venda é obrigatório.")
                .MaximumLength(50).WithMessage("O número da venda deve ter no máximo 50 caracteres.");

            RuleFor(v => v.ClienteId)
                .NotEmpty().WithMessage("O cliente associado à venda é obrigatório.");

            RuleFor(v => v.FilialId)
                .NotEmpty().WithMessage("A filial associada à venda é obrigatória.");

            RuleFor(v => v.DataVenda)
                .NotEqual(DateTime.MinValue).WithMessage("A data da venda é obrigatória.");

            RuleFor(v => v.ValorTotal)
                .GreaterThan(0).WithMessage("O valor total da venda deve ser maior que zero.")
                .Must((venda, valorTotal) => valorTotal == venda.Itens.Sum(i => (i.ValorUnitario * i.Quantidade) - i.Desconto))
                .WithMessage("O valor total da venda não corresponde à soma dos itens.");

            RuleFor(v => v.Itens)
                .NotEmpty().WithMessage("A venda deve ter pelo menos um item.")
                .Must(itens => itens.All(i => i.Quantidade > 0))
                .WithMessage("Cada item de venda deve ter uma quantidade maior que zero.")
                .Must(itens => itens.All(i => i.ValorUnitario > 0))
                .WithMessage("Cada item de venda deve ter um valor unitário maior que zero.")
                .Must(itens => itens.All(i => i.Desconto >= 0))
                .WithMessage("O desconto em um item de venda não pode ser negativo.");

            RuleForEach(v => v.Itens).SetValidator(new ItemVendaValidation());
        }
    }

}
