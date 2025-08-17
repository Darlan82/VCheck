[Voltar ao README](README.md) | [Vis�o Geral](modular-monolith-overview.md)

## Compara��o: Monolitos vs. Microservices

Aqui est� uma compara��o breve entre arquiteturas monol�ticas tradicionais e microservices, destacando onde o Monolito Modular se encaixa como uma ponte. Representado em tabela para disposi��o lado a lado:

| Aspecto                  | Monolito Tradicional                          | Microservices                                 |
|--------------------------|-----------------------------------------------|-----------------------------------------------|
| Deployment               | Um artefato de deployment                     | Muitos artefatos de deployment                |
| Comunica��o              | Via chamadas de m�todo (in-memory)            | Via chamadas de rede (HTTP, gRPC, etc.)       |
| Escalabilidade           | Verticalmente escal�vel                       | Horizontal e verticalmente escal�vel          |
| Banco de Dados           | Um banco de dados                             | Muitos bancos de dados                        |
| Consist�ncia             | Suporte a transa��es ACID                     | Consist�ncia eventual                         |
| Gerenciamento de Equipes | Dif�cil com equipes grandes                   | Escala bem com equipes grandes                |

O Monolito Modular herda a simplicidade do monolito (um deployment, transa��es f�ceis) mas adiciona modularidade para mitigar desvantagens, preparando para uma migra��o futura para microservices.

Em .NET, use solu��es com m�ltiplos projetos para m�dulos, compartilhando um banco de dados inicial mas isolando l�gica de neg�cios.

