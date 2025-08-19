# Infraestrutura

## 1. Ambiente de desenvolvimento

Este projeto é um Host do .Net Aspire. \
Ele provisiona e orquestra toda infra estrutura localmente. \
Para rodar é necessário estar com o docker rodando.

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

### 1.2 Autenticação e Autorização (Keycloak)

A solução de identidade segue o System Design (uso de IdP externo) e os requisitos de negócio de controlar perfis Executor e Supervisor.

#### 1.2.1 Orquestração pelo Host Aspire
No `Program.cs` do Host:
```csharp
var keycloak = builder.AddKeycloak("keycloak", adminUsername: username, adminPassword: password)
    .WithDataVolume()
    .WithRealmImport("./KeycloackConfiguration/transport-realm.json");
```
Isso executa um container Keycloak, persiste dados (volume) e importa automaticamente o realm `transport` a partir do arquivo JSON.
A API (`vcheck-api`) declara dependência com `.WithReference(keycloak).WaitFor(keycloak)` garantindo que Keycloak esteja pronto antes de subir a API.

#### 1.2.2 Realm importado: `transport`
Arquivo: `src/Host/KeycloackConfiguration/transport-realm.json`.
Contém:
- Roles de realm:
  - `executor` (executa e preenche checklists)
  - `supervisor` (aprova ou reprova checklists)
- Usuários iniciais (senha `123`):
  - `user.executor` -> role `executor`
  - `user.supervisor` -> role `supervisor`
  - `user.comum` -> role padrão (`default-roles-transport`) sem permissões específicas de checklist
- Client confidencial `vcheck-api` (publicClient=false) com:
  - `secret`: `S3cr3t`
  - `directAccessGrantsEnabled`: true (habilita Resource Owner Password / password flow)
  - Mappers:
    - Audience para incluir `vcheck-api` no access token
    - Realm roles em claim `roles` (multivalued)

Esses elementos suportam:
- RF-01..RF-06 (executor inicia/preenche; supervisor aprova) através de segregação por roles
- RNF Concorrência / Auditabilidade ao permitir tokens JWT com claims de usuário e roles

#### 1.2.3 Configuração da API (JWT)
Em `src/API/Program.cs`:
```csharp
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer(serviceName: "keycloak", realm: "transport", options =>
    {
        options.Audience = "vcheck-api";
        options.Authority = "http://localhost:8080/realms/transport"; // ajustado dinamicamente via configuração Aspire
        options.TokenValidationParameters.RoleClaimType = "roles"; // claim usada para [Authorize(Roles="...")]
        if (builder.Environment.IsDevelopment()) options.RequireHttpsMetadata = false;
    });
```
O Swagger foi configurado com fluxo OAuth2 Password apontando para os endpoints do realm.

#### 1.2.4 Autenticação via Swagger UI
Passos:
1. Suba o Host: `dotnet run` em `VCheck.Host` (Aspire inicia Keycloak e API).
2. Abra o Swagger da API (ex.: https://localhost:<porta>/swagger).
3. Clique em "Authorize".
4. Preencha:
   - client_id: `vcheck-api`
   - client_secret: `S3cr3t`
   - username: `user.executor` (ou `user.supervisor`)
   - password: `123`
   - scopes: selecione `openid profile email` se desejar.
5. Após autorizar, o botão mostrará o lock fechado e as requisições incluirão o Bearer token.

Use `user.executor` para fluxos de execução de checklist e `user.supervisor` para endpoints futuros de aprovação.

#### 1.2.5 Obtenção de token via linha de comando (opcional)
```bash
curl -X POST \
  http://localhost:8080/realms/transport/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=vcheck-api" \
  -d "client_secret=S3cr3t" \
  -d "username=user.executor" \
  -d "password=123"
```
Resposta conterá `access_token` (JWT) usado no header: `Authorization: Bearer <token>`.

#### 1.2.6 Evolução
- Podem ser adicionadas novas roles (ex.: auditor) estendendo o mesmo processo de realm import.
- Para produção recomenda-se TLS, rotação do client secret e desabilitar password flow substituindo por Authorization Code + PKCE conforme estratégia de frontend.

Isso alinha a implementação ao System Design (IdP externo, segurança gerenciada) e aos requisitos de negócio de perfis distintos Executor/Supervisor.



