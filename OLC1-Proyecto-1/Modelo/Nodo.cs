using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_Proyecto_1.Modelo
{
    class Nodo
    {
        String elemento;
        String primero;
        String ultimo;
        bool esAnulable;
        bool esHoja;
        int index;
        Nodo hijoDerecho;
        Nodo hijoIzquierdo;
        private static int correlative = 1;

        public string Elemento { get => elemento; set => elemento = value; }
        public string Primero { get => primero; set => primero = value; }
        public string Ultimo { get => ultimo; set => ultimo = value; }
        public bool EsAnulable { get => esAnulable; set => esAnulable = value; }
        public bool EsHoja { get => esHoja; set => esHoja = value; }
        internal Nodo HijoDerecho { get => hijoDerecho; set => hijoDerecho = value; }
        internal Nodo HijoIzquierdo { get => hijoIzquierdo; set => hijoIzquierdo = value; }

        public Nodo()
        {
        }
        public Nodo(String elemento, Nodo hijoDerecho, Nodo hijoIzquierdo, bool stringAnulable)
        {
            this.Elemento = elemento;
            this.index = correlative++;
            this.HijoDerecho = hijoDerecho;
            this.HijoIzquierdo = hijoIzquierdo;
            this.Ultimo = "0";
            this.Primero = "0";
            this.EsAnulable = stringAnulable;
            this.EsHoja = false;

        }
        public Nodo(String elemento)
        {
            this.Elemento = elemento;
            this.index = correlative++;
            this.HijoDerecho = null;
            this.HijoIzquierdo = null;
            this.EsAnulable = false;
            this.Primero = "0";
            this.Ultimo = "0";
            this.EsHoja = false;
        }

        public void print(String path, String nombre)
        {
            try
            {
                if (!Directory.Exists(path + "\\" + "Arbol"))
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\" + "Arbol");
                }
                String pngPath = path + "\\" + "Arbol";

                System.IO.File.WriteAllText(path + "\\" + "GrafoArbol.dot", GetGraphviz());
                var command = "dot -Tpng \"" + path + "\\GrafoArbol.dot\"  -o \"" + pngPath + "\\" + nombre + ".png\"   ";
                var processStartInfo = new ProcessStartInfo("cmd", "/C" + command);
                var process = new System.Diagnostics.Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit();

            }
            catch (Exception)
            {
                MessageBox.Show("Error al escribir el archivo aux_grafico.dot", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private String GetGraphviz()
        {
            return "digraph ArbolNodo{\n" +
                   "rankdir=TB;\n" +
                   "node [shape = record, style=filled, fillcolor=white];\n" +
                    GetCuerpo() +
                    "}\n";
        }
        private String GetCuerpo()
        {
            String rotuloNodo;

            String stringReplace = elemento.Replace('"', ' ');
            if (elemento.Equals("ε"))
            {
                stringReplace = "epsilon";
            }
            String stringAnulable = "F";

            if (stringReplace.Equals("|"))
            {
                stringReplace = "or";
            }
            if (EsAnulable)
            {
                stringAnulable = "V";
            }

            if (hijoIzquierdo == null && hijoDerecho == null)
            {
                rotuloNodo = "nodo" + index + " [ label =\"" + Primero + "|" + stringReplace + "\\l" + stringAnulable + "|" + Ultimo + "\"];\n";
            }
            else
            {
                rotuloNodo = "nodo" + index + " [ label =\"" + Primero + "|" + stringReplace + "\\l" + stringAnulable + "|" + Ultimo + "\"];\n";
            }
            if (hijoIzquierdo != null)
            {
                rotuloNodo = rotuloNodo + hijoIzquierdo.GetCuerpo() +
                   "nodo" + index + "->nodo" + hijoIzquierdo.index + "\n";
            }
            if (hijoDerecho != null)
            {
                rotuloNodo = rotuloNodo + hijoDerecho.GetCuerpo() +
                   "nodo" + index + "->nodo" + hijoDerecho.index + "\n";
            }
            return rotuloNodo;
        }
    }
}
