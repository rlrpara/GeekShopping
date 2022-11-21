namespace GeekShopping.Web.Models;
public class ApiResult<T>
{
    public Paginacao paginacao { get; set; }
    public IEnumerable<T> dados { get; set; }
}

public class Paginacao
{
    public int paginaAtual { get; set; }
    public int quantidadePorPagina { get; set; }
    public int totalPagina { get; set; }
    public int totalRegistros { get; set; }
}

