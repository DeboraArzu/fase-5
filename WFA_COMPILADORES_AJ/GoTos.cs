using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_COMPILADORES_AJ
{
    class GoTos
    {
        public static List<Listas> Quernel = new List<Listas>();
        public static int Punto, Dolar;
        bool Ingreso = true;
        bool EsNucleo;
        int Posicion;

        List<List<int>> Produccion = new List<List<int>>();
        List<List<int>> LookAhead = new List<List<int>>();
        List<string> NoTerminales = new List<string>();

        List<List<int>> NuevoNucleo = new List<List<int>>();
        List<List<int>> NuevoLa = new List<List<int>>();
        List<string> NuevoNt = new List<string>();

        public void Parseo()
        {
            Produccion.Clear();
            LookAhead.Clear();
            NoTerminales.Clear();
            Quernel.Clear();
            NuevoLa.Clear();
            NuevoNucleo.Clear();
            NuevoNt.Clear();
            PrimerEstado();
            CalcularQuernels();
        }

        //Calcular el estado inicial
        public void PrimerEstado()
        {
            RepresentarPunto();
            List<int> La = new List<int>();
            La.Add(Dolar);
            GenerarEstado("<start>", La);
        }

        public void RepresentarPunto()
        {
            Punto = Gramatica.ListaTokens.Count() + 1;
            Dolar = Punto + 1;
        }

        public bool TerminalNull(int p)
        {
            if (Gramatica.ListaNoTerminal[(-p) - 1].First.Contains(0))
            {
                return true;
            }
            return false;
        }

        public void CalcularQuernels()
        {
            for (int q = 0; q < Quernel.Count; q++)
            {
                //Generar Estados
                JuntarListas(q);
                for (int n = 0; n < Produccion.Count; n++)
                {
                    int PosPunto = Produccion[n].IndexOf(Punto);
                    if (PosPunto + 1 < Produccion[n].Count)
                    {
                        int E = Produccion[n][PosPunto + 1];
                        NuevoLa.Clear(); NuevoNucleo.Clear(); NuevoNt.Clear();
                        ObtenerNucleo(E, n);
                        //El estado nuevo existe?
                        if (!ExisteEstado(NuevoNucleo, NuevoNt))
                        {
                            Listas NuevoEstado = new Listas();
                            NuevoEstado.N = Quernel.Count;
                            AgregarListasListas(NuevoEstado.LAN, NuevoLa);
                            AgregarListasListas(NuevoEstado.PN, NuevoNucleo);
                            //Crear Goto
                            NuevoEstado.GE.Add(q);
                            if (E < 0)
                            {
                                NuevoEstado.GT.Add(ObtenerNoTerminal(E));
                            }
                            else
                            {
                                NuevoEstado.GT.Add(Gramatica.ListaTokens[E - 1].simbolo);
                            }
                            //Agregar no terminales
                            for (int u = 0; u < NuevoNt.Count; u++)
                            {
                                NuevoEstado.NTN.Add(NuevoNt[u]);
                            }
                            //Generar Look Ahead
                            for (int j = 0; j < NuevoEstado.PN.Count; j++)
                            {
                                int PosPunto2 = NuevoEstado.PN[j].IndexOf(Punto);
                                if (PosPunto2 + 1 < NuevoEstado.PN[j].Count)
                                {
                                    int EAA = NuevoEstado.PN[j][PosPunto2 + 1];
                                    //No terminal
                                    if (EAA < 0)
                                    {
                                        bool produccion = false;
                                        List<int> LAPadre = new List<int>();
                                        for (int i = PosPunto2 + 2; i < NuevoEstado.PN[j].Count; i++)
                                        {
                                            int EAA2 = NuevoEstado.PN[j][i];
                                            //Terminal
                                            if (EAA2 > 0)
                                            {
                                                produccion = true;
                                                LAPadre.Add(EAA2);
                                                break;
                                            }
                                            else
                                            {
                                                AgregarElementos(LAPadre, Gramatica.ListaNoTerminal[(-EAA2) - 1].First);
                                                if (!TerminalNull(EAA2))
                                                {
                                                    produccion = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //Toda la produccion es nula, mandar el LA del padre
                                        if (!produccion)
                                        {
                                            AgregarElementos(LAPadre, NuevoEstado.LAN[j]);
                                            GenerarProduccion(ObtenerNoTerminal(EAA), NuevoEstado, LAPadre);
                                        }
                                        else
                                        {
                                            GenerarProduccion(ObtenerNoTerminal(EAA), NuevoEstado, LAPadre);
                                        }
                                    }
                                }
                            }
                            Quernel.Add(NuevoEstado);
                        }
                        else //Ya existe
                        {
                            //Actualizar Goto
                            Quernel[Posicion].GE.Add(q);
                            if (E < 0)
                            {
                                Quernel[Posicion].GT.Add(ObtenerNoTerminal(E));
                            }
                            else
                            {
                                Quernel[Posicion].GT.Add(Gramatica.ListaTokens[E - 1].simbolo);
                            }
                            //Actualizar look ahead
                            Generar_sub_estados(Posicion, NuevoLa, NuevoNt, NuevoNucleo);
                            int x = 3;
                        }
                        n = -1;
                    }
                    else
                    {
                        Produccion.Remove(Produccion[n]);
                        NoTerminales.Remove(NoTerminales[n]);
                        LookAhead.Remove(LookAhead[n]);
                        n = -1;
                    }
                }
            }
        }

        public void Generar_sub_estados(int estado, List<List<int>> LA, List<string> NT, List<List<int>> Nucleo)
        {
            List<List<bool>> Cambios = new List<List<bool>>();
            bool OcurrioCambio = false;
            InicializarCambios(estado, Cambios);
            for (int i = 0; i < Quernel[estado].PN.Count; i++)
            {
                for (int j = 0; j < NT.Count; j++)
                {
                    if (Quernel[estado].NTN[i] == NT[j] && NucleoIgual(Quernel[estado].PN[i], Nucleo[j]))
                    {
                        if (AgregarElementos(Quernel[estado].LAN[i], LA[j]))
                        {
                            //Actualizar todas las producciones dentro del estado
                            Cambios[0][i] = true;
                            OcurrioCambio = true;
                            ActualizarLAProduccion(estado, i, Cambios);
                        }
                    }
                }
            }
            if (OcurrioCambio)
            {
                ActualizarEstados(estado, Cambios);
            }
        }

        public void InicializarCambios(int estado, List<List<bool>> Cambios)
        {
            Cambios.Clear();
            Cambios.Add(new List<bool>());
            Cambios.Add(new List<bool>());
            for (int i = 0; i < Quernel[estado].PN.Count; i++)
            {
                Cambios[0].Add(false);
            }
            for (int i = 0; i < Quernel[estado].P.Count; i++)
            {
                Cambios[1].Add(false);
            }
        }

        public void ActualizarEstados(int estado, List<List<bool>> Cambios)
        {
            for (int i = 0; i < Cambios[0].Count; i++)
            {
                //Si en esta se dio un cambio
                if (Cambios[0][i])
                {
                    int X = Quernel[estado].PN[i].IndexOf(Punto) + 1;
                    if (X < Quernel[estado].PN[i].Count)
                    {
                        int GNT = Quernel[estado].PN[i][X];
                        int SigEstado;
                        if (GNT < 0)
                        {
                            SigEstado = EncontrarEstado(ObtenerNoTerminal(GNT), estado);
                        }
                        else
                        {
                            SigEstado = EncontrarEstado(Gramatica.ListaTokens[GNT - 1].simbolo, estado);
                        }
                        if (SigEstado != -1)
                        {
                            Generar_sub_estados(SigEstado, Quernel[estado].LAN, Quernel[estado].NTN, Quernel[estado].PN);
                        }
                    }
                }
            }
            for (int i = 0; i < Cambios[1].Count; i++)
            {
                if (Cambios[1][i])
                {
                    int X = Quernel[estado].P[i].IndexOf(Punto) + 1;
                    if (X < Quernel[estado].P[i].Count)
                    {
                        int GNT = Quernel[estado].P[i][X];
                        int SigEstado;
                        if (GNT < 0)
                        {
                            SigEstado = EncontrarEstado(ObtenerNoTerminal(GNT), estado);
                        }
                        else
                        {
                            SigEstado = EncontrarEstado(Gramatica.ListaTokens[GNT - 1].simbolo, estado);
                        }
                        if (SigEstado != -1)
                        {
                            Generar_sub_estados(SigEstado, Quernel[estado].LA, Quernel[estado].NT, Quernel[estado].P);
                        }
                    }
                }
            }
        }

        public int EncontrarEstado(string GNT, int GE)
        {
            for (int i = 0; i < Quernel.Count; i++)
            {
                for (int j = 0; j < Quernel[i].GT.Count; j++)
                {
                    if (Quernel[i].GT[j] == GNT)
                    {
                        if (Quernel[i].GE[j] == GE)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        public void ActualizarLAProduccion(int estado, int PosNucleo, List<List<bool>> Cambios)
        {
            int PosPunto = Quernel[estado].PN[PosNucleo].IndexOf(Punto);
            if (PosPunto + 1 < Quernel[estado].PN[PosNucleo].Count)
            {
                int E = Quernel[estado].PN[PosNucleo][PosPunto + 1];
                if (E < 0)
                {
                    bool produccion = false;
                    List<int> LA = new List<int>();
                    for (int i = PosPunto + 2; i < Quernel[estado].PN[PosNucleo].Count; i++)
                    {
                        int EA = Quernel[estado].PN[PosNucleo][i];
                        if (EA > 0)
                        {
                            if (!LA.Contains(EA))
                            {
                                LA.Add(EA);
                            }
                            produccion = true;
                            break;
                        }
                        else
                        {
                            AgregarElementos(LA, Gramatica.ListaNoTerminal[(-EA) - 1].First);
                            if (!TerminalNull(EA))
                            {
                                produccion = true;
                                break;
                            }
                        }
                    }
                    if (!produccion)
                    {
                        AgregarElementos(LA, Quernel[estado].LAN[PosNucleo]);
                        for (int u = 0; u < Quernel[estado].LA.Count; u++)
                        {
                            if (Quernel[estado].NT[u] == ObtenerNoTerminal(E))
                            {
                                if (AgregarElementos(Quernel[estado].LA[u], Quernel[estado].LAN[PosNucleo]))
                                {
                                    Cambios[1][u] = true;
                                    ActualizarLAP(Quernel[estado], u, Quernel[estado].NT[PosNucleo], LA/*Quernel[estado].LAN[PosNucleo]*/);
                                }
                            }
                        }
                    }
                }
            }


        }

        public void AgregarListasListas(List<List<int>> Destino, List<List<int>> Origen)
        {
            for (int i = 0; i < Origen.Count; i++)
            {
                Destino.Add(new List<int>());
                for (int j = 0; j < Origen[i].Count; j++)
                {
                    Destino[i].Add(Origen[i][j]);
                }
            }
        }

        public void ObtenerNucleo(int elemento, int j)
        {
            for (int i = j; i < Produccion.Count; i++)
            {
                int P = Produccion[i].IndexOf(Punto);
                if (P + 1 < Produccion[i].Count)
                {
                    if (Produccion[i][P + 1] == elemento)
                    {
                        NuevoNucleo.Add(new List<int>());
                        AgregarProduccion(NuevoNucleo[NuevoNucleo.Count - 1], Produccion[i]);
                        NuevoNucleo[NuevoNucleo.Count - 1][P] = NuevoNucleo[NuevoNucleo.Count - 1][P + 1];
                        NuevoNucleo[NuevoNucleo.Count - 1][P + 1] = Punto;
                        Produccion.Remove(Produccion[i]);
                        NuevoLa.Add(new List<int>());
                        AgregarElementos(NuevoLa[NuevoLa.Count - 1], LookAhead[i]);
                        LookAhead.Remove(LookAhead[i]);
                        NuevoNt.Add(NoTerminales[i]);
                        NoTerminales.Remove(NoTerminales[i]);
                        i = -1;
                    }
                }
            }
        }

        public bool ExisteEstado(List<List<int>> Nucleo, List<string> NT)
        {
            int encontro = 0;
            for (int i = 0; i < Quernel.Count; i++)
            {
                if (Nucleo.Count == Quernel[i].NTN.Count)
                {
                    for (int j = 0; j < Nucleo.Count; j++)
                    {
                        for (int u = 0; u < Quernel[i].NTN.Count; u++)
                        {
                            if (NT[j] == Quernel[i].NTN[u] && EstadoIgual(Nucleo[j], Quernel[i].PN[u]))
                            {
                                encontro++;
                            }
                        }
                    }
                    if (encontro == Nucleo.Count)
                    {
                        Posicion = i;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool EstadoIgual(List<int> Uno, List<int> Dos)
        {
            if (Uno.Count != Dos.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Uno.Count; i++)
                {
                    if (Uno[i] != Dos[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool NucleoIgual(List<int> Uno, List<int> Dos)
        {
            List<int> Aux1 = new List<int>();
            List<int> Aux2 = new List<int>();
            for (int i = 0; i < Uno.Count; i++)
            {
                if (Uno[i] != Punto)
                {
                    Aux1.Add(Uno[i]);
                }
            }
            for (int i = 0; i < Dos.Count; i++)
            {
                if (Dos[i] != Punto)
                {
                    Aux2.Add(Dos[i]);
                }
            }
            if (EstadoIgual(Aux1, Aux2))
            {
                return true;
            }
            return false;

        }


        public void JuntarListas(int estado)
        {
            Produccion.Clear();
            LookAhead.Clear();
            NoTerminales.Clear();
            for (int i = 0; i < Quernel[estado].PN.Count; i++)
            {
                Produccion.Add(Quernel[estado].PN[i]);
                LookAhead.Add(Quernel[estado].LAN[i]);
                NoTerminales.Add(Quernel[estado].NTN[i]);
            }
            for (int i = 0; i < Quernel[estado].P.Count; i++)
            {
                Produccion.Add(Quernel[estado].P[i]);
                LookAhead.Add(Quernel[estado].LA[i]);
                NoTerminales.Add(Quernel[estado].NT[i]);
            }
        }

        public void GenerarEstado(string NoTerminal, List<int> LA)
        {
            Listas nuevo = new Listas();
            nuevo.N = Quernel.Count;
            foreach (var P in Gramatica.ListaProducciones)
            {
                if (P.NoTerminal == NoTerminal)
                {
                    List<int> Produccion = new List<int>();
                    nuevo.LAN.Add(new List<int>());
                    nuevo.PN.Add(new List<int>());
                    AgregarProduccion(Produccion, P.Elementos);
                    //Agregar produccion
                    Produccion.Insert(0, Punto);
                    nuevo.PN[nuevo.PN.Count - 1] = Produccion;
                    nuevo.NTN.Add(NoTerminal);
                    nuevo.LAN[nuevo.LAN.Count - 1] = LA;
                    int E = P.Elementos[0];
                    if (E < 0)
                    {
                        if (P.Elementos.Count > 1)
                        {
                            int EA = P.Elementos[1];
                            //Terminal
                            if (EA > 0)
                            {
                                nuevo.LAN[nuevo.LAN.Count - 1].Add(EA);
                            }
                            //No terminal
                            else
                            {
                                AgregarFirstLockAheadNucleo(nuevo, nuevo.LAN.Count - 1, -EA);
                                if (TerminalNull(EA))
                                {
                                    for (int u = 2; u < P.Elementos.Count; u++)
                                    {
                                        EA = P.Elementos[u];
                                        if (EA > 0)
                                        {
                                            nuevo.LAN[nuevo.LAN.Count - 1].Add(EA);
                                            break;
                                        }
                                        else
                                        {
                                            AgregarFirstLockAheadNucleo(nuevo, nuevo.LAN.Count - 1, -EA);
                                            if (!TerminalNull(EA))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < nuevo.NTN.Count; i++)
            {
                int E = nuevo.PN[i][1];
                if (E < 0)
                {
                    GenerarProduccion(ObtenerNoTerminal(E), nuevo, nuevo.LAN[i]);
                }
            }
            for (int i = nuevo.NTN.Count - 1; i > -1; i--)
            {
                nuevo.NT.Insert(0, nuevo.NTN[i]);
                nuevo.P.Insert(0, nuevo.PN[i]);
                nuevo.LA.Insert(0, nuevo.LAN[i]);
            }
            nuevo.PN.Clear();
            nuevo.LAN.Clear();
            nuevo.NTN.Clear();
            Quernel.Add(nuevo);
        }

        public void ActualizarLAP(Listas nuevo, int pos, string nt, List<int> LA)
        {
            //Produccion modificada
            if (nuevo.P[pos].Count > 1)
            {
                int E = nuevo.P[pos][1];
                //Obtener el siguiente elemento
                if (E < 0)
                {
                    bool produccion = false;
                    for (int i = 2; i < nuevo.P[pos].Count; i++)
                    {
                        E = nuevo.P[pos][i];
                        if (E > 0)
                        {
                            produccion = true;
                            break;
                        }
                        if (!TerminalNull(E))
                        {
                            produccion = true;
                            break;
                        }
                    }
                    if (!produccion)
                    {
                        for (int u = 0; u < nuevo.LA.Count; u++)
                        {
                            if (nuevo.NT[u] == ObtenerNoTerminal(E))
                            {
                                if (AgregarElementos(nuevo.LA[u], LA))
                                {
                                    ActualizarLAP(nuevo, u, nt, LA);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GenerarProduccion(string NoTerminal, Listas nuevo, List<int> LA)
        {
            int nuevos = 0;
            List<int> Aux = new List<int>();
            List<int> LookAhead = new List<int>();
            for (int i = 0; i < Gramatica.ListaProducciones.Count; i++)
            {
                if (Gramatica.ListaProducciones[i].NoTerminal == NoTerminal)
                {
                    List<int> Produccion = new List<int>();
                    AgregarProduccion(Produccion, Gramatica.ListaProducciones[i].Elementos);
                    Produccion.Insert(0, Punto);
                    if (ProduccionExiste(Produccion, nuevo, NoTerminal))
                    {
                        Ingreso = false;
                        for (int j = 0; j < nuevo.LA.Count; j++)
                        {
                            if (nuevo.NT[j] == NoTerminal)
                            {
                                if (AgregarElementos(nuevo.LA[j], LA))
                                {
                                    ActualizarLAP(nuevo, j, NoTerminal, nuevo.LA[j]);
                                }
                            }
                        }
                        break;
                    }
                    Ingreso = true;
                    nuevos++;
                    nuevo.P.Add(new List<int>());
                    nuevo.LA.Add(new List<int>());
                    AgregarElementos(nuevo.LA[nuevo.LA.Count - 1], LA);
                    nuevo.P[nuevo.P.Count - 1] = Produccion;
                    nuevo.NT.Add(NoTerminal);
                    
                }
            }
            int n = nuevo.NT.Count - nuevos; int f = nuevo.NT.Count;
            for (int i = n; i < f; i++)
            {
                if (nuevo.P[i].Count > 1)
                {
                    int E = nuevo.P[i][1];
                    if (E < 0)
                    {
                        bool produccion = false;
                        for (int u = 2; u < nuevo.P[i].Count; u++)
                        {
                            int EA = nuevo.P[i][u];
                            if (EA > 0)
                            {
                                if (!LookAhead.Contains(EA))
                                {
                                    LookAhead.Add(EA);
                                }
                                GenerarProduccion(ObtenerNoTerminal(E), nuevo, LookAhead);
                                produccion = true;
                                break;
                            }
                            else
                            {
                                AgregarElementos(LookAhead, Gramatica.ListaNoTerminal[(-EA) - 1].First);
                                if (!TerminalNull(EA))
                                {
                                    produccion = true;
                                    GenerarProduccion(ObtenerNoTerminal(E), nuevo, LookAhead);
                                    break;
                                }
                            }
                        }
                        if (!produccion)
                        {
                            AgregarElementos(LookAhead, nuevo.LA[i]);
                            GenerarProduccion(ObtenerNoTerminal(E), nuevo, LookAhead);
                        }
                    }
                    
                }
            }
        }

        public bool AgregarElementos(List<int> listadestino, List<int> listafuente)
        {
            bool cambio = false;
            for (int i = 0; i < listafuente.Count; i++)
            {
                if (!listadestino.Contains(listafuente[i]))
                {
                    if (listafuente[i] != 0)
                    {
                        listadestino.Add(listafuente[i]);
                        cambio = true;
                    }
                }
            }
            return cambio;
        }

        public void AgregarProduccion(List<int> listadestino, List<int> listafuente)
        {
            for (int i = 0; i < listafuente.Count; i++)
            {
                if (listafuente[i] != 0)
                {
                    listadestino.Add(listafuente[i]);
                }
            }
        }

        public bool ProduccionExiste(List<int> produccion, Listas estado, string nt)
        {
            EsNucleo = false;
            for (int i = 0; i < estado.PN.Count; i++)
            {
                if (produccion.Count == estado.PN[i].Count && estado.NTN[i] == nt)
                {
                    if (EstadoIgual(produccion, estado.PN[i]))
                    {
                        return true;
                    }
                }
            }
            for (int i = 0; i < estado.P.Count; i++)
            {
                if (produccion.Count == estado.P[i].Count && estado.NT[i] == nt)
                {
                    if (EstadoIgual(produccion, estado.P[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string ObtenerNoTerminal(int n)
        {
            return Gramatica.ListaNoTerminal[(-n) - 1].NT;
        }

        public void AgregarLA(List<int> la, Listas Quernel, int p)
        {
            for (int i = 0; i < la.Count; i++)
            {
                if (!Quernel.LA[p].Contains(la[i]))
                {
                    Quernel.LA[p].Add(la[i]);
                }
            }
        }

        public void AgregarFirstLockAheadNucleo(Listas Quernel, int lan, int nt)
        {
            for (int i = 0; i < Gramatica.ListaNoTerminal[nt].First.Count; i++)
            {
                int elemento = Gramatica.ListaNoTerminal[nt].First[i];
                if (!Quernel.LAN[lan].Contains(elemento))
                {
                    if (elemento != 0)
                    {
                        Quernel.LAN[lan].Add(elemento);
                    }
                }
            }
        }
    }
}
