using OLC1_Proyecto_1.AutomataFinito;
using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorExpresionRegular
    {
        private readonly static ControladorExpresionRegular instancia = new ControladorExpresionRegular();
        private ArrayList arrayListExpresionRegular = new ArrayList();

        public ControladorExpresionRegular()
        {
        }

        /**
         * SINGLETON 
         */
        public static ControladorExpresionRegular Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListExpresionRegular { get => arrayListExpresionRegular; set => arrayListExpresionRegular = value; }

        /**
         * OBTENER ELEMENTOS 
         */
        public void ObtenerElementos(String path)
        {
            String texto = "";
            ArrayList l = ControladorToken.Instancia.ArrayListTokens;
            for (int i = 0; i < l.Count; i++)
            {
                ArrayList temp = new ArrayList(); //ELEMENTOS DE LA EXPRESION
                Token t = (Token)l[i];

                if (t.Lexema.ToLower().Equals("conj"))
                {
                    for (int j = i + 1; j < l.Count; j++)
                    {
                        if (((Token)l[j]).Lexema.Equals(";"))
                        {
                            i = j;
                            break;
                        }
                    }
                }
                else
                {
                    if (t.Lexema.Equals(">"))
                    {
                        //BUSCA NOMBRE DE EXPRESION
                        for (int j = i; j > 0; j--)
                        {
                            Token a = (Token)l[j];
                            if (a.Descripcion.Equals("Identificador"))
                            {
                                texto = a.Lexema;
                                break;
                            }
                        }

                        Token t1 = (Token)l[i + 2]; //INICIO DE EXPRESION

                        if (t1 != null && !t1.Lexema.Equals("~") && !t1.Lexema.Equals(","))
                        {
                            //GUARDA ELEMENTOS DE EXPRESION
                            for (int j = i + 1; j < l.Count; j++)
                            {
                                Token t2 = (Token)l[j];
                                if (!t2.Lexema.Equals(";")) ///LIMITE EXPRESION
                                {
                                    if (!t2.Lexema.Equals("{") && !t2.Lexema.Equals("}"))
                                    {
                                        string a = t2.Lexema;
                                        temp.Add(a);
                                    }
                                }
                                else
                                {
                                    Agregar(texto, temp, path);
                                    i = j;
                                    break;
                                }
                            }
                        }
                    }

                }
            }
        }

        /**
         * AGREGAR EXPRESION REGULAR 
         */
        public void Agregar(String nombre, ArrayList arrayList, String path)
        {
            ArrayList vuelta = new ArrayList(); // array que va a almacenar los elementos en orden inverso
            for (int i = arrayList.Count - 1; i >= 0; i--)
            {
                vuelta.Add(arrayList[i]);
                ControladorNodo.Instancia.InsertStack(arrayList[i].ToString());

            }
            String ast = "";
            foreach (var item in vuelta)
            {
                ast = ast + " " + item;
            }

            ExpresionRegular re = new ExpresionRegular(nombre, vuelta);
            ArrayListExpresionRegular.Add(re);
            ControladorNodo.Instancia.Print(nombre, path);

            //EXPRESION DE PREFIJA A POSTFIJA
            ArrayList regularExpresion = ControladorNodo.Instancia.ConvertExpression(ControladorNodo.Instancia.getRoot());
            ArrayList regex = new ArrayList();
            try
            {
                regex = ControladorRegex.Instancia.NotacionInfijaAPostfija(regularExpresion);
            }
            catch (Exception)
            {
                Console.WriteLine("Error en la expresion D:");
            }

            string st = "";
            foreach (var item in regularExpresion)
            {
                st = st + item;
            }

            Console.WriteLine(nombre + "->" + st);

            //CONSTRUCCION AFN
            AutomataFinitoNoDeterminista aFN = new AutomataFinitoNoDeterminista();
            aFN.ConstruirAutomata(regex);
            Automata afn_result = aFN.Afn;
            ControladorThompson.Instancia.generarDOT("AFN", nombre, afn_result);

            //CONSTRUCCION AFD
            AutomataFinitoDeterminista AFD = new AutomataFinitoDeterminista();
            AFD.ConversionAFN(afn_result);
            Automata afd_result = AFD.Afd;

            //CONSTRUCCION AUTOMATA FINAL
            Automata afd_trampa = AFD.EliminarEstados(afd_result);
            ControladorThompson.Instancia.generarDOT("AFD", nombre, afd_trampa);

            //CONSTRUCCION TABLA
            ControladorThompson.Instancia.TableConstructor(nombre, path, afd_trampa);

            //GUARDA EL AUTOMATA PARA EVALUAR
            ControladorEvaluador.Instancia.Agregar(nombre, afd_trampa);

            //LIMPIAR ARBOL
            ControladorNodo.Instancia.Limpieza();
        }

    }
}
