infra (Terraform)

Prop�sito: Diret�rio com c�digo de Infraestrutura como C�digo (IaC) para provisionar recursos Azure que suportam a aplica��o em ambientes N�O locais (dev cloud, staging, prod). Complementa o provisionamento local feito pelo .NET Aspire.

Vis�o Geral:
- Em desenvolvimento local usamos o Host Aspire (containers: SQL Server, Keycloak, jobs de migra��o e API).
- Em nuvem usaremos Terraform para provisionar recursos gerenciados/PAAS que n�o s�o simulados pelo Aspire.

Escopo Planejado de Recursos (conforme System Design):
- Azure App Service (API .NET)
- Azure SQL Database (Serverless)
- Azure API Management (gateway + valida��o de JWT)
- Azure Static Web Apps (futuro frontend SPA + CDN global)
- (Opcional Futuro) Monitoramento / Log Analytics, Application Insights, Key Vault, Storage Accounts

Estrutura (prevista):
- modules/ -> M�dulos reutiliz�veis (ex.: sql, app_service, apim, static_web_app)
- environments/ -> Arquivos *.tfvars por ambiente (dev, staging, prod)
- scripts/ -> Scripts auxiliares (wrap terraform init/plan/apply, valida��o format, etc.)
- global/ -> (opcional) state, naming conventions, providers compartilhados

Pr�ximos Passos:
1. Definir backend remoto de state (ex.: Azure Storage) � criar backend.tf.
2. Criar provider Azure (azure.tf) com bloqueio de vers�es.
3. Implementar naming convention central (locals + prefixos por ambiente).
4. Criar m�dulo `sql` (server + db) com par�metros de tier, auto-pause (serverless) e tags.
5. Criar m�dulo `app_service` (plan + webapp) com slot opcional e configura��o de connection strings.
6. Criar m�dulo `apim` com produtos/policies b�sicos (incluindo pol�tica de valida��o de JWT � apontar Authority / Audience).
7. Criar m�dulo `static_web_app` (para SPA futura) � habilitar roles e environment variables necess�rios.
8. Pipeline CI: terraform fmt/validate/plan (PR) e apply (main/prod) com controle manual.
9. Integrar secrets sens�veis via Key Vault (futuro) e referenciar via `azurerm_key_vault_secret`.
10. Documentar fluxo de promo��o: dev -> staging -> prod (mesmos m�dulos, tfvars distintos).

Conex�o com System Design:
- Alinha com decis�o de separa��o: Aspire (local) vs Terraform (cloud).
- Facilita escalabilidade inicial e governan�a (tags, naming, pol�ticas) sem antecipar microservices.

Observa��es:
- N�o versionar arquivos de state locais (terraform.tfstate) � usar backend remoto.
- Aplicar padr�o de tags (ex.: `environment`, `owner`, `cost-center`, `system= vcheck`).
- Validar custos estimados antes do apply em ambientes superiores.
