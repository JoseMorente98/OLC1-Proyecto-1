using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorConjunto
    {
        private readonly static ControladorConjunto instancia = new ControladorConjunto();
        private ArrayList arrayListConjuntos = new ArrayList();

        public ControladorConjunto()
        {
        }

        /**
         * SINGLETON 
         */
        public static ControladorConjunto Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListConjuntos { get => arrayListConjuntos; set => arrayListConjuntos = value; }

        /**
         * AGREGAR CONJUNTOS 
         */
        public void Agregar(String name, ArrayList arrayList, bool isInterval)
        {
            ArrayList arrayListNuevosElementos = new ArrayList();
            Conjunto conjunto = null;

            if (isInterval)
            {
                for (int i = 0; i < arrayList.Count; i++)
                {
                    if (arrayList[i].Equals("~"))
                    {
                        arrayListNuevosElementos = getNewElements(((String)arrayList[i - 1])[0], ((String)arrayList[i + 1])[0]);
                        break;
                    }
                }
                conjunto = new Conjunto(name, arrayListNuevosElementos);
            }
            else
            {
                conjunto = new Conjunto(name, arrayList);
            }
            ArrayListConjuntos.Add(conjunto);
        }

        /**
         * LIMPIAR CONJUNTOS 
         */
        public void Limpieza()
        {
            ArrayListConjuntos.Clear();
        }

        /**
         * GET CONJUNTOS 
         */
        public ArrayList GetArray()
        {
            return ArrayListConjuntos;
        }

        /**
         * ENSAMBLAR CONJUNTOS
         */
        public void ArmarConjuntos()
        {
            bool isInterval = false;
            ArrayList arrayListTokens = ControladorToken.Instancia.ArrayListTokens;
            for (int i = 0; i < arrayListTokens.Count; i++)
            {
                Token tok = (Token)arrayListTokens[i];
                if (tok.Lexema.ToLower().Equals("conj"))
                {
                    String name = ((Token)arrayListTokens[i + 2]).Lexema;

                    int pos = i + 5;
                    ArrayList elements = new ArrayList();
                    for (int j = pos; j < arrayListTokens.Count; j++)
                    {
                        Token t = (Token)arrayListTokens[j];
                        if (!t.Lexema.Equals(";"))
                        {
                            if (!t.Lexema.Equals(","))
                            {
                                if (t.Lexema.Equals("~"))
                                {
                                    isInterval = true;
                                }
                                string cadena = t.Lexema.Trim('"');
                                if (cadena.Length > 1)
                                {
                                    //SI VIENE [:TODO:] SE SEPARA LA CADENA CARACTER POR CARACTER
                                    foreach (String e in TODOElements(cadena))
                                    {
                                        elements.Add(e);
                                    }
                                }
                                else
                                {
                                    //SOLO VIENE UN CARACTER
                                    elements.Add(cadena);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    Agregar(name, elements, isInterval);
                }
            }
        }

        /**
         * GET NUEVOS ELEMENTOS DEL CONJUNTO
         */
        public ArrayList getNewElements(char ant, char sig)
        {
            ArrayList arrayList = new ArrayList();

            //DIGITOS
            if (char.IsDigit(ant))
            {


                for (int i = int.Parse(ant.ToString()); i < int.Parse(sig.ToString()) + 1; i++)
                {
                    arrayList.Add(i.ToString());
                }
                return arrayList;
               
            }
            //LETRAS
            else if (char.IsLetter(ant))
            {
                int initValue = (int)ant;
                int endValue = (int)sig;

                for (int i = initValue; i <= endValue; i++)
                {
                    arrayList.Add(((char)i).ToString());
                }
                return arrayList;                
            }
            //ASCII CODES 32 TO 125
            else if ((int)ant >= 32 && (int)sig <= 125)
            {
                for (int i = (int)ant; i <= (int)sig; i++)
                {

                    if (!char.IsDigit(ant) && !char.IsDigit(sig) && !char.IsLetter(ant) && !char.IsLetter(sig))
                    {
                        arrayList.Add(((char)i).ToString());
                    }
                }
                return arrayList;
            }
            return null;
        }


        /**
         * GET ELEMENTOS DEL CONJUNTO
         */
        public ArrayList GetElemntsOfSet(String setName)
        {
            foreach (Conjunto conjunto in ArrayListConjuntos)
            {
                if (conjunto.Nombre.Equals(setName))
                {
                    return conjunto.ArrayListElemento;
                }
            }
            return null;
        }

        /**
         * MANEJO DE TODO
         */
        public ArrayList TODOElements(String cadena)
        {
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < cadena.Length; i++)
            {
                arrayList.Add(cadena[i].ToString());
            }

            return arrayList;
        }

        /**
         * MOSTRAR CONJUNTOS 
         */
        public void ShowSets()
        {
            foreach (Conjunto conjunto in ArrayListConjuntos)
            {
                Console.WriteLine(conjunto.Nombre);
                foreach (String e in conjunto.ArrayListElemento)
                {
                    Console.WriteLine("-" + e);
                }
            }
        }
    }
}
