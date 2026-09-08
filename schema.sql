CREATE DATABASE IF NOT EXISTS freegames_db;
USE freegames_db;

CREATE TABLE IF NOT EXISTS jogos (
    id INT PRIMARY KEY,
    titulo VARCHAR(150) NOT NULL,
    imagem VARCHAR(255),
    descricao_curta TEXT,
    genero VARCHAR(50),
    plataforma VARCHAR(50),
    editora VARCHAR(100),
    desenvolvedor VARCHAR(100),
    url_jogo VARCHAR(255)
);