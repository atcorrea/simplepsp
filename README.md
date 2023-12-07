# SIMPLE PSP

## Projeto
Este projeto simula um PSP (Payment Service Provider) com algumas funcionalidades básicas como criação de transação e criação de recebíveis de acordo com um conjunto de regras definidas. Foi utilizado como teste técnico para um processo seletivo. 

## Tecnologias utilizadas
- Dotnet 8
- SQL Server
- Docker Compose
- Entity Framework (ORM e Migrations)

## Sobre a arquitetura do projeto
O projeto foi planejado seguindo alguns conceitos de arquitetura limpa, com a lógica de domínio no coração da aplicação e com as dependências externas em camadas mais externas da aplicação. Também foram aproveitados, com algumas liberdades, alguns conceitos do Domain Driven Design como a organização em agregados e os objetos de domínio com lógica de negócio, tudo com o objetivo de manter a complexidade do negócio apartada da complexidade da solução.

## Executando a aplicação
Antes de iniciar a execução, certifique-se que o Docker compose esteja instalado.
Para executar a aplicação basta executar o seguinte comando na raiz do projeto:
```
docker compose up
```
Este irá fazer o build da imagem docker da aplicação e também subirá um servidor SQL Server em um container para persistência dos dados.

Assim que o build terminar, acesse em seu navegador http://localhost:8080/swagger

Nesta página há o swagger que descreve as operações suportadas pela API da aplicação.


## Endpoints
### Processamento de transação

Cria uma nova transação e a salva no banco de dados. Após a criação da transação, também é criado um recebível (payable) correspondente, de acordo com o método de pagamento.
```
[POST] http://localhost:8080/Transactions
```
**Lista de parâmetros:**
| Parametro        | Descrição                                         | Tipo                                 | Obrigatório | Exemplo             |
|------------------|---------------------------------------------------|--------------------------------------|-------------|---------------------|
| storeId          | Identificador do cliente que receberá a transação | string                               | Sim         | 18164750000164      |
| value            | Valor da transação                                | decimal                              | Sim         | 100                 |
| description      | Descrição da transação                            | string                               | Sim         | Smartband XYZ 3.0   |
| paymentMethod    | Método de pagamento                               | string - (debit_card ou credit_card) | Sim         | 1                   |
| cardNumber       | Número do cartão                                  | string                               | Sim         | 1234-5678-9876-5432 |
| cardHolderName   | Nome do portador do cartão                        | string                               | Sim         | NOME DO PORTADOR    |
| cardValidity     | Data de validade do cartão                        | DateTime                             | Sim         | 2023-12-06          |
| cardSecurityCode | Código de verificação do cartão (CVV)             | string                               | Sim         | 123                 |

### Lista de transações

Retorna uma lista completa com todas as transações efetuadas.
```
[GET] http://localhost:8080/Transactions
```

### Saldo disponível
Retorna o valor de saldo de tudo que o cliente já recebeu (recebíveis pagos);
```
[GET] http://localhost:8080/Stores/[STOREID]/balances/available
```
**Lista de parâmetros:**
| Parametro        | Descrição                                         | Tipo                           | Obrigatório | Exemplo             |
|------------------|---------------------------------------------------|--------------------------------|-------------|---------------------|
| storeId          | Identificador do cliente que receberá a transação | string                         | Sim         | 18164750000164      |

### Saldo a receber
Retorna o valor de saldo de tudo que o cliente tem a receber (recebíveis agendados);
```
[GET] http://localhost:8080/Stores/[STOREID]/balances/available
```
**Lista de parâmetros:**
| Parametro        | Descrição                                         | Tipo                           | Obrigatório | Exemplo             |
|------------------|---------------------------------------------------|--------------------------------|-------------|---------------------|
| storeId          | Identificador do cliente que receberá a transação | string                         | Sim         | 18164750000164      |

## Banco de dados
O banco de dados da aplicação foi criado utilizando a abordagem code-first, com migrations geradas pelo Entity Framework.
O banco conta com 2 tabelas:
- Payables: Para armazenamento dos dados dos recebíveis
- Transactions: Para armazenamento dos dados das transações

## Testes
O projeto conta com uma suite de testes de unidade e de integração.
Para executar os testes de integração, é necessário o container de banco de dados da aplicação esteja rodando na máquina para que os testes consigam fazer as operações de leitura/escrita necessárias em um banco de dados que é recriado cada vez que os testes de integração são executados

Para executar o container, você pode subir a aplicação com o docker compose normalmente, conforme descrito na seção "Executando a aplicação".
Caso tenha parado a execução do docker compose após seu inicio, você pode voltar apenas o container do banco através do comando:
```
docker container start simplepsp-db-1
```

## Evolução
Para próximas versões desta aplicação, seria interessante adicionar alguns pontos abaixo:
- **Processamento assíncrono de recebíveis**

    Hoje a criação de transação também faz a criação do recebível. Entendo que esse processo poderia ser apartado em um fluxo assíncrono.
    Após o registro da transação, o sistema poderia postar uma mensagem em uma fila e retornar sucesso na API mais rapidamente.
    Separando os fluxos também seria possivel inserir mais ações no processamento do recebível, como comunicação com alguma API externa, retentativas em caso de falha, entre outros, sem interferir na resposta da chamada inicial.

- **Pagamento de recebíveis agendados**

    Seria interessante um fluxo apartado que olhasse para a base de dados de recebíveis identificando os pagamentos pendentes e com a data de pagamento no dia atual (ou anteriores) e realizasse as ações necessárias para o pagamento destes. Uma abordagem possivel para esse processamento seria utilizando uma Azure Function que executasse 1x por dia.

- **Idempotência**

    Seria interessante implementar uma solução de idempotência na API para evitar que a mesma transação seja processada duas vezes.