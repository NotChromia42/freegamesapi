using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace FreeToGameProxy.Controllers
{
    [ApiController]
    [Route("api/externa")]
    public class JogosExternaController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public JogosExternaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        // GET: api/externa/jogos/540
        [HttpGet("jogos/{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            try
            {
                var url = $"https://www.freetogame.com/api/game?id={id}";
                var dados = await _httpClient.GetFromJsonAsync<object>(url);
                return Ok(dados);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Erro ao comunicar com a API externa: {ex.Message}");
            }
        }
    }
}