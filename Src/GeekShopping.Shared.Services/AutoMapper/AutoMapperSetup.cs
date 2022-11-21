using AutoMapper;
using GeekShoping.Shared.Services.ViewModel;
using GeekShoping.Shared.Services.ViewModel.filtro;
using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;

namespace GeekShopping.Shared.Services.AutoMapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            #region [ViewModelToDomain]
            CreateMap<filtroProductViewModel, filtroProduct>();
            CreateMap<filtroPaginacaoViewModel, filtroPaginacao>();

            CreateMap<ProductViewModel, Product>();
            #endregion



            #region [DomainToViewModel]
            CreateMap<filtroProduct, filtroProductViewModel>();
            CreateMap<filtroPaginacao, filtroPaginacaoViewModel>();
            
            CreateMap<Product, ProductViewModel>();
            #endregion
        }
    }
}
