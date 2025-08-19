# Código-Fonte (.NET)

Este diretório contém todas as soluções e projetos .NET que compõem o backend modular do VCheck.

## Organização
- API: `API/` -> Projeto ASP.NET Core que expõe os endpoints públicos.
- Host (Aspire): `Host/` -> Orquestração local (SQL Server, Keycloak, jobs de migração, API).
- Módulos de Domínio:
  - Frota: `Modules/Fleet/` (lógica e migração dedicadas).
  - Checklists: `Modules/Checklists/` (lógica e migração dedicadas).
- Shared Kernel: `SharedKernel/` -> Tipos compartilhados mínimos (interfaces, contratos, cross-cutting simples).

## Conceito de Monólito Modular
A solução é um único processo (monólito) que internalmente separa domínios em módulos com:
- DbContext próprio e migrações isoladas.
- Casos de uso expostos via interfaces de módulo (`IFleetModule`, `IChecklistsModule`).
- Regras de negócio encapsuladas e não acessadas diretamente por outros módulos.

Essa abordagem permite futura extração para microserviços se gatilhos (performance, cadence de deploy, complexidade) ocorrerem.

## Projetos
| Projeto | Tipo | Descrição |
|--------|------|-----------|
| VCheck.Api | ASP.NET Core API | Endpoints HTTP, autenticação JWT (Keycloak), composição dos módulos. |
| VCheck.Host | Aspire Host | Orquestração local (containers + dependências). |
| VCheck.Modules.Fleet | Class Library | Regras e persistência do domínio de Frota. |
| VCheck.Modules.Fleet.MigrationService | Worker Service | Aplica migrações do módulo de Frota. |
| VCheck.Modules.Checklists | Class Library | Regras e persistência do domínio de Checklists. |
| VCheck.Modules.Checklists.MigrationService | Worker Service | Aplica migrações do módulo de Checklists. |
| VCheck.SharedKernel | Class Library | Contratos e utilidades compartilhadas mínimos. |

## Fluxo de Inicialização Local (resumo)
1. Aspire Host sobe containers (SQL, Keycloak).
2. Services de migração rodam e atualizam os bancos de cada módulo.
3. API inicia após migrações concluídas.
4. Swagger disponível para testes (autenticação via Keycloak). Consulte [../docs/getting-started.md](../docs/getting-started.md).

## Autenticação Local
- Realm importado: `transport` (Keycloak)
- Usuários: `user.executor`, `user.supervisor`, `user.comum` (senha `123`)
- Roles: `executor`, `supervisor`

## Convenções de Código
- C# 12 / .NET 8
- FluentValidation para validações de comandos.
- Lógica de caso de uso em classes específicas (`UseCases/<Feature>`).
- Controllers finos, delegando a interfaces de módulo.

## Próximos Passos Técnicos
- Adicionar testes unitários e de integração.
- Introduzir logging estruturado (Serilog) e correlação.
- Implementar versionamento de API (v1, v2...).
- Harden de segurança (policy-based authorization refinada, claims extras).

---
Documentação de negócio e arquitetura: ver [/docs](../docs).
