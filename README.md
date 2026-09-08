# FreeToGame Web API

Projeto desenvolvido para a unidade curricular **UC00614 - Desenvolvimento de Aplicações Web** (Etapas 1 a 6).

Esta aplicação consiste numa Web API em ASP.NET Core que consome a API externa do **FreeToGame**, permite importar os dados para uma base de dados local MySQL e disponibiliza endpoints próprios para consulta e filtragem do catálogo.

## Tema e Tecnologias
- **Tema Atribuído:** Jogos (Free-to-Play)
- **API Externa Consumida:** [FreeToGame API](https://www.freetogame.com/api-doc)
- **Tecnologias:** .NET 8 / 10, ASP.NET Core Web API, C#, MySQL, `MySqlConnector`, Swagger (Swashbuckle).

---

## Estrutura de Endpoints

### 1. API Externa & Importação (`/api/externa`)
| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/externa/jogos` | Consulta diretamente os jogos da API externa em tempo real. |
| `POST` | `/api/externa/importar?limite=20` | Importa/sincroniza os jogos da API externa para a base de dados MySQL local. |

### 2. API Local & Base de Dados (`/api/jogos`)
| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/jogos` | Retorna a lista de todos os jogos guardados na base de dados MySQL. |
| `GET` | `/api/jogos/{id}` | Retorna os detalhes de um jogo específico guardado na base de dados pelo ID. |
| `GET` | `/api/jogos/pesquisa` | Pesquisa e filtra jogos na base de dados por termo (no título/descrição) ou por género (ex: `?termo=shooter&genero=mmorpg`). |

---

## Configuração e Instalação

### 1. Pré-requisitos
- .NET SDK (versão 8.0 ou superior)
- Servidor MySQL a correr na tua máquina local (ex: MySQL Community Server, MariaDB ou via XAMPP).

### 2. Configurar a Base de Dados
1. Executa o ficheiro `schema.sql` no teu cliente MySQL (ex: MySQL Workbench) para criar a base de dados `freegames_db` e a tabela `jogos`.
2. Cria o ficheiro de configuração `appsettings.json` na raiz do projeto (podes duplicar e renomear o ficheiro `appsettings.Example.json`).
3. Define as tuas credenciais de acesso ao MySQL no `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=freegames_db;User Id=root;Password=A_TUA_PASSWORD;"
     }
   }
