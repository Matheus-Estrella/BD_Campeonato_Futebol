using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Campeonato_Futebol
{
    internal class Connect
    {
        readonly string conexao = "Data Source = 127.0.0.1; Initial Catalog = Campeonato_Futebol;User Id=sa; Password=SqlServer2019!";


        public string Get_Path()
        {
            return this.conexao;
        }
    }
}
