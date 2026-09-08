const listaJogos = document.getElementById("jogos");
const mensagem = document.getElementById("mensagem");
const pesquisa = document.getElementById("pesquisa");
const filtroGenero = document.getElementById("genero");

// API do projeto
const enderecoAPI = "http://localhost:5000/api/jogos";

let buscaAtual = null;

// Cria e mostra os cards dos jogos
function mostrarJogos(jogos) {
    listaJogos.replaceChildren();

    jogos.forEach((jogo) => {
        // Cria o card
        const card = document.createElement("article");
        card.className = "card-jogo";
        // Cria a imagem do jogo
        const imagem = document.createElement("img");
        imagem.src = jogo.imagem;
        imagem.alt = `Capa de ${jogo.titulo}`;
        imagem.loading = "lazy";
        // Mostra o título
        const titulo = document.createElement("h2");
        titulo.textContent = jogo.titulo;
        // Mostra a descrição
        const descricao = document.createElement("p");
        descricao.textContent = jogo.descricaoCurta;
        // Cria a área de gênero e plataforma
        const categorias = document.createElement("div");
        categorias.className = "categorias";
        const genero = document.createElement("span");
        genero.textContent = jogo.genero;
        const plataforma = document.createElement("span");
        plataforma.textContent = jogo.plataforma;
        // Coloca as informações dentro do card
        categorias.append(genero, plataforma);
        card.append(imagem, titulo, descricao, categorias);
        // Adiciona o card à página
        listaJogos.append(card);
    });

    // conta a quantidade de jogos
    mensagem.textContent = jogos.length === 0
        ? "Nenhum jogo encontrado."
        : `${jogos.length} jogos encontrados.`;
}

// Filtro de gêneros
function preencherGeneros(jogos) {
    // evita generos repetidos
    const generos = [...new Set(
        jogos.map((jogo) => jogo.genero).filter(Boolean)
    )].sort();

    // opção que permite mostrar todos os gêneros
    filtroGenero.replaceChildren(
        new Option("Todos os gêneros", "")
    );

    // Adiciona cada gênero à lista
    generos.forEach((genero) => {
        filtroGenero.add(new Option(genero, genero));
    });
}

// Carrega todos os jogos quando a página é aberta
async function carregarJogos() {
    mensagem.textContent = "Carregando jogos...";

    // Bloqueia os campos até terminar o carregamento
    pesquisa.disabled = true;
    filtroGenero.disabled = true;

    try {
        // Consulta os jogos guardados no MySQL através da API
        const resposta = await fetch(enderecoAPI);
        // Verifica se a API respondeu com sucesso
        if (!resposta.ok) {
            throw new Error(`Erro da API: ${resposta.status}`);
        }

        // Converte o JSON pro JS ler
        const jogos = await resposta.json();

        // Preenche os gêneros e mostra os cards
        preencherGeneros(jogos);
        mostrarJogos(jogos);

        // Libera a pesquisa e o filtro
        pesquisa.disabled = false;
        filtroGenero.disabled = false;
    } catch (erro) {
        // mostra uma mensagem de erro
        listaJogos.replaceChildren();
        mensagem.textContent =
            "Não foi possível carregar os jogos. Verifique se a API está ligada.";

        console.error("Erro ao carregar jogos:", erro);
    }
}

// Pesquisa os jogos combinando o texto e o gênero selecionado
async function filtrarJogos() {
    // Cancela a consulta anterior para evitar resultados fora de ordem
    if (buscaAtual) {
        buscaAtual.abort();
    }

    const controle = new AbortController();
    buscaAtual = controle;

    // Prepara os valores para enviar à API
    const parametros = new URLSearchParams({
        termo: pesquisa.value.trim(),
        genero: filtroGenero.value
    });

    mensagem.textContent = "Buscando jogos...";

    try {
        // Consulta o endpoint de pesquisa do banco
        const resposta = await fetch(
            `${enderecoAPI}/pesquisa?${parametros}`,
            { signal: controle.signal }
        );

        // Verifica se a consulta funcionou
        if (!resposta.ok) {
            throw new Error(`Erro da API: ${resposta.status}`);
        }

        const jogos = await resposta.json();

        // Mostra apenas o resultado da busca mais recente
        if (!controle.signal.aborted) {
            mostrarJogos(jogos);
        }
    } catch (erro) {
        // Uma consulta cancelada não precisa mostrar mensagem de erro
        if (controle.signal.aborted) {
            return;
        }

        // Remove os resultados anteriores para não confundir o usuário
        listaJogos.replaceChildren();
        mensagem.textContent =
            "Não foi possível filtrar os jogos. Tente novamente.";

        console.error("Erro ao filtrar jogos:", erro);
    }
}

// Pesquisa quando o usuário digita ou apaga o texto
pesquisa.addEventListener("input", filtrarJogos);

// Filtra quando o usuário escolhe outro gênero
filtroGenero.addEventListener("change", filtrarJogos);

// Inicia o carregamento da página
carregarJogos();