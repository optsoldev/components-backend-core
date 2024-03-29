using AutoMapper;
using Optsol.Playground.Application.ViewModels.CartaoCredito;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Application.Mappers.CartaoCredito
{
    public class CartaoCreditoEntityToViewModelMapper : Profile
    {
        public CartaoCreditoEntityToViewModelMapper()
        {
            CreateMap<Domain.Entities.CartaoCredito, CartaoCreditoResponse>()
                .ForMember(item => item.Validade, item => item.MapFrom(src => src.Validade.ToString("MM/yy")));

        }
    }
}
