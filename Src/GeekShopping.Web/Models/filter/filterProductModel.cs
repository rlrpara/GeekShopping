namespace GeekShopping.Web.Models.filter
{
    public class filterProductModel : filterPaginationModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
    }
}
