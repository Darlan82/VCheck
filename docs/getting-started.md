# Guia de Execução (Getting Started)

Este guia foca em **como rodar rapidamente** o ambiente local. Para detalhes aprofundados de infraestrutura (provisionamento de banco, Keycloak, migrações, etc.) consulte: [Infraestrutura / Host Aspire](../src/Host/README.md).

## 1. Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Docker Desktop (ou engine compatível) em execução

## 2. Clonar o repositório
```bash
git clone <url-do-repositorio>
cd VCheck
```

## 3. Subir todo o ambiente local (API + SQL Server + Keycloak + Jobs de Migração)
Execute a aplicação Host (Aspire):
```bash
dotnet run --project src/Host/VCheck.Host.csproj
```
O painel do Aspire abrirá no navegador listando os recursos (SQL, Keycloak, API e jobs de migração).

## 4. Acessar a API (Swagger)
Abra o endpoint Swagger da API (exemplo – ver porta real no painel Aspire):
```
https://localhost:<porta_api>/swagger
```

## 5. Autenticar no Swagger (Keycloak)
Clique em "Authorize" e use:
- client_id: `vcheck-api`
- client_secret: `S3cr3t`
- username / password (escolha):
  - `user.executor` / `123` (Perfil Executor)
  - `user.supervisor` / `123` (Perfil Supervisor)
  - `user.comum` / `123` (Perfil comum sem permissões de checklist)

Após autorizar as requisições incluirão automaticamente o token Bearer.

## 6. Fluxos Básicos Disponíveis
(Endpoints podem evoluir; consulte o Swagger para a versão atual.)
- Criar veículo (ex.: preparação para iniciar checklist)
- Iniciar checklist (role: executor)
- Atualizar item do checklist (role: executor)
- Submeter checklist para aprovação (role: executor)
- Aprovar checklist (role: supervisor)

## 7. Reset / Limpeza
Para reiniciar o ambiente removendo dados persistidos:
1. Pare a execução (`Ctrl+C`).
2. Remova volumes (opcional) via Docker Desktop ou `docker volume ls` / `docker volume rm` conforme necessidade.
3. Rode novamente o Host.

## 8. Documentação Complementar
- Requisitos de Negócio: [docs/core_business.md](core_business.md)
- System Design: [docs/system_design.md](system_design.md)
- Monolito Modular (guia): [docs/modular-monolith/README.md](modular-monolith/README.md)
- Infraestrutura detalhada (Banco, Keycloak, Migrações): [src/Host/README.md](../src/Host/README.md)

## 9. Próximos Passos (Evolução)
- Adicionar testes automatizados para fluxos críticos
- Endpoint de consulta/aprovação avançada
- Harden de segurança (TLS, fluxo Authorization Code + PKCE em vez de password flow)

---
Este guia cobre apenas a execução local rápida. Para compreender decisões arquiteturais consulte o System Design.
