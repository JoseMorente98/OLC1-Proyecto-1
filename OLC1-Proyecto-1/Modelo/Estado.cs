using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class Estado
    {
        private int idEstado;
        private ArrayList transiciones = new ArrayList();

        public Estado(int idEstado, ArrayList transiciones)
        {
            this.IdEstado = idEstado;
            this.Transiciones = transiciones;
        }

        public Estado(int idEstado)
        {
            this.IdEstado = idEstado;
        }

        public Estado()
        {
        }

        public int IdEstado { get => idEstado; set => idEstado = value; }
        public ArrayList Transiciones { get => transiciones; set => transiciones = value; }

        public void agregarTransicion(Transicion t)
        {
            Transiciones.Add(t);
        }

        public override string ToString()
        {
            return IdEstado.ToString();
        }
    }
}
