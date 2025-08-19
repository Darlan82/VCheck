# C�digo-Fonte (.NET)

Este diret�rio cont�m todas as solu��es e projetos .NET que comp�em o backend modular do VCheck.

## Organiza��o
- API: `API/` -> Projeto ASP.NET Core que exp�e os endpoints p�blicos.
- Host (Aspire): `Host/` -> Orquestra��o local (SQL Server, Keycloak, jobs de migra��o, API).
- M�dulos de Dom�nio:
  - Frota: `Modules/Fleet/` (l�gica e migra��o dedicadas).
  - Checklists: `Modules/Checklists/` (l�gica e migra��o dedicadas).
- Shared Kernel: `SharedKernel/` -> Tipos compartilhados m�nimos (interfaces, contratos, cross-cutting simples).

## Conceito de Mon�lito Modular
A solu��o � um �nico processo (mon�lito) que internalmente separa dom�nios em m�dulos com:
- DbContext pr�prio e migra��es isoladas.
- Casos de uso expostos via interfaces de m�dulo (`IFleetModule`, `IChecklistsModule`).
- Regras de neg�cio encapsuladas e n�o acessadas diretamente por outros m�dulos.

Essa abordagem permite futura extra��o para microservi�os se gatilhos (performance, cadence de deploy, complexidade) ocorrerem.

## Projetos
| Projeto | Tipo | Descri��o |
|--------|------|-----------|
| VCheck.Api | ASP.NET Core API | Endpoints HTTP, autentica��o JWT (Keycloak), composi��o dos m�dulos. |
| VCheck.Host | Aspire Host | Orquestra��o local (containers + depend�ncias). |
| VCheck.Modules.Fleet | Class Library | Regras e persist�ncia do dom�nio de Frota. |
| VCheck.Modules.Fleet.MigrationService | Worker Service | Aplica migra��es do m�dulo de Frota. |
| VCheck.Modules.Checklists | Class Library | Regras e persist�ncia do dom�nio de Checklists. |
| VCheck.Modules.Checklists.MigrationService | Worker Service | Aplica migra��es do m�dulo de Checklists. |
| VCheck.SharedKernel | Class Library | Contratos e utilidades compartilhadas m�nimos. |

## Fluxo de Inicializa��o Local (resumo)
1. Aspire Host sobe containers (SQL, Keycloak).
2. Services de migra��o rodam e atualizam os bancos de cada m�dulo.
3. API inicia ap�s migra��es conclu�das.
4. Swagger dispon�vel para testes (autentica��o via Keycloak). Consulte [../docs/getting-started.md](../docs/getting-started.md).

## Autentica��o Local
- Realm importado: `transport` (Keycloak)
- Usu�rios: `user.executor`, `user.supervisor`, `user.comum` (senha `123`)
- Roles: `executor`, `supervisor`

## Conven��es de C�digo
- C# 12 / .NET 8
- FluentValidation para valida��es de comandos.
- L�gica de caso de uso em classes espec�ficas (`UseCases/<Feature>`).
- Controllers finos, delegando a interfaces de m�dulo.

## Pr�ximos Passos T�cnicos
- Adicionar testes unit�rios e de integra��o.
- Introduzir logging estruturado (Serilog) e correla��o.
- Implementar versionamento de API (v1, v2...).
- Harden de seguran�a (policy-based authorization refinada, claims extras).

---
Documenta��o de neg�cio e arquitetura: ver [/docs](../docs).
