namespace freegamesapi.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty;
        public string DescricaoCurta { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public string Editora { get; set; } = string.Empty;
        public string Desenvolvedor { get; set; } = string.Empty;
        public string UrlJogo { get; set; } = string.Empty;
    }

    public class FreeToGameDto
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public string thumbnail { get; set; } = string.Empty;
        public string short_description { get; set; } = string.Empty;
        public string genre { get; set; } = string.Empty;
        public string platform { get; set; } = string.Empty;
        public string publisher { get; set; } = string.Empty;
        public string developer { get; set; } = string.Empty;
        public string freetogame_profile_url { get; set; } = string.Empty;
    }
}