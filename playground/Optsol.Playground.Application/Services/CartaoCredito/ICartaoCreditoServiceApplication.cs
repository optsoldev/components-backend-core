using Optsol.Components.Application.Service;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Services.CartaoCredito
{
    public interface ICartaoCreditoServiceApplication : IBaseServiceApplication<CartaoCreditoEntity, CartaoCreditoViewModel, CartaoCreditoViewModel, InsertCartaoCreditoViewModel, UpdateCartaoCreditoViewModel>
    {
    }
}
