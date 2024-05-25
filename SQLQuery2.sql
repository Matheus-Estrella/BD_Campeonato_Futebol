
-- PRA PONTUAR OS TIMES DE ACORDO COM AS REGRAS DO CAMPEONATO

CREATE PROCEDURE Pontuar_Times
AS
BEGIN
    UPDATE Partida
    SET pontuacao_casa = 
        CASE 
            WHEN gols_casa > gols_visita THEN 5
            WHEN gols_casa = gols_visita THEN 1
            ELSE 0
        END,
        pontuacao_visitante = 
        CASE 
            WHEN gols_casa < gols_visita THEN 3
            WHEN gols_casa = gols_visita THEN 1
            ELSE 0
        END;
END;

-- Atualiza pontuação, gols feitos, gols recebidos, gols feitos record e gols recebidos record para cada time

CREATE PROCEDURE Resultado_Campeonato
AS
BEGIN
    UPDATE tf
    SET pontuacao = ISNULL(soma_pontuacao, 0),
        gols_feitos = ISNULL(soma_gols_feitos, 0),
        gols_recebidos = ISNULL(soma_gols_recebidos, 0),
        gols_feitos_record = ISNULL(max_gols_feitos, 0),
        gols_recebidos_record = ISNULL(max_gols_recebidos, 0)
    FROM Time_futebol tf
    LEFT JOIN (
        SELECT 
            t.id_time,
            SUM(CASE WHEN p.id_casa = t.id_time THEN p.pontuacao_casa ELSE p.pontuacao_visitante END) as soma_pontuacao,
            SUM(CASE WHEN p.id_casa = t.id_time THEN p.gols_casa ELSE p.gols_visita END) as soma_gols_feitos,
            SUM(CASE WHEN p.id_casa = t.id_time THEN p.gols_visita ELSE p.gols_casa END) as soma_gols_recebidos,
            MAX(CASE WHEN p.id_casa = t.id_time THEN p.gols_casa ELSE p.gols_visita END) as max_gols_feitos,
            MAX(CASE WHEN p.id_casa = t.id_time THEN p.gols_visita ELSE p.gols_casa END) as max_gols_recebidos
        FROM Time_futebol t
        LEFT JOIN Partida p ON t.id_time = p.id_casa OR t.id_time = p.id_visitante
        GROUP BY t.id_time
    ) AS dados_partida ON tf.id_time = dados_partida.id_time;
END;

--

CREATE PROCEDURE Rankear_Tabela
AS
BEGIN
    DELETE FROM Ranking;

    ;WITH colocados AS (
        SELECT id_time,
               pontuacao,
               ROW_NUMBER() OVER (ORDER BY pontuacao DESC, NEWID()) AS posicao
        FROM Time_futebol
    )
    INSERT INTO Ranking (id_colocado, pontuacao)
    SELECT id_time, pontuacao
    FROM colocados
    WHERE posicao <= (SELECT COUNT(*) FROM Time_futebol);
END;
