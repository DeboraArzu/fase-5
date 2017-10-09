using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_COMPILADORES_AJ
{
    class Tabla_Producciones
    {
        public int Produccion;
        public int Longitud;
        public int Siguiente;
        public List<int> Elementos;
        public string NoTerminal;

        public Tabla_Producciones(int p, int l, int s, string nt)
        {
            this.Produccion = p;
            this.Longitud = l;
            this.Siguiente = s;
            this.Elementos = new List<int>();
            this.NoTerminal = nt;
        }
    }
}
