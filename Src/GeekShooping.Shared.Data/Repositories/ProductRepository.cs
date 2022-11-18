using GeekShopping.Shared.Domain.Entities;
using GeekShopping.Shared.Domain.Entities.filtro;
using GeekShopping.Shared.Domain.Interface;
using System.Text;

namespace GeekShopping.Shared.Data.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        #region [Propriedades Privadas]
        private readonly IBaseRepository _baseRepository;
        #endregion

        #region [Métodos Privados]
        private string ObterFiltros(filtroProduct filtro)
        {
            var sqlPesquisa = new StringBuilder();

            sqlPesquisa.AppendLine($" WHERE email ILIKE '%{filtro.Email}%'");
            sqlPesquisa.AppendLine($"   AND nome ilike '%{filtro.Nome}%'");

            return sqlPesquisa.ToString();
        }
        #endregion

        #region [Construtor]
        public ProductRepository(IBaseRepository baseRepository) => _baseRepository = baseRepository;
        #endregion

        public Task<bool> Atualizar(Product Product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Inserir(Product Product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ObterEntidade(Product Product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> ObterPorCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> ObterTodos(filtroProduct filtro)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalRegistros(filtroProduct filtro)
        {
            throw new NotImplementedException();
        }
    }
}
