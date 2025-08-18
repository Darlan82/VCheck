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

#### 1.1.3 Migração de Banco de Dados

A manutenção das migrações de cada módulo é facilitada por scripts específicos:

- Para o módulo de Frota: utilize o script `AddMigrationFleetModule.ps1`.
- Para o módulo de Checklists: utilize o script `AddMigrationChecklistsModule.ps1`.

Esses scripts automatizam o processo de criação de novas migrações para cada contexto (DbContext) e garantem padronização na nomenclatura (timestamp) e uso do projeto de inicialização correto.

##### 1.1.3.1 Execução dos Jobs de Migração

Cada módulo possui um job de migração dedicado (Worker Service) responsável por aplicar as migrações ao iniciar:

- `VCheck.Modules.Fleet.MigrationService.csproj`
- `VCheck.Modules.Checklists.MigrationService.csproj`

A orquestração local desses jobs é realizada pelo Host Aspire (`VCheck.Host.csproj`). Ao iniciar o Host (`dotnet run` no projeto Host), 
ele provisiona os recursos necessários (SQL Server, Keycloak, etc.) e executa automaticamente os jobs de migração para garantir que o banco de 
dados esteja atualizado conforme os modelos de cada módulo antes de subir a API principal.

Fluxo simplificado de inicialização:
1. Aspire inicia e provisiona o container do SQL Server.
2. Jobs de migração dos módulos são executados e aplicam pendências (se existirem).
3. A API principal somente inicia após a conclusão dos jobs de migração (cadeia de dependências configurada no Host).



