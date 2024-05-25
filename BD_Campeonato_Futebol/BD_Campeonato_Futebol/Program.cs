
using BD_Campeonato_Futebol;
using System.Collections.Generic;
using System.Data.SqlClient;
using static BD_Campeonato_Futebol.Aux_Func;

#region
/*
Create Table Time_futebol(
id_time int identity(1,1),
nome varchar(30) not null,
apelido varchar(10) not null,
fundacao Date not null,
pontuacao int null,
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
	gols_recebidos int null,
	gols_feitos int null,
	primary key (colocado),
	FOREIGN KEY (id_colocado) REFERENCES Time_futebol(id_time)
)
 */
#endregion  
// CONSTRUÇÃO DO BANCO DE DADOS SQL



int MAXGOLS = 18;
//36
int NUMTIMES = 5;
int MAXPARTIDAS = (NUMTIMES-1)*NUMTIMES;

Connect conn = new Connect();
SqlConnection conexaobanco = new SqlConnection(conn.Get_Path());
// Rodar Jogo
int stop = MAXPARTIDAS;
int visitante = 1;
int casa = NUMTIMES;
#region CONFIGURANDO TABELAS PARA QUE CONVERSEM COM O CÓDIGO E ESTEJAM ZERADAS
string cmd_create = @"
    IF OBJECT_ID('Ranking', 'U') IS NOT NULL
    BEGIN
        DROP TABLE Ranking;
    END;

    IF OBJECT_ID('Partida', 'U') IS NOT NULL
    BEGIN
        DROP TABLE Partida;
    END;

    IF OBJECT_ID('Time_futebol', 'U') IS NOT NULL
    BEGIN
        DROP TABLE Time_futebol;
    END;

    CREATE TABLE Time_futebol(
        id_time INT IDENTITY(1,1),
        nome VARCHAR(30) NOT NULL,
        apelido VARCHAR(10) NOT NULL,
        fundacao DATE NOT NULL,
        pontuacao INT NULL,
        gols_feitos INT NULL,
        gols_recebidos INT NULL,
        gols_feitos_record INT NULL,
        gols_recebidos_record INT NULL,
        PRIMARY KEY (id_time)
    );

    CREATE TABLE Partida(
        n_partida INT IDENTITY (1,1),
        id_casa INT NOT NULL,
        id_visitante INT NOT NULL,
        gols_casa INT NULL,
        gols_visita INT NULL,
        pontuacao_casa INT NULL,
        pontuacao_visitante INT NULL,
        PRIMARY KEY(n_partida, id_casa, id_visitante)
    );

    CREATE TABLE Ranking(
        colocado INT IDENTITY(1,1) NOT NULL,
        id_colocado INT NOT NULL,
        pontuacao INT NULL,
        PRIMARY KEY (colocado),
        FOREIGN KEY (id_colocado) REFERENCES Time_futebol(id_time)
    );

    INSERT INTO Time_futebol (nome, apelido, fundacao) VALUES
    ('Unidos do Tio Paulo', 'UTP', '2024-04-16'),
    ('Alexa Futebol CLub', 'AFC', '2024-03-15'),
    ('Java Hatters Team', 'JHT', '1995-05-23'),
    ('Dragon Ball Z', 'DBZ', '1989-04-26'),
    ('Cavaleiros do Zodiaco', 'CDZ', '1986-10-11');
";
conexaobanco.Open();
SqlCommand cmd_Inicializar_Tabelas;
using (cmd_Inicializar_Tabelas = new SqlCommand(cmd_create, conexaobanco))
{
    cmd_Inicializar_Tabelas.CommandType = System.Data.CommandType.Text;
    cmd_Inicializar_Tabelas.ExecuteNonQuery();
}
conexaobanco.Close();
#endregion
#region SALVANDO PARTIDAS NO BANCO DE DADOS
conexaobanco.Open();
do
{
    for(int i= 1; i<= NUMTIMES; i++)
    {
        if(i != visitante) // Quando ocorrer uma partida...
        {
            int pontuacao_casa = 0;
            int pontuacao_visitante = 0;
            List<int> jogo = Run_Partida(MAXGOLS, casa, visitante);
            // Transferindo valores para o BD
            SqlCommand cmd_partida = new SqlCommand();
            cmd_partida.CommandText = "INSERT INTO Partida(id_casa,id_visitante,gols_casa,gols_visita) VALUES(@id_casa,@id_visitante,@gols_casa,@gols_visita)";
            #region
            SqlParameter id_casa = new SqlParameter("@id_casa", System.Data.SqlDbType.Int);
            SqlParameter id_visitante = new SqlParameter("@id_visitante", System.Data.SqlDbType.Int);
            SqlParameter gols_casa = new SqlParameter("@gols_casa", System.Data.SqlDbType.Int);
            SqlParameter gols_visita = new SqlParameter("@gols_visita", System.Data.SqlDbType.Int);
            #endregion
            // SqlParameters
            #region
            id_casa.Value = casa;
            id_visitante.Value = i;
            gols_casa.Value = jogo[1];
            gols_visita.Value = jogo[0];
            #endregion
            // Sql values
            #region
            cmd_partida.Parameters.Add(id_casa);
            cmd_partida.Parameters.Add(id_visitante);
            cmd_partida.Parameters.Add(gols_casa);
            cmd_partida.Parameters.Add(gols_visita);
            #endregion
            // Sql Add
            cmd_partida.Connection = conexaobanco;
            cmd_partida.ExecuteNonQuery();
            stop--;
        }
    }
    casa--;
    visitante++;
} while (stop != 0);
SqlCommand cmd_pontuar;
using (cmd_pontuar = new SqlCommand("Pontuar_Times", conexaobanco))
{
    cmd_pontuar.CommandType = System.Data.CommandType.StoredProcedure;
    cmd_pontuar.ExecuteNonQuery();
}
using (cmd_pontuar = new SqlCommand("Resultado_Campeonato", conexaobanco))
{
    cmd_pontuar.CommandType = System.Data.CommandType.StoredProcedure;
    cmd_pontuar.ExecuteNonQuery();
}
using (cmd_pontuar = new SqlCommand("Rankear_Tabela", conexaobanco))
{
    cmd_pontuar.CommandType = System.Data.CommandType.StoredProcedure;
    cmd_pontuar.ExecuteNonQuery();
}



conexaobanco.Close();

#endregion



#region SALVANDO OS OBJETOS DE TIMES_FUTEBOL E PARTIDAS EXTRAÍDOS DO BD
conexaobanco.Open();
SqlCommand cmd_obj = new SqlCommand("SELECT id_time, nome, apelido, fundacao, pontuacao, gols_feitos, gols_recebidos, gols_feitos_record, gols_recebidos_record FROM Time_futebol", conexaobanco);
List<Time_futebol> participantes = new List<Time_futebol>();
using (SqlDataReader reader = cmd_obj.ExecuteReader())
{
    while (reader.Read())
    {
        int id_time = reader.GetInt32(0);
        string nome = reader.GetString(1);
        string apelido = reader.GetString(2);
        DateOnly fundacao = DateOnly.FromDateTime(reader.GetDateTime(3));
        int pontuacao = reader.GetInt32(4);
        int gols_feitos = reader.GetInt32(5);
        int gols_recebidos = reader.GetInt32(6);
        int gols_feitos_record = reader.GetInt32(7);
        int gols_recebidos_record = reader.GetInt32(8);
        Time_futebol time = new Time_futebol(id_time, nome, apelido, fundacao, pontuacao, gols_feitos, gols_recebidos, gols_feitos_record, gols_recebidos_record);
        participantes.Add(time);
    }
}
cmd_obj = new SqlCommand("SELECT  n_partida, id_casa, id_visitante, gols_casa, gols_visita, pontuacao_casa, pontuacao_visitante FROM Partida", conexaobanco);
List<Partida> campeonato = new List<Partida>();
using (SqlDataReader reader = cmd_obj.ExecuteReader())
{
    while (reader.Read())
    {
        int n_partida = reader.GetInt32(0);
        int id_casa = reader.GetInt32(1);
        int id_visitante = reader.GetInt32(2);
        int gols_casa = reader.GetInt32(3);
        int gols_visita = reader.GetInt32(4);
        int pontuacao_casa = reader.GetInt32(5);
        int pontuacao_visitante = reader.GetInt32(6);
        Partida jogo = new Partida(n_partida, id_casa, id_visitante, gols_casa, gols_visita, pontuacao_casa, pontuacao_visitante);
        campeonato.Add(jogo);
    }
}
conexaobanco.Close();
#endregion

int[] ranks = Vetor_Ranks(participantes);
#region MENU DE OPÇÕES
int op = 0;
conexaobanco.Open();
SqlCommand cmd_Menu;
do
{
    string Comando_Selecionado = "";
    Jump();
    op = Menu_Campeonato();
    switch (op)
    {
        case 1:
            Imprimir_Rank(1, 1, participantes, ranks);
            break;
        case 2:
            Imprimir_Rank(0, 1, participantes, ranks);
            break;
        case 3:
            Imprimir_Gols(0, participantes);
            break;
        case 4:
            Imprimir_Gols_Partida(0, participantes, campeonato);
            break;
        case 5:
            Imprimir_Gols_Partida(1, participantes, campeonato);
            break;
        case 6:
            Imprimir_Gols(1, participantes);
            break;
    }
} while (op != 7);
conexaobanco.Close();
#endregion

static int[] Vetor_Ranks(List<Time_futebol> participantes)
{
    int[] ranking = new int[participantes.Count];

    for (int i = 0; i < participantes.Count; i++)
    {
        ranking[i] = participantes[i].id_time;
    }

    Array.Sort(ranking, (a, b) => participantes[b - 1].pontuacao.CompareTo(participantes[a - 1].pontuacao));

    return ranking;
}
static void Imprimir_Rank(int amostragem, int colocado, List<Time_futebol> participantes, int[] ranks)
{
    int id_rank = 0;
    int pontuacao_rank = 0;
    if(amostragem == 0)
    {
        colocado = 1;
    }
    for(int i=0; i<participantes.Count();i++)
    {
        if(i == colocado-1)
        {
            int id_colocado = participantes[i].id_time;
            foreach(Time_futebol time in participantes)
            {
                if(time.id_time == id_colocado)
                {
                    Console.Write($"\n[{i + 1}º lugar] ==> {time.nome} [{time.apelido},{time.fundacao}] fez" +
                        $" {time.pontuacao} pontos (gols feitos: {time.gols_feitos},gols sofridos: {time.gols_recebidos})");
                }
            }
        }
        if(amostragem == 0)
        {
            colocado++;
        }
    }
}



static void Imprimir_Gols(int opcao, List<Time_futebol> participantes) // Mais gols = opcao 0, Todos Gols Rank = opcao 1
{
    int mais_gols = 0;
    int id_mais_gols = 0;
    int count = 0;
    foreach (Time_futebol time in participantes)
    {
        if (time.gols_feitos_record > mais_gols)
        {
            mais_gols = time.gols_feitos_record;
            id_mais_gols = time.id_time;
        }
        if (opcao == 0)
        {
            Console.WriteLine($"\nO time: {participantes[count].nome} " +
                $"[{participantes[count].apelido}], fez {participantes[count].gols_feitos_record} gols em uma partida");
            break;
        }
    }
    if (opcao == 1)
    {
        for (int i = 0; i < participantes.Count(); i++)
        {
            if (participantes[i].id_time == id_mais_gols)
            {
                break;
            }
            count++;
        }
        Console.WriteLine($"\nO time que mais fez gols foi: {participantes[count].nome} " +
            $"[{participantes[count].apelido}], com {participantes[count].gols_feitos_record}");

    }
}




static void Imprimir_Gols_Partida(int op, List<Time_futebol> participantes, List<Partida> campeonato)
{
    /*
     Quem é que tomou mais gols no campeonato? opcao 0
     Qual é o jogo que teve mais gols? opcao 1
    */
    if(op == 0)
    {
        int recebeu_mais_gols = 0;
        foreach(Time_futebol time in participantes)
        {
            if(recebeu_mais_gols < time.gols_recebidos)
            {
                recebeu_mais_gols = time.gols_recebidos;
            }
        }
        foreach(Time_futebol time in participantes)
        {
            if(time.gols_recebidos == recebeu_mais_gols)
            {
                Console.WriteLine($"\nO time que recebeu mais gols no campeonato foi {time.nome}, com {time.gols_recebidos} gols recebidos");
            }
        }
    }
    if(op == 1)
    {
        int jogo_mais_gols = 0;
        int n_partida = 0;
        foreach(Partida jogo in campeonato)
        {
            if (jogo.gols_casa > jogo_mais_gols)
            {
                jogo_mais_gols = jogo.gols_casa;
            }
            if (jogo.gols_visita > jogo_mais_gols)
            {
                jogo_mais_gols = jogo.gols_visita;
            }
        }
        foreach(Partida jogo in campeonato)
        {
            if(jogo.gols_visita  == jogo_mais_gols || jogo.gols_casa == jogo_mais_gols)
            {
                int next= 0;
                Console.Write($"\nO jogo com mais gols feitos foi o {jogo.n_partida}º, entre ");
                foreach (Time_futebol time in participantes)
                {
                    if(time.id_time == jogo.id_casa)
                    {
                        Console.Write($" {time.nome} ");
                        next++;
                    }
                    if(next == 1)
                    {
                        Console.Write($" e ");
                    }
                    if (time.id_time == jogo.id_visitante)
                    {
                        Console.Write($" {time.nome} ");
                        next++;
                    }
                }
                break;
            }
        }
    }

}

#region
try
{

}
catch (Exception e)
{
    Console.Clear();
    Console.WriteLine("\n Ocorreu um Erro");
    Console.WriteLine(e.ToString());
}
#endregion
// Try catch de conexão -> Utilizar em cada operação com o BD isoladamente    ----> LEMBRETE IMPORTANTE
