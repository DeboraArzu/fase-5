using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_COMPILADORES_AJ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Gramatica gra = new Gramatica();
        GoTos go = new GoTos();
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
                if (gra.AnalizarTexto(rchtb_Texto.Text))
                {
                    rchtb_Texto.SelectAll();
                    rchtb_Texto.SelectionBackColor = Color.Green;
                    gra.CalcularFirst();
                    CargarTablas();
                    go.Parseo();
                    CargarParseo();
                    btnSave.Visible = true;
                    btnGenerar.Visible = true;
                }
                else
                {
                    txtMensaje.Text = "";
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
                dgvProducciones.Rows[i].Cells[3].Value = ObtenerElementos(Gramatica.ListaProducciones[i].Elementos);
            }
        }

        public string ObtenerElementos(List<int> lista)
        {
            string aux = "";
            for (int i = 0; i < lista.Count; i++)
            {
                //Negativo no terminal
                if (lista[i] < 0)
                {
                    aux += "[ " + gra.ObtenerNoTerminal(lista[i]) + " ]=>";
                }
                else if (lista[i] > 0)
                {
                    //Positivo terminal
                    aux += "[ " + gra.ObtenerTerminal(lista[i]) + " ]=>";
                }
                else
                {
                    aux += "[ ɛ ]=>";
                }
            }
            if (aux.Length > 2)
            {
                return aux.Substring(0, aux.Length - 2);
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
                dtgvNoTerminalTable.Rows[i].Cells[3].Value = ObtenerElementos(Gramatica.ListaNoTerminal[i].First);
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

        public void CargarParseo()
        {
            string texto = "";
            for (int i = 0; i < GoTos.Quernel.Count; i++)
            {
                texto += GoTos.Quernel[i].N + " : GOTO ";
                //Goto
                for (int j = 0; j < GoTos.Quernel[i].GE.Count; j++)
                {
                    texto += "(" + GoTos.Quernel[i].GE[j] + "," + GoTos.Quernel[i].GT[j] + "),";
                }
                texto += "\n";
                for (int j = 0; j < GoTos.Quernel[i].NTN.Count; j++)
                {
                    texto += GoTos.Quernel[i].NTN[j] + " -> " + ObtenerEstados(GoTos.Quernel[i].PN[j]) + "     ║ " + ObtenerEstados(GoTos.Quernel[i].LAN[j]) + "\n";
                }
                for (int j = 0; j < GoTos.Quernel[i].NT.Count; j++)
                {
                    texto += GoTos.Quernel[i].NT[j] + " -> " + ObtenerEstados(GoTos.Quernel[i].P[j]) + "    ║ " + ObtenerEstados(GoTos.Quernel[i].LA[j]) + "\n";
                }
                texto += "\n------------------------------------------------------------\n\n";
            }
            txtEstados.Text = texto;
        }

        public string ObtenerEstados(List<int> lista)
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
                    if (lista[i] == GoTos.Punto)
                    {
                        aux += "•";
                    }
                    else if (lista[i] == GoTos.Dolar)
                    {
                        aux += "$";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                escribir_data(savefile.FileName);
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                Llenar_tok(savefile.FileName);
            }
        }
        public void Llenar_tok(string ruta) {
            TextWriter sw = new StreamWriter(ruta);
            int TokenActual = 1;
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

        public void escribir_data(string ruta) {
            TextWriter sw = new StreamWriter(ruta);
            sw.WriteLine("[Tabla Tokens]");
            sw.Write("Número".PadRight(20, ' '));
            sw.Write("Símbolo".PadRight(20, ' '));
            sw.Write("Presedencia".PadRight(20, ' '));
            sw.WriteLine("Asociatividad".PadRight(20, ' '));
            for (int i = 0; i < Gramatica.ListaTokens.Count; i++)
            {
                sw.Write(Gramatica.ListaTokens[i].numero.ToString().PadRight(20, ' ')+"║");
                sw.Write(SustituirComillas(Gramatica.ListaTokens[i].simbolo).PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaTokens[i].presedencia.ToString().PadRight(20, ' ') + "║");
                sw.WriteLine(Gramatica.ListaTokens[i].asociatividad.PadRight(20, ' ') + "║");
            }
            sw.WriteLine();
            sw.WriteLine("[Tabla de No Terminales]");
            sw.Write("No terminal".PadRight(20, ' ') + "║");
            sw.Write("Producción".PadRight(20, ' ') + "║");
            sw.WriteLine("First".PadRight(20, ' ') + "║");
            for (int i = 0; i < Gramatica.ListaNoTerminal.Count; i++)
            {
                sw.Write(Gramatica.ListaNoTerminal[i].NT.PadRight(20, ' ')+"║");
                sw.Write(Gramatica.ListaNoTerminal[i].Produccion.ToString().PadRight(20, ' ') + "║");
                sw.WriteLine(ObtenerElementos(Gramatica.ListaNoTerminal[i].First).PadRight(20, ' ') + "║");
            }
            sw.WriteLine();
            sw.WriteLine("[Tabla de Producciones]");
            sw.Write("Producción".PadRight(20, ' ') + "║");
            sw.Write("Longitud".PadRight(20, ' ') + "║");
            sw.Write("Siguiente".PadRight(20, ' ') + "║");
            sw.WriteLine("Elementos".PadRight(20, ' ') + "║");
            for (int i = 0; i < Gramatica.ListaProducciones.Count; i++)
            {
                sw.Write(Gramatica.ListaProducciones[i].Produccion.ToString().PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaProducciones[i].Longitud.ToString().PadRight(20, ' ') + "║");
                sw.Write(Gramatica.ListaProducciones[i].Siguiente.ToString().PadRight(20, ' ') + "║");
                sw.WriteLine(ObtenerElementos(Gramatica.ListaProducciones[i].Elementos).PadRight(20, ' ') + "║");
            }
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

    }
}
