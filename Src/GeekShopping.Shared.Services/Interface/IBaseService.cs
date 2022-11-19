namespace GeekShoping.Shared.Services.Interface;

public interface IBaseService
{
    void QueryAsync(string sql);
    IEnumerable<T> QueryAsync<T>(string where) where T : class;
    T BuscarPorIdAsync<T>(int id) where T : class;
    T BuscarPorQueryAsync<T>(string query);
    T BuscarPorQueryGeradorAsync<T>(string sqlWhere = null) where T : class;
    IEnumerable<T> BuscarTodosPorQueryAsync<T>(string query = null) where T : class;
    IEnumerable<T> BuscarTodosPorQueryGeradorAsync<T>(string sqlWhere = null) where T : class;
    int AdicionarAsync<T>(T entidade) where T : class;
    int AtualizarAsync<T>(int id, T entidade) where T : class;
    int ExcluirAsync<T>(int id) where T : class;
    int? ObterUltimoRegistroAsync<T>() where T : class;
}
