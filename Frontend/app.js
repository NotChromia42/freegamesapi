// Elementos
const listaJogos = document.getElementById("jogos");
const mensagem = document.getElementById("mensagem");
const pesquisa = document.getElementById("pesquisa");

let todosJogos = [];

// Listar jogos
function mostrarJogos(jogos) 
{
    // Remove os cards anteriores
    listaJogos.replaceChildren();

    jogos.forEach((jogo) => {
        const card = document.createElement("article");
        card.className = "card-jogo";

        // Mostra a imagem do jogo
        const imagem = document.createElement("img");
        imagem.src = jogo.imagem;
        imagem.alt = `Capa de ${jogo.titulo}`;
        imagem.loading = "lazy";

        // Usa textContent para exibir os textos recebidos
        const titulo = document.createElement("h2");
        titulo.textContent = jogo.titulo;

        const descricao = document.createElement("p");
        descricao.textContent = jogo.descricaoCurta;

        // Mostra o gênero e a plataforma
        const categorias = document.createElement("div");
        categorias.className = "categorias";
        const genero = document.createElement("span");
        genero.textContent = jogo.genero;
        const plataforma = document.createElement("span");
        plataforma.textContent = jogo.plataforma;

        // Junta os elementos e coloca o card na página
        categorias.append(genero, plataforma);
        card.append(imagem, titulo, descricao, categorias);
        listaJogos.append(card);
    });

    mensagem.textContent = jogos.length === 0
        ? "Nenhum jogo encontrado."
        : `${jogos.length} jogos encontrados.`;
}

// Consulta a API
async function carregarJogos() {
    mensagem.textContent = "Carregando jogos...";
    pesquisa.disabled = true;

    try {
        const resposta = await fetch(
            "http://localhost:5000/api/jogos"
        );

        // verificador API
        if (!resposta.ok) {
            throw new Error(`Erro da API: ${resposta.status}`);
        }

        // Converte o JSON pro JS usar
        todosJogos = await resposta.json();

        mostrarJogos(todosJogos);
        pesquisa.disabled = false;
    } catch (erro) {
        // erro de buscar os dados
        mensagem.textContent =
            "Não foi possível carregar os jogos. Verifique se a API está ligada.";

        console.error("Erro ao carregar jogos:", erro);
    }
}

// Filtra os jogos conforme você digita no campo de pesquisa
pesquisa.addEventListener("input", () => {
    const termo = pesquisa.value.trim().toLowerCase();

    const jogosFiltrados = todosJogos.filter((jogo) =>
        jogo.titulo.toLowerCase().includes(termo)
    );

    mostrarJogos(jogosFiltrados);
});

// Busca os jogos quando a página é aberta
carregarJogos();