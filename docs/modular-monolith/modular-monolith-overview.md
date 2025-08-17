[Voltar ao README](README.md)

## O que é um Monolito Modular?

Um Monolito Modular é uma abordagem de design de software em que um aplicativo monolítico é projetado com ênfase em módulos intercambiáveis e potencialmente reutilizáveis. É um nome explícito para um sistema monolítico construído de forma modular.

- **Definição Principal**: Consiste em partes separadas que, quando combinadas, formam um todo completo. Os módulos são projetados para serem independentes, facilitando a manutenção e a evolução.

Em .NET, isso pode ser implementado usando projetos separados dentro de uma solução única, com assemblies para cada módulo, aplicando princípios de DDD como Bounded Contexts.

Essa abordagem evita a complexidade inicial de microservices, conforme recomendado por especialistas como Martin Fowler: "Você não deve iniciar um novo projeto com microservices, mesmo se tiver certeza de que a aplicação será grande o suficiente para justificá-lo."

Para mais detalhes, consulte [Comparação com Monolitos e Microservices](comparison.md) ou [Referências](references.md).