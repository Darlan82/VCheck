[Voltar ao README](README.md)

## O que � um Monolito Modular?

Um Monolito Modular � uma abordagem de design de software em que um aplicativo monol�tico � projetado com �nfase em m�dulos intercambi�veis e potencialmente reutiliz�veis. � um nome expl�cito para um sistema monol�tico constru�do de forma modular.

- **Defini��o Principal**: Consiste em partes separadas que, quando combinadas, formam um todo completo. Os m�dulos s�o projetados para serem independentes, facilitando a manuten��o e a evolu��o.

Em .NET, isso pode ser implementado usando projetos separados dentro de uma solu��o �nica, com assemblies para cada m�dulo, aplicando princ�pios de DDD como Bounded Contexts.

Essa abordagem evita a complexidade inicial de microservices, conforme recomendado por especialistas como Martin Fowler: "Voc� n�o deve iniciar um novo projeto com microservices, mesmo se tiver certeza de que a aplica��o ser� grande o suficiente para justific�-lo."

Para mais detalhes, consulte [Compara��o com Monolitos e Microservices](comparison.md) ou [Refer�ncias](references.md).