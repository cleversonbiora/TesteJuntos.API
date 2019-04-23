# Teste Junto

Implementado em .NET Core, utilizando uma estrutura simplificada do DDD criada por mim, que evita situações com referência circular.
O metodo de login escolhido foi uma versão modificada do Indentity Core utilizando o Dapper no lugar do EntityFramework para persistir os dados.
E para autenticação na API optei por utilizar um JWT que alem de compacto, facil de manipular é otimo para uso em microserviços.
Toda o tratamento de exceção e retorno da Api é feito através de middlewares, simplificando a camado da controller e padronizando os retornos.

Proximos passos:
- Melhorar os Testes Unitarios para abranger mais casos.

