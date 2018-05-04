using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crl.TablaMetodos
{
    public class SimbolosM
    {
        public String tipo;
        public String valor;
        public String nombre;
        public String no;
        public ParseTreeNode raiz;
        public List<Parametros> parametro = new List<Parametros>();
    }
}