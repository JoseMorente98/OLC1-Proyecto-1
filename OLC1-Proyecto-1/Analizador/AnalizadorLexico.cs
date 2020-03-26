using OLC1_Proyecto_1.Controlador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.Analizador
{
    class AnalizadorLexico
    {
        private readonly static AnalizadorLexico instancia = new AnalizadorLexico();
        String auxiliar = "";

        private AnalizadorLexico()
        {
        }

        public static AnalizadorLexico Instancia
        {
            get
            {
                return instancia;
            }
        }

        public void Scanner(String entrada)
        {
            int estado = 0;
            int columna = 0;
            int row = 1; ;

            for (int i = 0; i < entrada.Length; i++)
            {
                char letra = entrada[i];
                columna++;

                switch (estado)
                {
                    case 0:
                        //SI VIENE LETRA
                        if (char.IsLetter(letra))
                        {
                            estado = 1;
                            auxiliar += letra;
                        }
                        //SI VIENE SALTO DE LINEA
                        else if (letra == '\n')
                        {
                            estado = 0;
                            columna = 0;//COLUMNA 0
                            row++; //FILA INCREMENTA

                        }
                        //VERIFICA ESPACIOS EN BLANCO
                        else if (char.IsWhiteSpace(letra))
                        {
                            estado = 0;
                            //VERIFICA SI VIENE NUMERO
                        }
                        else if (char.IsDigit(letra))
                        {
                            estado = 2;
                            auxiliar += letra;
                        }
                        //VERIFICA SI ES PUNTUACION
                        else if (char.IsPunctuation(letra))
                        {
                            switch (letra)
                            {
                                case '.':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Punto");
                                    break;
                                case ',':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Coma");
                                    break;
                                case ':':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_DosPuntos");
                                    break;
                                case ';':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_PuntoComa");
                                    break;
                                case '{':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_LlaveIzquierda");
                                    break;
                                case '}':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_LlaveDerecha");
                                    break;
                                case '[':
                                    Console.WriteLine("entro");
                                    estado = 11;
                                    break;
                                case ']':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Corchete_Der");
                                    break;
                                case '?':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Interrogacion");
                                    break;
                                case '%':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Porcentaje");
                                    break;
                                case '*':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Multiplicacion");
                                    break;
                                case '\\':
                                    estado = 10;
                                    auxiliar += letra;
                                    break;
                                case '/':
                                    estado = 3;
                                    auxiliar += letra;
                                    break;
                                case '"':
                                    estado = 8;
                                    auxiliar += letra;
                                    break;
                                case '-':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Resta");
                                    break;
                                default:
                                    ControladorToken.Instancia.AgregarError(row, columna, letra.ToString(), "TD_Desconocido");
                                    break;
                            }
                        }

                        //VERIFICA SI ES SIMBOLO
                        else if (char.IsSymbol(letra))
                        {
                            switch (letra)
                            {
                                case '>':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Mayor");
                                    break;
                                case '~':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Virgulilla");
                                    break;
                                case '+':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Suma");
                                    break;
                                case '|':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Pleca");
                                    break;
                                case '<':
                                    estado = 5;
                                    auxiliar += letra;
                                    break;
                                /*SIMBOLOS ASCII*/
                                case '!':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Exclamacion");
                                    break;
                                case '#':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Numeral");
                                    break;
                                case '$':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Simbolo_Dolar");
                                    break;
                                case '&':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_&");
                                    break;
                                case '(':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Parentesis_Izq");
                                    break;
                                case ')':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Parentesis_Der");
                                    break;
                                case '=':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Igual");
                                    break;
                                case '@':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Arroba");
                                    break;

                                case '^':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Simbolo");
                                    break;
                                case '_':
                                    ControladorToken.Instancia.AgregarToken(row, columna - 1, letra.ToString(), "TK_Guion_Bajo");
                                    break;
                                default:
                                    ControladorToken.Instancia.AgregarError(row, columna, letra.ToString(), "TD_Desconocido");
                                    break;
                            }
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarError(row, columna, letra.ToString(), "TD_Desconocido");
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(letra) || letra == '_')
                        {
                            auxiliar += letra;
                            estado = 1;
                        }
                        else
                        {
                            if (auxiliar.Equals("CONJ"))
                            {
                                ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length - 1), auxiliar, "PR_" + auxiliar);
                            }
                            else
                            {
                                ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length - 1), auxiliar, "Identificador");
                            }
                            auxiliar = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 2:
                        if (char.IsDigit(letra))
                        {
                            auxiliar += letra;
                            estado = 2;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(row, columna, auxiliar, "Digito");
                            auxiliar = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 3:
                        if (letra == '/')
                        {
                            estado = 4;
                            auxiliar += letra;
                        }
                        break;
                    case 4:
                        if (letra != '\n')
                        {
                            auxiliar += letra;
                            estado = 4;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(row, 0, auxiliar, "ComentarioLinea");
                            row++; columna = 0;
                            estado = 0;
                            auxiliar = "";
                        }
                        break;
                    case 5:

                        if (letra == '!')
                        {
                            estado = 6;
                            auxiliar += letra;
                        }
                        break;
                    case 6:
                        if (letra != '!')
                        {
                            if (letra == '\n') { row++; columna = 0; }
                            auxiliar += letra;
                            estado = 6;
                        }
                        else
                        {
                            auxiliar += letra;
                            estado = 7;
                        }
                        break;
                    case 7:
                        if (letra == '>')
                        {
                            auxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(row, columna, auxiliar, "ComentarioMultilinea");
                            estado = 0;
                            auxiliar = "";
                        }
                        break;
                    case 8:
                        if (letra != '"')
                        {
                            if (letra == '\n') { row++; columna = 0; }
                            auxiliar += letra;
                            estado = 8;
                        }
                        else
                        {
                            estado = 9;
                            auxiliar += letra;
                            i--; columna--;
                        }
                        break;
                    case 9:
                        if (letra == '"')
                        {
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), auxiliar, "Cadena");
                            estado = 0;
                            auxiliar = "";
                        }
                        break;
                    case 10:
                        estado = 0;
                        if (letra == 't')
                        {
                            auxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), ('\t').ToString(), "TK_Tabulacion");
                            auxiliar = "";
                            break;
                        }
                        else if (letra == '"')
                        {
                            auxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), ('\"').ToString(), "TK_Comilla_Doble");
                            auxiliar = "";
                            break;
                        }
                        else if (letra == 'n' || letra == 'r')
                        {
                            auxiliar += letra;
                            Console.WriteLine(auxiliar);
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), ('\n').ToString(), "TK_Salto_Linea");
                            auxiliar = "";
                            break;
                        }
                        if (letra.ToString().Equals("'"))
                        {
                            auxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), ('\'').ToString(), "TK_Comilla_Simple");
                            auxiliar = "";
                            break;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), auxiliar, "TD_Desconocido");
                            auxiliar = "";
                            break;
                        }

                    case 11:
                        if (letra != ':')
                        {
                            ControladorToken.Instancia.AgregarToken(row, columna - 1, "[", "TK_Corchete_Izq");
                            auxiliar = "";
                            estado = 0;
                        }
                        else
                        {
                            auxiliar += "\"";
                            estado = 12;
                        }
                        break;
                    case 12:
                        if (letra != ':')
                        {
                            if (letra == '\n') { row++; columna = 0; }
                            if (letra == '\t') { columna++; }
                            auxiliar += letra;
                            estado = 12;
                        }
                        else
                        {
                            auxiliar += "\"";
                            ControladorToken.Instancia.AgregarToken(row, (columna - auxiliar.Length), auxiliar, "Cadena_TODO");
                            estado = 0;
                            auxiliar = "";
                            i = i + 1;
                        }
                        break;
                    default:
                        ControladorToken.Instancia.AgregarError(row, columna, letra.ToString(), "TD_Desconocido");
                        break;
                }
            }
        }
    }
}
