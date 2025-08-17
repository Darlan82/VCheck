# Infraestrutura

## 1. Ambiente de desenvolvimento

Este projeto é um Host do .Net Aspire. \
Ele provisiona e orquestra toda infra estrutura localmente. \
Para rodar é necessário estar com o docker e estar rodando.

### 1.1 Banco de dados
O Host do Aspire provisionará uma imagem do `SQL Server` via `Docker` na inicialização do Host.

#### 1.1.1 Host fixo (padrão)
Para acessar o banco atravez do `SQL Server Management Studio` você deve conectar com usuário `sa` 
a senha está fixada `S3nh@F0rte123!` e porta `5433`. \
Nessa caso o endereço fica `tcp:127.0.0.1,5433`.

#### 1.1.2 Host dinâmico
É possivel mudar para porta senha dinâmica. Caso queira a senha vc pode obter atravez do painel do Aspire 
na env `MSSQL_SA_PASSWORD` ou via linha de comando `dotnet user-secrets list` na saída `Parameters:sqlserver-password`. \
O endereço do banco será levantado em `tcp:127.0.0.1,porta` e a porta é criada dinamicamente. \
A porta vc pode pegar no painel do Aspire ou via comando `docker ps --filter name=sql` 
na saída da coluna `PORTS` da tabela exemplo: 

| CONTAINER ID		| IMAGE											| COMMAND					| CREATED			| STATUS			| PORTS							| NAMES					| 
| ---				| --											| --						| --				| --				| --							| --					|
| aa02d5bc8dcf		| mcr.microsoft.com/mssql/server:2022-latest	| "/opt/mssql/bin/perm…"	| 51 minutes ago	| Up 51 minutes		| 127.0.0.1:53028->1433/tcp		| sqlserver-ygcdhgvf	|

Nessa caso o endereço fica `tcp:127.0.0.1,53028`.

#### 1.1.3 Criação de Base de dados e população dos dados

O Aspire executa uma tarefa de migração que está no projeto `MercadoD.Infra.Persistence.Sql.JobMigration` automaticamente. \
A migração cria as tabelas e somente popula os dados para o desenvolvedor no ambiente de desenvolvimento e
sua definição está na classe `DbDevInitializer` no projeto `MercadoD.Infra.Persistence.Sql`.

Para saber como trabalhar com a migração do Banco de Dados veja o documento [MercadoD.Infra.Persistence.Sql](../MercadoD.Infra.Persistence.Sql/README.md).

