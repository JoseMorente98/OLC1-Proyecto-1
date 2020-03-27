using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class ExpresionRegular
    {
        private String nombre;
        private ArrayList arrayListElemento;

        public ExpresionRegular()
        {
            ArrayListElemento = new ArrayList();
        }

        public ExpresionRegular(String nombre, ArrayList arrayListElemento)
        {
            this.Nombre = nombre;
            this.ArrayListElemento = arrayListElemento;
        }

        public void toString()
        {
            Console.WriteLine("Nombre: " + nombre);
            Console.WriteLine("ArrayListElemento:");
            foreach (object item in arrayListElemento)
            {
                Console.WriteLine("\t-" + item);
            }
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public ArrayList ArrayListElemento { get => arrayListElemento; set => arrayListElemento = value; }
    }
}
