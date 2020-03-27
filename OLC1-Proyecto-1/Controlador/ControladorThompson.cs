using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorThompson
    {
        private readonly static ControladorThompson instancia = new ControladorThompson();
        ArrayList arrayErrores = new ArrayList();
        public ControladorThompson()
        {
        }

        /**
         * INSTANCIA
         */
        public static ControladorThompson Instancia
        {
            get
            {
                return instancia;
            }
        }

        /**
         * CERRADURA E
         */
        public HashSet<Estado> CerraduraE(Estado cerraduraEstado)
        {
            Stack<Estado> stackCerradura = new Stack<Estado>();
            Estado actual = cerraduraEstado;
            HashSet<Estado> resultado = new HashSet<Estado>();

            stackCerradura.Push(actual);
            resultado.Add(cerraduraEstado); 
            while (stackCerradura.Count > 0)
            {
                actual = stackCerradura.Pop();

                foreach (Transicion t in (ArrayList)actual.Transiciones)
                {
                    if (t.Simbolo.Equals("ε") && !resultado.Contains(t.Fin))
                    {
                        resultado.Add(t.Fin);
                        stackCerradura.Push(t.Fin);
                    }
                }
            }
            return resultado;
        }

        /**
         * MUEVE POR ESTADOS
         */
        public HashSet<Estado> Mueve(HashSet<Estado> hashSetEstado, String simbolo)
        {
            HashSet<Estado> hashSetAlcanzado = new HashSet<Estado>();
            foreach (Estado iterador in hashSetEstado)
            {
                foreach (Transicion t in (ArrayList)iterador.Transiciones)
                {
                    Estado estadoSiguiente = t.Fin;
                    String stringSimbolo = (String)t.Simbolo;
                    if (stringSimbolo.Equals(simbolo))
                    {
                        hashSetAlcanzado.Add(estadoSiguiente);
                    }
                }
            }
            return hashSetAlcanzado;
        }

        /**
         * MUEVE POR ESTADO
         */
        public Estado Mueve(Estado estado, String simbolo)
        {
            List<Estado> hashSetAlcanzado = new List<Estado>();
            foreach (Transicion t in (ArrayList)estado.Transiciones)
            {
                Estado estadoSiguiente = t.Fin;
                String stringSimbolo = (String)t.Simbolo;
                if (stringSimbolo.Equals(simbolo) && !hashSetAlcanzado.Contains(estadoSiguiente))
                {
                    hashSetAlcanzado.Add(estadoSiguiente);
                }
            }
            return hashSetAlcanzado[0];
        }

        /**
         * MUEVE EN UN CONJUNTO
         */
        public HashSet<Estado> MueveEnConjunto(HashSet<Estado> hashSetEstado, String simbolo)
        {
            HashSet<Estado> hashSetAlcanzado = new HashSet<Estado>();
            foreach (Estado iterador in hashSetEstado)
            {
                foreach (Transicion t in (ArrayList)iterador.Transiciones)
                {
                    Estado estadoSiguiente = t.Fin;
                    String stringSimbolo = (String)t.Simbolo;
                    stringSimbolo = stringSimbolo.Trim('"');

                    String stringSimboloEvaluar = "";
                    if (stringSimbolo.Contains("\\n"))
                    {
                        stringSimboloEvaluar = stringSimbolo.Replace("\\n", ('\n').ToString());
                    }
                    if (stringSimbolo.Contains("\\r"))
                    {

                        stringSimboloEvaluar = stringSimbolo.Replace("\\r", ('\n').ToString());
                    }
                    if (stringSimbolo.Contains("\\t"))
                    {
                        stringSimboloEvaluar = stringSimbolo.Replace("\\t", ('\t').ToString());
                    }
                    else
                    {
                        stringSimboloEvaluar = stringSimbolo;
                    }

                    //CONVERTIR CHAR
                    Char value;
                    bool resultado;
                    resultado = Char.TryParse(stringSimboloEvaluar, out value);

                    //AGREGAR EL ESTADO A HASSHETALCANZADO
                    if (resultado && (ControladorConjunto.Instancia.GetElemntsOfSet(stringSimboloEvaluar) == null))
                    {
                        if (stringSimboloEvaluar.Equals(simbolo))
                        {
                            hashSetAlcanzado.Add(estadoSiguiente);
                        }
                    }
                    //SI NO SE PUEDE VIENE CONJUNTO
                    else
                    {
                        //BUSCA LISTA DE CONJUNTOS
                        ArrayList arrayListConjunto = ControladorConjunto.Instancia.GetElemntsOfSet(stringSimboloEvaluar);
                        if (arrayListConjunto != null)
                        {
                            foreach (String letraConjunto in arrayListConjunto)
                            {
                                //EL ELEMENTO SE ENCUENTRA EN EL CONJUNTO A ANALIZAR
                                if (letraConjunto.Equals(simbolo))
                                {
                                    hashSetAlcanzado.Add(estadoSiguiente);
                                }
                            }
                        }
                        else if (arrayListConjunto == null && stringSimboloEvaluar.Equals(simbolo))
                        {
                            hashSetAlcanzado.Add(estadoSiguiente);
                        }
                    }
                }
            }
            return hashSetAlcanzado;
        }


        /**
         * EVALUAR EXPRESION
         */
        public Boolean EvaluateExpression(String stringRegex, Automata automata, ArrayList ar, bool isString)
        {
            Estado estadoInicial = automata.Inicio;
            List<Estado> hashSetEstado = automata.Estados;
            List<Estado> listAceptacion = new List<Estado>(automata.Aceptacion);

            HashSet<Estado> hashSetConjunto = CerraduraE(estadoInicial);

            if (isString)
            {
                foreach (String charRegex in ar)
                {
                    //MOVER ENTRE CONJUNTOS
                    hashSetConjunto = MueveEnConjunto(hashSetConjunto, charRegex);

                    HashSet<Estado> hashSetTemp = new HashSet<Estado>();
                    IEnumerator<Estado> iter = hashSetConjunto.GetEnumerator();

                    while (iter.MoveNext())
                    {
                        Estado estadoSiguiente = iter.Current;
                        hashSetTemp.UnionWith(CerraduraE(estadoSiguiente));
                    }
                    hashSetConjunto = hashSetTemp;
                }
            }
            else
            {
                for (int i = 0; i < stringRegex.Length; i++)
                {
                    //MOVER ENTRE CONJUNTOS
                    Char charRegex = stringRegex[i];
                    hashSetConjunto = MueveEnConjunto(hashSetConjunto, charRegex.ToString());
                    if (hashSetConjunto.Count == 0)
                    {
                        ControladorEvaluador.Instancia.AgregarError(charRegex.ToString(), i);
                    }
                    HashSet<Estado> hashSetTemp = new HashSet<Estado>();
                    IEnumerator<Estado> iter = hashSetConjunto.GetEnumerator();

                    while (iter.MoveNext())
                    {
                        Estado estadoSiguiente = iter.Current;
                        hashSetTemp.UnionWith(CerraduraE(estadoSiguiente));
                    }
                    hashSetConjunto = hashSetTemp;
                }
            }

            bool respuesta = false;
            foreach (Estado aceptation_State in listAceptacion)
            {
                if (hashSetConjunto.Contains(aceptation_State))
                {
                    respuesta = true;
                }
            }
            if (respuesta)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * GENERACION DE DOT AUTOMATA
         */
        public void generarDOT(String nombreArchivo, String pngname, Automata automataFinito)
        {
            String graphvizTexto = "digraph MetodoDeThompson {\n";

            graphvizTexto += "\trankdir=LR;" + "\n";

            graphvizTexto += "\tgraph [label=\"" + nombreArchivo + "\", labelloc=t, fontsize=20]; \n";
            graphvizTexto += "\tnode [shape=doublecircle,fontcolor=white, style=filled,color=firebrick4];";
            for (int i = 0; i < automataFinito.Aceptacion.Count; i++)
            {
                graphvizTexto += " " + automataFinito.Aceptacion[i];
            }
            graphvizTexto += ";" + "\n";
            graphvizTexto += "\tnode [shape=circle];" + "\n";
            graphvizTexto += "\tnode [color=firebrick4,fontcolor=white];\n" + "	edge [color=black];" + "\n";

            graphvizTexto += "\tsecret_node [style=invis];\n" + "	secret_node -> " + automataFinito.Inicio + " [label=\"inicio\"];" + "\n";

            for (int i = 0; i < automataFinito.Estados.Count; i++)
            {
                ArrayList t = (automataFinito.Estados[i]).Transiciones;
                for (int j = 0; j < t.Count; j++)
                {
                    graphvizTexto += "\t" + ((Transicion)t[j]).DOT_String() + "\n";
                }
            }
            graphvizTexto += "}";

            System.IO.File.WriteAllText(nombreArchivo + ".dot", graphvizTexto);
            String path = Application.StartupPath;

            try
            {
                if (!Directory.Exists(path + "\\" + nombreArchivo))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\" + nombreArchivo);
                }

                String pngPath = path + "\\" + nombreArchivo;
                ControladorImagen.Instancia.Agregar(nombreArchivo + " " + pngname + ".png", automataFinito.Tipo, pngPath);

                var command = "dot -Tpng \"" + path + "\\" + nombreArchivo + ".dot\"  -o \"" + pngPath + "\\" + nombreArchivo + " " + pngname + ".png\"   ";
                var processStartInfo = new ProcessStartInfo("cmd", "/C" + command);
                var process = new System.Diagnostics.Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception)
            {
                 MessageBox.Show("Error al escribir grafo aux_grafico.dot", "Error",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         * GENERACION TABLA DE TRANSICION
         */
        public void TableConstructor(String fileName, String path, Automata afd)
        {
            try
            {
                if (!Directory.Exists(path + "\\" + "Tabla Transicion"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\" + "Tabla Transicion");
                }

                String pngPath = path + "\\" + "Tabla Transicion";

                ControladorImagen.Instancia.Agregar(fileName + "Table.png", "Transicion", pngPath);

                System.IO.File.WriteAllText(path + "\\" + "TableTransition.dot", GetCodeGraphviz(afd, fileName));
                var command = "dot -Tpng \"" + path + "\\" + "TableTransition.dot\"  -o \"" + pngPath + "\\" + fileName + "Table.png\"   ";
                var processStartInfo = new ProcessStartInfo("cmd", "/C" + command);
                var process = new System.Diagnostics.Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();

            }
            catch (Exception)
            {
                MessageBox.Show("Error al escribir grafo aux_grafico.dot", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
        * OBTENER CODIGO DE GRAPHVIZ
        */
        public String GetCodeGraphviz(Automata afd, String nombre)
        {
            Automata hashSetTemp = afd;
            String textoGrafo = "";

            List<String> listGrafo = new List<String>();
            List<int> hashSetEstado = new List<int>();

            foreach (var item in hashSetTemp.Estados)
            {
                foreach (Transicion o in item.Transiciones)
                {
                    if (!listGrafo.Contains(o.Simbolo))
                    {
                        listGrafo.Add(o.Simbolo);
                    }
                    int InicioID = o.Inicio.IdEstado;
                    int finID = o.Fin.IdEstado;

                    if (!hashSetEstado.Contains(InicioID))
                    {
                        hashSetEstado.Add(InicioID);
                    }
                    if (!hashSetEstado.Contains(finID))
                    {
                        hashSetEstado.Add(finID);
                    }
                }
            }

            string[,] matrizTransicion = new string[hashSetEstado.Count() + 1, listGrafo.Count() + 1];
            for (int i = 0; i < listGrafo.Count(); i++)
            {
                matrizTransicion[0, i + 1] = listGrafo[i];
            }
            hashSetEstado.Sort();
            for (int i = 0; i < hashSetEstado.Count(); i++)
            {
                matrizTransicion[i + 1, 0] = hashSetEstado[i].ToString();
            }
            int indiceEstado = 0;
            int indiceTerminal = 0;

            foreach (var item in afd.Estados)
            {
                foreach (Transicion transicion in item.Transiciones)
                {
                    int a = transicion.Inicio.IdEstado;
                    string b = transicion.Simbolo;
                    indiceEstado = hashSetEstado.FindIndex(e => e == a);
                    indiceTerminal = listGrafo.FindIndex(e => e == b);

                    String finID = transicion.Fin.IdEstado.ToString();
                    foreach (var i in hashSetTemp.Aceptacion)
                    {

                        if (i.IdEstado == transicion.Fin.IdEstado)
                        {
                            finID = finID + "#";
                        }
                    }
                    matrizTransicion[indiceEstado + 1, indiceTerminal + 1] = finID;
                }
            }

            for (int i = 0; i < matrizTransicion.GetLength(0); i++)
            {
                textoGrafo = textoGrafo + "\n<TR>\n";
                for (int j = 0; j < matrizTransicion.GetLength(1); j++)
                {
                    if (matrizTransicion[i, j] == null)
                    {
                        textoGrafo = textoGrafo + "\t<TD width=\"75\">-</TD>\n";
                    }
                    else
                    {
                        textoGrafo = textoGrafo + "\t<TD width=\"75\">" + matrizTransicion[i, j].Replace('"', ' ') + "</TD>\n";
                    }
                }
                textoGrafo = textoGrafo + "</TR>\n";
            }

            return "digraph TablaTransicion {\n" +
              "\n\tgraph [rankdir=LR, label=\"Tabla de Transicion " + nombre + "\", labelloc=t, fontsize=30, pad=0.5, nodesep=0.5, ranksep=2];\n"
               + "\n\tnode[shape=none];\n" +
               "\n\ttable[label =<\n  <TABLE BORDER=\"1\" CELLBORDER=\"1\" CELLSPACING=\"0\"> " + textoGrafo + "\n</TABLE>>];\n}";
        }
    }
}
