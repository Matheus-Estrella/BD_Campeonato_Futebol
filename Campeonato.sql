
Create Table Time_futebol(
id_time int identity(1,1),
nome varchar(30) not null,
apelido varchar(10) not null,
fundacao Date not null,
pontuacao int null,
gols_feitos int null,
gols_recebidos int null,
gols_feitos_record int null,
gols_recebidos_record int null,
PRIMARY KEY (id_time)
);

Create table Partida(
n_partida int identity (1,1),
id_casa int not null,
id_visitante int not null,
gols_casa int null,
gols_visita int null,
pontuacao_casa int null,
pontuacao_visitante int null,
Primary Key(n_partida,id_casa,id_visitante),
);

Create table Ranking(
	colocado int identity(1,1) not null,
	id_colocado int not null,
	pontuacao int null,
	primary key (colocado),
	FOREIGN KEY (id_colocado) REFERENCES Time_futebol(id_time)
)

INSERT INTO Time_futebol (nome, apelido, fundacao) VALUES
('Unidos do Tio Paulo', 'UTP', '2024-04-16'),
('Alexa Futebol CLub', 'AFC', '2024-03-15'),
('Java Hatters Team', 'JHT', '1995-05-23'),
('Dragon Ball Z', 'DBZ', '1989-04-26'),
('Cavaleiros do Zodiaco', 'CDZ', '1986-10-11');

Select * from Time_futebol
Select * from Partida
Select * from Ranking

Drop table Ranking
Drop table Partida
Drop table Time_futebol 
