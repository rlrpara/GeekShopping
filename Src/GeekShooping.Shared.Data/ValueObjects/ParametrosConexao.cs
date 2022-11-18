using GeekShopping.Shared.Data.Enumerables;

namespace GeekShopping.Shared.Data.ValueObjects;

public class ParametrosConexao
{
    public string? Servidor { get; set; } = "";
    public string? Porta { get; set; } = "";
    public string? NomeBanco { get; set; } = "";
    public string? Usuario { get; set; } = "";
    public string? Senha { get; set; } = "";
    public int TipoBanco { get; set; } = 0;
}
