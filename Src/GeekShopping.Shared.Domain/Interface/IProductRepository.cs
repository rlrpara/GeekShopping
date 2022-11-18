using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;

namespace GeekShopping.Shared.Domain.Interface
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Product> ObterPorCodigo(int codigo);
        Task<int> TotalRegistros(filtroProduct filtro);
        Task<IEnumerable<Product>> ObterTodos(filtroProduct filtro);
        Task<bool> ObterEntidade(Product Product);
        Task<bool> Inserir(Product Product);
        Task<bool> Atualizar(Product Product);
    }
}
