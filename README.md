# VCheck - API de Checklist de Ve�culos

Este reposit�rio cont�m o material do projeto t�cnico **VCheck.**, uma iniciativa fict�cia.

**O PROJETO N�O � UMA SOLU��O COMPLETA E NEM UM MVP.** 

**LOGO N�O EST� CONTEMPLADO TODOS ASPECTOS DE NEG�CIO.** 

Este projeto � fict�cio e utilizado apenas para fins de demonstra��o, desafios t�cnicos e treinamento. \
![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

## Licen�a
Este projeto est� licenciado sob a licen�a [MIT](LICENSE).

## Estrutura do Reposit�rio

- [/docs](/docs)  � Documenta��o.
	- Guia r�pido de execu��o: [Getting Started](docs/getting-started.md)
	- Requisitos de neg�cio: [Core Business](docs/core_business.md)
	- System Design da solu��o: [System Design](docs/system_design.md)
	- Arquitetura de Monolito Modular: [Monolito Modular](docs/modular-monolith/README.md)
- [/src](/src) � C�digo-fonte (.NET)
- [/infra](/infra) � Infraestrutura como c�digo (IaC) para provisionamento de recursos no Azure.

## Vis�o Geral
A solu��o demonstra um backend .NET 8 em arquitetura de Mon�lito Modular que exp�e uma API para execu��o de checklists de ve�culos. 
A autentica��o local � feita via Keycloak orquestrado pelo Host Aspire, suportando perfis (roles) de Executor e Supervisor conforme requisitos de neg�cio.

Para o racional arquitetural completo consulte o [System Design](docs/system_design.md).

## Como Executar (Resumo)
Para passos detalhados use o guia: [Getting Started](docs/getting-started.md). Resumo:
1. Pr�-requisitos: .NET 8 SDK + Docker.
2. `dotnet run --project src/Host/VCheck.Host.csproj` (Aspire sobe SQL Server, Keycloak, jobs de migra��o e API).
3. Abrir Swagger da API e autenticar (client: `vcheck-api`, secret: `S3cr3t`, usu�rios: `user.executor` ou `user.supervisor`, senha: `123`).

Detalhes de infraestrutura (migra��es, realm, usu�rios) em: [Infraestrutura / Host](src/Host/README.md).

## Roadmap (Indicativo)
- Completar fluxos de checklist (listagem, hist�rico, reprova��o com motivo)
- Testes automatizados e cobertura m�nima
- Observabilidade (logs estruturados, tracing)
- Builds e pipelines com an�lise est�tica (SonarQube)
- Endpoints de consulta para supervisores
- Ajustar fluxo de autentica��o para Authorization Code + PKCE ao introduzir frontend SPA

