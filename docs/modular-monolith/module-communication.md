[Voltar ao README](README.md) | [Definição de Módulos](defining-modules.md)

## Comunicação entre Módulos

No Monolito Modular, a comunicação é via chamadas de método in-memory para performance:

- **Vantagens**: Rápida, sem overhead de rede.
- **Restrições**: Apenas chame APIs públicas de outros módulos para minimizar acoplamento.
- **Desafios**: Introduz acoplamento em runtime; requer reimplementação ao extrair para microservice (ex: mudar para HTTP).

### Diagrama em Mermaid (Exemplo de Comunicação)

```mermaid
graph LR
    A[Orders Module] -->|Method Call| B[Catalog Module]
    subgraph "Monolith"
    A
    B
    end