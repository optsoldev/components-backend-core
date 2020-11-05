using AutoMapper;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entidades;

namespace Optsol.Playground.Application.Mappers.CartaoCredito
{
    public class CartaoCreditoEntityToViewModelMapper : Profile
    {
        public CartaoCreditoEntityToViewModelMapper()
        {
            CreateMap<CartaoCreditoEntity, CartaoCreditoViewModel>();
        }
    }
}
