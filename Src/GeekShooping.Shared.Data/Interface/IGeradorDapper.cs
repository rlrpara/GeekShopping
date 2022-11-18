namespace GeekShopping.Shared.Data.Interface;

public interface IGeradorDapper
{
    string? ObterChavePrimaria<T>() where T : class;
    string? ObterNomeTabela<T>() where T : class;
    string ObterColunasSelect<T>(bool paraGrid = false, T? entidade = null, bool quebraLinha = true) where T : class;
    string? RetornaCamposSelect<T>() where T : class;
    string? ObterDelete<T>(int id) where T : class;
    string? CriarTabela<T>() where T : class;
    string? GeralSqlSelecaoControles<T>(string? sqlWhere) where T : class;
    string? GeralSqlUpdateControles<T>(int id, T entidade) where T : class;
    public string? GeralSqlInsertControles<T>(T entidade) where T : class;
}
