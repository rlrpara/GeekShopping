using GeekShoping.Shared.Services.Interface;
using GeekShopping.Shared.Data.ValueObjects;
using GeekShopping.Shared.Domain.Interface;

namespace GeekShoping.Shared.Services.Service
{
    public class BaseService : IBaseService
    {
        #region [Propriedades Privadas]
        protected IBaseRepository _baseRepository;
        private ParametrosConexao parametrosConexao;
        #endregion

        #region [Construtor]
        public BaseService(IBaseRepository baseRepository) => _baseRepository = baseRepository;
        public BaseService(ParametrosConexao parametrosConexao) => this.parametrosConexao = parametrosConexao;
        #endregion

        #region [Métodos Públicos]
        public int AdicionarAsync<T>(T entidade) where T : class => _baseRepository.AdicionarAsync(entidade).Result;
        public int AtualizarAsync<T>(int id, T entidade) where T : class => _baseRepository.AtualizarAsync(id, entidade).Result;
        public T BuscarPorIdAsync<T>(int id) where T : class => _baseRepository.BuscarPorIdAsync<T>(id).Result;
        public T BuscarPorQueryAsync<T>(string query) => _baseRepository.BuscarPorQueryAsync<T>(query).Result;
        public T BuscarPorQueryGeradorAsync<T>(string? sqlWhere = null) where T : class => _baseRepository.BuscarPorQueryGeradorAsync<T>(sqlWhere).Result;
        public IEnumerable<T> BuscarTodosPorQueryAsync<T>(string? query = null) where T : class => _baseRepository.BuscarTodosPorQueryAsync<T>(query).Result;
        public IEnumerable<T> BuscarTodosPorQueryGeradorAsync<T>(string? sqlWhere = null) where T : class => _baseRepository.BuscarTodosPorQueryGeradorAsync<T>(sqlWhere).Result;
        public int ExcluirAsync<T>(int id) where T : class => _baseRepository.ExcluirAsync<T>(id).Result;
        public int? ObterUltimoRegistroAsync<T>() where T : class => _baseRepository.ObterUltimoRegistroAsync<T>().Result;
        public void QueryAsync(string sql) => _baseRepository.QueryAsync(sql);
        public IEnumerable<T> QueryAsync<T>(string where) where T : class => _baseRepository.QueryAsync<T>(where).Result;
        #endregion
    }
}
