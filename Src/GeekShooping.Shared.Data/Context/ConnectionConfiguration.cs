using GeekShopping.Shared.Data.ValueObjects;
using System.Data;

namespace GeekShopping.Shared.Data.Context;

public class ConnectionConfiguration
{
    #region [Métodos Privados]
    private static IDbConnection? Inicia(IDbConnection? conexao)
    {
        if (conexao != null)
        {
            if (conexao.State == ConnectionState.Open) conexao.Close();
            if (conexao.State == ConnectionState.Closed) conexao.Open();
        }
        return conexao;
    }
    #endregion

    #region Métodos Públicos
    public static IDbConnection? AbrirConexao(ParametrosConexao parametrosConexao) => Inicia(new DeafultSqlConnectionFactory(parametrosConexao).Conexao());
    #endregion
}
