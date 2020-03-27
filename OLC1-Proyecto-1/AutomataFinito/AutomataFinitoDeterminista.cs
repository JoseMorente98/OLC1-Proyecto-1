using OLC1_Proyecto_1.Controlador;
using OLC1_Proyecto_1.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_Proyecto_1.AutomataFinito
{
    class AutomataFinitoDeterminista
    {
        private Automata afd;
        private readonly ControladorThompson controladorThompson;

        public AutomataFinitoDeterminista()
        {
            this.controladorThompson = new ControladorThompson();
            Afd = new Automata();
        }

        internal Automata Afd { get => afd; set => afd = value; }

        /**
          * CONVERSION DE AUTOMATA AFN A AFD 
          */
        public void ConversionAFN(Automata afn)
        {
            Automata automata = new Automata();
            Queue<HashSet<Estado>> queueSubconjuntos = new Queue<HashSet<Estado>>();
            Estado estadoInicial = new Estado(0);
            automata.Inicio = (estadoInicial);
            automata.AgregarEstado(estadoInicial);

            //CERRADURA-E(0)
            HashSet<Estado> array_inicial = controladorThompson.CerraduraE(afn.Inicio);

            //AGREGAR ESTADOS DE ACEPTACION
            foreach (Estado aceptacion in afn.Aceptacion)
            {
                if (array_inicial.Contains(aceptacion))
                {
                    automata.AgregarEstadoAceptacion(estadoInicial);
                }
            }

            //AGREGAR A LA LISTA EL PRIMER SUBCONJUNTO
            queueSubconjuntos.Enqueue(array_inicial);
            //SUBCONJUNTOS TEMPORAL
            ArrayList arrayListTemporal = new ArrayList();
            //INDICE ACTUAL
            int indiceEstadoInicio = 0;

            while (queueSubconjuntos.Count > 0)
            {
                //SUBCONJUNTO ACTUAL
                HashSet<Estado> hashSetActual = queueSubconjuntos.Dequeue();

                foreach (Object simbolo in afn.Alfabeto)
                {
                    //MUEVE DE SUBCONJUNTOS
                    HashSet<Estado> move_result = controladorThompson.Mueve(hashSetActual, (String)simbolo);
                    HashSet<Estado> resultado = new HashSet<Estado>();

                    //CERRADURA-E(MUEVE(LIST))
                    foreach (Estado e in move_result)
                    {
                        resultado.UnionWith(controladorThompson.CerraduraE(e));
                    }
                    Estado anterior = automata.Estados[indiceEstadoInicio];

                    //SI EL SUBCONJUNTO FUE CREADO SOLO SE AGREGAN LAS TRANSICIONES
                    int contador = 0;
                    int indexOf = 0;
                    if (arrayListTemporal.Count > 0)
                    {
                        for (int i = 0; i < arrayListTemporal.Count; i++)
                        {
                            string texto = "";
                            string texto2 = "";
                            HashSet<Estado> a = (HashSet<Estado>)arrayListTemporal[i];
                            foreach (Estado item in a)
                            {
                                texto = texto + item.IdEstado;
                            }
                            foreach (Estado item in resultado)
                            {
                                texto2 = texto2 + item.IdEstado;
                            }
                            if (texto.Equals(texto2))
                            {
                                indexOf = i;
                                contador = 1;
                                break;
                            }
                        }
                    }

                    if (contador == 1)
                    {
                        List<Estado> arrayListSubconjuntoAnterior = automata.Estados;
                        Estado estadoAnterior = anterior;
                        Estado estadoSiguiente = arrayListSubconjuntoAnterior[indexOf + 1];
                        estadoAnterior.agregarTransicion(new Transicion(estadoAnterior, estadoSiguiente, simbolo.ToString()));
                    }
                    //SI EL SUBCONJUNTO NO EXISTE SE CREA NUEVO ESTADO
                    else
                    {
                        arrayListTemporal.Add(resultado);
                        queueSubconjuntos.Enqueue(resultado);
                        Estado estadoNuevo = new Estado(arrayListTemporal.IndexOf(resultado) + 1);
                        anterior.agregarTransicion(new Transicion(anterior, estadoNuevo, simbolo.ToString()));
                        automata.AgregarEstado(estadoNuevo);
                        //SE VERIFICA EL ESTADO DE ACEPTACION
                        foreach (Estado aceptacion in afn.Aceptacion)
                        {
                            if (resultado.Contains(aceptacion))
                            {
                                automata.AgregarEstadoAceptacion(estadoNuevo);
                            }
                        }
                    }
                }

                indiceEstadoInicio++;
            }
            this.afd = automata;
            DefinirAlfabeto(afn);
            //ASIGNARLE EL TIPO AL AUTOMATA
            this.afd.Tipo = ("AFD");
        }

        /**
          * DEFINIR ALFABETO
          */
        private void DefinirAlfabeto(Automata afn)
        {
            this.afd.Alfabeto = afn.Alfabeto;
        }

        /**
          * ELIMINACION DE ESTADOS TRAMPA
          */
        public Automata EliminarEstados(Automata afd)
        {
            ArrayList arrayListEliminarEstados = new ArrayList();
            /**
             * SE CALCULA ESTADOS TRAMPA LOS ESTADOS QUE TIENEN 
             * TRANSICIONES CON TODAS LAS LETRAS DEL ALFABETO HACIA SI MISMO
             */
            for (int i = 0; i < afd.Estados.Count; i++)
            {
                int cantidadTransiciones = afd.Estados[i].Transiciones.Count;


                int transitionCount = 0;
                foreach (Transicion t in (ArrayList)(afd.Estados[i]).Transiciones)
                {
                    if (afd.Estados[i] == t.Fin)
                    {
                        transitionCount++;
                    }
                }
                if (cantidadTransiciones == transitionCount && transitionCount != 0)
                {
                    arrayListEliminarEstados.Add(afd.Estados[i]);
                }
            }
            /**
             * SE ELIMINAN TRANSICIONES REPETIDAS Y SE ELIMNA EL ESTADO DEL AUTOMATA 
             */
            for (int i = 0; i < arrayListEliminarEstados.Count; i++)
            {
                for (int j = 0; j < afd.Estados.Count; j++)
                {
                    ArrayList arraytransition = afd.Estados[j].Transiciones;
                    int cont = 0;
                    while (arraytransition.Count > cont)
                    {
                        Transicion t = (Transicion)arraytransition[cont];
                        //SE VERIFICA LAS TRANSICIONES DEL ESTADO A ELIMINAR
                        if (t.Fin == arrayListEliminarEstados[i])
                        {
                            afd.Estados[j].Transiciones.Remove(t);
                            cont--;
                        }
                        cont++;

                    }
                }
                //ELIMINAR ESTADO
                afd.Estados.Remove((Estado)arrayListEliminarEstados[i]);
            }
            //ENUMERACION DE ESTADOS
            for (int i = 0; i < afd.Estados.Count; i++)
            {
                afd.Estados[i].IdEstado = i;
            }
            return afd;
        }
    }
}
