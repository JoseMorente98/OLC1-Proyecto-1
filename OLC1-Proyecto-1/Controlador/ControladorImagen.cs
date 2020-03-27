using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorImagen
    {
        private readonly static ControladorImagen instancia = new ControladorImagen();
        private ArrayList arrayListImagen = new ArrayList();

        private ControladorImagen()
        {
        }

        /**
         * SINGLETON 
         */
        public static ControladorImagen Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListImagen { get => arrayListImagen; set => arrayListImagen = value; }

        /**
         * AGREGAR IMAGEN 
         */
        public void Agregar(string nombre, string tipo, string path)
        {
            Imagen imagen = new Imagen(nombre, tipo, path);
            ArrayListImagen.Add(imagen);
        }

        /**
         * BUSCAR IMAGEN 
         */
        public Imagen BuscarImagen(string nombre)
        {
            foreach(Imagen imagen in ArrayListImagen)
            {
                if(imagen.Nombre.Equals(nombre) )
                {
                    return imagen;
                }
            }
            return null;
        }

    }
}
