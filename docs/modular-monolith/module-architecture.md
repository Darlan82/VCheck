[Voltar ao README](README.md) | [Desafios](challenges.md)

## Arquitetura de M�dulos

A arquitetura t�pica de um m�dulo em um Monolito Modular segue padr�es como Clean Architecture ou Onion Architecture, com camadas claras:

- **Presentation/API**: Camada de entrada (controllers em .NET).
- **Application**: L�gica de aplica��o, orquestra��o.
- **Domain**: Modelos de dom�nio, regras de neg�cio (DDD).
- **Infrastructure/Data**: Acesso a dados, reposit�rios.

Isso permite isolamento e testabilidade.

### Diagrama em Mermaid (Onion Architecture)

```mermaid
graph TD
    A[Presentation] --> B[Application]
    B --> C[Domain]
    C --> D[Infrastructure]
    D --> C
    B --> D
    subgraph "Module Layers"
    A
    B
    C
    D
    end
```