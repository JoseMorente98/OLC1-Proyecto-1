using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class Evaluador
    {
        string nombreExpresion;
        Automata afd;

        public Evaluador(string nombreExpresion, Automata afd)
        {
            this.nombreExpresion = nombreExpresion;
            this.afd = afd;
        }

        public string NombreExpresion { get => nombreExpresion; set => nombreExpresion = value; }
        internal Automata Afd { get => afd; set => afd = value; }
    }
}
