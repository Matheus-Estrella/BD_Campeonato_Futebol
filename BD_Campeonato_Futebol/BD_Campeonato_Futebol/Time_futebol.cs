using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Campeonato_Futebol
{
    internal class Time_futebol
    {
        /*
        id_time int identity(1,1),
        nome varchar(30) not null,
        apelido varchar(10) not null,
        fundacao Date not null,
        PRIMARY KEY (id_time)
        */
        public int id_time { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public DateOnly fundacao { get; set; }
        public int pontuacao { get; set; }
        public int gols_feitos { get; set; }
        public int gols_recebidos { get; set; }
        public int gols_feitos_record { get; set; }
        public int gols_recebidos_record { get; set; }

        //DateOnly fundacao;
        public Time_futebol(){}

        public Time_futebol(int id_time, string nome, string apelido, DateOnly fundacao,int pontuacao, int gols_feitos, int gols_recebidos, int gols_feitos_record, int gols_recebidos_record)
        {
            this.id_time = id_time;
            this.nome = nome;
            this.apelido = apelido;
            this.fundacao = fundacao;
            this.pontuacao = pontuacao;
            this.gols_feitos = gols_feitos;
            this.gols_recebidos = gols_recebidos;
            this.gols_feitos_record = gols_feitos_record;
            this.gols_recebidos_record = gols_recebidos_record;
        }

        public override string? ToString()
        {
            return $"Time [{this.id_time}]: {this.nome} [{this.apelido}]";
        }
    }
}
