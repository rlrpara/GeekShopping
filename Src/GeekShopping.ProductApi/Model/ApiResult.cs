namespace GeekShopping.ProductApi.Model;

public class ApiResult<T> where T : class
{
    public Paginacao Paginacao { get; private set; }
    public List<T>? Dados { get; private set; }

    public ApiResult()
    {
        Paginacao = new Paginacao();
    }

    public void AddPaginacao(int paginaAtual, int quantidadePorPagina, int totalPaginas, int totalRegistros, List<T> dados)
    {
        Paginacao.PaginaAtual = paginaAtual;
        Paginacao.QuantidadePorPagina = quantidadePorPagina;
        Paginacao.TotalPagina = totalPaginas;
        Paginacao.TotalRegistros = totalRegistros;
        Dados = dados;
    }
}

public class Paginacao
{
    public int PaginaAtual { get; set; }
    public int QuantidadePorPagina { get; set; }
    public int TotalPagina { get; set; }
    public int TotalRegistros { get; set; }

}
