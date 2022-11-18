using GeekShopping.Shared.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Shared.Domain.Entities
{
    [Table(name: "PRODUCT")]
    public class Product : BaseEntity
    {
        [Nota(Lenght = 150)]
        [Required]
        [Column(name: "NAME", Order = 2)]
        public string? Name { get; set; }

        [Nota]
        [Required]
        [Column(name: "PRICE", Order = 3)]
        public decimal Price { get; set; }

        [Nota(Lenght = 500)]
        [Column(name: "DESCRIPTION", Order = 4)]
        public string? Description { get; set; }

        [Nota(Lenght = 50)]
        [Column(name: "CATEGORY_NAME", Order = 5)]
        public string? CategoryName { get; set; }

        [Nota(Lenght = 300)]
        [Column(name: "IMAGE_URL", Order = 6)]
        public string? ImageUrl { get; set; }
    }
}
