using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OLC1_Proyecto_1.Controlador
{
    class ControladorEvaluador
    {
        private readonly static ControladorEvaluador instancia = new ControladorEvaluador();
        ArrayList arrayListAutomatas = new ArrayList();
        ArrayList arrayListTokensEvaluados = new ArrayList();
        ArrayList arrayListErroresEvaluados = new ArrayList();
        String stringError = "";
        private ControladorEvaluador()
        {
        }

        /**
         * SINGLETON 
         */
        public static ControladorEvaluador Instancia
        {
            get
            {
                return instancia;
            }
        }

        public ArrayList ArrayListAutomatas { get => arrayListAutomatas; set => arrayListAutomatas = value; }
        public ArrayList ArrayListTokensEvaluados { get => arrayListTokensEvaluados; set => arrayListTokensEvaluados = value; }
        public ArrayList ArrayListErroresEvaluados { get => arrayListErroresEvaluados; set => arrayListErroresEvaluados = value; }
        public string StringError { get => stringError; set => stringError = value; }

        /**
         * AGREGAR EVALUACION 
         */
        public void Agregar(string name, Automata afd)
        {
            Evaluador evaluador = new Evaluador(name, afd);
            ArrayListAutomatas.Add(evaluador);
        }

        /**
         *  EVALUACION DE CARACTERES
         */
        public bool SimulateExpression(string nombreExpresion, string cadenaAEvaluar)
        {
            clearTokens();

            if (cadenaAEvaluar.Contains("\\n"))
            {
                cadenaAEvaluar.Replace("\\n", ('\n').ToString());
            }
            if (cadenaAEvaluar.Contains("\\r"))
            {
                cadenaAEvaluar.Replace("\\r", ('\n').ToString());
            }
            if (cadenaAEvaluar.Contains("\\t"))
            {
                cadenaAEvaluar.Replace("\\t", ('\t').ToString());
            }

            //ARRAY LIST CON ALFABETO
            ArrayList arrayListAlfabeto = new ArrayList();
            bool validacion = false;
            
            //ITERACION DE AUTOMATAS
            foreach (Evaluador evaluador in ArrayListAutomatas)
            {
                //BUSCAR EL AUTOMATA POR NOMBRE
                if (evaluador.NombreExpresion.Equals(nombreExpresion))
                {
                    Automata afdTemporal = evaluador.Afd;
                    //DEFINICION DE TIPO DE ALFABETO, SIMBOLOS, CADENAS, MEZCLA
                    int swcase = 0;

                    //SE ITERA EL ALFABETO
                    foreach (var alfabeto in afdTemporal.Alfabeto)
                    {
                        //QUITAR ESPACIOS Y COMILLAS
                        String caracterAlfabeto = alfabeto.Trim('"');

                        Char value;
                        bool result;
                        result = Char.TryParse(caracterAlfabeto, out value);

                        if (result && (ControladorConjunto.Instancia.GetElemntsOfSet(caracterAlfabeto) == null))
                        {
                            if (result || caracterAlfabeto.Equals("\\n") || caracterAlfabeto.Equals("\\t") || caracterAlfabeto.Equals("\\r")
                            || caracterAlfabeto.Equals("\"") || caracterAlfabeto.Equals("\'"))
                            {
                                if (caracterAlfabeto.Equals("\\n"))
                                {

                                    arrayListAlfabeto.Add(('\n').ToString()); ;
                                }
                                else if (caracterAlfabeto.Equals("\\t"))
                                {
                                    arrayListAlfabeto.Add(('\t').ToString()); ;
                                }
                                else if (caracterAlfabeto.Equals("\\r"))
                                {
                                    arrayListAlfabeto.Add(('\r').ToString()); ;
                                }
                                else
                                {
                                    arrayListAlfabeto.Add(caracterAlfabeto);
                                }
                            }
                        }
                        else
                        {
                            //CADENA O CONJUNTO BUSCA ELEMENTOS DE CONJUNTO
                            ArrayList arrayListChar = ControladorConjunto.Instancia.GetElemntsOfSet(caracterAlfabeto);
                        
                            if (arrayListChar != null)
                            {
                                //ITERAR ELEMENTOS
                                foreach (var letter in arrayListChar)
                                {
                                    String letter_temp = letter.ToString().Trim('"');

                                    //AGREGAR AL ALFABETO
                                    if (!arrayListAlfabeto.Contains(letter_temp))
                                    {
                                        arrayListAlfabeto.Add(letter_temp.ToString());
                                    }
                                }
                            }
                            
                            //VIENE CADENA
                            else if (arrayListChar == null)
                            {
                                arrayListAlfabeto.Add(caracterAlfabeto);
                                swcase = 1;
                            }
                            else
                            {
                                //MOSTRAR ERROR
                                StringError = ">> No se ha declarado '" + caracterAlfabeto + "' este conjunto. No podemos evaluar: " + cadenaAEvaluar + ".\n";
                                return false;
                            }
                        }

                    }

                    String str_temp = cadenaAEvaluar.Trim('"');

                    switch (swcase)
                    {
                        //SOLO VIENE SIMBOLOS O CONJUNTOS
                        case 0:
                            int countAux = 1;
                            Char ch = ' ';
                            for (int i = 0; i < str_temp.Length; i++)
                            {
                                //VERIFICA QUE LOS ELEMENTOS PERTENEZCAN AL ALFABETO
                                ch = str_temp[i];

                                if (!arrayListAlfabeto.Contains(ch.ToString()))
                                {
                                    countAux = 0;
                                    //GUARDAR EN ARRAY DE ERRORES
                                    Token t = new Token(0, str_temp[i].ToString(), "Carcter_" + str_temp[i].ToString(), i, 0);
                                    ArrayListErroresEvaluados.Add(t);
                                    break;
                                }
                            }
                            //CONTADOR == 1 ALFABETO CONTIENE TODOS LOS CARACTERES
                            if (countAux == 1)
                            {
                               //SE EVALUA CADA EXPRESION
                                validacion = ControladorThompson.Instancia.EvaluateExpression(str_temp, afdTemporal, null, false);

                                //CADENA VALIDA
                                if (validacion)
                                {
                                    //AGREGAR TOKEN'S
                                    for (int i = 0; i < str_temp.Length; i++)
                                    {
                                        Token t = new Token(0, str_temp[i].ToString(), "Carcter_" + str_temp[i].ToString(), i, 0);
                                        ArrayListTokensEvaluados.Add(t);
                                    }

                                }
                                else
                                {
                                    StringError = ">> El siguiente texto" + cadenaAEvaluar + " contiene errores D:.\n";
                                }
                            }
                            else
                            {
                                StringError = ">> El siguiente texto" + cadenaAEvaluar + " contiene errores D:" + ". El caracter " + ch + ", no se encuentra en el alfabeto\n";
                                return false;
                            }
                            break;
                        //VIENEN CADENAS
                        case 1:
                            //CADENAS A EVALUAR
                            ArrayList arrayListCadenas = new ArrayList();
                            ArrayList arrayListCadenas2 = new ArrayList();
                            ArrayList arrayListCabeza = new ArrayList();

                            String iteradorCadena = "";

                            foreach (String i in arrayListAlfabeto)
                            {
                                if (i.Length > 1)
                                {
                                    if (i.Contains("\\n"))
                                    {
                                        arrayListCadenas.Add(i.Replace("\\n", ('\n').ToString()));
                                    }
                                    if (i.Contains("\\r"))
                                    {
                                        arrayListCadenas.Add(i.Replace("\\r", ('\n').ToString()));
                                    }
                                    if (i.Contains("\\t"))
                                    {
                                        arrayListCadenas.Add(i.Replace("\\t", ('\t').ToString()));
                                    }
                                    else
                                    {
                                        arrayListCadenas.Add(i);
                                    }
                                    arrayListCabeza.Add(i[0].ToString());
                                }
                            }

                            for (int i = 0; i < str_temp.Length; i++)
                            {

                                char a = str_temp[i];
                                if (arrayListCabeza.Contains(a.ToString()))
                                {
                                    int contInterno = 0;
                                    for (int j = i; j < str_temp.Length; j++)
                                    {
                                        iteradorCadena = iteradorCadena + str_temp[j];
                                        if (arrayListCadenas.Contains(iteradorCadena))
                                        {
                                            i = j;
                                            contInterno = 1;
                                            break;
                                        }
                                    }
                                    if (contInterno != 0)
                                    {
                                        arrayListCadenas2.Add(iteradorCadena);
                                        iteradorCadena = "";
                                    }
                                    else
                                    {
                                        arrayListCadenas2.Add(str_temp[i].ToString());
                                    }
                                }
                                else
                                {
                                    arrayListCadenas2.Add(str_temp[i].ToString());
                                }
                                iteradorCadena = "";
                            }

                            validacion = ControladorThompson.Instancia.EvaluateExpression(str_temp, afdTemporal, arrayListCadenas2, true);
                            if (!validacion)
                            {
                                StringError = ">> La cadena de texto " + cadenaAEvaluar + " contiene errores D:.\n";
                                return false;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                }

            }
            return validacion;
        }

        /**
         * ARRAY LIST ERRORES EVALUADOS
         */
        public void AgregarError(String lexema, int col)
        {
            Token token = new Token(0, lexema, "TK_" + lexema, col, 0);
            ArrayListErroresEvaluados.Add(token);
        }

        /**
         * RETORNAR ERROR
         */
        public String GetError()
        {
            return StringError;
        }

        /**
         * LIMPIAR LISTA
         */
        public void clearList()
        {
            ArrayListAutomatas.Clear();
            ArrayListTokensEvaluados.Clear();
            ArrayListErroresEvaluados.Clear();
        }

        /**
         * LIMPIAR TOKENS
         */
        public void clearTokens()
        {
            ArrayListTokensEvaluados.Clear();
            ArrayListErroresEvaluados.Clear();
        }

        /**
         *  REPORTE DE TOKENS
         */
        public void ReporteTokenXML(String path, String nombre)
        {
            XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement xElement = new XElement("ListaToken");
            xDocument.Add(xElement);
            foreach (Token token in ArrayListTokensEvaluados)
            {
                XElement elementToken = new XElement("Token",
                    new XElement("Nombre", token.Descripcion),
                    new XElement("Valor", token.Lexema),
                    new XElement("Fila", token.Fila),
                    new XElement("Columna", token.Columna)
                );
                xElement.Add(elementToken);
            }
            
            if (!Directory.Exists(path + "\\Reportes\\Tokens"))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\Reportes\\Tokens");
            }

            String tokenPath = path + "\\Reportes\\Tokens\\XMLTokens " + nombre + ".xml";
            xDocument.Save(tokenPath);
            ControladorImagen.Instancia.Agregar(nombre + ".XML", "Token", tokenPath);
        }

        /**
         *  REPORTE DE ERRORES
         */
        public void ReporteErrorXML(String path, String nombre)
        {
            XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement xElement = new XElement("ListaErrores");
            xDocument.Add(xElement);
            foreach (Token token in ArrayListErroresEvaluados)
            {
                XElement elementToken = new XElement("Error",
                    new XElement("Nombre", token.Descripcion),
                    new XElement("Valor", token.Lexema),
                    new XElement("Fila", token.Fila),
                    new XElement("Columna", token.Columna)
                );
                xElement.Add(elementToken);
            }
            
            if (!Directory.Exists(path + "\\Reportes\\Errores"))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\Reportes\\Errores");
            }

            String errorPath = path + "\\Reportes\\Errores\\XmlError " + nombre + ".xml";
            xDocument.Save(errorPath);
            ControladorImagen.Instancia.Agregar(nombre + ".XML", "Error", errorPath);
        }
    }
}
