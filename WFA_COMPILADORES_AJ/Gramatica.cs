using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_COMPILADORES_AJ
{
    class Gramatica
    {
            //Variables globales
            string MensageError;
            int PosicionError, PosicionActual, TerminaChr, NuevaDefinicion, parentesis, ContadorToken, Presedencia;
            bool Conjunto, Apostrofe, Comillas, CHR, valor, Pipe, AccionEnToken;
            int[] rango = new int[2];
            string textocompleto;
            public bool AlgunaProduccion;
            int PosicionNoTerminal;
            int NumeroNoTerminal;
            int ContadorProduccion;
            char[] texto1;
            int NuevaProduccion;
            bool start;
            //Bool
            bool check;
            bool LeyoToken;
            bool To, YaEmpezo, comment, cc, cerro;
            //Datos del Compilador
            public string NombreCompilador;

            //Listas
            public List<string> ListaUnidades = new List<string>();
            public List<string> ListaConjuntos = new List<string>();
            public static List<TOKEN> ListaTokens = new List<TOKEN>();
            public List<string> ListaKeywords = new List<string>();
            public static List<TablaNo_Terminales> ListaNoTerminal = new List<TablaNo_Terminales>();
            public static List<Tabla_Producciones> ListaProducciones = new List<Tabla_Producciones>();
            List<string> Elementos = new List<string>();

            public List<string> ListaDefinicionesTokens = new List<string>();
            public List<bool> ListaCheckTokens = new List<bool>();
            public List<string> ListaDefinicionesConjuntos = new List<string>();

            //Inicializa todas las variables
            public void LimpiarDatos()
            {
                ListaCheckTokens.Clear();
                MensageError = "";
                PosicionError = 0;
                NombreCompilador = "";
                PosicionActual = 0;
                LeyoToken = false;
                rango[0] = rango[1] = -1;
                ListaTokens.Clear();
                ListaConjuntos.Clear();
                ListaUnidades.Clear();
                ListaKeywords.Clear();
                ListaNoTerminal.Clear();
                check = false;
                ListaNoTerminal.Clear();
                ListaProducciones.Clear();
                start = false;
                //Fase6.Querner.Clear();
                ListaDefinicionesConjuntos.Clear();
                ListaDefinicionesTokens.Clear();
            }

            //Metodo que analiza todo el texto
            public bool AnalizarTexto(string texto)
            {
                LimpiarDatos();
                textocompleto = texto;
                texto1 = texto.ToCharArray();
                if (AnalizarTitulo(texto))
                {
                    if (PosicionActual < texto.Length)
                    {
                        if (ExistenUnidades(texto.Substring(PosicionActual)))
                        {
                            if (PosicionActual < texto.Length)
                            {
                                int HayUnidad = HayUnidades(texto.Substring(PosicionActual));
                                if (HayUnidad == 3)
                                {
                                    return EstablecerError(PosicionActual + PosicionError, "Se debe de definir al menos una unidad.");
                                }
                                if (HayUnidad == 2)
                                {
                                    return EstablecerError(PosicionActual + PosicionError, "Se esperaba una unidad.");
                                }
                                if (HayUnidad == 0)
                                {
                                    return EstablecerError(PosicionError, "No se puede nombrar una unidad con una palabra reservada.");
                                }
                                if (HayUnidad == 1)
                                {
                                    if (!AnalizarUnidades(texto.Substring(PosicionActual)))
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                return EstablecerError(texto.Length - 1, "Definicion del compilador incompleta.");
                            }
                        }
                        if (PosicionActual < texto.Length)
                        {
                            if (AnalizarTokens(texto.Substring(PosicionActual)))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return EstablecerError(texto.Length - 1, "Definicion del compilador incompleta.");
                        }
                    }
                    else
                    {
                        return EstablecerError(texto.Length - 1, "Definicion del compilador incompleta.");
                    }
                }
                return false;
            }

            //Analiza la sección de tokens
            public bool AnalizarTokens(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (palabra.ToLower() == "tokens")
                    {
                        PosicionActual += n + palabra.Length;
                        ContadorToken = 1;
                        Presedencia = 1;
                        return LeerTokens(texto.Substring(n + 6));
                    }
                    else
                    {
                        return EstablecerError(PosicionActual + n, "Se esperaba Tokens");
                    }
                }
                else
                {
                    return EstablecerError(PosicionActual + n, "Se esperaba Tokens");
                }
            }

            public void ArreglarStart()
            {
                bool encontro = false;
                for (int i = 0; i < ListaNoTerminal.Count; i++)
                {
                    if (ListaNoTerminal[i].NT == "<start>")
                    {
                        encontro = true;
                        ListaNoTerminal[i].Numero = 1;
                        TablaNo_Terminales aux = ListaNoTerminal[i];
                        ListaNoTerminal.RemoveAt(i);
                        ListaNoTerminal.Insert(0, aux);
                    }
                }
                if (!encontro)
                {
                TablaNo_Terminales inicio = new TablaNo_Terminales(1, "<start>", -1);
                    ListaNoTerminal.Insert(0, inicio);
                }
            }

            public bool LeerTokens(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                if (n == -1)
                {
                    return EstablecerError(PosicionActual, "La definición del compilador es incompleta.");
                }
                else
                {
                    PosicionActual += n;
                    if (texto[n] == '\"' || texto[n] == '\'')
                    {
                        if (DefinirOperador(texto.Substring(n), 0))
                        {
                            LeyoToken = true;
                            Presedencia++;
                            return LeerTokens(texto.Substring(NuevaDefinicion + 1 + n));
                        }
                        return false;
                    }
                    else if (Char.IsLetter(texto[n]))
                    {
                        string palabra = ObtenerPalabra(texto, n);
                        if (palabra.ToLower() == "keywords")
                        {
                            if (check)
                            {
                                PosicionActual += palabra.Length;
                                if (DefinirKeyword(texto.Substring(n + palabra.Length), 0))
                                {
                                    return LeerTokens(texto.Substring(texto.IndexOf('.') + 1));
                                }
                                return false;
                            }
                            return EstablecerError(PosicionActual, "No se definio nigún check en los tokens.");
                        }
                        else if (palabra.ToLower() == "comments")
                        {
                            To = YaEmpezo = comment = false;
                            PosicionActual += n;
                            if (DefinirComentario(texto.Substring(n), 0))
                            {
                                return LeerTokens(texto.Substring(texto.IndexOf('.') + 1));
                            }
                            return false;
                        }
                        else if (palabra.ToLower() == "productions")
                        {
                            PosicionActual += palabra.Length;
                            AlgunaProduccion = false;
                            NumeroNoTerminal = 2;
                            ContadorProduccion = 1;
                            PosicionActual = textocompleto.IndexOf(palabra) + 11;
                            if (AnalizarProductions(texto.Substring(n + palabra.Length)))
                            {
                                ArreglarStart();
                                return true;
                            }
                            return false;
                        }
                        else if (PalabraReservada(palabra))
                        {
                            return EstablecerError(n, "La palabra " + palabra + "esta reservada.");
                        }
                        else
                        {
                            texto = texto.Substring(n + palabra.Length);
                            if (ListaConjuntos.Contains(palabra))
                            {
                                return EstablecerError(PosicionActual, "El conjunto ya se definio previamente.");
                            }
                            else if (TokenDefinido(palabra))
                            {
                                return EstablecerError(PosicionActual, "El token ya se definio previamente.");
                            }
                            PosicionActual += palabra.Length;
                            n = SaltarEspacios(texto, 0);
                            PosicionActual += n;
                            switch (texto[n])
                            {
                                case '(':
                                    CHR = Comillas = Apostrofe = false;
                                    rango[0] = rango[1] = -1;
                                    int f = ObtenerFinalConjunto(texto);
                                    ListaConjuntos.Add(palabra);
                                    if (DefinirConjunto(texto, n + 1, f))
                                    {
                                        PosicionActual += NuevaDefinicion + 1;
                                        ListaDefinicionesConjuntos.Add(texto.Substring(n + 1, f - 2));
                                        return LeerTokens(texto.Substring(f + 1));
                                    }
                                    return false;
                                case '=':
                                    Pipe = valor = false;
                                    TOKEN nuevo = new TOKEN(ContadorToken, palabra.ToLower(), 0, "TOKEN");
                                    ListaTokens.Add(nuevo);
                                    ListaCheckTokens.Add(false);
                                    if (DefinirToken(0, texto.Substring(n + 1)))
                                    {
                                        PosicionActual += NuevaDefinicion + 1 + n;
                                        ContadorToken++;
                                        ListaDefinicionesTokens.Add(texto.Substring(n + 1, NuevaDefinicion));
                                        return LeerTokens(texto.Substring(NuevaDefinicion + 2 + n));
                                    }
                                    return false;
                                default:
                                    return EstablecerError(PosicionActual, "Se esperaba un signo igual o paréntesis abierto.");
                            }
                        }
                    }
                    else if (!LeyoToken)
                    {
                        return EstablecerError(PosicionActual + n, "Se debe de ingresar un token.");
                    }
                    return EstablecerError(PosicionActual, "Error en la definición del compilador.");
                }
            }

            public bool TokenDefinido(string x)
            {
                for (int i = 0; i < ListaTokens.Count; i++)
                {
                    if (ListaTokens[i].simbolo.ToLower() == x.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }

            //Metodo para leer las producciones
            public bool ExisteNoTerminal(string x)
            {
                for (int i = 0; i < ListaNoTerminal.Count; i++)
                {
                    if (ListaNoTerminal[i].NT == x.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool DefinidoNoTerminal(string x)
            {
                for (int i = 0; i < ListaNoTerminal.Count; i++)
                {
                    if (ListaNoTerminal[i].NT == x.ToLower())
                    {
                        if (ListaNoTerminal[i].Produccion == -1)
                        {
                            PosicionNoTerminal = i;
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }

            public bool AnalizarProductions(string texto)
            {
                if (texto.Trim() == "" && AlgunaProduccion)
                {
                    return true;
                }
                int n = SaltarEspacios(texto, 0);
                if (n != -1)
                {
                    if (texto[n] == '<')
                    {
                        n = SaltarEspacios(texto, n + 1);
                        string palabra = ObtenerPalabra(texto, n);
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '>')
                        {
                            n = SaltarEspacios(texto, n + 1);
                            if (texto[n] == '-' && texto[n + 1] == '>')
                            {
                                //Fase 4
                                if (ExisteNoTerminal("<" + palabra.ToLower() + ">"))
                                {
                                    if (!DefinidoNoTerminal("<" + palabra + ">"))
                                    {
                                        ListaNoTerminal[PosicionNoTerminal].Produccion = ContadorProduccion;
                                    }
                                }
                                else
                                {
                                    if (palabra.ToLower() != "start")
                                    {
                                        TablaNo_Terminales nuevo = new TablaNo_Terminales(NumeroNoTerminal, "<" + palabra.ToLower() + ">", ContadorProduccion);
                                        ListaNoTerminal.Add(nuevo);
                                        NumeroNoTerminal++;
                                    }
                                    else
                                    {
                                        TablaNo_Terminales nuevo = new TablaNo_Terminales(1, "<" + palabra.ToLower() + ">", ContadorProduccion);
                                        ListaNoTerminal.Add(nuevo);
                                        start = true;
                                    }
                                }
                                Elementos.Clear();
                                PosicionActual += n + 2;
                                if (DefinirProduccion(texto.Substring(n + 2), 0, "<" + palabra.ToLower() + ">"))
                                {
                                    AlgunaProduccion = true;
                                    return AnalizarProductions(texto.Substring(NuevaProduccion + n + 2));
                                }
                                return false;
                            }
                            return EstablecerError(PosicionActual + n, "Se esperaba -> despues del nombre de la producción.");
                        }
                        return EstablecerError(PosicionActual + n, "Se esperaba el signo >.");
                    }
                    return EstablecerError(PosicionActual + n, "Se esperaba la definición de una producción.");
                }
                else
                {
                    return EstablecerError(PosicionActual, "La definición de la producción es incorrecta.");
                }
            }

            public void AgregarElementos(int n)
            {
                for (int i = 0; i < Elementos.Count; i++)
                {
                    if (Elementos[i][0] == '\"' || Elementos[i][0] == '\'' || Char.IsLetter(Elementos[i][0]))
                    {
                        int posicion = ObtenerNumeroToken(Elementos[i].ToLower());
                        ListaProducciones[n].Elementos.Add(ListaTokens[posicion].numero);
                    }
                    else
                    {
                        int posicion = ObtenerNumeroNoTerminal(Elementos[i]);
                        ListaProducciones[n].Elementos.Add(-posicion);
                    }
                }
            }


            public int ObtenerNumeroToken(string token)
            {
                for (int i = 0; i < ListaTokens.Count; i++)
                {
                    if (Char.IsLetter(ListaTokens[i].simbolo[0]))
                    {
                        if (ListaTokens[i].simbolo.ToLower() == token.ToLower())
                        {
                            return i;
                        }
                    }
                    else
                    {
                        if (ListaTokens[i].simbolo.ToLower() == token.ToLower())
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }

            public int ObtenerNumeroNoTerminal(string x)
            {
                for (int i = 0; i < ListaNoTerminal.Count; i++)
                {
                    if (ListaNoTerminal[i].NT == x.ToLower())
                    {
                        return ListaNoTerminal[i].Numero;
                    }
                }
                return -1;
            }

            public void ModificarSiguiente()
            {
                if (ListaProducciones[ListaProducciones.Count - 2].Siguiente == ListaProducciones[ListaProducciones.Count - 1].Produccion)
                {
                    ListaProducciones[ListaProducciones.Count - 2].Siguiente = ListaProducciones[ListaProducciones.Count - 1].Siguiente;
                }
            }

            public int ObtenerPosicionAnterior(int n)
            {
                for (int i = 0; i < ListaProducciones.Count; i++)
                {
                    if (ListaProducciones[i].Siguiente == n)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public bool DefinirProduccion(string texto, int n, string noterminalactual)
            {
                n = SaltarEspacios(texto, n);
                if (n == -1)
                {
                    return EstablecerError(PosicionActual, "Error en la definición de la producción.");
                }
                if (Char.IsLetter(texto[n]) && texto[n] != 'ɛ' && texto[n] != 'Ɛ' && texto[n] != 'Ԑ' && texto[n] != 'ԑ')
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (TokenDefinido(palabra.ToLower()))
                    {
                        Elementos.Add(palabra);
                        return DefinirProduccion(texto, n + palabra.Length, noterminalactual);
                    }
                    return EstablecerError(PosicionActual + n, "El token ingresado no existe.");
                }
                else if (texto[n] == '<')
                {
                    n = SaltarEspacios(texto, n + 1);
                    string palabra = ObtenerPalabra(texto, n);
                    n = SaltarEspacios(texto, n + palabra.Length);
                    if (texto[n] == '>')
                    {
                        //Fase 4
                        if (!ExisteNoTerminal("<" + palabra.ToLower() + ">"))
                        {
                            TablaNo_Terminales nuevo = new TablaNo_Terminales(NumeroNoTerminal, "<" + palabra.ToLower() + ">", -1);
                            ListaNoTerminal.Add(nuevo);
                            NumeroNoTerminal++;
                        }
                        Elementos.Add("<" + palabra + ">");
                        //----Fase 4
                        return DefinirProduccion(texto, n + 1, noterminalactual);
                    }
                    return EstablecerError(PosicionActual + n, "Se esperaba el signo >.");
                }
                else if (texto[n] == '\'')
                {
                    if ((n + 2) < texto.Length)
                    {
                        if (texto[n + 2] == '\'')
                        {
                            if (ListaTokens.Count == 0)
                            {
                                TOKEN nuevo = new TOKEN(1, '\'' + texto[n + 1].ToString().ToLower() + '\'', 0, "");
                                ListaTokens.Add(nuevo);
                            }
                            else
                            {
                            if (ObtenerNumeroToken('\'' + texto[n + 1].ToString() + '\'') == -1)
                            {
                                int x = ListaTokens[ListaTokens.Count - 1].numero;
                                //int p = ListaTokens[ListaTokens.Count - 1].presedencia;
                                TOKEN nuevo = new TOKEN(x + 1, '\'' + texto[n + 1].ToString().ToLower() + '\'', 0, "");
                                    ListaTokens.Add(nuevo);
                                }
                            }
                            Elementos.Add('\'' + texto[n + 1].ToString() + '\'');
                            return DefinirProduccion(texto, n + 3, noterminalactual);
                        }
                        n = SaltarEspacios(texto, n + 1);
                        string palabra = "";
                        for (int i = n; i < texto.Length; i++)
                        {
                            if (texto[i] == '\'')
                            {
                                break;
                            }
                            else if (char.IsWhiteSpace(texto[i]))
                            {
                                break;
                            }
                            palabra += texto[i];
                        }
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '\'')
                        {
                            if (ListaTokens.Count == 0)
                            {
                                TOKEN nuevo = new TOKEN(1, "'" + palabra.ToLower() + "'", 0, "");
                                ListaTokens.Add(nuevo);
                            }
                            else
                            {
                                if (ObtenerNumeroToken("'" + palabra.ToLower() + "'") == -1)
                                {
                                    int x = ListaTokens[ListaTokens.Count - 1].numero;
                                    //int p = ListaTokens[ListaTokens.Count - 1].presedencia;
                                    TOKEN nuevo = new TOKEN(x + 1, "'" + palabra.ToLower() + "'" , 0, "");
                                    ListaTokens.Add(nuevo);
                                }
                            }
                            Elementos.Add("'" + palabra + "'");
                            return DefinirProduccion(texto, n + 1, noterminalactual);
                        }
                        return EstablecerError(PosicionActual + n, "Se esperaba el signo \'.");
                    }
                    return EstablecerError(PosicionActual + n, "La definición de la producción es incorrecta.");
                }
                else if (texto[n] == '\"')
                {
                    if ((n + 2) < texto.Length)
                    {
                        if (texto[n + 2] == '\"')
                        {
                            if (ListaTokens.Count == 0)
                            {
                                TOKEN nuevo = new TOKEN(1, '\'' + texto[n + 1].ToString().ToLower() + '\'', 0, "");
                                ListaTokens.Add(nuevo);
                            }
                            else
                            {
                                if (ObtenerNumeroToken("\"" + texto[n + 1].ToString() + "\"") == -1)
                                {
                                    int x = ListaTokens[ListaTokens.Count - 1].numero;
                                    //int p = ListaTokens[ListaTokens.Count - 1].presedencia;
                                    TOKEN nuevo = new TOKEN(x + 1, '\"' + texto[n + 1].ToString().ToLower() + '\"', 0, "");
                                    ListaTokens.Add(nuevo);
                                }
                            }
                            Elementos.Add('\"' + texto[n + 1].ToString() + '\"');
                            return DefinirProduccion(texto, n + 3, noterminalactual);
                        }
                        n = SaltarEspacios(texto, n + 1);
                        string palabra = "";
                        for (int i = n; i < texto.Length; i++)
                        {
                            if (texto[i] == '\"')
                            {
                                break;
                            }
                            else if (char.IsWhiteSpace(texto[i]))
                            {
                                break;
                            }
                            palabra += texto[i];
                        }
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '\"')
                        {
                            if (ListaTokens.Count == 0)
                            {
                                TOKEN nuevo = new TOKEN(1, '\'' + texto[n + 1].ToString().ToLower() + '\'', 0, "");
                                ListaTokens.Add(nuevo);
                            }
                            else
                            {
                                if (ObtenerNumeroToken("\"" + palabra.ToLower() + "\"") == -1)
                                {
                                    int x = ListaTokens[ListaTokens.Count - 1].numero;
                                    //int p = ListaTokens[ListaTokens.Count - 1].presedencia;
                                    TOKEN nuevo = new TOKEN(x + 1, "\"" + palabra.ToLower() + "\"", 0, "");
                                    ListaTokens.Add(nuevo);
                                }
                            }
                            Elementos.Add('\"' + palabra + '\"');
                            return DefinirProduccion(texto, n + 1, noterminalactual);
                        }
                        return EstablecerError(PosicionActual + n, "Se esperaba el signo \".");
                    }
                    return EstablecerError(PosicionActual + n, "La definición de la producción es incorrecta.");
                }
                else if (texto[n] == '{')
                {
                    if (ValidarProduccion(texto, n))
                    {
                        n = SaltarEspacios(texto, n + 1);
                        string palabra = ObtenerPalabra(texto, n);
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '}')
                        {
                            int x = SaltarEspacios(texto, n + 1);
                            if (texto[x] == '.')
                            {
                                if (Elementos.Count == 0)
                                {
                                    Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, 0, 0, noterminalactual);
                                    ListaProducciones.Add(nuevo);
                                    ListaProducciones[ListaProducciones.Count - 1].Elementos.Add(0);
                                    ContadorProduccion++;
                                    if (ListaProducciones[ListaProducciones.Count - 2].Siguiente == ListaProducciones[ListaProducciones.Count - 1].Produccion)
                                    {
                                        ListaProducciones[ListaProducciones.Count - 2].Siguiente = 0;
                                    }
                                    return DefinirProduccion(texto, n + 1, noterminalactual);
                                }
                                else
                                {
                                    Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, Elementos.Count, 0, noterminalactual);
                                    ListaProducciones.Add(nuevo);
                                    AgregarElementos(ListaProducciones.Count - 1);
                                    Elementos.Clear();
                                    ContadorProduccion++;
                                }
                                VerificarProduccion(noterminalactual);
                                return DefinirProduccion(texto, n + 1, noterminalactual);
                            }
                            else if (texto[x] == '|')
                            {
                                if (Elementos.Count == 0)
                                {
                                    Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, 0, ListaProducciones.Count + 2, noterminalactual);
                                    ListaProducciones.Add(nuevo);
                                    ListaProducciones[ListaProducciones.Count - 1].Elementos.Add(0);
                                    ContadorProduccion++;
                                    ModificarSiguiente();
                                    return DefinirProduccion(texto, n + 1, noterminalactual);
                                }
                                else
                                {
                                    Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, Elementos.Count, ListaProducciones.Count + 2, noterminalactual);
                                    ListaProducciones.Add(nuevo);
                                    AgregarElementos(ListaProducciones.Count - 1);
                                    Elementos.Clear();
                                    ContadorProduccion++;
                                }
                                VerificarProduccion(noterminalactual);
                                return DefinirProduccion(texto, n + 1, noterminalactual);
                            }
                            return EstablecerError(PosicionActual + x, "Se esperaba un '.' o '|' después de la '}'.");
                        }
                        return EstablecerError(PosicionActual + n, "Se esperaba el signo }.");
                    }
                    return EstablecerError(PosicionActual + n, "No se detecto nignun procedimiento con anterioridad.");
                }
                else if (texto[n] == '|')
                {
                    if (ValidarProduccion(texto, n))
                    {
                        if (Elementos.Count == 0)
                        {
                            Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, 0, ListaProducciones.Count + 2, noterminalactual);
                            ListaProducciones.Add(nuevo);
                            ListaProducciones[ListaProducciones.Count - 1].Elementos.Add(0);
                            ModificarSiguiente();
                            ContadorProduccion++;
                            return DefinirProduccion(texto, n + 1, noterminalactual);
                        }
                        else
                        {
                            Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, Elementos.Count, ListaProducciones.Count + 2, noterminalactual);
                            ListaProducciones.Add(nuevo);
                            AgregarElementos(ListaProducciones.Count - 1);
                            Elementos.Clear();
                            ContadorProduccion++;
                        }
                        VerificarProduccion(noterminalactual);
                        return DefinirProduccion(texto, n + 1, noterminalactual);
                    }
                    return EstablecerError(PosicionActual + n, "La definición de la producción es incorrecta.");
                }
                else if (texto[n] == '.')
                {
                    if (ValidarProduccion(texto, n))
                    {
                        if (Elementos.Count == 0)
                        {
                            Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, 0, 0, noterminalactual);
                            ListaProducciones.Add(nuevo);
                            ListaProducciones[ListaProducciones.Count - 1].Elementos.Add(0);
                            ContadorProduccion++;
                            if (ListaProducciones[ListaProducciones.Count - 2].Siguiente == ListaProducciones[ListaProducciones.Count - 1].Produccion)
                            {
                                ListaProducciones[ListaProducciones.Count - 2].Siguiente = 0;
                            }
                            PosicionActual += n + 1;
                            NuevaProduccion = n + 1;
                            return true;
                        }
                        else
                        {
                            Tabla_Producciones nuevo = new Tabla_Producciones(ListaProducciones.Count + 1, Elementos.Count, 0, noterminalactual);
                            ListaProducciones.Add(nuevo);
                            AgregarElementos(ListaProducciones.Count - 1);
                            Elementos.Clear();
                            ContadorProduccion++;
                        }
                        VerificarProduccion(noterminalactual);
                        PosicionActual += n + 1;
                        NuevaProduccion = n + 1;
                        return true;
                    }
                    return EstablecerError(PosicionActual + n, "La definición de la producción es incorrecta.");
                }
                else if (texto[n] == 'ɛ' || texto[n] == 'Ɛ' || texto[n] == 'Ԑ' || texto[n] == '§' || texto[n] == 'ԑ')
                {

                    return DefinirProduccion(texto, n + 1, noterminalactual);
                }
                return EstablecerError(PosicionActual + n, "La definición de la producción es incorrecta.");
            }

            public string ObtenerNoTerminal(int n)
            {
                return ListaNoTerminal[(-n) - 1].NT;
            }

            public string ObtenerTerminal(int n)
            {
                return ListaTokens[n - 1].simbolo;
            }

            public void VerificarProduccion(string produccion)
            {
                for (int i = ListaProducciones.Count - 2; i >= 0; i--)
                {
                    if (ListaProducciones[ListaProducciones.Count - 1].NoTerminal == ListaProducciones[i].NoTerminal)
                    {
                        ListaProducciones[i].Siguiente = ListaProducciones[ListaProducciones.Count - 1].Produccion;
                        break;
                    }
                }
            }

            public bool ValidarProduccion(string texto, int n)
            {
                for (int i = n - 1; i > 0; i--)
                {
                    if (!Char.IsWhiteSpace(texto[i]))
                    {
                        switch (texto[i])
                        {
                            //tambien caso de epsilon
                            case 'ɛ':
                                return true;
                            case '_':
                                return true;
                            case '>':
                                if (texto[i - 1] == '-')
                                {
                                    return false;
                                }
                                return true;
                            case '\'':
                                return true;
                            case '\"':
                                return true;
                            case '}':
                                return true;
                            default:
                                if (Char.IsLetterOrDigit(texto[i]))
                                {
                                    return true;
                                }
                                return false;
                        }
                    }
                }
                return false;
            }

            //Metodo para analizar commentarios

            public bool DefinirComentario(string texto, int n)
            {
                n = SaltarEspacios(texto, n);
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (palabra.ToLower() == "comments")
                    {
                        comment = true;
                        return DefinirComentario(texto, palabra.Length + n);
                    }
                    if (palabra.ToLower() == "to")
                    {
                        if (YaEmpezo)
                        {
                            To = true;
                            return DefinirComentario(texto, palabra.Length + n);
                        }
                        else
                        {
                            return EstablecerError(PosicionActual + n, "No se definio la parte inicial del comentario.");
                        }
                    }
                    if (comment)
                    {
                        return EstablecerError(PosicionActual + n, "Se esperaba la palabra To.");
                    }
                }
                else if (texto[n] == '\'')
                {
                    if (To)
                    {
                        if (!cc)
                        {
                            return EstablecerError(PosicionActual + n, "La definición del comentario es incorrecta.");
                        }
                        else
                        {
                            cerro = true;
                        }
                    }
                    else
                    {
                        YaEmpezo = true;
                        cc = true;
                    }
                    string palabra = "";
                    bool encontro = false;
                    int i;
                    for (i = n + 1; i < texto.Length; i++)
                    {
                        if (texto[i] != '\'')
                        {
                            palabra += texto[i];
                        }
                        else
                        {
                            encontro = true;
                            break;
                        }
                    }
                    if (encontro)
                    {
                        palabra = palabra.TrimEnd();
                        palabra = palabra.TrimStart();
                        if (palabra.IndexOf(' ') == -1 && palabra != "")
                        {
                            return DefinirComentario(texto, i + 1);
                        }
                    }
                    return EstablecerError(PosicionActual + n, "La definición del comentario es incorrecta.");
                }
                else if (texto[n] == '\"')
                {
                    if (To)
                    {
                        if (cc)
                        {
                            return EstablecerError(PosicionActual + n, "La definición del comentario es incorrecta.");
                        }
                        else
                        {
                            cerro = true;
                        }
                    }
                    else
                    {
                        YaEmpezo = true;
                        cc = false;
                    }
                    string palabra = "";
                    bool encontro = false;
                    int i;
                    for (i = n + 1; i < texto.Length; i++)
                    {
                        if (texto[i] != '\"')
                        {
                            palabra += texto[i];
                        }
                        else
                        {
                            encontro = true;
                            break;
                        }
                    }
                    if (encontro)
                    {
                        palabra = palabra.TrimEnd();
                        palabra = palabra.TrimStart();
                        if (palabra.IndexOf(' ') == -1 && palabra != "")
                        {
                            return DefinirComentario(texto, i + 1);
                        }
                    }
                    return EstablecerError(PosicionActual + n, "La definición del comentario es incorrecta.");
                }
                else if (texto[n] == '.')
                {
                    if (cerro)
                    {
                        PosicionActual += n + 1;
                        return true;
                    }
                }
                return EstablecerError(PosicionActual + n, "Error en la definición del Comment.");
            }

            //Metodo para analizar keywords

            public bool DefinirKeyword(string texto, int n)
            {
                n = SaltarEspacios(texto, n);
                if (n == -1)
                {
                    return EstablecerError(PosicionActual, "Error en la definición de los Keywords.");
                }
                else
                {
                    if (texto[n] == '\"')
                    {
                        n = SaltarEspacios(texto, n + 1);
                        int x = n;
                        string palabra = ObtenerPalabra(texto, n);
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '\"')
                        {
                            if (EsPalabra(palabra))
                            {
                                n = SaltarEspacios(texto, n + 1);
                                ListaKeywords.Add(palabra.ToLower());
                                if (texto[n] == '.')
                                {
                                    PosicionActual += n;
                                    return true;
                                }
                                else if (texto[n] == ',')
                                {
                                    return DefinirKeyword(texto, n + 1);
                                }
                                return EstablecerError(PosicionActual + n, "");
                            }
                            return EstablecerError(PosicionActual + x, "La definición de la Keyword es incorrecta.");
                        }
                        return EstablecerError(PosicionActual + x, "La definición del Keyword es incorrecta.");
                    }
                    else if (texto[n] == '\'')
                    {
                        n = SaltarEspacios(texto, n + 1);
                        int x = n;
                        string palabra = ObtenerPalabra(texto, n);
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '\'')
                        {
                            if (EsPalabra(palabra))
                            {
                                n = SaltarEspacios(texto, n + 1);
                                ListaKeywords.Add(palabra.ToLower());
                                if (texto[n] == '.')
                                {
                                    PosicionActual += n;
                                    return true;
                                }
                                else if (texto[n] == ',')
                                {
                                    return DefinirKeyword(texto, n + 1);
                                }
                                return EstablecerError(PosicionActual + n, "Se esperaba una ',' o un '.'.");
                            }
                            return EstablecerError(PosicionActual + x, "La definición del Keyword es incorrecta.");
                        }
                        return EstablecerError(PosicionActual + x, "La definición del Keyword es incorrecta.");
                    }
                    return EstablecerError(PosicionActual, "Se esperaba la definición de un Keyword.");
                }
                return EstablecerError(PosicionActual, "Error en la definición del Keyword.");
            }


            //Metodo para Definir Tokens

            public bool ValidarExpresion(int n, string texto, int c)
            {
                if (texto != "")
                {
                    if (n < texto.Length)
                    {
                        n = SaltarEspacios(texto, n);
                        switch (texto[n])
                        {
                            case '\'':
                                {
                                    if ((n + 2) < texto.Length)
                                    {
                                        if (texto[n + 2] == '\'' && texto[n + 1] != '\'')
                                        {
                                            valor = true;
                                            UtilizacionPipe();
                                            NuevaDefinicion = n + 2;
                                            if (Pipe)
                                            {
                                                Pipe = false;
                                            }
                                            return ValidarExpresion(n + 3, texto, c);
                                        }
                                    }
                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case '\"':
                                {

                                    if ((n + 2) < texto.Length)
                                    {
                                        if (texto[n + 2] == '\"' && texto[n + 1] != '\"')
                                        {
                                            valor = true;
                                            UtilizacionPipe();
                                            NuevaDefinicion = n + 2;
                                            if (Pipe)
                                            {
                                                Pipe = false;
                                            }
                                            return ValidarExpresion(n + 3, texto, c);
                                        }
                                    }

                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case '(':
                                {
                                    parentesis++;
                                    if (texto.Length >= (n + 1))
                                    {
                                        if (texto[n + 1] != ')')
                                        {
                                            bool x = false;
                                            for (int i = n + 1; i < texto.Length; i++)
                                            {
                                                if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                                                {
                                                    switch (texto[i])
                                                    {
                                                        case '\'':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '\"':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '(':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '[':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                    }
                                                    if (!x)
                                                    {
                                                        if (Char.IsLetter(texto[i]))
                                                        {
                                                            x = true;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!x)
                                            {
                                                return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                            }
                                            else
                                            {
                                                return ValidarExpresion(n + 1, texto, c);
                                            }
                                        }
                                    }
                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case ')':
                                {
                                    parentesis--;
                                    if (parentesis < 0)
                                    {
                                        return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                    }
                                    else
                                    {
                                        valor = true;
                                        UtilizacionPipe();
                                        return ValidarExpresion(n + 1, texto, c);
                                    }
                                }
                            case '*':
                                {
                                    if (ValidarSimbolo(n, texto, c))
                                    {
                                        valor = true;
                                        return ValidarExpresion(n + 1, texto, c);
                                    }
                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case '+':
                                {
                                    if (ValidarSimbolo(n, texto, c))
                                    {
                                        valor = true;
                                        return ValidarExpresion(n + 1, texto, c);
                                    }
                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case '?':
                                {
                                    if (ValidarSimbolo(n, texto, c))
                                    {
                                        valor = true;
                                        return ValidarExpresion(n + 1, texto, c);
                                    }
                                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                }
                            case '.':
                                {
                                    if (ValidarSimbolo(n, texto, c))
                                    {
                                        if (parentesis == 0)
                                        {
                                            if (!Pipe)
                                            {
                                                NuevaDefinicion = n;
                                                valor = true;
                                                return true;
                                            }
                                        }
                                    }
                                    return EstablecerError(PosicionActual, "La definición del token es incorrecta.");
                                }
                            case '|':
                                {
                                    if (valor)
                                    {
                                        if (!AccionEnToken)
                                        {
                                            bool x = false;
                                            for (int i = n + 1; i < texto.Length; i++)
                                            {
                                                if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                                                {
                                                    switch (texto[i])
                                                    {
                                                        case '\'':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '\"':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '(':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                        case '[':
                                                            {
                                                                x = true;
                                                                break;
                                                            }
                                                    }
                                                    if (!x)
                                                    {
                                                        if (Char.IsLetter(texto[i]))
                                                        {
                                                            x = true;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            if (!x)
                                            {
                                                return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                            }
                                            else
                                            {
                                                Pipe = true;
                                                return ValidarExpresion(n + 1, texto, c);
                                            }
                                        }
                                        else
                                        {
                                            return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                    }
                                }
                            default:
                                {
                                    if (Char.IsLetter(texto[n]))
                                    {
                                        string palabra = ObtenerPalabra(texto, n);
                                        if (palabra.ToLower() == "check")
                                        {
                                            n = SaltarEspacios(texto, n + 5);
                                            if (texto[n] == '.')
                                            {
                                                check = true;
                                                NuevaDefinicion = n;
                                                ListaCheckTokens[ListaCheckTokens.Count - 1] = true;
                                                return true;
                                            }
                                            return EstablecerError(PosicionActual + n, "Se esperaba un punto al final de la definición del token.");
                                        }
                                        else if (PalabraReservada(palabra))
                                        {
                                            return EstablecerError(PosicionActual + n, "Uso de una palabra reservada en la definición del token.");
                                        }
                                        else if (ListaConjuntos.Contains(palabra))
                                        {
                                            valor = true;
                                            NuevaDefinicion = n + palabra.Length;
                                            if (Pipe)
                                            {
                                                Pipe = false;
                                            }
                                            return ValidarExpresion(n + palabra.Length, texto, c);
                                        }
                                        else
                                        {
                                            return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                                    }
                                }
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return EstablecerError(PosicionActual + n, "La definición del token es incorrecta.");
                }
            }

            public bool DefinirToken(int n, string texto)
            {
                parentesis = 0;
                AccionEnToken = false;
                if (ValidarExpresion(n, texto, n))
                {
                    return true;
                }
                return false;
            }

            public bool ValidarSimbolo(int n, string texto, int c)
            {
                for (int i = n - 1; i >= c; i--)
                {
                    if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                    {
                        switch (texto[i])
                        {
                            case ')':
                                {
                                    return true;
                                }
                            case '\'':
                                {
                                    return true;
                                }
                            case '\"':
                                {
                                    return true;
                                }
                        }
                        if (Char.IsLetterOrDigit(texto[i]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public void UtilizacionPipe()
            {
                if (Pipe)
                {
                    Pipe = false;
                }
            }

            public void CargarAsociatividad(string a)
            {
                for (int i = ListaTokens.Count - 1; i >= 0; i--)
                {
                    if (ListaTokens[i].asociatividad == "")
                    {
                        ListaTokens[i].asociatividad = a;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //Metodo para Definir Operadores
            public bool DefinirOperador(string texto, int n)
            {
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (palabra.ToLower() == "left")
                    {
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '.')
                        {
                            PosicionActual += n + 1;
                            NuevaDefinicion = n;
                            CargarAsociatividad("left");
                            return true;
                        }
                        else
                        {
                            return EstablecerError(PosicionActual + n, "Se esperaba un punto.");
                        }
                    }
                    else if (palabra.ToLower() == "right")
                    {
                        n = SaltarEspacios(texto, n + palabra.Length);
                        if (texto[n] == '.')
                        {
                            PosicionActual += n + 1;
                            NuevaDefinicion = n;
                            CargarAsociatividad("right");
                            Presedencia++;
                            return true;
                        }
                        else
                        {
                            return EstablecerError(PosicionActual + n, "Se esperaba un punto.");
                        }
                    }
                }
                else if (texto[n] == '\"')
                {
                    n = SaltarEspacios(texto, n + 1);
                    int i;
                    for (i = n; i < texto.Length; i++)
                    {
                        if (texto[i] == '\"')
                        {
                            string[] aux = texto.Substring(n, i - n).Split(' ');
                            TOKEN nuevo = new TOKEN(ContadorToken, '\"' + aux[0].ToLower() + '\"', Presedencia, "");
                            ListaTokens.Add(nuevo);
                            if (aux.Length > 1)
                            {
                                return EstablecerError(PosicionActual + n, "El operador no se definio correctamente.");
                            }
                            else
                            {
                                n = SaltarEspacios(texto, i + 1);
                                ContadorToken++;
                                if (Char.IsLetter(texto[n]))
                                {
                                    return DefinirOperador(texto, n);
                                }
                                else if (texto[n] == ',')
                                {
                                    n = SaltarEspacios(texto, n + 1);
                                    return DefinirOperador(texto, n);
                                }
                                else if (texto[n] == '.')
                                {
                                    PosicionActual += n + 1;
                                    NuevaDefinicion = n;
                                    CargarAsociatividad("null");
                                    return true;
                                }
                                return EstablecerError(PosicionActual + n, "Se esperaba una ','.");
                            }
                        }
                    }
                    if (i == texto.Length)
                    {
                        return EstablecerError(PosicionActual + i, "El operador no se definio correctamente.");
                    }
                }
                else if (texto[n] == '\'')
                {
                    n = SaltarEspacios(texto, n + 1);
                    int i;
                    for (i = n; i < texto.Length; i++)
                    {
                        if (texto[i] == '\'')
                        {
                            string[] aux = texto.Substring(n, i - n).Split(' ');
                            TOKEN nuevo = new TOKEN(ContadorToken, '\'' + aux[0].ToLower() + '\'', Presedencia, "");
                            ListaTokens.Add(nuevo);
                            if (aux.Length > 1)
                            {
                                return EstablecerError(PosicionActual + n, "El operador no se definio correctamente.");
                            }
                            else
                            {
                                n = SaltarEspacios(texto, i + 1);
                                ContadorToken++;
                                if (Char.IsLetter(texto[n]))
                                {
                                    return DefinirOperador(texto, n);
                                }
                                else if (texto[n] == ',')
                                {
                                    n = SaltarEspacios(texto, n + 1);
                                    return DefinirOperador(texto, n);
                                }
                                else if (texto[n] == '.')
                                {
                                    PosicionActual += n + 1;
                                    NuevaDefinicion = n;
                                    CargarAsociatividad("null");
                                    return true;
                                }
                                return EstablecerError(PosicionActual + n, "Se esperaba una ','.");
                            }
                        }
                    }
                    if (i == texto.Length)
                    {
                        return EstablecerError(PosicionActual + i, "El operador no se definio correctamente.");
                    }
                }
                else if (texto[n] == '.')
                {
                    if (FinalOperador(texto, n))
                    {
                        PosicionActual += n + 1;
                        NuevaDefinicion = n;
                        CargarAsociatividad("null");
                        return true;
                    }
                    return EstablecerError(PosicionActual + n, "Error en la definición del operador.");
                }
                return EstablecerError(PosicionActual + n, "Error en la definición del operador.");
            }

            public bool FinalOperador(string texto, int n)
            {
                for (int i = n - 1; i > texto.Length; i--)
                {
                    if (!Char.IsWhiteSpace(texto[i]))
                    {
                        switch (texto[i])
                        {
                            case '\'':
                                return true;
                            case '\"':
                                return true;
                            default: return false;
                        }
                    }
                }
                return false;
            }

            //Metodos para Definir Conjuntos
            public int ObtenerChr(int n, string texto)
            {
                if ((n + 2) < texto.Length)
                {
                    string chr = texto[n] + texto[n + 1].ToString() + texto[n + 2].ToString();
                    if (chr.ToLower() == "chr")
                    {
                        for (int i = n + 3; i < texto.Length; i++)
                        {
                            if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                            {
                                if (texto[i] == '(')
                                {
                                    int x = TerminaParentesis(i + 1, texto);
                                    if (x != -1)
                                    {
                                        string numero = texto.Substring(i + 1, x - i - 1);
                                        if (EsNumero(numero))
                                        {
                                            numero = QuitarEspacios(numero);
                                            TerminaChr = x;
                                            return int.Parse(numero);
                                        }
                                    }
                                    return -1;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return -1;
            }

            public bool EsNumero(string texto)
            {
                int n = -1;
                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                    {
                        n = i;
                        break;
                    }
                }
                string x = ""; int h = -1;
                if (n != -1)
                {
                    for (int i = n; i < texto.Length; i++)
                    {
                        if (Char.IsDigit(texto[i]))
                        {
                            x += texto[n];
                        }
                        else
                        {
                            h = i;
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }
                if (h != -1)
                {
                    for (int i = h; i < texto.Length; i++)
                    {
                        if (texto[i] != ' ' && texto[i] != '\n' && texto[i] != '\t')
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public int TerminaParentesis(int n, string texto)
            {
                for (int i = n; i < texto.Length; i++)
                {
                    if (texto[i].Equals(')') && Char.IsNumber(texto[i - 1]))
                    {
                        return i;
                    }
                }
                return -1;
            }

            public int ObtenerFinalConjunto(string texto)
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    if (texto[i] == ')')
                    {
                        if (!Char.IsNumber(texto[i - 1]) || texto[i] == '\'' || texto[i] == '\"' || texto[i - 1] == ')')
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }

            public string QuitarEspacios(string texto)
            {
                int i = 0;
                string aux = "";
                while (i < texto.Length)
                {
                    if (texto[i] != ' ' && texto[i] != '\t' && texto[i] != '\n')
                    {
                        aux += texto[i];
                    }
                    i++;
                }
                return aux;
            }

            public bool ValidarCierre(int n, string lenguaje)
            {
                for (int i = n - 1; i > -1; i--)
                {
                    if (lenguaje[i] != '\t' && lenguaje[i] != '\n' && lenguaje[i] != ' ')
                    {
                        if (lenguaje[i] == '\'' || lenguaje[i] == '\"' || lenguaje[i] == ')')
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }

            //Metodo que define al conjunto
            public bool DefinirConjunto(string texto, int caracteractual, int caracterfinal)
            {
                if (texto != "")
                {
                    if (caracteractual <= caracterfinal)
                    {
                        caracteractual = SaltarEspacios(texto, caracteractual);
                        switch (texto[caracteractual])
                        {
                            case '\'':
                                {
                                    if ((caracteractual + 2) < texto.Length)
                                    {
                                        if (texto[caracteractual + 2] == '\'')
                                        {
                                            if (rango[0] == -1 && rango[1] == -1)
                                            {
                                                rango[0] = texto[caracteractual + 1];
                                                Apostrofe = Conjunto = true;
                                                return DefinirConjunto(texto, caracteractual + 3, caracterfinal);
                                            }
                                            else
                                            {
                                                if (Apostrofe)
                                                {
                                                    if (rango[1] == -1)
                                                    {
                                                        rango[1] = texto[caracteractual + 1];
                                                    }
                                                    if (rango[1] >= rango[0])
                                                    {
                                                        CHR = Comillas = Apostrofe = Conjunto = false;
                                                        rango[0] = rango[1] = -1;
                                                        return DefinirConjunto(texto, caracteractual + 3, caracterfinal);
                                                    }
                                                    else
                                                    {
                                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                    }
                                                }
                                                else
                                                {
                                                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case '\"':
                                {
                                    if ((caracteractual + 2) < texto.Length)
                                    {
                                        if (texto[caracteractual + 2] == '\"')
                                        {
                                            if (rango[0] == -1 && rango[1] == -1)
                                            {
                                                rango[0] = texto[caracteractual + 1];
                                                Comillas = Conjunto = true;
                                                return DefinirConjunto(texto, caracteractual + 3, caracterfinal);
                                            }
                                            else
                                            {
                                                if (Comillas)
                                                {
                                                    if (rango[1] == -1)
                                                    {
                                                        rango[1] = texto[caracteractual + 1];
                                                    }
                                                    if (rango[1] >= rango[0])
                                                    {
                                                        CHR = Comillas = Apostrofe = Conjunto = false;
                                                        rango[0] = rango[1] = -1;
                                                        return DefinirConjunto(texto, caracteractual + 3, caracterfinal);
                                                    }
                                                    else
                                                    {
                                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                    }
                                                }
                                                else
                                                {
                                                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case '.':
                                {
                                    if ((caracteractual + 1) < texto.Length)
                                    {
                                        if (texto[caracteractual + 1] == '.')
                                        {
                                            if (rango[0] != -1)
                                            {
                                                return DefinirConjunto(texto, caracteractual + 2, caracterfinal);
                                            }
                                            else
                                            {
                                                return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                            }
                                        }
                                        else
                                        {
                                            return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case '+':
                                {
                                    if (rango[1] == -1)
                                    {
                                        Apostrofe = Comillas = CHR = false;
                                        rango[0] = -1;
                                        return DefinirConjunto(texto, caracteractual + 1, caracterfinal);
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case 'c':
                                {
                                    int x = ObtenerChr(caracteractual, texto);
                                    if (x != -1)
                                    {
                                        if (rango[0] == -1 && rango[1] == -1)
                                        {
                                            CHR = Conjunto = true;
                                            rango[0] = x;
                                            char aux = (char)x;
                                            return DefinirConjunto(texto, TerminaChr + 1, caracterfinal);
                                        }
                                        else
                                        {
                                            if (CHR)
                                            {
                                                rango[1] = x;
                                                if (rango[1] >= rango[0])
                                                {
                                                    rango[0] = rango[1] = -1;
                                                    CHR = Comillas = Apostrofe = Conjunto = false;
                                                    return DefinirConjunto(texto, TerminaChr + 1, caracterfinal);
                                                }
                                                else
                                                {
                                                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                }
                                            }
                                            else
                                            {
                                                return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case 'C':
                                {
                                    int x = ObtenerChr(caracteractual, texto);
                                    if (x != -1)
                                    {
                                        if (rango[0] == -1 && rango[1] == -1)
                                        {
                                            CHR = Conjunto = true;
                                            rango[0] = x;
                                            char aux = (char)x;
                                            return DefinirConjunto(texto, TerminaChr + 1, caracterfinal);
                                        }
                                        else
                                        {
                                            if (CHR)
                                            {
                                                rango[1] = x;
                                                if (rango[1] >= rango[0])
                                                {
                                                    rango[0] = rango[1] = -1;
                                                    CHR = Comillas = Apostrofe = Conjunto = false;
                                                    return DefinirConjunto(texto, TerminaChr + 1, caracterfinal);
                                                }
                                                else
                                                {
                                                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                                }
                                            }
                                            else
                                            {
                                                return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            case ')':
                                {
                                    if (rango[1] == -1)
                                    {
                                        if (!ValidarCierre(caracteractual, texto))
                                        {
                                            return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                        }
                                        else
                                        {
                                            NuevaDefinicion = caracteractual;
                                            rango[0] = rango[1] = -1;
                                            Comillas = Apostrofe = CHR = Conjunto = false;
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                    }
                                }
                            default:
                                {
                                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                                }

                        }
                    }
                    else
                    {
                        if (!Conjunto && rango[1] == -1)
                        {
                            NuevaDefinicion = PosicionActual;
                            rango[0] = -1;
                            Comillas = Apostrofe = CHR = Conjunto = false;
                            return true;
                        }
                        else
                        {
                            return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto correctamente.");
                        }
                    }
                }
                else
                {
                    return EstablecerError(PosicionActual + caracteractual, "No se definio el conjunto.");
                }
            }

            //Analiza la palabra para ver si es una palabra reservada
            public bool PalabraReservada(string palabra)
            {
                switch (palabra.ToLower())
                {
                    case "compiler":
                        return true;
                    case "left":
                        return true;
                    case "right":
                        return true;
                    case "tokens":
                        return true;
                    case "keywords":
                        return true;
                    case "comments":
                        return true;
                    case "units":
                        return true;
                    default:
                        return false;
                }
            }

            //Analiza si la seccion de unidades existe
            public bool ExistenUnidades(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (palabra.ToLower() == "units")
                    {
                        PosicionActual += palabra.Length + n;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //Analiza si hay unidades
            public int HayUnidades(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                PosicionError = 0;
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (PalabraReservada(palabra.ToLower()))
                    {
                        PosicionError = n;
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (texto[n] == '.')
                    {
                        PosicionError = n;
                        return 3;
                    }
                    return 2;
                }
            }

            //Analiza las unidades si hay
            public bool AnalizarUnidades(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                int FinUnidades = texto.IndexOf('.');
                if (FinUnidades == -1)
                {
                    return EstablecerError(PosicionActual + n, "Se esperaba punto al final de las unidades.");
                }
                string[] Unidades = texto.Substring(n, FinUnidades - n).Split(',');
                int posunidad = 0;
                for (int i = 0; i < Unidades.Length; i++)
                {
                    string nombreunidad = Unidades[i].TrimStart();
                    nombreunidad = nombreunidad.TrimEnd();
                    if (!EsPalabra(nombreunidad) || PalabraReservada(nombreunidad) || nombreunidad.IndexOf(' ') != -1)
                    {
                        return EstablecerError(PosicionActual + posunidad + n, "No se definio correctamente la unidad.");
                    }
                    ListaUnidades.Add(nombreunidad);
                    posunidad += Unidades[i].Length + 1;
                }
                PosicionActual += FinUnidades + 1;
                return true;
            }

            //Metodo que detecta si el string es un ID
            public bool EsPalabra(string palabra)
            {
                if (palabra == "")
                {
                    return false;
                }
                else
                {
                    if (char.IsLetter(palabra[0]))
                    {
                        for (int i = 1; i < palabra.Length; i++)
                        {
                            if (!Char.IsLetterOrDigit(palabra[i]) || palabra[i] == '_')
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            //Analiza el titulo 
            public bool AnalizarTitulo(string texto)
            {
                int n = SaltarEspacios(texto, 0);
                if (Char.IsLetter(texto[n]))
                {
                    string palabra = ObtenerPalabra(texto, n);
                    if (palabra.ToLower() == "compiler")
                    {
                        n = SaltarEspacios(texto, n + 8);
                        if (Char.IsLetter(texto[n]))
                        {
                            NombreCompilador = ObtenerPalabra(texto, n);
                            int punto = PuntoFinal(texto, n + NombreCompilador.Length);
                            if (punto == -1)
                            {
                                return EstablecerError(n + NombreCompilador.Length, "Se esperaba un punto.");
                            }
                            else
                            {
                                PosicionActual = punto + 1;
                                return true;
                            }
                        }
                        else
                        {
                            return EstablecerError(n, "Se esperaba el nombre del compilador.");
                        }
                    }
                    else
                    {
                        return EstablecerError(n + 1, "Se esperaba el nombre del compilador.");
                    }
                }
                else
                {
                    return EstablecerError(n, "Se esperaba el nombre del compilador.");
                }
            }

            //Obtiene la posicion de un punto
            public int PuntoFinal(string texto, int n)
            {
                for (int i = n; i < texto.Length; i++)
                {
                    if (texto[i] == '.')
                    {
                        return i;
                    }
                    else
                    {
                        if (!Char.IsWhiteSpace(texto[i]))
                        {
                            return -1;
                        }
                    }
                }
                return -1;
            }

            //Establece el error
            public bool EstablecerError(int n, string error)
            {
                PosicionError = n;
                MensageError = error;
                return false;
            }

            //Obtiene una palabra desde un caracter
            public string ObtenerPalabra(string texto, int n)
            {
                string palabra = "";
                for (int i = n; i < texto.Length; i++)
                {
                    if (Char.IsLetterOrDigit(texto[i]) || texto[i] == '_')
                    {
                        palabra += texto[i];
                    }
                    else
                    {
                        break;
                    }
                }
                return palabra;
            }

            //Saltar todos los espacios del texto
            public int SaltarEspacios(string texto, int n)
            {
                for (int i = n; i < texto.Length; i++)
                {
                    if (!Char.IsWhiteSpace(texto[i]))
                    {
                        return i;
                    }
                }
                return -1;
            }

            //Obtiene el mensaje de error
            public string GetMensajeError()
            {
                return MensageError;
            }
            //Obtiene la posicion del texto donde esta el error
            public int GetPosicionError()
            {
                return PosicionError;
            }
            //Establece el error que ocurrio


            //-----------------------------------------------Fase 5-----------------------------------------------//
            //Calcular el First de los no terminales
            public void CalcularFirst()
            {
                int cambio = 1;
                while (cambio > 0)
                {
                    cambio = 0;
                    for (int i = 0; i < ListaNoTerminal.Count; i++)
                    {
                        for (int j = 0; j < ListaProducciones.Count; j++)
                        {
                            //Encontrar las producciones que produce un no terminal
                            if (ListaProducciones[j].NoTerminal == ListaNoTerminal[i].NT)
                            {
                                int Elemento = ListaProducciones[j].Elementos[0];
                                //No Terminal
                                if (Elemento < 0)
                                {
                                    //Ya tiene first
                                    if (TieneFirst(-Elemento))
                                    {
                                        //Agregar si tiene
                                        if (AgregarFirst(-Elemento, i))
                                        {
                                            cambio++;
                                        }
                                        if (TerminalNull(-Elemento))
                                        {
                                            bool ProduccionNula = true;
                                            for (int u = 1; u < ListaProducciones[j].Elementos.Count; u++)
                                            {
                                                int AuxElemento = ListaProducciones[j].Elementos[u];
                                                //Terminal
                                                if (AuxElemento > 0)
                                                {
                                                    if (!ListaNoTerminal[i].First.Contains(AuxElemento))
                                                    {
                                                        ListaNoTerminal[i].First.Add(AuxElemento);
                                                        cambio++;
                                                    }
                                                    ProduccionNula = false;
                                                    break;
                                                }
                                                //No terminal
                                                else if (AuxElemento < 0)
                                                {
                                                    if (TieneFirst(-AuxElemento))
                                                    {
                                                        if (AgregarFirst(-AuxElemento, i))
                                                        {
                                                            cambio++;
                                                        }
                                                        if (!TerminalNull(-AuxElemento))
                                                        {
                                                            ProduccionNula = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ProduccionNula)
                                            {
                                                if (!ListaNoTerminal[i].First.Contains(0))
                                                {
                                                    ListaNoTerminal[i].First.Add(0);
                                                    cambio++;
                                                }
                                            }
                                        }
                                    }
                                }
                                //Terminal
                                else if (Elemento >= 0)
                                {
                                    //Agregar terminal al first
                                    if (!ListaNoTerminal[i].First.Contains(Elemento))
                                    {
                                        ListaNoTerminal[i].First.Add(Elemento);
                                        cambio++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public bool TieneFirst(int p)
            {
                if (ListaNoTerminal[p - 1].First.Count == 0)
                {
                    return false;
                }
                return true;
            }

            public bool AgregarFirst(int origen, int destino)
            {
                bool agrego = false;
                for (int i = 0; i < ListaNoTerminal[origen - 1].First.Count; i++)
                {
                    if (!ListaNoTerminal[destino].First.Contains(ListaNoTerminal[origen - 1].First[i]) && ListaNoTerminal[origen - 1].First[i] != 0)
                    {
                        ListaNoTerminal[destino].First.Add(ListaNoTerminal[origen - 1].First[i]);
                        agrego = true;
                    }
                }
                return agrego;
            }

            public bool TerminalNull(int p)
            {
                if (ListaNoTerminal[p - 1].First.Contains(0))
                {
                    return true;
                }
                return false;
            }

            //-----------------------------------------------Fase 6-----------------------------------------------//



        
}
}
