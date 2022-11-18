using FirebirdSql.Data.FirebirdClient;
using GeekShopping.Shared.Data.Enumerables;
using GeekShopping.Shared.Data.Interface;
using GeekShopping.Shared.Data.ValueObjects;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace GeekShopping.Shared.Data.Context;

public class DeafultSqlConnectionFactory : IConnectionFactory
{
    #region [Propriedades Privadas]
    private readonly ParametrosConexao _parametrosConexao;
    #endregion

    #region [Métodos Privados]
    private SqlConnection ObterStringConexaoSqlServer()
        => new($"Server={_parametrosConexao.Servidor},{_parametrosConexao.Porta};User Id={_parametrosConexao.Usuario};Password={_parametrosConexao.Senha};{(!string.IsNullOrWhiteSpace(_parametrosConexao.NomeBanco) ? $"Database={_parametrosConexao.NomeBanco}" : "")}");
    private MySqlConnection ObterStringConexaoMySql()
        => new($"Server={_parametrosConexao.Servidor}; User Id={_parametrosConexao.Usuario}; Password={_parametrosConexao.Senha}; Allow User Variables=True");
    private NpgsqlConnection ObterStringConexaoPostgres()
        => new($"Server={_parametrosConexao.Servidor};Port={_parametrosConexao.Porta};User ID={_parametrosConexao.Usuario};Password={_parametrosConexao.Senha};{(!string.IsNullOrWhiteSpace(_parametrosConexao.NomeBanco) ? $"Database={_parametrosConexao.NomeBanco?.ToLower()}" : "")}");
    private SqliteConnection ObterStringConexaoSqlite()
    {
        var caminho = Path.Combine($"{Directory.GetCurrentDirectory()}", _parametrosConexao.NomeBanco);
        if (!File.Exists(caminho))
            File.Create(caminho).Close();
        return new SqliteConnection($"Data Source={caminho};");
    }
    private FbConnection ObterStringConexaoFirebird()
        => new($"DataSource={_parametrosConexao.Servidor};Port={_parametrosConexao.Porta};Password={_parametrosConexao.Senha};User={_parametrosConexao.Usuario};{(!string.IsNullOrWhiteSpace(_parametrosConexao.NomeBanco) ? $"Database={_parametrosConexao.NomeBanco}" : "")};Charset=NONE;Pooling=True;");
    #endregion

    #region [Construtor]
    public DeafultSqlConnectionFactory(ParametrosConexao parametrosConexao) => _parametrosConexao = parametrosConexao;
    #endregion

    #region [Métodos Públicos]
    public IDbConnection? Conexao() => (ETipoBanco)_parametrosConexao.TipoBanco switch
    {
        ETipoBanco.SqlServer => ObterStringConexaoSqlServer(),
        ETipoBanco.Postgresql => ObterStringConexaoPostgres(),
        ETipoBanco.MySql => ObterStringConexaoMySql(),
        ETipoBanco.SqLite => ObterStringConexaoSqlite(),
        ETipoBanco.Firebird => ObterStringConexaoFirebird(),
        _ => null,
    };
    #endregion
}
