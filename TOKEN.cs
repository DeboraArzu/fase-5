using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFA_COMPILADORES_AJ
{
    class TOKEN
    {
        public int numero;
        public string simbolo;
        public int presedencia;
        public string asociatividad;

        public TOKEN(int n, string s, int p, string a)
        {
            this.numero = n;
            this.simbolo = s;
            this.presedencia = p;
            this.asociatividad = a;
        }

    }
}
