### Criar na pasta base do ProductApi o arquivo .env
Deve conter os seguintes itens:

SERVIDOR= ip do servidor do banco de dados.
PORTA= numero da porta do banco.
BANCO= nome do banco que o sistema criará automáricamente na primeira execução.
USUARIO= usuario com privilégios de db admin.
SENHA= senha de acesso ao banco.
TIPO_BANCO= numero do tipo de banco que o sistema pode criar. Tabela abaixo:

### Tabela
SqlServer = 1,
MySql = 2,
Firebird = 3,
Postgresql = 4,
SqLite = 6,
Odbc = 8
