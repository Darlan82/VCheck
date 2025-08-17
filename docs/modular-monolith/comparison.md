[Voltar ao README](README.md) | [Comparação](comparison.md)

## Desafios do Monolito Modular

Embora o Monolito Modular ofereça benefícios, apresenta desafios específicos:

- **Definir Módulos e Bounded Contexts**: Identificar limites claros baseados no domínio do negócio, usando DDD.
- **Comunicação entre Módulos**: Garantir acoplamento mínimo; preferir APIs públicas em vez de acesso direto.
- **Independência e Isolamento de Dados por Módulo**: Manter schemas ou tabelas separadas no mesmo banco para evitar dependências.

Em .NET, resolva isso com interfaces contratuais e injeção de dependência (DI) via ASP.NET Core.

Para soluções, veja [Definição de Módulos](defining-modules.md) e [Comunicação entre Módulos](module-communication.md).