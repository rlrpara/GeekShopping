using System.Data;

namespace GeekShopping.Shared.Data.Interface;

public interface IConnectionFactory
{
    IDbConnection Conexao();
}
