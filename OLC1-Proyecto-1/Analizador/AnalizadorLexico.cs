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
        String stringAuxiliar = "";

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

        public void Scanner(String entradaTexto)
        {
            int estado = 0;
            int columna = 0;
            int fila = 1; ;

            for (int i = 0; i < entradaTexto.Length; i++)
            {
                char letra = entradaTexto[i];
                columna++;

                switch (estado)
                {
                    case 0:
                        //SI VIENE LETRA
                        if (char.IsLetter(letra))
                        {
                            estado = 1;
                            stringAuxiliar += letra;
                        }
                        //SI VIENE SALTO DE LINEA
                        else if (letra == '\n')
                        {
                            estado = 0;
                            columna = 0;//COLUMNA 0
                            fila++; //FILA INCREMENTA

                        }
                        //VERIFICA ESPACIOS EN BLANCO
                        else if (char.IsWhiteSpace(letra))
                        {
                            //columna++;
                            estado = 0;
                            //VERIFICA SI VIENE NUMERO
                        }
                        else if (char.IsDigit(letra))
                        {
                            estado = 2;
                            stringAuxiliar += letra;
                        }

                        //VERIFICA SI ES PUNTUACION
                        else if (char.IsPunctuation(letra))
                        {
                            switch (letra)
                            {
                                //ASCII
                                case '!':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Exclamacion");
                                    break;
                                case '¡':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Exclamacion");
                                    break;
                                case '&':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_&");
                                    break;
                                case '@':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Arroba");
                                    break;
                                case '.':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Punto");
                                    break;
                                case ',':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Coma");
                                    break;
                                case ':':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_DosPuntos");
                                    break;
                                case ';':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_PuntoComa");
                                    break;
                                case '{':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_LlaveIzquierda");
                                    break;
                                case '}':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_LlaveDerecha");
                                    break;
                                case '[':
                                    estado = 11;
                                    break;
                                case '(':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Parentesis_Izq");
                                    break;
                                case ')':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Parentesis_Der");
                                    break;
                                case ']':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Corchete_Der");
                                    break;
                                case '?':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Interrogacion");
                                    break;
                                case '%':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Porcentaje");
                                    break;
                                case '*':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Multiplicacion");
                                    break;
                                case '\\':
                                    estado = 10;
                                    stringAuxiliar += letra;
                                    break;
                                case '/':
                                    estado = 3;
                                    stringAuxiliar += letra;
                                    break;
                                case '"':
                                    estado = 8;
                                    stringAuxiliar += letra;
                                    break;
                                case '-':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Resta");
                                    break;
                                default:
                                    ControladorToken.Instancia.AgregarError(fila, columna, letra.ToString(), "TD_Desconocido");
                                    break;
                            }
                        }
                        //VERIFICA SI ES SIMBOLO
                        else if (char.IsSymbol(letra))
                        {
                            switch (letra)
                            {
                                case '>':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Mayor");
                                    break;
                                case '~':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Virgulilla");
                                    break;
                                case '+':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Suma");
                                    break;

                                case '|':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Pleca");
                                    break;
                                case '<':
                                    estado = 5;
                                    stringAuxiliar += letra;
                                    break;
                                /*SIMBOLOS ASCII*/
                                case '#':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Numeral");
                                    break;
                                case '$':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Simbolo_Dolar");
                                    break;
                                case '=':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Igual");
                                    break;
                                case '^':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Simbolo");
                                    break;
                                case '_':
                                    ControladorToken.Instancia.AgregarToken(fila, columna - 1, letra.ToString(), "TK_Guion_Bajo");
                                    break;
                                default:
                                    ControladorToken.Instancia.AgregarError(fila, columna, letra.ToString(), "TD_Desconocido");
                                    break;
                            }
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarError(fila, columna, letra.ToString(), "TD_Desconocido");
                        }
                        break;
                    case 1:
                        if (char.IsLetterOrDigit(letra) || letra == '_')
                        {
                            stringAuxiliar += letra;
                            estado = 1;
                        }
                        else
                        {
                            if (stringAuxiliar.Equals("CONJ"))
                            {
                                ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length - 1), stringAuxiliar, "PR_" + stringAuxiliar);
                            }
                            else
                            {
                                ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length - 1), stringAuxiliar, "Identificador");
                            }
                            stringAuxiliar = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 2:
                        if (char.IsDigit(letra))
                        {
                            stringAuxiliar += letra;
                            estado = 2;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(fila, columna, stringAuxiliar, "Digito");
                            stringAuxiliar = "";
                            i--;
                            columna--;
                            estado = 0;
                        }
                        break;
                    case 3:
                        if (letra == '/')
                        {
                            estado = 4;
                            stringAuxiliar += letra;
                        }
                        break;
                    case 4:
                        if (letra != '\n')
                        {
                            stringAuxiliar += letra;
                            estado = 4;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(fila, 0, stringAuxiliar, "ComentarioLinea");
                            fila++; columna = 0;
                            estado = 0;
                            stringAuxiliar = "";
                        }
                        break;
                    case 5:

                        if (letra == '!')
                        {
                            estado = 6;
                            stringAuxiliar += letra;
                        }
                        else
                        {
                            stringAuxiliar = "";
                            ControladorToken.Instancia.AgregarToken(fila, columna - 1, "<", "TK_Menor");
                            estado = 0;
                            i--;
                        }
                        break;
                    case 6:
                        if (letra != '!')
                        {
                            if (letra == '\n') { fila++; columna = 0; }
                            stringAuxiliar += letra;
                            estado = 6;
                        }
                        else
                        {
                            stringAuxiliar += letra;
                            estado = 7;
                        }
                        break;
                    case 7:
                        if (letra == '>')
                        {
                            stringAuxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(fila, columna, stringAuxiliar, "ComentarioMultilinea");
                            estado = 0;
                            stringAuxiliar = "";
                        }
                        break;
                    case 8:
                        if (letra != '"')
                        {
                            if (letra == '\n') { fila++; columna = 0; }
                            stringAuxiliar += letra;
                            estado = 8;
                        }
                        else
                        {
                            estado = 9;
                            stringAuxiliar += letra;
                            i--; columna--;
                        }
                        break;
                    case 9:
                        if (letra == '"')
                        {
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), stringAuxiliar, "Cadena");
                            estado = 0;
                            stringAuxiliar = "";
                        }
                        break;
                    case 10:
                        estado = 0;
                        if (letra == 't')
                        {
                            stringAuxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), ('\t').ToString(), "TK_Tabulacion");
                            stringAuxiliar = "";
                            break;
                        }
                        else if (letra == '"')
                        {
                            stringAuxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), ('\"').ToString(), "TK_Comilla_Doble");
                            stringAuxiliar = "";
                            break;
                        }
                        else if (letra == 'n' || letra == 'r')
                        {
                            stringAuxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), ('\n').ToString(), "TK_Salto_Linea");
                            stringAuxiliar = "";
                            break;
                        }
                        if (letra.ToString().Equals("'"))
                        {
                            stringAuxiliar += letra;
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), ('\'').ToString(), "TK_Comilla_Simple");
                            stringAuxiliar = "";
                            break;
                        }
                        else
                        {
                            ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), stringAuxiliar, "TK_Division");
                            stringAuxiliar = "";
                            i--;
                            break;
                        }

                    case 11:
                        if (letra != ':')
                        {
                            ControladorToken.Instancia.AgregarToken(fila, columna - 1, "[", "TK_Corchete_Izq");
                            stringAuxiliar = "";
                            estado = 0;
                        }
                        else
                        {
                            stringAuxiliar += "\"";
                            estado = 12;
                        }
                        break;

                    case 12:
                        if (letra != ':')
                        {
                            if (letra == '\n') { fila++; columna = 0; }
                            if (letra == '\t') { columna++; }
                            stringAuxiliar += letra;
                            estado = 12;
                        }
                        else
                        {
                            if (entradaTexto[i + 1].Equals(']'))
                            {
                                stringAuxiliar += "\"";
                                ControladorToken.Instancia.AgregarToken(fila, (columna - stringAuxiliar.Length), stringAuxiliar, "Cadena_TODO");
                                stringAuxiliar = "";
                                estado = 0;
                                i = i + 1;
                            }
                            else
                            {
                                stringAuxiliar += letra;
                                estado = 12;
                            }
                        }
                        break;
                    default:
                        ControladorToken.Instancia.AgregarError(fila, columna, letra.ToString(), "TD_Desconocido");
                        break;
                }
            }
        }
    }
}
