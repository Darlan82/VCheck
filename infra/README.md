infra (Terraform)

Propósito: Diretório com código de Infraestrutura como Código (IaC) para provisionar recursos Azure que suportam a aplicação em ambientes NÃO locais (dev cloud, staging, prod). Complementa o provisionamento local feito pelo .NET Aspire.

Visão Geral:
- Em desenvolvimento local usamos o Host Aspire (containers: SQL Server, Keycloak, jobs de migração e API).
- Em nuvem usaremos Terraform para provisionar recursos gerenciados/PAAS que não são simulados pelo Aspire.

Escopo Planejado de Recursos (conforme System Design):
- Azure App Service (API .NET)
- Azure SQL Database (Serverless)
- Azure API Management (gateway + validação de JWT)
- Azure Static Web Apps (futuro frontend SPA + CDN global)
- (Opcional Futuro) Monitoramento / Log Analytics, Application Insights, Key Vault, Storage Accounts

Estrutura (prevista):
- modules/ -> Módulos reutilizáveis (ex.: sql, app_service, apim, static_web_app)
- environments/ -> Arquivos *.tfvars por ambiente (dev, staging, prod)
- scripts/ -> Scripts auxiliares (wrap terraform init/plan/apply, validação format, etc.)
- global/ -> (opcional) state, naming conventions, providers compartilhados

Próximos Passos:
1. Definir backend remoto de state (ex.: Azure Storage) – criar backend.tf.
2. Criar provider Azure (azure.tf) com bloqueio de versões.
3. Implementar naming convention central (locals + prefixos por ambiente).
4. Criar módulo `sql` (server + db) com parâmetros de tier, auto-pause (serverless) e tags.
5. Criar módulo `app_service` (plan + webapp) com slot opcional e configuração de connection strings.
6. Criar módulo `apim` com produtos/policies básicos (incluindo política de validação de JWT – apontar Authority / Audience).
7. Criar módulo `static_web_app` (para SPA futura) – habilitar roles e environment variables necessários.
8. Pipeline CI: terraform fmt/validate/plan (PR) e apply (main/prod) com controle manual.
9. Integrar secrets sensíveis via Key Vault (futuro) e referenciar via `azurerm_key_vault_secret`.
10. Documentar fluxo de promoção: dev -> staging -> prod (mesmos módulos, tfvars distintos).

Conexão com System Design:
- Alinha com decisão de separação: Aspire (local) vs Terraform (cloud).
- Facilita escalabilidade inicial e governança (tags, naming, políticas) sem antecipar microservices.

Observações:
- Não versionar arquivos de state locais (terraform.tfstate) – usar backend remoto.
- Aplicar padrão de tags (ex.: `environment`, `owner`, `cost-center`, `system= vcheck`).
- Validar custos estimados antes do apply em ambientes superiores.
