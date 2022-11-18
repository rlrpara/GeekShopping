using Dapper;
using GeekShopping.Shared.Data.Context;
using GeekShopping.Shared.Data.Interface;
using GeekShopping.Shared.Data.ValueObjects;
using GeekShopping.Shared.Domain.Interface;
using System.Data;
using System.Text;

namespace GeekShopping.Shared.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        #region [Propriedades Privadas]
        private ParametrosConexao _parametrosConexao;
        private readonly IGeradorDapper _geradorDapper;
        #endregion

        #region [Métodos Privados]
        private IDbConnection ObterConexao() => ConnectionConfiguration.AbrirConexao(_parametrosConexao);
        private ParametrosConexao ObterParametrosConexao() => new()
        {
            Servidor = Environment.GetEnvironmentVariable("SERVIDOR"),
            Porta = Environment.GetEnvironmentVariable("PORTA"),
            NomeBanco = Environment.GetEnvironmentVariable("BANCO"),
            Usuario = Environment.GetEnvironmentVariable("USUARIO"),
            Senha = Environment.GetEnvironmentVariable("SENHA"),
            TipoBanco = Convert.ToInt32(Environment.GetEnvironmentVariable("TIPO_BANCO"))
        };
        #endregion

        #region [Construtor]
        public BaseRepository()
        {
            _parametrosConexao = ObterParametrosConexao();
            _geradorDapper = new GeradorDapper(_parametrosConexao);
        }
        #endregion

        #region [Métodos Públicos]
        public async Task<int> AdicionarAsync<T>(T entidade) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.ExecuteAsync(_geradorDapper.GeralSqlInsertControles(entidade));
            }
            catch
            {
                return default;
            }
        }
        public async Task<int> AtualizarAsync<T>(int id, T entidade) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.ExecuteAsync(_geradorDapper.GeralSqlUpdateControles(id, entidade), commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public async Task<T> BuscarPorIdAsync<T>(int id) where T : class
        {
            try
            {
                var sqlPesquisa = new StringBuilder();

                sqlPesquisa.AppendLine($"SELECT {_geradorDapper.RetornaCamposSelect<T>()}");
                sqlPesquisa.AppendLine($"  FROM {_geradorDapper.ObterNomeTabela<T>()}");
                sqlPesquisa.AppendLine($" WHERE {_geradorDapper.ObterChavePrimaria<T>()} = {id}");

                using var conn = ObterConexao();
                return await conn.QueryFirstOrDefaultAsync<T>(sqlPesquisa.ToString(), commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public async Task<T> BuscarPorQueryAsync<T>(string query)
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.QueryFirstOrDefaultAsync<T>(query, commandType: CommandType.Text, transaction: null);
            }
            catch
            {
                return default;
            }
        }
        public async Task<T> BuscarPorQueryGeradorAsync<T>(string? sqlWhere = null) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.QueryFirstOrDefaultAsync<T>(_geradorDapper.GeralSqlSelecaoControles<T>(sqlWhere), commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public async Task<IEnumerable<T>> BuscarTodosPorQueryAsync<T>(string? query = null) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.QueryAsync<T>(query, commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public async Task<IEnumerable<T>> BuscarTodosPorQueryGeradorAsync<T>(string? sqlWhere = null) where T : class
        {
            try
            {
                return await BuscarTodosPorQueryAsync<T>(_geradorDapper.GeralSqlSelecaoControles<T>(sqlWhere));
            }
            catch
            {
                return default;
            }
        }
        public async Task<int> ExcluirAsync<T>(int id) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.ExecuteAsync($"{_geradorDapper.ObterDelete<T>(id)}", commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public async Task<int?> ObterUltimoRegistroAsync<T>() where T : class
        {
            try
            {
                var sqlPesquisa = new StringBuilder();
                sqlPesquisa.AppendLine($"  SELECT TOP 1 {_geradorDapper.ObterChavePrimaria<T>()}");
                sqlPesquisa.AppendLine($"    FROM {_geradorDapper.ObterNomeTabela<T>()}");
                sqlPesquisa.AppendLine($"ORDER BY {_geradorDapper.ObterChavePrimaria<T>()} DESC");

                using var conn = ObterConexao();
                return await conn.QueryFirstOrDefaultAsync<int?>(sqlPesquisa.ToString(), commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }
        public void QueryAsync(string sql)
        {
            try
            {
                using var conn = ObterConexao();
                conn.QueryAsync(sql, commandType: CommandType.Text);
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string where) where T : class
        {
            try
            {
                using var conn = ObterConexao();
                return await conn.QueryAsync<T>(where, commandType: CommandType.Text);
            }
            catch
            {
                return default;
            }
        }

        #endregion
    }
}
