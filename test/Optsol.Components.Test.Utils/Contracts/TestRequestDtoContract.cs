using FluentValidation;
using Optsol.Components.Test.Utils.ViewModels;

namespace Optsol.Components.Test.Utils.Contracts
{
    public class TestRequestDtoContract : AbstractValidator<TestRequestDto>
    {
        public TestRequestDtoContract()
        {
            RuleFor(request => request.Nome).MinimumLength(3).MaximumLength(70).WithMessage("O nome deve conter de 3 a 70 caracteres");
            RuleFor(request => request.Contato).EmailAddress().WithMessage("E-mail inválido");
        }
    }
}
