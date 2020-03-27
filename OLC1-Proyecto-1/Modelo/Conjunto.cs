using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class Conjunto
    {
        private String nombre;
        private ArrayList elementos;

        public Conjunto()
        {
            ArrayListElemento = new ArrayList();
        }

        public Conjunto(String n, ArrayList ar)
        {
            this.Nombre = n;
            this.ArrayListElemento = ar;
        }

        public void toString()
        {
            Console.WriteLine("Nombre: " + nombre);
            Console.WriteLine("ArrayListElemento:");
            foreach (object item in elementos)
            {
                Console.WriteLine("\t-" + item);
            }
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public ArrayList ArrayListElemento { get => elementos; set => elementos = value; }
    }
}
