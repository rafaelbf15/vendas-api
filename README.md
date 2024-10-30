# Vendas API

API para gerenciamento de vendas que permite criar, atualizar e consultar dados de vendas. Desenvolvida com .NET 8, utiliza autenticação JWT e API Key.

## Instalação do Docker

1. **Instalar o Docker**:
   - **Windows/macOS**: Baixe e instale o Docker Desktop a partir do [site oficial do Docker](https://www.docker.com/get-started).
   - **Linux**: Siga o guia de instalação para sua distribuição Linux [aqui](https://docs.docker.com/engine/install/).

2. **Verificar a instalação**:
   - Após instalar, execute o seguinte comando para verificar se o Docker está funcionando corretamente:
     ```bash
     docker --version
     ```

## Configuração e Execução do Projeto

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/rafaelbf15/vendas-api.git
   cd vendas-api

2. **Executar com Docker Compose**:
   - Execute o Docker Compose para iniciar os contêineres do projeto:
     ```bash
     docker-compose up -d
     ```

## Endpoints

### Criar uma Venda

- **Endpoint**: `POST /api/v1/Vendas`
- **Exemplo de uso com `curl`**:
   ```bash
   curl -X 'POST' \
     'http://localhost:5082/api/v1/Vendas' \
     -H 'accept: */*' \
     -H 'ApiKey: 9a28dcafffd34fd68a7e4f505b08c82d' \
     -H 'Content-Type: application/json' \
     -d '{
       "numero": "V2131323",
       "dataVenda": "2024-10-29",
       "clienteId": "2ee19143-4d46-42ad-ba1e-2dad99625c8c",
       "filialId": "08703488-e04f-4cf6-ab1a-95bb6e453a55",
       "valorTotal": 1000,
       "cancelado": false,
       "itens": [
         {
           "produtoId": "c873c887-6c7a-41ca-a060-6ac4b4a2aa3f",
           "quantidade": 1,
           "valorUnitario": 1000,
           "desconto": 0,
           "valorTotalItem": 1000
         }
       ]
     }'

# Tecnologias Utilizadas

- **.NET 8**: Framework utilizado para desenvolver a API de Vendas.
- **Docker e Docker Compose**: Containerização da aplicação e orquestração de serviços.
- **MySQL**: Banco de dados relacional.
- **RabbitMQ**: Sistema de mensagens para comunicação entre serviços.
- **Git Flow**: Fluxo de controle de versionamento para facilitar a colaboração.
- **Commit Semântico**: Padrão de mensagens de commit para manter clareza e consistência no histórico.



