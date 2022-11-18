using GeekShoping.Shared.Services.ViewModel;
using GeekShoping.Shared.Services.ViewModel.filtro;

namespace GeekShoping.Shared.Services.Interface
{
    public interface IProductService : IBaseService
    {
        int ObterTotalRegistros(filtroProductViewModel filtro);
        bool ObterEntidade(ProductViewModel model);
        ProductViewModel ObterPorCodigo(long codigo);
        IEnumerable<ProductViewModel> ObterTodos(filtroProductViewModel filtro);
        bool Inserir(ProductViewModel model);
        bool Atualizar(ProductViewModel model);
        bool Deletar(ProductViewModel model);
    }
}
