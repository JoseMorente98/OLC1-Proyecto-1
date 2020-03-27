using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class Transicion
    {
        private Estado inicio;
        private Estado fin;
        private String simbolo;

        public String Simbolo { get => simbolo; set => simbolo = value; }
        internal Estado Inicio { get => inicio; set => inicio = value; }
        internal Estado Fin { get => fin; set => fin = value; }

        public Transicion(Estado inicio, Estado fin, String simbolo)
        {
            this.Inicio = inicio;
            this.Fin = fin;
            this.Simbolo = simbolo;
        }

        public Transicion()
        {
        }

        public override string ToString()
        {
            return "(" + Inicio.IdEstado + "-" + Simbolo + "-" + Fin.IdEstado + ")";
        }

        public String DOT_String()
        {

            String text = Simbolo;
            if (Simbolo.Contains("\\"))
            {
                text = text.Replace("\\", "\\\\");
            }
            return (Inicio + " -> " + Fin + " [label=\"" + text.Replace("\"", "") + "\"];");
        }
    }
}
