using Crl.Analizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;

namespace Crl
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Compilador comp = new Compilador();
            bool resultado = comp.analizar(TextBox1.Text);
            TextBox3.Text = Compilador.consola.Replace("\"","").Replace("'","").Replace("`","");
            if (resultado)
            {

               // TextBox2.Text = "correcto";
            }
            else
            {
              //  TextBox2.Text = "incorrecto";
            }
        }

        #region Referencias Autogeneradas (NO CAMBIAR)
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        #endregion

        protected void Button6_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "C:\\Users\\arnol\\OneDrive\\Escritorio\\album");
        }
    }
}