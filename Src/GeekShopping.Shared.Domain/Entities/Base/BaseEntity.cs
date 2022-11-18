using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Shared.Domain.Entities.Base;

public class BaseEntity
{
    [Nota(PrimaryKey = true, Index = true)]
    [Column(name: "ID", Order = 1)]
    public long Codigo { get; set; }

    [Nota]
    [Column(name: "DATA_CADASTRO", Order = 199)]
    public DateTime? DataCadastro { get; set; }

    [Nota]
    [Column(name: "DATA_ATUALIZACAO", Order = 200)]
    public DateTime? DataAtualizacao { get; set; }
}
