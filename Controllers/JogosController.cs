using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using freegamesapi.Models;

namespace freegamesapi.Controllers
{
    [ApiController]
    [Route("api/jogos")]
    public class JogosController : ControllerBase
    {
        private readonly string _connectionString;

        public JogosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // GET: api/jogos
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var jogos = new List<Jogo>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT id, titulo, imagem, descricao_curta, genero, plataforma, editora, desenvolvedor, url_jogo FROM jogos;";
            using var cmd = new MySqlCommand(sql, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                jogos.Add(MapearJogo(reader));
            }

            return Ok(jogos);
        }

        // GET: api/jogos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT id, titulo, imagem, descricao_curta, genero, plataforma, editora, desenvolvedor, url_jogo FROM jogos WHERE id = @id;";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return Ok(MapearJogo(reader));
            }

            return NotFound(new { mensagem = $"Jogo com ID {id} não encontrado na base de dados." });
        }

        // GET: api/jogos/pesquisa?termo=...&genero=...
        [HttpGet("pesquisa")]
        public async Task<IActionResult> Pesquisar([FromQuery] string? termo, [FromQuery] string? genero)
        {
            var jogos = new List<Jogo>();
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = "SELECT id, titulo, imagem, descricao_curta, genero, plataforma, editora, desenvolvedor, url_jogo FROM jogos WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(termo))
            {
                sql += " AND (titulo LIKE @termo OR descricao_curta LIKE @termo)";
            }
            if (!string.IsNullOrWhiteSpace(genero))
            {
                sql += " AND genero LIKE @genero";
            }

            using var cmd = new MySqlCommand(sql, connection);

            if (!string.IsNullOrWhiteSpace(termo))
            {
                cmd.Parameters.AddWithValue("@termo", $"%{termo}%");
            }
            if (!string.IsNullOrWhiteSpace(genero))
            {
                cmd.Parameters.AddWithValue("@genero", $"%{genero}%");
            }

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                jogos.Add(MapearJogo(reader));
            }

            return Ok(jogos);
        }

        private static Jogo MapearJogo(MySqlDataReader reader)
        {
            int ordId = reader.GetOrdinal("id");
            int ordTitulo = reader.GetOrdinal("titulo");
            int ordImagem = reader.GetOrdinal("imagem");
            int ordDesc = reader.GetOrdinal("descricao_curta");
            int ordGenero = reader.GetOrdinal("genero");
            int ordPlataforma = reader.GetOrdinal("plataforma");
            int ordEditora = reader.GetOrdinal("editora");
            int ordDev = reader.GetOrdinal("desenvolvedor");
            int ordUrl = reader.GetOrdinal("url_jogo");

            return new Jogo
            {
                Id = reader.GetInt32(ordId),
                Titulo = reader.GetString(ordTitulo),
                Imagem = reader.IsDBNull(ordImagem) ? string.Empty : reader.GetString(ordImagem),
                DescricaoCurta = reader.IsDBNull(ordDesc) ? string.Empty : reader.GetString(ordDesc),
                Genero = reader.IsDBNull(ordGenero) ? string.Empty : reader.GetString(ordGenero),
                Plataforma = reader.IsDBNull(ordPlataforma) ? string.Empty : reader.GetString(ordPlataforma),
                Editora = reader.IsDBNull(ordEditora) ? string.Empty : reader.GetString(ordEditora),
                Desenvolvedor = reader.IsDBNull(ordDev) ? string.Empty : reader.GetString(ordDev),
                UrlJogo = reader.IsDBNull(ordUrl) ? string.Empty : reader.GetString(ordUrl)
            };
        }
    }
}