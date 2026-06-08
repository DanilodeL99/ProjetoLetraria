CREATE DATABASE Letraria;
USE Letraria;

CREATE TABLE usuarios (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    tipo_usuario ENUM('PROFESSOR', 'ALUNO') NOT NULL,
    cndb VARCHAR(50),
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE catalogos (
    id_catalogo INT PRIMARY KEY AUTO_INCREMENT,
    id_professor INT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    descricao TEXT,
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_professor) REFERENCES usuarios(id_usuario)
);

CREATE TABLE livros (
    id_livro INT PRIMARY KEY AUTO_INCREMENT,
    titulo VARCHAR(200) NOT NULL,
    autor VARCHAR(150) NOT NULL,
    resumo TEXT NOT NULL,
    genero VARCHAR(100) NOT NULL,
    imagem_capa VARCHAR(255),
    tipo_acesso ENUM('DIGITAL', 'COMPRA') NOT NULL,
    link_compra VARCHAR(255),
    arquivo_livro VARCHAR(255),
    possui_amostra BOOLEAN DEFAULT FALSE,
    limite_amostra INT,
    preco DECIMAL(10,2),
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE catalogo_livros (
    id_catalogo INT,
    id_livro INT,
    PRIMARY KEY (id_catalogo, id_livro),
    FOREIGN KEY (id_catalogo) REFERENCES catalogos(id_catalogo),
    FOREIGN KEY (id_livro) REFERENCES livros(id_livro)
);

CREATE TABLE compartilhamentos (
    id_compartilhamento INT PRIMARY KEY AUTO_INCREMENT,
    id_catalogo INT NOT NULL,
    id_aluno INT NOT NULL,
    data_compartilhamento TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_catalogo) REFERENCES catalogos(id_catalogo),
    FOREIGN KEY (id_aluno) REFERENCES usuarios(id_usuario)
);

CREATE TABLE avaliacoes (
    id_avaliacao INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    id_livro INT NOT NULL,
    nota INT NOT NULL CHECK (nota >= 1 AND nota <= 5),
    comentario TEXT,
    data_avaliacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_livro) REFERENCES livros(id_livro)
);

CREATE TABLE compras (
    id_compra INT PRIMARY KEY AUTO_INCREMENT,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    valor DECIMAL(10,2) NOT NULL,
    metodo_pagamento ENUM('CARTAO', 'PIX') NOT NULL,
    status_pagamento ENUM('PENDENTE', 'PAGO', 'CANCELADO') DEFAULT 'PENDENTE',
    data_compra TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_aluno) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_livro) REFERENCES livros(id_livro)
);

CREATE TABLE biblioteca_pessoal (
    id_biblioteca INT PRIMARY KEY AUTO_INCREMENT,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_adicao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_aluno) REFERENCES usuarios(id_usuario),
    FOREIGN KEY (id_livro) REFERENCES livros(id_livro)
);

CREATE TABLE redefinicao_senha (
    id_redefinicao INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    token VARCHAR(255) NOT NULL,
    expiracao DATETIME NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

CREATE TABLE carrinho (
    id_carrinho INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT NOT NULL,
    id_livro INT NOT NULL,
    quantidade INT DEFAULT 1
);

CREATE TABLE curtidas(
    id_curtida INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT,
    id_avaliacao INT
);

CREATE TABLE comentarios(
    id_comentario INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario INT,
    id_avaliacao INT,
    texto TEXT
);

CREATE TABLE comentarios_avaliacao
(
    id_comentario INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_avaliacao INT NOT NULL,
    texto TEXT NOT NULL,
    data_comentario DATETIME NOT NULL
);

INSERT INTO livros
(
titulo,
autor,
resumo,
genero,
imagem_capa,
tipo_acesso,
preco
)
VALUES
(
'Dom Casmurro',
'Machado de Assis',
'Um clássico da literatura brasileira.',
'Literatura Brasileira',
'https://m.media-amazon.com/images/I/71DMa1Qq1-L.jpg',
'DIGITAL',
29.90
),

(
'1984',
'George Orwell',
'Distopia política clássica.',
'Terror Psicológico',
'https://m.media-amazon.com/images/I/61NAx5pd6XL.jpg',
'COMPRA',
39.90
),

(
'O Hobbit',
'J.R.R Tolkien',
'Aventura fantástica na Terra Média.',
'Fantasia',
'https://m.media-amazon.com/images/I/91M9xPIf10L.jpg',
'COMPRA',
49.90
);

ALTER TABLE usuarios
MODIFY tipo_usuario ENUM('ADMIN', 'PROFESSOR', 'ALUNO') NOT NULL;

INSERT INTO usuarios
(nome, email, senha, tipo_usuario)
VALUES
(
'Administrador',
'admin@letraria.com',
'123456',
'ADMIN'
);

CREATE TABLE tags (
    id_tag INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(80) NOT NULL UNIQUE
);

CREATE TABLE livro_tags (
    id_livro INT NOT NULL,
    id_tag INT NOT NULL,
    PRIMARY KEY (id_livro, id_tag),
    FOREIGN KEY (id_livro) REFERENCES livros(id_livro),
    FOREIGN KEY (id_tag) REFERENCES tags(id_tag)
);

INSERT INTO tags (nome) VALUES
('Literatura Brasileira'),
('Romance'),
('Fantasia'),
('Terror'),
('Drama'),
('Filosofia'),
('Biografia'),
('Suspense'),
('Aventura'),
('Clássico');

ALTER TABLE usuarios
ADD COLUMN nome_exibicao VARCHAR(100) NULL,
ADD COLUMN foto_perfil VARCHAR(255) NULL;
