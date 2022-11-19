namespace GeekShoping.Shared.Services.ViewModel.filtro;

public class filtroProductViewModel : filtroPaginacaoViewModel
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
}
