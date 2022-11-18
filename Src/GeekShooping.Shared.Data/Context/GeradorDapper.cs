using GeekShopping.Shared.Data.Enumerables;
using GeekShopping.Shared.Data.Interface;
using GeekShopping.Shared.Data.ValueObjects;
using GeekShopping.Shared.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace GeekShopping.Shared.Data.Context;

public class GeradorDapper : IGeradorDapper
{
    #region [Propriedades Privadas]
    private ParametrosConexao _parametrosConexao;
    #endregion

    #region [Construtor]
    public GeradorDapper(ParametrosConexao parametrosConexao) => _parametrosConexao = parametrosConexao;
    #endregion

    #region Métodos Privados
    private Nota? ObterAtributoNota(PropertyInfo x) => x.GetCustomAttribute(typeof(Nota)) as Nota;
    private IEnumerable<PropertyInfo> ObterListaPropriedadesClasse<T>(T entidade = null) where T : class
    {
        if (entidade is null)
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault())?.Order);
        else
            return entidade.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => ((ColumnAttribute)p.GetCustomAttributes(typeof(ColumnAttribute)).FirstOrDefault())?.Order);
    }
    private string? TipoPropriedade(PropertyInfo item)
    {
        return item.PropertyType.Name switch
        {
            "Int32" => ObtemParaInteiro(),
            "Int64" => "bigint DEFAULT NULL",
            "Double" => "decimal(18,2)",
            "Single" => "float",
            "DateTime" => "date DEFAULT CURRENT_TIMESTAMP",
            "Boolean" => ObtemParaBoleando(),
            "Nullable`1" => ObtemParaTipoNulo(item.PropertyType.FullName),
            _ => "varchar(255) NULL",
        };
    }
    private string? ObterParaData() => (ETipoBanco)_parametrosConexao.TipoBanco switch
    {
        ETipoBanco.Postgresql => "timestamp DEFAULT CURRENT_TIMESTAMP",
        _ => "datetime DEFAULT CURRENT_TIMESTAMP"
    };
    private string ObtemParaInteiro()
    {
        return (ETipoBanco)_parametrosConexao.TipoBanco switch
        {
            ETipoBanco.SqlServer => "int(11) DEFAULT NULL",
            ETipoBanco.MySql => "int(11) DEFAULT NULL",
            ETipoBanco.Firebird => "int(11) DEFAULT NULL",
            ETipoBanco.Postgresql => "int4 DEFAULT NULL",
            ETipoBanco.SqLite => "INTEGER DEFAULT NULL",
            _ => "int(11) DEFAULT NULL",
        };
    }
    private string? ObtemParaBoleando() => (ETipoBanco)_parametrosConexao.TipoBanco switch
    {
        ETipoBanco.SqlServer => "tinyint(1) NOT NULL DEFAULT 1",
        ETipoBanco.MySql => "tinyint(1) NOT NULL DEFAULT 1",
        ETipoBanco.Firebird => "bit NOT NULL DEFAULT 1",
        ETipoBanco.Postgresql => "bool NOT NULL DEFAULT true",
        ETipoBanco.SqLite => "INTEGER NOT NULL DEFAULT 1",
        _ => "tinyint(1) NOT NULL DEFAULT 1",
    };
    private string ObtemParaTipoNulo(string fullName)
    {
        if (fullName.Contains("Int32"))
        {
            return (ETipoBanco)_parametrosConexao.TipoBanco switch
            {
                ETipoBanco.SqlServer => "int DEFAULT NULL",
                ETipoBanco.MySql => "int DEFAULT NULL",
                ETipoBanco.Firebird => "int DEFAULT NULL",
                ETipoBanco.Postgresql => "int4 DEFAULT NULL",
                ETipoBanco.SqLite => "INTEGER DEFAULT NULL",
                _ => "int DEFAULT NULL",
            };
        }
        else if (fullName.Contains("DateTime"))
            return "date DEFAULT NULL";
        else
        {
            return (ETipoBanco)_parametrosConexao.TipoBanco switch
            {
                ETipoBanco.SqlServer => "varchar(255) NULL",
                ETipoBanco.MySql => "varchar(255) NULL",
                ETipoBanco.Firebird => "varchar(255) NULL",
                ETipoBanco.Postgresql => "varchar(255) NULL",
                ETipoBanco.SqLite => "TEXT NULL",
                _ => "varchar(255) NULL",
            };
        }
    }
    private string FormataValor<T>(PropertyInfo x, T entidade) where T : class
    {
        var propriedade = x.PropertyType.Name.ToLower();

        if (propriedade.Contains("string"))
            return $"'{x.GetValue(entidade)}'";
        else if (propriedade.Contains("datetime"))
            return $"'{Convert.ToDateTime(x.GetValue(entidade)):yyyy-MM-dd HH:mm:ss}'";
        else if (propriedade.Contains("nullable`1"))
            if (x.PropertyType.FullName.ToLower().Contains("datetime"))
                return $"'{Convert.ToDateTime(x.GetValue(entidade)):yyyy-MM-dd HH:mm:ss}'";
            else if (x.PropertyType.FullName.ToLower().Contains("string"))
                return $"'{x.GetValue(entidade)}'";
            else
                return $"{x.GetValue(entidade)}";
        else
            return $"{x.GetValue(entidade)}";
    }
    private string ObterValorInsert<T>(T entidade) where T : class
        => string.Join($", ", ObterListaPropriedadesClasse(entidade).ToList()
                .Where(x => ObterAtributoNota(x).UseSearch && !ObterAtributoNota(x).PrimaryKey && x.GetCustomAttributes().FirstOrDefault() is not KeyAttribute)
                .Select(x => $"{FormataValor(x, entidade)}")
                .ToList());
    private string ObterColunasInsert<T>() where T : class
        => string.Join($", ", ObterListaPropriedadesClasse<T>().ToList()
                .Where(x => ObterAtributoNota(x).UseSearch && !ObterAtributoNota(x).PrimaryKey && x.GetCustomAttributes().FirstOrDefault() is not KeyAttribute)
                .Select(x => $"{x.GetCustomAttribute<ColumnAttribute>()?.Name}")
                .ToList());
    private string ObterUseNomeBanco()
        => _parametrosConexao.TipoBanco.Equals(ETipoBanco.MySql) ? $"USE {_parametrosConexao.NomeBanco};" : "";
    private void ObterConstraintSql<T>(StringBuilder sqlConstraint, Nota opcoesBase, string nomeCampo) where T : class
    {
        string tabelaChaveEstrangeira = $"{opcoesBase.ForeignKey.ToLower()}";
        string nomeChave = $"FK_{ObterNomeTabela<T>()}_{nomeCampo}".ToUpper();

        switch ((ETipoBanco)_parametrosConexao.TipoBanco)
        {
            case ETipoBanco.MySql:
                sqlConstraint.AppendLine($"CALL PROC_DROP_FOREIGN_KEY('{ObterNomeTabela<T>()}', '{nomeChave}');");
                sqlConstraint.AppendLine($"ALTER TABLE {_parametrosConexao.NomeBanco}.{ObterNomeTabela<T>()}");
                sqlConstraint.AppendLine($"ADD CONSTRAINT {nomeChave} FOREIGN KEY ({nomeCampo})");
                sqlConstraint.AppendLine($"REFERENCES {_parametrosConexao.NomeBanco}.{tabelaChaveEstrangeira} (ID) ON DELETE NO ACTION ON UPDATE NO ACTION;{Environment.NewLine}");
                break;
            case ETipoBanco.SqlServer:
                break;
            case ETipoBanco.Firebird:
                break;
            case ETipoBanco.Postgresql:
                sqlConstraint.AppendLine($"ALTER TABLE {ObterNomeTabela<T>()} DROP CONSTRAINT IF EXISTS {nomeChave};");
                sqlConstraint.AppendLine($"ALTER TABLE {ObterNomeTabela<T>()} ADD CONSTRAINT {nomeChave} FOREIGN KEY ({nomeCampo})");
                sqlConstraint.AppendLine($"REFERENCES {tabelaChaveEstrangeira} (ID);{Environment.NewLine}");
                break;
            case ETipoBanco.SqLite:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Métodos Públicos
    public string? ObterChavePrimaria<T>() where T : class => ObterListaPropriedadesClasse<T>()
        .Where(x => ObterAtributoNota(x) is not null && (ObterAtributoNota(x).PrimaryKey || x.GetCustomAttributes().FirstOrDefault() is KeyAttribute))
        .Select(x => x.GetCustomAttribute<ColumnAttribute>()?.Name ?? "")
        .FirstOrDefault();
    public string? ObterNomeTabela<T>() where T : class
    {
        dynamic nomeTabela = typeof(T).GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute");

        return nomeTabela?.Name;
    }
    public string ObterColunasSelect<T>(bool paraGrid = false, T? entidade = null, bool quebraLinha = true) where T : class
    {
        if (!paraGrid && entidade is null)
            return string.Join($", {(quebraLinha ? Environment.NewLine : "")}       ", ObterListaPropriedadesClasse<T>()
                .Where(x => ObterAtributoNota(x)?.UseSearch ?? false && (x.GetCustomAttribute(typeof(Nota)) is not null))
                .Select(x => $"{x.GetCustomAttribute<ColumnAttribute>()?.Name?.Trim()} AS {x.Name}")
                .ToList());
        else
            return string.Join($", {Environment.NewLine}       ", ObterListaPropriedadesClasse(entidade)
                .Where(x => ObterAtributoNota(x).UseSearch && !string.IsNullOrWhiteSpace(x.GetCustomAttribute<ColumnAttribute>().Name) && !string.IsNullOrWhiteSpace(x.GetValue(entidade)?.ToString()) && (!ObterAtributoNota(x).PrimaryKey && x.GetCustomAttributes().FirstOrDefault() is not KeyAttribute))
                .Select(x => $"{x.GetCustomAttribute<ColumnAttribute>()?.Name?.Trim()} = {FormataValor(x, entidade)}")
                .ToList());
    }
    public string? RetornaCamposSelect<T>() where T : class
        => string.Join($", {Environment.NewLine}       ", ObterListaPropriedadesClasse<T>()
            .Where(x => ObterAtributoNota(x)?.UseSearch ?? false && ObterAtributoNota(x) is not null)
            .Select(x => $"{x.GetCustomAttribute<ColumnAttribute>()?.Name?.Trim() ?? ""} AS {x.Name}")
            .ToList())?.Trim();
    public string? ObterDelete<T>(int id) where T : class
    {
        var sqlDelete = new StringBuilder();

        sqlDelete.AppendLine(ObterUseNomeBanco());
        sqlDelete.AppendLine($"DELETE FROM {ObterNomeTabela<T>()}");
        sqlDelete.AppendLine($" WHERE {ObterChavePrimaria<T>()} = {id}");

        return sqlDelete?.ToString()?.Trim();
    }
    public string? CriarTabela<T>() where T : class
    {
        List<string> campos = new();
        StringBuilder sqlConstraint = new();
        StringBuilder sqlIndice = new();

        foreach (PropertyInfo item in ObterListaPropriedadesClasse<T>())
        {
            var opcoesBase = ObterAtributoNota(item);

            if (opcoesBase is not null && opcoesBase.UseInDatabase)
            {
                var nomeCampo = item.GetCustomAttribute<ColumnAttribute>().Name;

                if (!string.IsNullOrWhiteSpace(nomeCampo) && !ObterAtributoNota(item).PrimaryKey && item.GetCustomAttributes().FirstOrDefault() is not KeyAttribute)
                    campos.Add($"{nomeCampo} {TipoPropriedade(item)}");

                if (!string.IsNullOrEmpty(opcoesBase.ForeignKey))
                    ObterConstraintSql<T>(sqlConstraint, opcoesBase, nomeCampo);
            }
        }

        var sqlPesquisa = new StringBuilder();

        switch ((ETipoBanco)_parametrosConexao.TipoBanco)
        {
            case ETipoBanco.MySql:
                sqlPesquisa.AppendLine($"USE {_parametrosConexao.NomeBanco};");
                sqlPesquisa.AppendLine($"CREATE TABLE IF NOT EXISTS {_parametrosConexao.NomeBanco}.{ObterNomeTabela<T>()} (");
                sqlPesquisa.AppendLine($"  {ObterChavePrimaria<T>()} int(11) NOT NULL AUTO_INCREMENT,");
                sqlPesquisa.AppendLine($"  {string.Join($",{Environment.NewLine}   ", campos.ToArray())},");
                sqlPesquisa.AppendLine($"  PRIMARY KEY ({ObterChavePrimaria<T>()})");
                sqlPesquisa.AppendLine($")");
                sqlPesquisa.AppendLine($"ENGINE = INNODB,");
                sqlPesquisa.AppendLine($"CHARACTER SET utf8,");
                sqlPesquisa.AppendLine($"COLLATE utf8_general_ci;{Environment.NewLine}");
                if (!string.IsNullOrEmpty(sqlConstraint.ToString()))
                    sqlPesquisa.AppendLine(sqlConstraint.ToString());
                break;

            case ETipoBanco.SqlServer:
                sqlPesquisa.AppendLine($"USE {_parametrosConexao.NomeBanco};");
                sqlPesquisa.AppendLine($"IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{ObterNomeTabela<T>()}')");
                sqlPesquisa.AppendLine($"BEGIN");
                sqlPesquisa.AppendLine($"   CREATE TABLE {ObterNomeTabela<T>()}(");
                sqlPesquisa.AppendLine($"        {ObterChavePrimaria<T>()} int IDENTITY(1,1) NOT NULL,");
                sqlPesquisa.AppendLine($"        {string.Join($",{Environment.NewLine}   ", campos.ToArray())},");
                sqlPesquisa.AppendLine($"  CONSTRAINT [PK_{ObterNomeTabela<T>()}] PRIMARY KEY CLUSTERED ");
                sqlPesquisa.AppendLine($"(");
                sqlPesquisa.AppendLine($"   {ObterChavePrimaria<T>()} ASC");
                sqlPesquisa.AppendLine($")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
                sqlPesquisa.AppendLine($") ON [PRIMARY]");
                sqlPesquisa.AppendLine($"END");
                break;

            case ETipoBanco.Firebird:
                sqlPesquisa.AppendLine($"");
                break;

            case ETipoBanco.Postgresql:
                sqlPesquisa.AppendLine($"CREATE TABLE IF NOT EXISTS {ObterNomeTabela<T>()} (");
                sqlPesquisa.AppendLine($"  {ObterChavePrimaria<T>()} int4 NOT NULL GENERATED ALWAYS AS IDENTITY,");
                sqlPesquisa.AppendLine($"  {string.Join($",{Environment.NewLine}   ", campos.ToArray())},");
                sqlPesquisa.AppendLine($"  PRIMARY KEY ({ObterChavePrimaria<T>()})");
                sqlPesquisa.AppendLine($");");
                if (!string.IsNullOrEmpty(sqlConstraint.ToString()))
                    sqlPesquisa.AppendLine(sqlConstraint.ToString());
                sqlPesquisa.AppendLine(sqlIndice.ToString());
                break;

            case ETipoBanco.SqLite:
                sqlPesquisa.AppendLine($"CREATE TABLE IF NOT EXISTS {ObterNomeTabela<T>()} (");
                sqlPesquisa.AppendLine($"  {ObterChavePrimaria<T>()} INTEGER PRIMARY KEY AUTOINCREMENT,");
                sqlPesquisa.AppendLine($"  {string.Join($",{Environment.NewLine}   ", campos.ToArray())}");
                sqlPesquisa.AppendLine($")");
                break;

            default:
                sqlPesquisa.AppendLine($"");
                break;
        }

        return sqlPesquisa.ToString().Trim();
    }
    public string? GeralSqlSelecaoControles<T>(string? sqlWhere) where T : class
    {
        var sqlPesquisa = new StringBuilder();

        sqlPesquisa.AppendLine($"SELECT {ObterColunasSelect<T>()}");
        sqlPesquisa.AppendLine($"  FROM {ObterNomeTabela<T>()}");
        sqlPesquisa.AppendLine($"{(string.IsNullOrWhiteSpace(sqlWhere?.Trim()) ? string.Empty : $"WHERE {sqlWhere}")}");

        return sqlPesquisa?.ToString()?.Trim();
    }
    public string? GeralSqlUpdateControles<T>(int id, T entidade) where T : class
    {
        var sqlPesquisa = new StringBuilder();

        sqlPesquisa.AppendLine(ObterUseNomeBanco());
        sqlPesquisa.AppendLine($"UPDATE {ObterNomeTabela<T>()}");
        sqlPesquisa.AppendLine($"   SET {string.Join($",{Environment.NewLine}       ", ObterColunasSelect(true, entidade))}");
        sqlPesquisa.AppendLine($" WHERE {ObterChavePrimaria<T>()} = {id}");

        return sqlPesquisa?.ToString()?.Trim();
    }
    public string? GeralSqlInsertControles<T>(T entidade) where T : class
    {
        var sqlPesquisa = new StringBuilder();

        sqlPesquisa.AppendLine(ObterUseNomeBanco());
        sqlPesquisa.AppendLine($"INSERT INTO {ObterNomeTabela<T>()} ({ObterColunasInsert<T>()})");
        sqlPesquisa.AppendLine($"                     VALUES ({string.Join($", ", ObterValorInsert(entidade))})");

        return sqlPesquisa?.ToString()?.Trim();
    }

    #endregion
}
