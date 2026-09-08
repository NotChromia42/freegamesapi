using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using freegamesapi.Models;

namespace freegamesapi.Controllers
{
    [ApiController]
    [Route("api/externa")]
    public class JogosExternaController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;

        public JogosExternaController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // GET: api/externa/jogos
        [HttpGet("jogos")]
        public async Task<IActionResult> GetGames()
        {
            try
            {
                var url = "https://www.freetogame.com/api/games";
                var dados = await _httpClient.GetFromJsonAsync<object>(url);
                return Ok(dados);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Erro ao comunicar com a API externa: {ex.Message}");
            }
        }

        // POST: api/externa/importar
        [HttpPost("importar")]
        public async Task<IActionResult> ImportarParaMySQL([FromQuery] int limite = 20)
        {
            try
            {
                var url = "https://www.freetogame.com/api/games";
                var jogosExternos = await _httpClient.GetFromJsonAsync<List<FreeToGameDto>>(url);

                if (jogosExternos == null || !jogosExternos.Any())
                {
                    return BadRequest("Nenhum dado retornado pela API externa.");
                }

                var selecao = jogosExternos.Take(limite).ToList();

                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                int inseridos = 0;
                foreach (var g in selecao)
                {
                    string sql = @"
                        INSERT INTO jogos (id, titulo, imagem, descricao_curta, genero, plataforma, editora, desenvolvedor, url_jogo)
                        VALUES (@id, @titulo, @imagem, @descricao_curta, @genero, @plataforma, @editora, @desenvolvedor, @url_jogo)
                        ON DUPLICATE KEY UPDATE 
                            titulo = VALUES(titulo),
                            imagem = VALUES(imagem),
                            descricao_curta = VALUES(descricao_curta),
                            genero = VALUES(genero),
                            plataforma = VALUES(plataforma),
                            editora = VALUES(editora),
                            desenvolvedor = VALUES(desenvolvedor),
                            url_jogo = VALUES(url_jogo);";

                    using var cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", g.id);
                    cmd.Parameters.AddWithValue("@titulo", g.title);
                    cmd.Parameters.AddWithValue("@imagem", g.thumbnail);
                    cmd.Parameters.AddWithValue("@descricao_curta", g.short_description);
                    cmd.Parameters.AddWithValue("@genero", g.genre);
                    cmd.Parameters.AddWithValue("@plataforma", g.platform);
                    cmd.Parameters.AddWithValue("@editora", g.publisher);
                    cmd.Parameters.AddWithValue("@desenvolvedor", g.developer);
                    cmd.Parameters.AddWithValue("@url_jogo", g.freetogame_profile_url);

                    await cmd.ExecuteNonQueryAsync();
                    inseridos++;
                }

                return Ok(new { mensagem = $"{inseridos} jogos importados/atualizados com sucesso na base de dados local!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro durante a importação: {ex.Message}");
            }
        }
    }
}