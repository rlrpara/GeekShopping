namespace GeekShopping.Web.Models;

public class ProductModel
{
    public long Codigo { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public bool? Ativo { get; set; }
    public DateTime? DataCadastro { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
