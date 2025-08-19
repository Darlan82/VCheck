# Guia de Execu��o (Getting Started)

Este guia foca em **como rodar rapidamente** o ambiente local. Para detalhes aprofundados de infraestrutura (provisionamento de banco, Keycloak, migra��es, etc.) consulte: [Infraestrutura / Host Aspire](../src/Host/README.md).

## 1. Pr�-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- Docker Desktop (ou engine compat�vel) em execu��o

## 2. Clonar o reposit�rio
```bash
git clone <url-do-repositorio>
cd VCheck
```

## 3. Subir todo o ambiente local (API + SQL Server + Keycloak + Jobs de Migra��o)
Execute a aplica��o Host (Aspire):
```bash
dotnet run --project src/Host/VCheck.Host.csproj
```
O painel do Aspire abrir� no navegador listando os recursos (SQL, Keycloak, API e jobs de migra��o).

## 4. Acessar a API (Swagger)
Abra o endpoint Swagger da API (exemplo � ver porta real no painel Aspire):
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
  - `user.comum` / `123` (Perfil comum sem permiss�es de checklist)

Ap�s autorizar as requisi��es incluir�o automaticamente o token Bearer.

## 6. Fluxos B�sicos Dispon�veis
(Endpoints podem evoluir; consulte o Swagger para a vers�o atual.)
- Criar ve�culo (ex.: prepara��o para iniciar checklist)
- Iniciar checklist (role: executor)
- Atualizar item do checklist (role: executor)
- Submeter checklist para aprova��o (role: executor)
- Aprovar checklist (role: supervisor)

## 7. Reset / Limpeza
Para reiniciar o ambiente removendo dados persistidos:
1. Pare a execu��o (`Ctrl+C`).
2. Remova volumes (opcional) via Docker Desktop ou `docker volume ls` / `docker volume rm` conforme necessidade.
3. Rode novamente o Host.

## 8. Documenta��o Complementar
- Requisitos de Neg�cio: [docs/core_business.md](core_business.md)
- System Design: [docs/system_design.md](system_design.md)
- Monolito Modular (guia): [docs/modular-monolith/README.md](modular-monolith/README.md)
- Infraestrutura detalhada (Banco, Keycloak, Migra��es): [src/Host/README.md](../src/Host/README.md)

## 9. Pr�ximos Passos (Evolu��o)
- Adicionar testes automatizados para fluxos cr�ticos
- Endpoint de consulta/aprova��o avan�ada
- Harden de seguran�a (TLS, fluxo Authorization Code + PKCE em vez de password flow)

---
Este guia cobre apenas a execu��o local r�pida. Para compreender decis�es arquiteturais consulte o System Design.
