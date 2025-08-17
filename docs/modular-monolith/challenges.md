[Voltar ao README](README.md) | [Visão Geral](modular-monolith-overview.md)

## Comparação: Monolitos vs. Microservices

Aqui está uma comparação breve entre arquiteturas monolíticas tradicionais e microservices, destacando onde o Monolito Modular se encaixa como uma ponte. Representado em tabela para disposição lado a lado:

| Aspecto                  | Monolito Tradicional                          | Microservices                                 |
|--------------------------|-----------------------------------------------|-----------------------------------------------|
| Deployment               | Um artefato de deployment                     | Muitos artefatos de deployment                |
| Comunicação              | Via chamadas de método (in-memory)            | Via chamadas de rede (HTTP, gRPC, etc.)       |
| Escalabilidade           | Verticalmente escalável                       | Horizontal e verticalmente escalável          |
| Banco de Dados           | Um banco de dados                             | Muitos bancos de dados                        |
| Consistência             | Suporte a transações ACID                     | Consistência eventual                         |
| Gerenciamento de Equipes | Difícil com equipes grandes                   | Escala bem com equipes grandes                |

O Monolito Modular herda a simplicidade do monolito (um deployment, transações fáceis) mas adiciona modularidade para mitigar desvantagens, preparando para uma migração futura para microservices.

Em .NET, use soluções com múltiplos projetos para módulos, compartilhando um banco de dados inicial mas isolando lógica de negócios.

