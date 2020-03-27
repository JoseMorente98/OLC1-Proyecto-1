using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Modelo
{
    class Imagen
    {
        private String nombre;
        private String tipo;
        private String path;

        public string Tipo { get => tipo; set => tipo = value; }
        public string Path { get => path; set => path = value; }
        public string Nombre { get => nombre; set => nombre = value; }

        public Imagen(string nombre, string tipo, string path)
        {
            Nombre = nombre;
            Tipo = tipo;
            Path = path;
        }

        public Imagen()
        {
        }

        public override string ToString()
        {
            return Nombre;
        }

    }
}
