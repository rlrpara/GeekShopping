namespace GeekShopping.Shared.Domain.Entities.filtro
{
    public class filtroProduct : filtroPaginacao
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
    }
}
