using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.AutomataFinito
{
    class AutomataFinitoNoDeterminista
    {
        private Automata afn;
        private String regex;

        internal Automata Afn { get => afn; set => afn = value; }
        public string Regex { get => regex; set => regex = value; }

        public AutomataFinitoNoDeterminista(string regex)
        {
            this.Regex = regex;
        }

        public AutomataFinitoNoDeterminista()
        {

        }

        /**
         * CONSTRUCCIÓN DE AUTOMATA 
         */
        public void ConstruirAutomata(ArrayList arrayList)
        {
            Stack pilaAFN = new Stack();

            foreach (String c in arrayList)
            {
                //Console.WriteLine(c);
                switch (c)
                {
                    case "*":
                        Automata kleene = CerraduraKleene((Automata)pilaAFN.Pop());
                        pilaAFN.Push(kleene);
                        this.Afn = kleene;
                        break;
                    case ".":
                        Automata concat_param1 = (Automata)pilaAFN.Pop();
                        Automata concat_param2 = (Automata)pilaAFN.Pop();

                        Automata concat_result = Concatenacion(concat_param2, concat_param1);

                        pilaAFN.Push(concat_result);
                        this.Afn = concat_result;
                        break;
                    case "|":
                        Automata union_param1 = (Automata)pilaAFN.Pop();
                        Automata union_param2 = (Automata)pilaAFN.Pop();
                        Automata union_result = Alternacion(union_param1, union_param2);
                        pilaAFN.Push(union_result);
                        this.Afn = union_result;
                        break;
                    case "?":
                        Automata s = SimpleAFN("ε");
                        Automata union_q1 = (Automata)pilaAFN.Pop();
                        Automata qtion = Alternacion(union_q1, s);
                        pilaAFN.Push(qtion);
                        this.Afn = qtion;
                        break;
                    default:
                        Automata simple = SimpleAFN(c);
                        pilaAFN.Push(simple);
                        this.Afn = simple;
                        break;
                }
            }
            this.Afn.CrearAlfabeto(arrayList);
            this.Afn.Tipo = "AFN";
        }

        /**
         * CONSTRUCCIÓN DE AFN SIMPLE 
         */
        public Automata SimpleAFN(String simboloRegex)
        {
            Automata automataAFN = new Automata();
            //definir los nuevos estados
            Estado inicial = new Estado(0);
            Estado aceptacion = new Estado(1);
            //crear una transicion unica con el simbolo
            Transicion transicion = new Transicion(inicial, aceptacion, simboloRegex);
            inicial.agregarTransicion(transicion);
            //ESTADOS CREADOS
            automataAFN.AgregarEstado(inicial);
            automataAFN.AgregarEstado(aceptacion);
            //AGREGA INICIO Y ACEPTACION
            automataAFN.Inicio = (inicial);
            automataAFN.AgregarEstadoAceptacion(aceptacion);
            automataAFN.LenguajeR = simboloRegex + "";
            return automataAFN;
        }

        /**
         * CERRADURA DE KLEENE  
         */
        public Automata CerraduraKleene(Automata automataAFN)
        {
            Automata automataCerraduraKleene = new Automata();
            //CREAR ESTADO INICIAL
            Estado nuevoInicio = new Estado(0);
            automataCerraduraKleene.AgregarEstado(nuevoInicio);
            automataCerraduraKleene.Inicio = (nuevoInicio);

            //ESTADOS INTERMEDIOS
            for (int i = 0; i < automataAFN.Estados.Count; i++)
            {
                Estado tmp = (Estado)automataAFN.Estados[i];
                tmp.IdEstado = (i + 1);
                automataCerraduraKleene.AgregarEstado(tmp);
            }

            //SE CREA ESTADO DE ACEPTACION
            Estado nuevoFin = new Estado(automataAFN.Estados.Count + 1);
            automataCerraduraKleene.AgregarEstado(nuevoFin);
            automataCerraduraKleene.AgregarEstadoAceptacion(nuevoFin);

            //ESTADOS CLAVES PARA CERRADURA
            Estado anteriorInicio = automataAFN.Inicio;
            List<Estado> anteriorFin = automataAFN.Aceptacion;

            //AGREGAR TRANSICIONES DESDE EL ESTADO INICIAL
            nuevoInicio.Transiciones.Add(new Transicion(nuevoInicio, anteriorInicio, "ε"));
            nuevoInicio.Transiciones.Add(new Transicion(nuevoInicio, nuevoFin, "ε"));

            //AGREGAR TRANSICIONES DESDE EL ESTADO FINAL2ERT
            for (int i = 0; i < anteriorFin.Count; i++)
            {
                anteriorFin[i].Transiciones.Add(new Transicion((Estado)anteriorFin[i], anteriorInicio, "ε"));
                anteriorFin[i].Transiciones.Add(new Transicion((Estado)anteriorFin[i], nuevoFin, "ε"));
            }
            automataCerraduraKleene.Alfabeto = (automataAFN.Alfabeto);
            automataCerraduraKleene.LenguajeR = (automataAFN.LenguajeR);
            return automataCerraduraKleene;
        }

        /**
         * CONSTRUCCIÓN DE CONCATENACIÓN 
         */
        public Automata Concatenacion(Automata automata1, Automata automata2)
        {
            Automata automataConcatenacion = new Automata();

            //CONTADOR NUEVO AUTOMATA
            int i = 0;
            for (i = 0; i < automata1.Estados.Count; i++)
            {
                Estado tmp = automata1.Estados[i];
                tmp.IdEstado = i;
                //ESTADO INICIAL
                if (i == 0)
                {
                    automataConcatenacion.Inicio = (tmp);
                }
                //CUANDO LLEGA AL ULTIMO CONCATENA EL ULTIMO CON EL PRIMERO CON EPSILON
                if (i == automata1.Estados.Count - 1)
                {
                    //ESTADOS DE ACEPTACION
                    for (int k = 0; k < automata1.Aceptacion.Count; k++)
                    {
                        tmp.agregarTransicion(new Transicion(automata1.Aceptacion[k], automata2.Inicio, "ε"));
                    }
                }
                automataConcatenacion.AgregarEstado(tmp);
            }


            //AGREGA ESTADOS Y TRANSICIONES DEL AUTOMATA
            for (int j = 0; j < automata2.Estados.Count; j++)
            {
                Estado tmp = automata2.Estados[j];
                tmp.IdEstado = i;

                //DEFINE ESTADO DE ACEPTACION
                if (automata2.Estados.Count - 1 == j)
                {
                    automataConcatenacion.AgregarEstadoAceptacion(tmp);
                }
                automataConcatenacion.AgregarEstado(tmp);
                i++;
            }

            HashSet<String> alfabeto = new HashSet<String>();
            alfabeto.UnionWith(automata1.Alfabeto);
            alfabeto.UnionWith(automata2.Alfabeto);
            automataConcatenacion.Alfabeto = (alfabeto);
            automataConcatenacion.LenguajeR = (automata1.LenguajeR + " " + automata2.LenguajeR);
            return automataConcatenacion;
        }

        /**
         * CONSTRUCCIÓN DE ALTERNACIÓN 
         */
        public Automata Alternacion(Automata automata1, Automata automata2)
        {
            Automata automataAlternacion = new Automata();
            Estado nuevoInicio = new Estado(0);
            //SE CREA UN ESTADO CON EPSILON AL ANTERIOR
            nuevoInicio.agregarTransicion(new Transicion(nuevoInicio, automata2.Inicio, "ε"));

            automataAlternacion.AgregarEstado(nuevoInicio);
            automataAlternacion.Inicio = (nuevoInicio);
            int i = 0;
            //AGREGAR ESTADOS DEL SEGUNDO AUTOMATA
            for (i = 0; i < automata1.Estados.Count; i++)
            {
                Estado tmp = automata1.Estados[i];
                tmp.IdEstado = i + 1;
                automataAlternacion.AgregarEstado(tmp);
            }
            //AGREGAR LOS ESTADOS DEL PRIMER AUTOMATA
            for (int j = 0; j < automata2.Estados.Count; j++)
            {
                Estado tmp = automata2.Estados[j];
                tmp.IdEstado = i + 1;
                automataAlternacion.AgregarEstado(tmp);
                i++;
            }
            //NUEVO ESTADO FINAL
            Estado nuevoFin = new Estado(automata1.Estados.Count + automata2.Estados.Count + 1);
            automataAlternacion.AgregarEstado(nuevoFin);
            automataAlternacion.AgregarEstadoAceptacion(nuevoFin);

            Estado anteriorInicio = automata1.Inicio;
            List<Estado> anteriorFin = automata1.Aceptacion;
            List<Estado> anteriorFin2 = automata2.Aceptacion;

            //AGREGAR TRANSICIONES DESDE EL NUEVO ESTADO INICIAL
            nuevoInicio.Transiciones.Add(new Transicion(nuevoInicio, anteriorInicio, "ε"));

            //AGREGAR TRANSICIONES DESDE EL ANTERIOR AFN AL ESTADO FINAL
            for (int k = 0; k < anteriorFin.Count; k++)
                anteriorFin[k].Transiciones.Add(new Transicion(anteriorFin[k], nuevoFin, "ε"));
            //AGREGAR TRANSICIONES DESDE EL ANTERIOR AFN AL ESTADO FINAL
            for (int k = 0; k < anteriorFin.Count; k++)
                anteriorFin2[k].Transiciones.Add(new Transicion(anteriorFin2[k], nuevoFin, "ε"));

            HashSet<String> alfabeto = new HashSet<String>();
            alfabeto.UnionWith(automata1.Alfabeto);
            alfabeto.UnionWith(automata2.Alfabeto);
            automataAlternacion.Alfabeto = (alfabeto);
            automataAlternacion.LenguajeR = (automata1.LenguajeR + " " + automata2.LenguajeR);
            return automataAlternacion;
        }
    }
}
