using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace WFA_COMPILADORES_AJ
{
    public partial class FRMPRINCIPAL : Form
    {
        string texto = "";
        public FRMPRINCIPAL()
        {
            InitializeComponent();
        }

        Gramatica gra = new Gramatica();
        Gotos go = new Gotos();
        private void cARGARDOCUMENTOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ventana = new OpenFileDialog();
            ventana.Title = "SELECCIONE ARCHIVO";
            ventana.ShowDialog();
            this.rchtb_Texto.Clear();
            try
            {
                if (ventana.FileName != "")
                {
                    string root = ventana.FileName.ToString();
                    string[] lines = System.IO.File.ReadAllLines(root);
                    string ln = "";
                    foreach (string line in lines)
                    {
                        ln = ln + line + "\n";
                    }
                    this.rchtb_Texto.Text = ln + "  ";
                }
            }
            catch (IOException)
            {
                MessageBox.Show("ERROR EN LA LECTURA DEL ARCHIVO");
                throw;
            }
        }

        private void aNALIZARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rchtb_Texto.Text.Trim() != "")
            {
                gra = new Gramatica();
                rchtb_Texto.SelectAll();
                rchtb_Texto.SelectionBackColor = Color.Black;
                texto = rchtb_Texto.Text;
                fix();
                if (gra.AnalizarTexto(texto))
                {
                    //Se pinta todo el texto de verde para indicar que esta correcto
                    rchtb_Texto.SelectAll();
                    gra.CalcularFirst();
                    CargarTablas();
                    go.Parseo();
                    CargarParseo();
                    txtMensaje.Text = "Archivo Correcto!!!";
                }
                else
                {
                    //Se pinta de rojo hasta donde se detecto el error
                    rchtb_Texto.Select(0, gra.GetPosicionError());
                    rchtb_Texto.SelectionBackColor = Color.Red;
                    txtMensaje.Text = gra.GetMensajeError();
                }
            }
            else
            {
                MessageBox.Show("No se a ingresado ningún texto.");
            }
        }
        public void Imprimir_estados(List<string> d)
        {
            TextWriter sw = new StreamWriter("C:\\Users\\Debora\\Desktop\\GOTO.txt");
            foreach (string s in d)
            {
                richTextBox1.Text += s + "\n";
                sw.WriteLine(s + "\n");
            }
            sw.Close();
        }
        private void FRMPRINCIPAL_Load(object sender, EventArgs e)
        {

        }

        public void escribir_Tablas(string ruta)
        {
            TextWriter sw = new StreamWriter(ruta);
            sw.WriteLine("Tabla de Tokens");
            sw.Write("Número".PadRight(20, ' '));
            sw.Write("Símbolo".PadRight(20, ' '));
            sw.Write("Presedencia".PadRight(20, ' '));
            sw.WriteLine("Asociatividad".PadRight(20, ' '));
            for (int i = 0; i < Gramatica.ListaTokens.Count; i++)
            {
                sw.Write(Gramatica.ListaTokens[i].numero.ToString().PadRight(20, ' '));
                sw.Write(SustituirComillas(Gramatica.ListaTokens[i].simbolo).PadRight(20, ' '));
                sw.Write(Gramatica.ListaTokens[i].presedencia.ToString().PadRight(20, ' '));
                sw.WriteLine(Gramatica.ListaTokens[i].asociatividad.PadRight(20, ' '));
            }
            sw.WriteLine();
            sw.WriteLine("Tabla de No Terminales");
            sw.Write("No terminal".PadRight(20, ' ') + "║");
            sw.Write("Producción".PadRight(20, ' ') + "║");
            sw.WriteLine("First".PadRight(20, ' ') + "║");
            for (int i = 0; i < Gramatica.ListaNoTerminal.Count; i++)
            {
                sw.Write(Gramatica.ListaNoTerminal[i].NT.PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaNoTerminal[i].Produccion.ToString().PadRight(20, ' ') + "║");
                sw.WriteLine(ObtenerListaElementos(Gramatica.ListaNoTerminal[i].First).PadRight(20, ' ') + "║");
            }
            sw.WriteLine();
            sw.WriteLine("Tabla de Producciones");
            sw.Write("Producción".PadRight(20, ' ') + "║");
            sw.Write("Longitud".PadRight(20, ' ') + "║");
            sw.Write("Siguiente".PadRight(20, ' ') + "║");
            sw.WriteLine("Elementos".PadRight(20, ' ') + "║");
            for (int i = 0; i < Gramatica.ListaProducciones.Count; i++)
            {
                sw.Write(Gramatica.ListaProducciones[i].Produccion.ToString().PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaProducciones[i].Longitud.ToString().PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaProducciones[i].Siguiente.ToString().PadRight(20, ' ') + "║");
                sw.WriteLine(ObtenerListaElementos(Gramatica.ListaProducciones[i].Elementos).PadRight(20, ' ') + "║");
            }
            sw.Close();
        }
        public void CargarTablas()
        {
            CargarTokens();
            CargarNoTerminales();
            CargarProducciones();
        }

        public void CargarProducciones()
        {
            dgvProducciones.Rows.Clear();
            for (int i = 0; i < Gramatica.ListaProducciones.Count; i++)
            {
                dgvProducciones.Rows.Add();
                dgvProducciones.Rows[i].Cells[0].Value = Gramatica.ListaProducciones[i].Produccion;
                dgvProducciones.Rows[i].Cells[1].Value = Gramatica.ListaProducciones[i].Longitud;
                dgvProducciones.Rows[i].Cells[2].Value = Gramatica.ListaProducciones[i].Siguiente;
                dgvProducciones.Rows[i].Cells[3].Value = ObtenerListaElementos(Gramatica.ListaProducciones[i].Elementos);
            }
        }

        public string ObtenerListaElementos(List<int> lista)
        {
            string aux = "";
            for (int i = 0; i < lista.Count; i++)
            {
                //Negativo no terminal
                if (lista[i] < 0)
                {
                    aux += "[ " + gra.ObtenerNoTerminal(lista[i]) + " ] => ";
                }
                else if (lista[i] > 0)
                {
                    //Positivo terminal
                    aux += "[ " + gra.ObtenerTerminal(lista[i]) + " ] => ";
                }
                else
                {
                    aux += "[ ɛ ] =>";
                }
            }
            if (aux.Length > 3)
            {
                return aux.Substring(0, aux.Length - 3);
            }
            return aux;
        }

        public void CargarTokens()
        {
            TABLA1.Rows.Clear();
            for (int i = 0; i < Gramatica.ListaTokens.Count; i++)
            {
                TABLA1.Rows.Add();
                TABLA1.Rows[i].Cells[0].Value = Gramatica.ListaTokens[i].numero;
                TABLA1.Rows[i].Cells[1].Value = Gramatica.ListaTokens[i].simbolo;
                if (Gramatica.ListaTokens[i].presedencia == 0)
                {
                    TABLA1.Rows[i].Cells[2].Value = ' ';
                }
                else
                {
                    TABLA1.Rows[i].Cells[2].Value = Gramatica.ListaTokens[i].presedencia;
                }
                if (Gramatica.ListaTokens[i].asociatividad == "null" || Gramatica.ListaTokens[i].asociatividad == "" || Gramatica.ListaTokens[i].asociatividad == "TOKEN")
                {
                    TABLA1.Rows[i].Cells[3].Value = ' ';
                }
                else
                {
                    TABLA1.Rows[i].Cells[3].Value = Gramatica.ListaTokens[i].asociatividad;
                }
            }
        }

        public void CargarNoTerminales()
        {
            dtgvNoTerminalTable.Rows.Clear();
            for (int i = 0; i < Gramatica.ListaNoTerminal.Count; i++)
            {
                dtgvNoTerminalTable.Rows.Add();
                dtgvNoTerminalTable.Rows[i].Cells[0].Value = Gramatica.ListaNoTerminal[i].Numero;
                dtgvNoTerminalTable.Rows[i].Cells[1].Value = Gramatica.ListaNoTerminal[i].NT;
                dtgvNoTerminalTable.Rows[i].Cells[3].Value = ObtenerListaElementos(Gramatica.ListaNoTerminal[i].First);
                if (Gramatica.ListaNoTerminal[i].Produccion == -1)
                {
                    dtgvNoTerminalTable.Rows[i].Cells[2].Value = "Error";
                }
                else
                {
                    dtgvNoTerminalTable.Rows[i].Cells[2].Value = Gramatica.ListaNoTerminal[i].Produccion;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                escribir_Tablas(savefile.FileName);
            }
        }
        public void Llenar_tok(string ruta)
        {
            int TokenActual = 0;
            TextWriter sw = new StreamWriter(ruta);
            sw.WriteLine("Tokens");
            for (int i = 0; i < gra.ListaConjuntos.Count; i++)
            {
                string conjunto = gra.ListaConjuntos[i];
                string definicion = gra.ListaDefinicionesConjuntos[i];
                sw.WriteLine("\t" + conjunto.ToLower() + " { " + definicion + " }");
            }
            for (int i = 0; i < Gramatica.ListaTokens.Count; i++)
            {
                string numero = (i + 1).ToString();
                char inicial = Gramatica.ListaTokens[i].simbolo[0];
                if (inicial == '\'' || inicial == '"')
                {
                    string simbolo = SustituirComillas(Gramatica.ListaTokens[i].simbolo);
                    string definicion = "";
                    for (int j = 1; j < simbolo.Length - 1; j++)
                    {
                        if (simbolo[j] == '\'')
                        {
                            definicion += "\"" + simbolo[j].ToString() + "\"";
                        }
                        else
                        {
                            definicion += "'" + simbolo[j].ToString() + "'";
                        }
                    }
                    sw.WriteLine("\tToken " + numero + " = " + definicion);
                }
                else if (Char.IsLetter(inicial))
                {
                    string reservadas = "";
                    string definicion = gra.ListaDefinicionesTokens[TokenActual];
                    if (gra.ListaCheckTokens[TokenActual])
                    {
                        reservadas = " [ Reservadas() ]";
                        definicion = QuitarCheck(definicion);
                    }
                    sw.WriteLine("\tToken " + numero + " = " + definicion.ToLower() + reservadas);
                    TokenActual++;
                }
            }
            int n = Gramatica.ListaTokens.Count + 1;
            sw.WriteLine("Acciones");
            sw.WriteLine("Reservadas()");
            sw.WriteLine("{");

            for (int i = 0; i < gra.ListaKeywords.Count; i++)
            {
                sw.WriteLine("\t" + n.ToString() + " = '" + gra.ListaKeywords[i] + "'");
                n++;
            }
            sw.WriteLine("}");
            sw.WriteLine("Error = " + n.ToString());
            sw.Close();
        }

        public string QuitarCheck(string definicion)
        {
            definicion = definicion.ToLower();
            int x = definicion.IndexOf("check");
            return definicion.Substring(0, definicion.Length - (definicion.Length - x));
        }
        public string SustituirComillas(string x)
        {
            string cadena = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == '"')
                {
                    cadena += "'";
                }
                else
                {
                    cadena += x[i].ToString();
                }
            }
            return cadena;
        }
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                Llenar_tok(savefile.FileName);
            }
        }
        public void CargarParseo()
        {
            string texto = "";
            for (int i = 0; i < Gotos.kernel.Count; i++)
            {
                if (Gotos.kernel[i].N < 301)
                {
                    texto += Gotos.kernel[i].N + " : Goto ";
                    //Goto
                    for (int j = 0; j < Gotos.kernel[i].GE.Count; j++)
                    {
                        texto += "(" + Gotos.kernel[i].GE[j] + "," + Gotos.kernel[i].GT[j] + ") ";
                    }
                    texto += "\n";
                    for (int j = 0; j < Gotos.kernel[i].NTN.Count; j++)
                    {
                        texto += Gotos.kernel[i].NTN[j] + " -> " + ObtenerListaParseo(Gotos.kernel[i].PN[j]) + " \t║ " + ObtenerListaParseo(Gotos.kernel[i].LAN[j]) + "\n";
                    }
                    for (int j = 0; j < Gotos.kernel[i].NT.Count; j++)
                    {
                        texto += Gotos.kernel[i].NT[j] + " -> " + ObtenerListaParseo(Gotos.kernel[i].P[j]) + " \t║ " + ObtenerListaParseo(Gotos.kernel[i].LA[j]) + "\n";
                    }
                    texto += "-----------------------------------------------------------\n\n";
                }
            }
            richTextBox1.Text = texto;
            //
            TextWriter sw = new StreamWriter("C:\\Users\\Debora\\Desktop\\GOTO.txt");
            sw.WriteLine(texto);
            sw.Close();
        }

        public string ObtenerListaParseo(List<int> lista)
        {
            string aux = "";
            for (int i = 0; i < lista.Count; i++)
            {
                //Negativo no terminal
                if (lista[i] < 0)
                {
                    aux += gra.ObtenerNoTerminal(lista[i]) + " ";
                }
                else
                {
                    //Positivo terminal
                    if (lista[i] == Gotos.Punto)
                    {
                        aux += "• ";
                    }
                    else if (lista[i] == Gotos.Dolar)
                    {
                        aux += "$ ";
                    }
                    else
                    {
                        aux += gra.ObtenerTerminal(lista[i]) + " ";
                    }
                }
            }
            if (aux.Length > 1)
            {
                return aux.Substring(0, aux.Length - 1);
            }
            return aux;
        }
        private void rchtb_Texto_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtMensaje_TextChanged(object sender, EventArgs e)
        {

        }

        #region metodos
        private void convertirtokens()
        {
            if (esets)
            {
                string pattern = @"tokens";
                string replacement = "";
                Regex rgx = new Regex(pattern);
                string result = rgx.Replace(texto, replacement);
                this.texto = "";
                this.texto = result;
            }
        }

        private void convertirsets()
        {
            if (esets)
            {
                string pattern = @"sets";
                string replacement = "tokens";
                Regex rgx = new Regex(pattern);
                string result = rgx.Replace(texto, replacement);
                texto = result;
            }
        }

        bool esets = false;
        private void conjuntos(string texto)
        {
            //-------------------------------------sets inicio-----------------------------------------------------------
            string pat = @"sets";
            int inicio = 0;
            esets = false;
            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(texto);
            if (m.Success)
            {
                esets = true;
                inicio = m.Index;
            }
            //----------------------------------------toknes final --------------------------------------------------------
            pat = @"tokens";
            int final = 0;
            int final2 = 0;
            // Instantiate the regular expression object.
            r = new Regex(pat, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            m = r.Match(texto);
            if (m.Success)
            {
                final = m.Index;
                final2 = final - inicio;
            }
            //------------------------------------------------------------------------------------------------------------
            string primerstr = texto.Substring(0, inicio);
            string segundostr = texto.Substring(inicio, final2); //string que me interesa contiene la palabra sets
            string finalstr = texto.Substring(final); //contiene la palabra tokens
            string pattern, pattern2, replacement, replacement2, result = "";
            Regex rgx, rgx2, rgx3;
            //pre arreglo 
            if (esets)
            {
                pattern2 = @"(\'\.)(?!\.)";
                replacement2 = "\' .";
                rgx2 = new Regex(pattern2);
                result = rgx2.Replace(segundostr, replacement2);
                //Console.WriteLine(result);

                pattern = @"(\)\.)(?!\.)";
                replacement = ") .";
                rgx = new Regex(pattern);
                result = rgx.Replace(result, replacement);
                // Console.WriteLine(result);

                pattern = @"[^(""|\'|<|>)]=|=[^(""|\')]";
                replacement = "(";
                rgx3 = new Regex(pattern);
                result = rgx3.Replace(result, replacement);
                // Console.WriteLine(result);

                pattern = @"([^\'|\.|\)])\."; //((\'|\))\.)(?!(\.))
                replacement = ")";
                rgx = new Regex(pattern);
                result = rgx.Replace(result, replacement);
                // Console.WriteLine(result);
                segundostr = result;
            }
            // Console.WriteLine(segundostr);
            this.texto = primerstr + segundostr + finalstr;
            // Console.WriteLine(this.texto);
        }

        private void End()
        {
            string pat = @"end\.";
            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            // Match the regular expression pattern against a text string.
            Match m = r.Match(texto);
            if (m.Success)
            {
                //pre arreglo  
                string pattern2 = @"end\.";
                string replacement2 = "";
                Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
                string result = rgx2.Replace(this.texto, replacement2);
                // Console.WriteLine(result);
                this.texto = result;
            }
            else
            {
                // mens = "Falta End. al final del archivo";
                MessageBox.Show("Falta End. al final del archivo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pat2 = @"(start\=)";
            // Instantiate the regular expression object.
            Regex r2 = new Regex(pat2, RegexOptions.IgnoreCase);
            // Match the regular expression pattern against a text string.
            Match m2 = r2.Match(texto);
            if (m2.Success)
            {
                //pre arreglo  
                string pattern2 = @"(start((\s|\t)*)\=)";
                string replacement2 = "<start> -> ";
                Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);
                string result = rgx2.Replace(this.texto, replacement2);
                // Console.WriteLine(result);
                this.texto = result;
            }
        }

        private void fix()
        {
            conjuntos(texto);
            convertirtokens();
            convertirsets();
            End();
        }

        private void keyworks(string texto)
        {
            string pat = @"keywords";
            int inicio = 0;
            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(texto.ToLower());
            if (m.Success)
            {
                //-------------------------------------keywords inicio-----------------------------------------------------------
                inicio = m.Index;
                //----------------------------------------producciones final --------------------------------------------------------
                pat = @"productions";
                int final = 0;
                int final2 = 0;
                // Instantiate the regular expression object.
                r = new Regex(pat, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                m = r.Match(texto);
                if (m.Success)
                {
                    final = m.Index;
                    final2 = final - inicio;
                }
                //------------------------------------------------------------------------------------------------------------
                string primerstr = texto.Substring(0, inicio);
                string segundostr = texto.Substring(inicio, final2); //string que me interesa
                string finalstr = texto.Substring(final);

                segundostr = "";

                this.texto = primerstr + segundostr + finalstr;
                Console.WriteLine("final: {0}", this.texto);
            }


        }
        #endregion

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
