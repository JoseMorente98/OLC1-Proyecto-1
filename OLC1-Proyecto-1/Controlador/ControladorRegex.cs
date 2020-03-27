using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorRegex
    {
        private readonly static ControladorRegex instancia = new ControladorRegex();

        public ControladorRegex() { }

        /**
          * INSTANCIA
          */
        public static ControladorRegex Instancia
        {
            get
            {
                return instancia;
            }
        }

         /**
          * OBTENER LA PRECEDENCIA DEL CARACTER 
          */
        private int ObtenerPrecedencia(String c)
        {

            if (c.Equals("("))
            {
                return 1;
            }
            else if (c.Equals("|"))
            {
                return 2;
            }
            else if (c.Equals("."))
            {
                return 3;
            }
            else if (c.Equals("*") || c.Equals("+") || c.Equals("?"))
            {
                return 4;
            }
            else
            {
                return 6;
            }
        }

        /**
          * RETORNAR EXPRESION REGULAR 
          */
        public ArrayList FormatoRegex(ArrayList regex)
        {
            ArrayList arrayListRegex = new ArrayList();
            List<String> listOperadores = new List<String>();
            listOperadores.Add("|");
            listOperadores.Add("?");
            listOperadores.Add("+");
            listOperadores.Add("*");
            List<String> listOperadorBinario = new List<String>();
            listOperadorBinario.Add("|");

            //RECORRER CADENA
            for (int i = 0; i < regex.Count; i++)
            {
                String c1 = (String)regex[i];

                if (i + 1 < regex.Count)
                {
                    String c2 = (String)regex[i + 1];
                    arrayListRegex.Add(c1);

                    if (!c1.Equals("(") && !c2.Equals(")") && !listOperadores.Contains(c2) && !listOperadorBinario.Contains(c1))
                    {
                        arrayListRegex.Add(".");
                    }
                }
            }
            arrayListRegex.Add(regex[regex.Count - 1]);
            return arrayListRegex;
        }

        /**
         * RETORNAR EN MODO POSFIJO 
         */
        public ArrayList NotacionInfijaAPostfija(ArrayList regex)
        {
            ArrayList arrayListPostfija = new ArrayList();
            Stack<String> stackRegex = new Stack<String>();

            ArrayList arrayListFormatoRegex = FormatoRegex(regex);
            foreach (String c in arrayListFormatoRegex)
            {
                switch (c)
                {
                    case "(":
                        stackRegex.Push(c);
                        break;
                    case ")":
                        while (!stackRegex.Peek().Equals("("))
                        {
                            arrayListPostfija.Add(stackRegex.Pop());
                        }
                        stackRegex.Pop();
                        break;
                    default:
                        while (stackRegex.Count() > 0)
                        {
                            String peekedChar = stackRegex.Peek();
                            int charArribaPrecedente = ObtenerPrecedencia(peekedChar);
                            int currentCharPrecedence = ObtenerPrecedencia(c);

                            if (charArribaPrecedente >= currentCharPrecedence)
                            {
                                arrayListPostfija.Add(stackRegex.Pop());
                            }
                            else
                            {
                                break;
                            }
                        }
                        stackRegex.Push(c);
                        break;
                }
            }

            while (stackRegex.Count() > 0)
                arrayListPostfija.Add(stackRegex.Pop());
            return arrayListPostfija;
        }
    }
}
