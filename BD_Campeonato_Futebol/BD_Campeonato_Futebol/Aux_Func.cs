using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_Campeonato_Futebol
{
    internal class Aux_Func
    {

        // ---- ---- FUNÇÕES PARA FORMATAÇÕES ---- ---- //
        #region

        public static void Jump()
        {
            Console.WriteLine("\n\nDigite enter para continuar ...");
            Console.ReadKey();
            Console.Clear();
        }

        /* TOOL TIPS:
        Used for transitions between showed screens */
        #endregion
        // ---- ---- FUNÇÕES DE INPUTS E SHOWS ---- ---- //
        #region

        public static int InputValue(int op, int lower, int higher, string info)
        {
            int value = 0;
            if (op == 0)
            {
                do
                {
                    value = int.Parse(Console.ReadLine());
                    if ((value < lower || value > higher))
                    {
                        if (info == "")
                        {
                            Console.WriteLine($"\nDigite uma opção válida (entre {lower} e {higher})");
                        }
                        else
                        {
                            Console.WriteLine($"\n{info}");
                        }
                        Jump();
                    }
                } while (value < lower || value > higher);
            }
            if (op == 1)
            {
                do
                {
                    value = int.Parse(Console.ReadLine());
                    if ((value < lower))
                    {
                        if (info == "")
                        {
                            Console.WriteLine($"\nDigite uma opção válida, maior que {lower}");
                        }
                        else
                        {
                            Console.WriteLine($"\n{info}");
                        }
                        Jump();
                    }
                } while (value < lower);
            }
            if (op == 2)
            {
                do
                {
                    value = int.Parse(Console.ReadLine());
                    if ((value > higher))
                    {
                        if (info == "")
                        {
                            Console.WriteLine($"\nDigite uma opção válida, menor que {higher}");
                        }
                        else
                        {
                            Console.WriteLine($"\n{info}");
                        }
                        Jump();
                    }
                } while (value > higher);
            }
            return value;
        }
        /* TOOL TIPS:
        Make a user insert an correct value between both values (op == 0, Insert Between, op == 1 Insert Higher than Lower, op == 2 Insert Lower than Higher)
        May also add a different pattern answer in info, or pattern ("")
        */

        public static void Menu(string a, string b, string c, string d, string e, string f, string g, string h)
        {
            int exit_count = 3;
            Console.WriteLine($"\nEscolha uma das opções\n [1]  {a}\n [2]  {b}");
            if (c != "")
            {
                Console.WriteLine($" [{exit_count}]  {c}");
                exit_count++;
            }
            if (d != "")
            {
                Console.WriteLine($" [{exit_count}]  {d}");
                exit_count++;
            }
            if (e != "")
            {
                Console.WriteLine($" [{exit_count}]  {e}");
                exit_count++;
            }
            if (f != "")
            {
                Console.WriteLine($" [{exit_count}]  {f}");
                exit_count++;
            }
            if (g != "")
            {
                Console.WriteLine($" [{exit_count}]  {g}");
                exit_count++;
            }
            if (h != "")
            {
                Console.WriteLine($" [{exit_count}]  {h}");
                exit_count++;
            }
            Console.WriteLine($" [{exit_count}]  Sair");
        }
        /* TOOL TIPS:
        If want's to create an generic menu, just input phrases and the option will be showed whith and exit*/

        public static void PrintNavegableList(List<string> list)
        {
            int op = 0, indice = 0;
            do
            {
                Menu("Primeiro Registro", "Próximo Registro", "Registro Anterior", "Último Registro", "", "", "", "");
                op = InputValue(0, 1, 5, "");
                switch (op)
                {
                    case 1:
                        indice = 0;
                        break;
                    case 2:
                        if (indice == list.Count - 1)
                        {
                            indice = 0;
                        }
                        else
                        {
                            indice++;
                        }
                        break;
                    case 3:
                        if (indice == 0)
                        {
                            indice = list.Count - 1;
                        }
                        else
                        {
                            indice--;
                        }
                        break;
                    case 4:
                        indice = list.Count - 1;
                        break;
                }
                PrintFormatedElement(list[indice]);
                if (indice != 0)
                {
                    Console.Write("\n        <- ");
                }
                else
                {
                    Console.Write("\n           ");
                }
                Console.Write($"( {indice} / {list.Count} )");
                if (indice != list.Count)
                {
                    Console.Write(" ->        \n");
                }
                else
                {
                    Console.Write("           \n");
                }
                Jump();
            } while (op != 5);
        }
        /* TOOL TIPS:
           MAY NAVEGATE TO FIRST, LAST, NEXT OR PREVIOUS ELEMENT IN ANY KIND OF LIST
        */
        public static void PrintFormatedElement(string element)
        {
            Console.WriteLine($" {element}");
        }  // ----- > AUXILIAR FUNCTION FOR PRINT A NAVEGABLE LIST. USER MUST FORMAT AS NECESSARY !!!

        #endregion

        // ---- ---- FUNÇÕES PARA RETORNOS E CONSTANTES ---- ---- //
        #region

        public static string Today(int pattern, int format)
        {
            string txt = "";
            if (pattern == 1)
            {
                txt = DateTime.Now.ToString("MMddyyyy");
            }
            else
            {
                txt = DateTime.Now.ToString("ddMMyyyy");
            }
            if (format == 1)
            {
                txt = $"{txt.Substring(0, 2)}/{txt.Substring(2, 2)}/{txt.Substring(4, 4)}";
            }
            return txt;
        }
        /* TOOL TIPS:
        Retorna uma string com a data atual (formatada com "xx/xx/xxxx", se format == 1, ou xxxxxxxx se não)
        A data será ddMMaaaa se pattern for diferente de 1, caso seja 1, retorna padrão USA*/

        #endregion

        // ---- ---- FUNÇÕES PARA SISTEMA ---- ---- //

        public static int Menu_Campeonato()
        {
            int n=0;
            do
            {
                Menu("Revelar Campeão", "Mostrar Ranking", "Mostrar mais gols feitos", "Mostrar mais gols sofridos", "Mostrar partida com mais gols", "Mostrar Records de Gols", "", "");
                Console.Write(":  ");
                n = int.Parse(Console.ReadLine());
                if ((n < 1 || n > 7))
                {
                    Console.WriteLine("\nInsira uma opção Válida");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (n< 1 || n>7);
            return n;
        }

        public static List<int> Run_Partida(int max_gols, int casa, int visitante)
        {
            List<int> partidas = new List<int>(4); // pontuações 0 = casa sofrido, 1 = casa feito, 2 = visitante sofrido, 3 = visitante feito
            Random rand = new Random();
            int casa_feito = rand.Next(1, max_gols);
            int visitante_feito = rand.Next(1, max_gols);
            partidas.Add(visitante_feito);
            partidas.Add(casa_feito);
            partidas.Add(casa_feito);
            partidas.Add(visitante_feito);
            return partidas;
        }

    }

}
