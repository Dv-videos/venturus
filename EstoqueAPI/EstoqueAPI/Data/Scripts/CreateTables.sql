CREATE DATABASE EstoqueDB;
USE EstoqueDB;

CREATE TABLE Produtos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    partnumber VARCHAR(100) NOT NULL,
    preco_medio DECIMAL(10,2) NOT NULL,
    estoque_atual INT NOT NULL
);

CREATE TABLE LogsErro (
    id INT AUTO_INCREMENT PRIMARY KEY,
    mensagem TEXT NOT NULL,
    data_hora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ConsumoDiario (
    id INT AUTO_INCREMENT PRIMARY KEY,                  
    produto_id INT NOT NULL,                            
    quantidade_consumida INT NOT NULL,                  
    data DATE NOT NULL,                                 
    FOREIGN KEY (produto_id) REFERENCES produtos(id)
);

CREATE TABLE logs_erro (
    id INT AUTO_INCREMENT PRIMARY KEY,
    mensagem TEXT NOT NULL,
    stack_trace TEXT,
    data_hora DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);