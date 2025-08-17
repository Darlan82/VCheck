[Voltar ao README](README.md) | [Defini��o de M�dulos](defining-modules.md)

## Comunica��o entre M�dulos

No Monolito Modular, a comunica��o � via chamadas de m�todo in-memory para performance:

- **Vantagens**: R�pida, sem overhead de rede.
- **Restri��es**: Apenas chame APIs p�blicas de outros m�dulos para minimizar acoplamento.
- **Desafios**: Introduz acoplamento em runtime; requer reimplementa��o ao extrair para microservice (ex: mudar para HTTP).

### Diagrama em Mermaid (Exemplo de Comunica��o)

```mermaid
graph LR
    A[Orders Module] -->|Method Call| B[Catalog Module]
    subgraph "Monolith"
    A
    B
    end