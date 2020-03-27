using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorNodo
    {
        private readonly static ControladorNodo instancia = new ControladorNodo();
        //ELEMENTOS DE EXPRESION REGULAR
        private ArrayList arrayListExpresionRegular = new ArrayList();
        //REORDENAR ELEMENTOS
        private ArrayList arrayListTemp = new ArrayList(); 
        //NOMBRE DE ELEMENTOS
        private ArrayList arrayListElementos = new ArrayList();
        //LISTA DE SIGUIENTES
        private ArrayList arrayListSiguientes = new ArrayList(); 
        //LISTA TABLA DE TRANSICION
        private ArrayList arrayListNewSiguientes = new ArrayList();
        //ARBOL BINARIO
        private Stack stackArbolBinario = new Stack();
        int index = 0;
        int cantidad = 0;
        Nodo raizArbol = null;
        ArrayList regularExpression = new ArrayList();

        /**
         * SINGLETON 
         */
        public static ControladorNodo Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListExpresionRegular { get => arrayListExpresionRegular; set => arrayListExpresionRegular = value; }
        public ArrayList ArrayListTemp { get => arrayListTemp; set => arrayListTemp = value; }
        public ArrayList ArrayListElementos { get => arrayListElementos; set => arrayListElementos = value; }
        public ArrayList ArrayListSiguientes { get => arrayListSiguientes; set => arrayListSiguientes = value; }
        public ArrayList ArrayListNewSiguientes { get => arrayListNewSiguientes; set => arrayListNewSiguientes = value; }
        public Stack StackArbolBinario { get => stackArbolBinario; set => stackArbolBinario = value; }
        public ArrayList RegularExpression { get => regularExpression; set => regularExpression = value; }
        internal Nodo RaizArbol { get => raizArbol; set => raizArbol = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }

        public ControladorNodo()
        {
        }

        /**
         * LIMPIAR ELEMENTOS 
         */
        public void Limpieza()
        {
            this.RaizArbol = null;
            this.Cantidad = 0;
            this.ArrayListSiguientes.Clear();
            this.ArrayListExpresionRegular.Clear();
            this.ArrayListTemp.Clear();
            this.RegularExpression.Clear();
        }

        /**
         * AGREGAR ELEMENTOS 
         */
        public void Agregar(String element)
        {
            ArrayListExpresionRegular.Add(element);
        }

        /**
         * IMPRIMIR ARBOL
         */
        public void Print(String name, String path)
        {
            if (StackArbolBinario.Count > 0)
            {
                Nodo nodoIzquierda = (Nodo)StackArbolBinario.Pop();
                RaizArbol = nodoIzquierda;
                Nodo temp = RaizArbol;
                Nodo nodoDerecha = new Nodo("#");
                Nodo dot = new Nodo(".", nodoDerecha, temp, false);

                NodoHoja(dot);
                TomarDecision(dot);
                EstablecerRoot(dot);

                dot.print(path, name + "Tree");
                index++;
            }
        }

        /**
         * INSERTAR  ELEMENTOS  AL ARBOL
         */
        public void InsertStack(String s)
        {

            Nodo nuevoNodo = new Nodo();
            Nodo nodoDerecha = new Nodo();
            Nodo nodoIzquierda = new Nodo();
            bool esAnulable = false;

            if (s.Equals("|") || s.Equals("."))
            {
                nodoIzquierda = (Nodo)StackArbolBinario.Pop();

                if (StackArbolBinario.Count != 0)
                {
                    nodoDerecha = (Nodo)StackArbolBinario.Pop();
                }
                else
                {
                    nodoDerecha = null;
                }

                if (s.Equals("|"))
                {
                    if (nodoIzquierda.EsAnulable || nodoDerecha.EsAnulable)
                    {
                        esAnulable = true;
                    }
                }
                else if (s.Equals("."))
                {
                    if (nodoIzquierda.EsAnulable && nodoDerecha.EsAnulable)
                    {
                        esAnulable = true;
                    }
                }
                nuevoNodo = new Nodo(s, nodoDerecha, nodoIzquierda, esAnulable);
                esAnulable = false;
                StackArbolBinario.Push(nuevoNodo);

            }
            else if (s.Equals("*") || s.Equals("+") || s.Equals("?"))
            {
                nodoIzquierda = (Nodo)StackArbolBinario.Pop();

                if (s.Equals("*") || s.Equals("?"))
                {
                    esAnulable = true;
                }
                else if (s.Equals("+"))
                {
                    if (nodoIzquierda.EsAnulable)
                    {
                        esAnulable = true;
                    }
                    else
                    {
                        esAnulable = false;
                    }
                }
                else
                {
                    esAnulable = true;
                }
                Nodo nodo = new Nodo(s, null, nodoIzquierda, esAnulable);
                esAnulable = false;
                StackArbolBinario.Push(nodo);
            }
            else
            {
                Nodo nodo = new Nodo(s);
                StackArbolBinario.Push(nodo);
            }
        }


        /**
         * AGREGAR ELEMENTOS HOJA 
         */
        private void NodoHoja(Nodo nodo)
        {
            if (nodo != null)
            {
                if (nodo.HijoIzquierdo == null && nodo.HijoDerecho == null)
                {
                    Cantidad++;

                    //Le ingresa el esAnulable o no
                    if (!nodo.Elemento.Equals("\"ε\""))
                    {
                        nodo.EsAnulable = false;
                        nodo.Primero = Cantidad.ToString();
                        nodo.Ultimo = Cantidad.ToString();
                    }
                    else
                    {
                        nodo.EsAnulable = true;
                        nodo.Primero = "\"Ø\"";
                        nodo.Ultimo = "\"Ø\"";
                    }
                    nodo.EsHoja = true;
                }
                NodoHoja(nodo.HijoIzquierdo);
                NodoHoja(nodo.HijoDerecho);
            }
        }

        string nuevoTexo = "";

        /**
         * TOMA DE DECISION
         */
        private void TomarDecision(Nodo nodo)
        {
            if (nodo != null)
            {
                if (nodo.Elemento.Equals("|"))
                {
                    Alternacion(nodo);
                }
                else if ((nodo.Elemento.Equals("*") || nodo.Elemento.Equals("?") || nodo.Elemento.Equals("+")) && nodo.EsHoja == false)
                {
                    Cuantificacion(nodo);
                }
                else if (nodo.Elemento.Equals(".") && nodo.EsHoja == false)
                {
                    setDotAntNext(nodo);
                }
            }
        }

        /**
         * ALTERNACION |
         */
        private void Alternacion(Nodo nodo)
        {
            if (nodo != null)
            {
                nodo.Primero = nodo.HijoIzquierdo.Primero + "," + nodo.HijoDerecho.Primero;
                nodo.Ultimo = nodo.HijoIzquierdo.Ultimo + "," + nodo.HijoDerecho.Ultimo;
                TomarDecision(nodo.HijoIzquierdo);
                TomarDecision(nodo.HijoDerecho);
            }
        }

        /**
         * CUANTIFICACION ? Y +
         */
        private void Cuantificacion(Nodo nodo)
        {
            if (nodo != null)
            {
                nodo.Primero = nodo.HijoIzquierdo.Primero;
                nodo.Ultimo = nodo.HijoIzquierdo.Ultimo;
                TomarDecision(nodo.HijoIzquierdo);
                TomarDecision(nodo.HijoDerecho);
            }
        }


        private void setDotAntNext(Nodo nodo)
        {
            if (nodo != null)
            {
                if (nodo.HijoIzquierdo.Elemento.Equals(".")
                        || nodo.HijoDerecho.Elemento.Equals(".")
                        || nodo.HijoDerecho.Elemento.Contains("0")
                        || nodo.HijoIzquierdo.Elemento.Contains("0")
                        || nodo.Primero.Contains("0")
                        || nodo.Ultimo.Contains("0"))
                {
                    TomarDecision(nodo.HijoIzquierdo);
                    TomarDecision(nodo.HijoDerecho);
                }
                if (nodo.HijoIzquierdo.EsAnulable)
                {
                    nodo.Primero = nodo.HijoIzquierdo.Primero + "," + nodo.HijoDerecho.Primero;
                }
                else
                {
                    nodo.Primero = nodo.HijoIzquierdo.Primero;
                }
                if (nodo.HijoDerecho.EsAnulable)
                {
                    nodo.Ultimo = nodo.HijoIzquierdo.Ultimo + "," + nodo.HijoDerecho.Ultimo;
                }
                else
                {
                    nodo.Ultimo = nodo.HijoDerecho.Ultimo;
                }

                TomarDecision(nodo.HijoIzquierdo);
                TomarDecision(nodo.HijoDerecho);
            }
        }
        
        /**
         * ESTABLECER RAIZ
         */
        public void EstablecerRoot(Nodo nodo)
        {
            if (nodo.HijoIzquierdo.EsAnulable)
            {
                nodo.Primero = nodo.HijoIzquierdo.Primero + "," + nodo.HijoDerecho.Primero;
            }
            else
            {
                nodo.Primero = nodo.HijoIzquierdo.Primero;
            }
            nodo.Ultimo = nodo.HijoDerecho.Ultimo;
        }

        /**
         * GET RAIZ ARBOL
         */
        public Nodo getRoot()
        {
            return RaizArbol;
        }

        /**
         * CONVERTIR EXPRESION EN PRE A POSTFIJA 
         */
        public ArrayList ConvertExpression(Nodo nodoRaiz)
        {

            if (nodoRaiz != null)
            {
                if (nodoRaiz.HijoIzquierdo == null && nodoRaiz.HijoDerecho == null)
                {
                    string b = nodoRaiz.Elemento;
                    string a = "";
                    if (nodoRaiz.Elemento.Equals("\".\"") ||
                        nodoRaiz.Elemento.Equals("\"*\""))
                    {
                        a = nodoRaiz.Elemento;
                    }
                    else if (nodoRaiz.Elemento == "\nodo")
                    {
                        a = ('\n').ToString();
                    }
                    else if (nodoRaiz.Elemento.Equals("\t"))
                    {
                        a = ('\t').ToString();

                    }
                    else if (nodoRaiz.Elemento.Equals("\r"))
                    {
                        a = ('\r').ToString();
                    }
                    else
                    {
                        a = nodoRaiz.Elemento.Trim('"');
                    }

                    RegularExpression.Add(a);
                }
                else
                {
                    if (nodoRaiz.Elemento.Equals("+"))
                    {
                        RegularExpression.Add("(");
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        RegularExpression.Add("(");
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        RegularExpression.Add(")");
                        RegularExpression.Add("*");
                        RegularExpression.Add(")");
                    }
                    if (nodoRaiz.Elemento.Equals("*"))
                    {
                        RegularExpression.Add("(");
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        RegularExpression.Add(")");
                        RegularExpression.Add("*");
                    }
                    else if (nodoRaiz.Elemento.Equals("|"))
                    {
                        RegularExpression.Add("(");
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        RegularExpression.Add("|");
                        ConvertExpression(nodoRaiz.HijoDerecho);
                        RegularExpression.Add(")");

                    }
                    else if (nodoRaiz.Elemento.Equals("?"))
                    {
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        RegularExpression.Add("?");
                    }
                    else if (nodoRaiz.Elemento.Equals("."))
                    {
                        ConvertExpression(nodoRaiz.HijoIzquierdo);
                        ConvertExpression(nodoRaiz.HijoDerecho);
                    }
                }
            }
            return RegularExpression;
        }
    }
}
