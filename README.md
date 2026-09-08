# FreeToGame Proxy Web API

Projeto desenvolvido para a unidade curricular **UC00614 - Desenvolvimento de Aplicações Web** (Etapas 1 a 6).

## Tema
- **Tema Atribuído:** Jogos
- **API Externa Consumida:** [FreeToGame API](https://www.freetogame.com/api-doc)

## Endpoints Criados
- `GET /api/externa/jogos` - Retorna a lista completa de jogos gratuitos em formato JSON.
- `GET /api/externa/jogos/{id}` - Retorna os detalhes de um jogo específico a partir do seu ID.

## Como Executar O Projeto

1. Certifica-te de que tens o SDK do .NET 8 instalado.
2. Clona o repositório:
   ```bash
   git clone <URL_DO_TEU_REPOSITORIO>
   cd freegamesapi
   ```
3. Executa o projeto:
   ```bash
   dotnet run
   ```
4. Acede à documentação do Swagger no teu browser (ex: `https://localhost:7123/swagger`).
