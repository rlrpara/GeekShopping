using AutoMapper;
using GeekShoping.Shared.Services.Interface;
using GeekShoping.Shared.Services.ViewModel;
using GeekShoping.Shared.Services.ViewModel.filtro;
using GeekShopping.Shared.Data.Repositories;
using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;
using GeekShopping.Shared.Domain.Interface;

namespace GeekShoping.Shared.Services.Service
{
    public class ProductService : BaseService, IProductService
    {
        #region [Propriedades Privadas]
        private readonly IMapper _mapper;
        private readonly IProductRepository _service;
        #endregion

        #region [Construtor]
        public ProductService(IBaseRepository baseRepository, IMapper mapper) : base(baseRepository)
        {
            _mapper = mapper;
            _service = new ProductRepository(baseRepository);
        }

        #endregion

        #region [Metodos Publicos]
        public int ObterTotalRegistros(filtroProductViewModel filtro) => _mapper.Map<int>(_service.TotalRegistros(_mapper.Map<filtroProduct>(filtro)).Result);
        public ProductViewModel ObterPorCodigo(long codigo) => _mapper.Map<ProductViewModel>(_service.ObterPorCodigo((int)codigo).Result);
        public IEnumerable<ProductViewModel> ObterTodos(filtroProductViewModel filtro) => _mapper.Map<IEnumerable<ProductViewModel>>(_service.ObterTodos(_mapper.Map<filtroProduct>(filtro)).Result);
        public bool ObterEntidade(ProductViewModel model) => _service.ObterEntidade(_mapper.Map<Product>(model)).Result;
        public bool Inserir(ProductViewModel model) => _service.Inserir(_mapper.Map<Product>(model)).Result;
        public bool Atualizar(ProductViewModel model)
        {
            model.Ativo ??= ObterPorCodigo(model.Codigo).Ativo;
            return _service.Atualizar(_mapper.Map<Product>(model)).Result;
        }
        public bool Deletar(ProductViewModel model)
        {
            model.Ativo = false;
            return _service.Atualizar(_mapper.Map<Product>(model)).Result;
        }
        #endregion
    }
}
