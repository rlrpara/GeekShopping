using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;

namespace GeekShopping.Shared.Domain.Interface
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Product> ObterPorCodigo(int codigo);
        Task<int> TotalRegistros(filtroProduct filtro);
        Task<IEnumerable<Product>> ObterTodos(filtroProduct filtro);
        Task<bool> ObterEntidade(Product product);
        Task<bool> Inserir(Product product);
        Task<bool> Atualizar(Product product);
    }
}
