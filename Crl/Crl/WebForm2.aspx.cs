using Crl.Analizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Crl
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Sintactico sintactico = new Sintactico();
            bool resultado = sintactico.analizar(TextBox1.Text);
            TextBox3.Text = Sintactico.consola;
            if (resultado)
            {

                TextBox2.Text = "correcto";
            }
            else
            {
                TextBox2.Text = "incorrecto";
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}