using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Campeonato_Futebol
{
    internal class Partida
    {
        /* 
        id_partida_time_casa int not null,
        id_partida_time_visitante int not null,
        gols_recebidos int null,
        gols_feitos int null,
        pontuacao_casa int null,
        pontuacao_visitante int null,
         */
        public int n_partida { get; set; }
        public int id_casa { get; set; }
        public int id_visitante { get; set; }
        public int gols_casa { get; set; }
        public int gols_visita { get; set; }
        int pontuacao_casa { get; set; }
        int pontuacao_visitante { get; set; }

        public Partida(int n_partida, int id_casa, int id_visitante, int gols_casa, int gols_visita, int pontuacao_casa, int pontuacao_visitante)
        {
            this.n_partida = n_partida;
            this.id_casa = id_casa;
            this.id_visitante = id_visitante;
            this.gols_casa = gols_casa;
            this.gols_visita = gols_visita;
            this.pontuacao_casa = pontuacao_casa;
            this.pontuacao_visitante = pontuacao_visitante;
        }

        
    }
}
