# FreeToGame Web Application

Projeto desenvolvido para a unidade curricular **UC00614 - Desenvolvimento de Aplicações Web** (Etapas 1 a 6).

Esta aplicação consiste numa Web API em ASP.NET Core interligada a uma interface Web (Frontend) em HTML, CSS e JavaScript. A API consome o serviço externo do **FreeToGame**, permite importar dados para uma base de dados local MySQL e disponibiliza endpoints de consulta e pesquisa consumidos pela interface do utilizador.

## Tema e Tecnologias
- **Tema Atribuído:** Jogos (Free-to-Play)
- **API Externa Consumida:** [FreeToGame API](https://www.freetogame.com/api-doc)
- **Backend:** .NET 8 / ASP.NET Core Web API, C#, MySQL, `MySqlConnector`, Swagger.
- **Frontend:** HTML5, CSS3, JavaScript Vanilla (`Frontend/`).

---

## Estrutura do Repositório

```text
freegamesapi/
├── Controllers/            # Controllers da Web API (JogosController, JogosExternaController)
├── Models/                 # Modelos de dados (Jogo.cs)
├── Frontend/               # Interface de utilizador
│   ├── index.html          # Estrutura principal da página
│   ├── style.css           # Estilização e layout
│   └── app.js              # Lógica de consumo da API em JavaScript
├── schema.sql              # Script SQL para criação da base de dados local
├── Program.cs              # Configuração da aplicação .NET
└── freegamesapi.csproj     # Ficheiro do projeto .NET
```

---

## Endpoints da API

### 1. API Externa & Importação (`/api/externa`)
| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/externa/jogos` | Consulta diretamente os jogos da API externa em tempo real. |
| `POST` | `/api/externa/importar?limite=20` | Importa/sincroniza jogos da API externa para a base de dados local. |

### 2. API Local & Base de Dados (`/api/jogos`)
| Método | Endpoint | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/jogos` | Retorna a lista de jogos guardados na base de dados MySQL. |
| `GET` | `/api/jogos/{id}` | Retorna os detalhes de um jogo específico pelo seu ID. |
| `GET` | `/api/jogos/pesquisa` | Pesquisa/filtra jogos por termo (título/descrição) ou género. |

---

## Como Executar o Projeto

### 1. Configurar a Base de Dados
1. Executa o ficheiro `schema.sql` no teu cliente MySQL (ex: MySQL Workbench) para criar a base de dados `freegames_db` e a respetiva tabela.
2. Cria o ficheiro `appsettings.json` na raiz do projeto (ou edita o existente) com as tuas credenciais do MySQL:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=freegames_db;User Id=root;Password=A_TUA_PASSWORD;"
     }
   }
   ```

### 2. Iniciar a API (Backend)
1. No terminal, entra na raiz do repositório e executa:
   ```bash
   dotnet run
   ```
2. A API ficará ativa e a documentação interativa estará acessível em:
   ```text
   http://localhost:5000/swagger
   ```

### 3. Abrir o Frontend
1. Garante que a API está a ser executada no terminal.
2. Abre o ficheiro `Frontend/index.html` diretamente no teu navegador (ou utiliza a extensão *Live Server* no VS Code).
3. A interface irá comunicar com a API local para listar, pesquisar e filtrar os jogos.
