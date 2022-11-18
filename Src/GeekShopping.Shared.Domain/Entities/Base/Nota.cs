namespace GeekShopping.Shared.Domain.Entities.Base;

public class Nota : Attribute
{
    public bool PrimaryKey { get; set; } = false;
    public bool UseInDatabase { get; set; } = true;
    public bool UseSearch { get; set; } = true;
    public string ForeignKey { get; set; } = "";
    public int Lenght { get; set; } = 255;
    public bool Index { get; set; } = false;
}
