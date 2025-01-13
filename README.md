# AVALIAÇÃO TÉCNICA
Teste técnico Venturus

# Projeto para Teste Técnico - Desenvolvedor .Net Sênior

## Descrição Geral
Este projeto foi desenvolvido como parte de um teste técnico para avaliar os conhecimentos e habilidades necessárias para a vaga de Desenvolvedor .Net Sênior. A aplicação consiste em uma API construída em **.Net 8**, utilizando o padrão **CQRS** implementado com **MediatR**, e com persistência de dados no banco de dados **MySQL**.

## Objetivo do Case
O objetivo principal do case é implementar um controle de estoque de peças que atenda aos seguintes requisitos:

- Alerta para saldo de estoque insuficiente ou inexistente.
- Proibição de consumir mais peças do que a quantidade disponível em estoque.
- Registro do consumo diário, incluindo o custo total calculado com base no preço médio do produto.
- Cadastro de produtos com a capacidade de armazenar o **part number**.
- Registro de logs para todas as chamadas com erro dentro do fluxo CQRS no banco de dados.
- Design de tabelas seguindo as três formas de normalização.

## Estrutura do Banco de Dados
As tabelas do banco de dados necessárias para a aplicação estão definidas no arquivo `Data/Scripts/CreateTables.sql`. Certifique-se de executar o script para configurar o banco de dados antes de iniciar a aplicação.

## Funcionalidades Implementadas
- **Verbos HTTP:**
  - **POST:** Para criar novos produtos e registrar consumo de estoque.
  - **PUT:** Para atualizar informações de produtos.
  - **DELETE:** Para remover produtos do sistema.
  - **GET:** Para listar produtos, verificar estoque e gerar relatórios de consumo.
- **Persistência de Dados:** Banco de dados MySQL.
- **CQRS com MediatR:** Separação clara entre comandos e consultas.
- **Log de Erros:** Registro de erros em chamadas CQRS diretamente no banco de dados.

## Tecnologias Utilizadas
- **.Net 8**
- **MediatR**
- **Dapper** para acesso ao banco de dados
- **MySQL** para persistência de dados
- **xUnit** para testes unitários
- **Moq** e **FluentAssertions** para validação de comportamento em testes

## Pasta do projeto
A solução e o projeto EstoqueaAPI,encontram-se nas seguintes pastas:
- EstoqueAPI/EstoqueAPI.sln
- EstoqueAPI/EstoqueAPI/EstoqueAPI.csproj

## Processo de Desenvolvimento
Este projeto foi elaborado utilizando uma abordagem iterativa com o suporte de inteligência artificial **ChatGPT** para:
- Gerar modelos iniciais de código.
- Agilizar o desenvolvimento ao refinar e ajustar implementações para atender aos requisitos do case.
- Garantir conformidade com as melhores práticas de desenvolvimento .Net.

## Como Executar o Projeto
1. Clone o repositório.
2. Certifique-se de que o ambiente tenha o **.Net 8 SDK** e **MySQL** instalados.
3. Configure a string de conexão ao banco de dados no arquivo `appsettings.json`.
4. Execute o script SQL localizado em `Data/Scripts/CreateTables.sql` para criar as tabelas necessárias.
5. Restaure as dependências:
   ```bash
   dotnet restore
   ```
6. Execute a aplicação:
   ```bash
   dotnet run
   ```
7. Acesse os endpoints via Swagger em `http://localhost:{porta}/swagger`.

8. O projeto EstoqueAPI
# EstoqueAPI

Esta aplicação é uma API de gerenciamento de produtos e controle de estoque, implementada com .NET, MediatR e Dapper. A API permite realizar operações como criar, consultar, atualizar e deletar produtos, além de realizar consumo de estoque e gerar relatórios de consumo diário.

---

## **Estrutura do Projeto**

```
EstoqueAPI
├── Application
│   ├── Commands
│   ├── Handlers
│   ├── Queries
├── Controllers
├── Data
├── Domain
│   ├── Entities
│   ├── Enums
├── Middlewares
├── Tests
│   ├── Handlers
│   ├── Controllers
│   ├── Services
├── Program.cs
├── appsettings.json
└── README.md
```
---

## Estrutura do Banco de Dados
As tabelas do banco de dados necessárias para a aplicação estão definidas no arquivo `Data/Scripts/CreateTables.sql`. Certifique-se de executar o script para configurar o banco de dados antes de iniciar a aplicação.

---

## Conexão com o Banco de Dados
Para conectar com o banco de dados Mysql corretamente, altere as credencias no arquivo:
appsettings.json

---


## **Dependências**

As principais dependências utilizadas neste projeto são:

- **Dapper**: Biblioteca de mapeamento objeto-relacional leve para interagir com o banco de dados.
- **MediatR**: Implementação do padrão CQRS para separar lógica de comandos e consultas.
- **FluentAssertions**: Biblioteca para facilitar asserções em testes.
- **Moq**: Framework para criação de mocks em testes unitários.

Para instalar as dependências:

Certifique-se de estar no diretório do projeto principal:

cd /caminho/para/EstoqueAPI
Restaure as dependências listadas no arquivo .csproj:

dotnet restore

Instalação Manual de Dependências (se necessário): Caso precise instalar dependências adicionais manualmente, use os comandos abaixo:

Dapper:
dotnet add package Dapper

MediatR e MediatR.Extensions.Microsoft.DependencyInjection:
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection

FluentAssertions (para testes):
dotnet add package FluentAssertions

Moq (para mocks nos testes):
dotnet add package Moq

xUnit (se for usado para testes):
dotnet add package xunit
dotnet add package xunit.runner.visualstudio

---

## **Como Buildar e Executar**

### **Buildar o Projeto**

```bash
# Realize o build do projeto
 dotnet build
```

### **Executar a Aplicação**

```bash
# Execute o projeto
 dotnet run --project EstoqueAPI
```

A aplicação será iniciada e estará disponível em: `http://localhost:5149`.

---

## **Endpoints**

### **Produtos**

- **`POST /api/Produtos`**: Cria um novo produto.
  - **Request Body**:
    ```json
    {
      "nome": "Produto A",
      "partNumber": "P001",
      "precoMedio": 10.5,
      "estoqueAtual": 100
    }
    ```

- **`GET /api/Produtos/{id}`**: Retorna um produto pelo ID.
  - **Resposta (200)**:
    ```json
    {
      "id": 1,
      "nome": "Produto A",
      "partNumber": "P001",
      "precoMedio": 10.5,
      "estoqueAtual": 100
    }
    ```

- **`GET /api/Produtos`**: Retorna todos os produtos.

- **`PUT /api/Produtos/{id}`**: Atualiza um produto pelo ID.
  - **Request Body**:
    ```json
    {
      "id": 1,
      "nome": "Produto Atualizado",
      "partNumber": "P002",
      "precoMedio": 12.5,
      "estoqueAtual": 50
    }
    ```

- **`DELETE /api/Produtos/{id}`**: Deleta um produto pelo ID.

### **Consumo de Estoque**

- **`POST /api/Produtos/{id}/consumir`**: Consome estoque de um produto.
  - **Request Body**:
    ```json
    {
      "produtoId": 1,
      "quantidadeConsumida": 10
    }
    ```

### **Relatório**

- **`GET /api/Produtos/relatorios/consumo?data={yyyy-MM-dd}`**: Retorna um relatório de consumo diário.

### **Verificar Estoque**

- **`GET /api/Produtos/{id}/estoque`**: Verifica o estoque disponível de um produto.

---

## **Testes Unitários**
# ProdutosControllerTests
1. **`CreateProduto_Should_Return_Ok_With_Id`**
   - Verifica se o controlador retorna o ID do produto recém-criado com status `200 OK`.

2. **`GetProdutoById_Should_Return_Ok_When_Produto_Exists`**
   - Retorna os detalhes do produto com status `200 OK` quando o produto existe.

3. **`GetProdutoById_Should_Return_NotFound_When_Produto_Does_Not_Exist`**
   - Retorna `404 Not Found` quando o produto não é encontrado.

4. **`GetAllProdutos_Should_Return_Ok_With_Produtos`**
   - Retorna uma lista de produtos com status `200 OK`.

5. **`GetAllProdutos_Should_Return_NotFound_When_No_Produtos`**
   - Retorna `404 Not Found` quando não há produtos cadastrados.

6. **`DeleteProduto_Should_Return_Ok_When_Successful`**
   - Retorna `200 OK` quando o produto é deletado com sucesso.

7. **`ConsumirEstoque_Should_Return_BadRequest_When_Quantidade_Insufficient`**
   - Retorna `400 Bad Request` quando a quantidade solicitada é maior que o estoque disponível.

8. **`RelatorioConsumo_Should_Return_Ok_With_Relatorio`**
   - Retorna o relatório de consumo diário com status `200 OK`.

# ConsumirEstoqueHandlerTests
1. **`Should_Return_Sucesso_When_Estoque_Is_Consumed_Successfully`**
   - Verifica se o consumo de estoque é realizado com sucesso quando há estoque suficiente e todas as operações são bem-sucedidas.

2. **`Should_Return_QuantidadeInsuficiente_When_Estoque_Is_Insufficient`**
   - Retorna `QuantidadeInsuficiente` quando a quantidade solicitada excede o estoque disponível.

3. **`Should_Return_QuantidadeInsuficiente_When_Produto_Does_Not_Exist`**
   - Retorna `QuantidadeInsuficiente` quando o produto não existe no banco de dados.

4. **`Should_Return_Falha_When_Update_Fails`**
   - Retorna `Falha` quando a atualização do estoque no banco de dados não é realizada com sucesso.

---

# GetProdutoByIdHandlerTests
1. **`Should_Return_Produto_When_Exists`**
   - Verifica se o handler retorna os detalhes do produto corretamente quando o produto existe no banco de dados.

2. **`Should_Return_Null_When_Produto_Not_Exists`**
   - Verifica se o handler retorna `null` quando o produto solicitado não existe no banco de dados.

---

# RelatorioConsumoHandlerTests
1. **`Should_Return_Relatorio_When_Data_Exists`**
   - Verifica se o handler retorna o relatório de consumo diário corretamente quando existem dados para a data consultada.

2. **`Should_Return_Empty_When_No_Data_Exists`**
   - Verifica se o handler retorna uma lista vazia quando não existem dados para a data consultada.

---

# VerificarEstoqueHandlerTests
1. **`Should_Return_EstoqueDisponivel_When_Produto_Exists`**
   - Verifica se o handler retorna corretamente o estoque disponível quando o produto existe no banco de dados.

2. **`Should_Return_Null_When_Produto_Does_Not_Exist`**
   - Verifica se o handler retorna `null` quando o produto consultado não existe no banco de dados.

---

## **Cenários Cobertos**

1. **Sucesso**: Todos os endpoints retornam os resultados esperados para entradas válidas.
2. **Erro**: Verifica cenários de falha como ausência de produtos, quantidade insuficiente e IDs inexistentes.
3. **Validação**: Testa a mensagem e o status HTTP para diferentes casos.

### **Executando os Testes**

```bash
# Execute os testes unitários
 dotnet test
```
---
## Decisõs Técnicas
Algumas decisões técnicas importantes que vamos destacar:
1. Estruturas de Classes Implementadas:
   - **IDapperExecutor:** Esta interface abstrai o uso do Dapper para facilitar a injeção de dependência, melhorar a testabilidade e evitar duplicação de código de acesso ao banco de dados.
   - **AcaoConsumirTipoResultado:** Enum criado para representar os resultados possíveis das operações de consumo de estoque, garantindo maior clareza e evitando o uso de valores mágicos no código.
   - **Entidades Dinâmicas DTO:** Foram implementadas classes como `EstoqueDisponivel` e `RelatorioConsumo` para representar objetos dinâmicos retornados por consultas específicas, mantendo as entidades principais separadas das respostas personalizadas da aplicação.
   - **LoggingMiddleware:** Middleware utilizado para interceptar todas as requisições, registrando detalhes de chamadas e facilitando o diagnóstico de problemas na aplicação.
   - **ErrorHandlingMiddleware:** Middleware responsável por centralizar o tratamento de erros na aplicação, retornando respostas padronizadas para o cliente e registrando os erros em um banco de dados para posterior análise.

2. Testes Unitários:
   - Foram implementados testes unitários utilizando **xUnit**, **Moq**, e **FluentAssertions** para garantir a qualidade do código e cobrir os principais cenários.

3. Organização do Projeto:
   - O projeto foi estruturado em camadas para garantir a separação de responsabilidades:
     - **Application**: Handlers e comandos/consultas CQRS.
     - **Domain**: Entidades e enums.
     - **Infrastructure**: Acesso ao banco de dados e serviços auxiliares.
     - **API**: Controladores e configuração de middlewares.




## Observações Finais
Este projeto foi construído com a colaboração de IA para acelerar o desenvolvimento e garantir que os requisitos do case fossem atendidos de forma eficiente e clara. Todas as implementações foram revisadas e ajustadas conforme necessário para cumprir os objetivos do teste técnico.
