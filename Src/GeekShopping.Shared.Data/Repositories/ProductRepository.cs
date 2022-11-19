using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;
using GeekShopping.Shared.Domain.Interface;
using System.Text;

namespace GeekShopping.Shared.Data.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        #region [Private Properties]
        private readonly IBaseRepository _baseRepository;
        #endregion

        #region [Private Methods]
        private string ObterFiltros(filtroProduct filtro)
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($" WHERE name like '%{filtro.Name}%'");

            return sqlPesquisa.ToString();
        }
        #endregion

        #region [Constructor]
        public ProductRepository(IBaseRepository baseRepository) => _baseRepository = baseRepository;
        #endregion

        #region [Public Methods]
        public async Task<int> TotalRegistros(filtroProduct filtro)
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($"SELECT COUNT(ID) as Total");
            sqlPesquisa.AppendLine($"  FROM prouct");
            sqlPesquisa.AppendLine(ObterFiltros(filtro));

            return await _baseRepository.BuscarPorQueryAsync<int>(sqlPesquisa.ToString());
        }
        public async Task<Product> ObterPorCodigo(int Codigo) => await _baseRepository.BuscarPorIdAsync<Product>(Codigo);
        public async Task<bool> ObterEntidade(Product product)
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($" (nome = '{product.Name}')");
            sqlPesquisa.AppendLine($"   AND id = {product.Codigo}");

            return await _baseRepository.BuscarPorQueryGeradorAsync<Product>(sqlPesquisa.ToString()) is not null;
        }
        public async Task<IEnumerable<Product>> ObterTodos(filtroProduct filtro)
            => await _baseRepository.BuscarTodosPorQueryGeradorAsync<Product>($" WHERE name like '%{filtro.Name}%'");
        public async Task<bool> Atualizar(Product product) => await _baseRepository.AtualizarAsync((int)product.Codigo, product) > 0;
        public async Task<bool> Inserir(Product product) => await _baseRepository.AdicionarAsync(product) > 0;

        #endregion
    }
}
