using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WFA_COMPILADORES_AJ
{
    class TablaNo_Terminales
    {
        public int Numero;
        public string NT;
        public List<int> First;
        public int Produccion;

        public TablaNo_Terminales(int n, string nt, int p)
        {
            this.Numero = n;
            this.NT = nt;
            this.Produccion = p;
            First = new List<int>();
        }
    } 
}
