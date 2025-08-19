# VCheck - API de Checklist de Veículos

Este repositório contém o material do projeto técnico **VCheck.**, uma iniciativa fictícia.

**O PROJETO NÃO É UMA SOLUÇÃO COMPLETA E NEM UM MVP.** 

**LOGO NÃO ESTÁ CONTEMPLADO TODOS ASPECTOS DE NEGÓCIO.** 

Este projeto é fictício e utilizado apenas para fins de demonstração, desafios técnicos e treinamento. \
![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

## Licença
Este projeto está licenciado sob a licença [MIT](LICENSE).

## Estrutura do Repositório

- [/docs](/docs)  — Documentação.
	- Guia rápido de execução: [Getting Started](docs/getting-started.md)
	- Requisitos de negócio: [Core Business](docs/core_business.md)
	- System Design da solução: [System Design](docs/system_design.md)
	- Arquitetura de Monolito Modular: [Monolito Modular](docs/modular-monolith/README.md)
- [/src](/src) — Código-fonte (.NET)
- [/infra](/infra) — Infraestrutura como código (IaC) para provisionamento de recursos no Azure.

## Visão Geral
A solução demonstra um backend .NET 8 em arquitetura de Monólito Modular que expõe uma API para execução de checklists de veículos. 
A autenticação local é feita via Keycloak orquestrado pelo Host Aspire, suportando perfis (roles) de Executor e Supervisor conforme requisitos de negócio.

Para o racional arquitetural completo consulte o [System Design](docs/system_design.md).

## Como Executar (Resumo)
Para passos detalhados use o guia: [Getting Started](docs/getting-started.md). Resumo:
1. Pré-requisitos: .NET 8 SDK + Docker.
2. `dotnet run --project src/Host/VCheck.Host.csproj` (Aspire sobe SQL Server, Keycloak, jobs de migração e API).
3. Abrir Swagger da API e autenticar (client: `vcheck-api`, secret: `S3cr3t`, usuários: `user.executor` ou `user.supervisor`, senha: `123`).

Detalhes de infraestrutura (migrações, realm, usuários) em: [Infraestrutura / Host](src/Host/README.md).

## Roadmap (Indicativo)
- Completar fluxos de checklist (listagem, histórico, reprovação com motivo)
- Testes automatizados e cobertura mínima
- Observabilidade (logs estruturados, tracing)
- Builds e pipelines com análise estática (SonarQube)
- Endpoints de consulta para supervisores
- Ajustar fluxo de autenticação para Authorization Code + PKCE ao introduzir frontend SPA

