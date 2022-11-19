### Criar na pasta base do ProductApi o arquivo .env
Deve conter os seguintes itens:

SERVIDOR= ip do servidor do banco de dados.<br />
PORTA= numero da porta do banco.<br />
BANCO= nome do banco que o sistema criará automáricamente na primeira execução.<br />
USUARIO= usuario com privilégios de db admin.<br />
SENHA= senha de acesso ao banco.<br />
TIPO_BANCO= numero do tipo de banco que o sistema pode criar. Tabela abaixo:<br />

### Tabela
SqlServer = 1,<br />
MySql = 2,<br />
Firebird = 3,<br />
Postgresql = 4,<br />
SqLite = 6,<br />
Odbc = 8<br />
