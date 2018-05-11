using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crl.Graficas
{
    public class GraficarAST
    {
        private static int contadore;
        private static String grafoexp;

        public static String getDOT(ParseTreeNode raiz)
        {
            grafoexp = "digraph G{";
            grafoexp += "node[shape=\"box\"];\n";
            grafoexp += "nodo0[label=\"" + escapare(raiz.ToString()).Replace("\"", "") + "\"];\n";
            contadore = 1;
            recorrerASTe("nodo0", raiz);
            grafoexp += "}";

            return grafoexp;
        }


        private static void recorrerASTe(String padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                if (!hijo.ToString().Equals("Expd"))
                {
                    String nombreHijo = "nodo" + contadore.ToString();
                    grafoexp += nombreHijo + "[label=\"" + escapare(hijo.ToString()).Replace("\"", "") + "\"];\n";
                    grafoexp += padre + "->" + nombreHijo + ";\n";
                    contadore++;
                    recorrerASTe(nombreHijo, hijo);
                }
                else {
                    grafoexp += "nodo" + contadore.ToString() + "[label=\"" + "Exprecion" + "\"];\n";
                    grafoexp += padre + "->" + "nodo" + contadore.ToString() + ";\n";
                    contadore++;
                    break;

                    //recorrerASTe(padre, hijo);
                }
        }
        }

        private static String escapare(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");


            return cadena;
        }
    }
}