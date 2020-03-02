using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorToken
    {
        private readonly static ControladorToken instancia = new ControladorToken();
        private ArrayList arrayListTokens = new ArrayList();
        private ArrayList arrayListErrors = new ArrayList();
        private int idToken = 1;
        private int idTokenError = 1;

        private ControladorToken()
        {
        }

        public static ControladorToken Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListTokens { get => arrayListTokens; set => arrayListTokens = value; }
        public ArrayList ArrayListErrors { get => arrayListErrors; set => arrayListErrors = value; }

        /**
         * AGREGAR TOKEN 
         */
        public void AgregarToken(int fila, int columna, string lexema, string descripcion)
        {
            Token token = new Token(idToken, lexema, descripcion, columna, fila);
            ArrayListTokens.Add(token);
            idToken++;
        }

        /**
         * AGREGAR TOKEN 
         */
        public void AgregarError(int fila, int columna, string lexema, string descripcion)
        {
            Token token = new Token(idTokenError, lexema, descripcion, columna, fila);
            ArrayListErrors.Add(token);
            idTokenError++;
        }

        /**
         * LIMPIAR VARIABLES 
         */
        public void Limpieza()
        {
            this.ArrayListErrors.Clear();
            this.ArrayListTokens.Clear();
            idToken = 1;
            idTokenError = 1;
        }
    }
}
