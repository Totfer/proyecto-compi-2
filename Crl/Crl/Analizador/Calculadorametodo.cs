using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;
using Crl.Pila;
using Crl.TablaMetodos;

namespace Crl.Analizador
{
    public class Calculadorametodo
    {
        public static String dato;
        public static String ResolverOperacion(ParseTreeNode root, PilaTS tabla,TablaM tablam)
        {

            String retorno = root.ToString();

            foreach (ParseTreeNode hijo in root.ChildNodes)
            {
                String a = hijo.ToString();
                if (hijo.ToString().Equals("Expd") && !root.ToString().Equals("Expd"))
                {
                    String resultado = Expresion(hijo, tabla, tablam);
                    dato = resultado;
                    if (resultado != null)
                    {
                        retorno = resultado;

                    }
                    else {
                        retorno = resultado;
                    }
                }

               // ResolverOperacion(hijo, tabla);


            }
            return retorno;
        }


        public static String Expresion(ParseTreeNode root, PilaTS tabla,TablaM tablam) 
        {
            String ass = root.ToString();
            switch (root.ChildNodes.Count)
            {

                case 1:
                    String[] numero = (root.ChildNodes.ElementAt(0).ToString().Split(' '));
                    String retorno = "";
                    if (numero[0].Equals("Llamada"))
                    {
                        String[] numero1 = (root.ChildNodes.ElementAt(0).ToString().Split(' '));

                        Simbolos sm = new Simbolos();
                        Tabla tb = new Tabla();
                        Compilador sin = new Compilador();
                        PilaTS pl = new PilaTS();
                        Tabla tabla1 = new Tabla();
                        bool cond = false;
                        bool cond2 = true;
                        foreach (Tabla tab in tabla.pila) {
                            foreach (Simbolos simb in tab.tabla)
                            {
                                if (cond2) {
                                    tabla1 = tab;
                                    cond2 = false;
                                }
                                if (simb.ambito.Equals("global")) {
                                    pl.pila.Push(tab);
                                    pl.pila.Push(tabla1);
                                    cond = true;
                                    break;
                                }
                            }
                            if (cond) { break; }
                        }
                        int x = 0;
                        while (tablam.tablam.Count>x) {

                            if (numero[0].Equals(tablam.tablam[x].nombre)) {

                                break;
                            }
                            x++;

                        }
                        DatoSB dato = new DatoSB();
                        dato = sin.Llamada_Metodo(root.ChildNodes.ElementAt(0), pl, tb, tablam,false,true);
                        sm = dato.retorno;
                        if (sm.valor == null)
                        {
                            numero[0] = "";
                        }
                        else
                        {
                            numero[0] = sm.valor;
                        }

                        switch (sin.tipom)
                        {
                            case "Int":
                                try
                                {
                                   numero[0] = Convert.ToString(Convert.ToInt32(numero[0]));
                                }
                                catch (Exception e)
                                {
                                    Compilador.error = "Error semantico en el retorno del metodo "+sm.nombre+ ". Se debe retornar un dato tipo Int\n";
                                }
                                break;
                            case "Bool":
                                if (numero[0].Equals("true") || numero[0].Equals("0") || numero[0].Equals("false") || numero[0].Equals("0"))
                                {
                                    numero[0] = numero[0];
                                }
                                else
                                {
                                    Compilador.error = "Error semantico en el retorno del metodo " + sm.nombre + ". Se debe retornar un dato tipo Bool\n";
                                }
                                break;
                            case "Double":
                                try
                                {
                                    numero[0] = Convert.ToString(Convert.ToDouble(numero[0]));
                                }
                                catch (Exception e)
                                {
                                    Compilador.error = "Error semantico en el retorno del metodo " + sm.nombre + ". Se debe retornar un dato tipo Double\n";
                                }
                                break;
                            case "String":
                                numero[0] = numero[0];
                                break;
                            case "Char":
                                if (numero[0].Contains("'"))
                                {
                                    numero[0] = numero[0];
                                }
                                else
                                {
                                    Compilador.error = "Error semantico en el retorno del metodo " + sm.nombre + ". Se debe retornar un dato tipo Char\n";
                                }
                                break;
                        }







                        retorno = numero[0];
                    }
                    else
                    {

                        if (numero.Count() > 1)
                        {
                            for (int i = 0; i < numero.Count() - 1; i++)
                            {
                                retorno += numero[i];

                            }

                        }
                        if (retorno.Equals("asignar"))
                        {
                            retorno = "";
                        }
                        else if (Compilador.tipod.Equals("String"))
                        {
                            bool bl = false;
                            foreach (Tabla tab in tabla.pila)
                            {
                                foreach (Simbolos sb in tab.tabla)
                                {

                                    if (sb.nombre.Equals(numero[0]))
                                    {
                                        bl = true;
                                        retorno = sb.valor;
                                    }
                                    if (bl) { break; }
                                }
                                if (bl) { break; }
                            }
                            if (retorno.Equals("true")) {
                                retorno = "1";
                            }else if (retorno.Equals("false"))
                            {
                                retorno = "0";
                            }
                            retorno = "`" + retorno + "`";

                        }
                        else if (Compilador.tipod.Equals("Bool"))
                        {
                            bool bl = false;
                            foreach (Tabla tab in tabla.pila)
                            {
                                foreach (Simbolos sb in tab.tabla)
                                {
                                    if (sb.nombre.Equals(numero[0]))
                                    {
                                        bl = true;
                                        retorno = sb.valor;
                                    }
                                    if (bl) { break; }
                                }
                                if (bl) { break; }
                            }
                            if (retorno.Equals("0"))
                            {
                                retorno = "false";
                            }
                            else if (retorno.Equals("1"))
                            {
                                retorno = "true";
                            }
                            return retorno;

                        }
                        else if (retorno.Equals("true"))
                        {
                            retorno = "1";
                        }
                        else if (retorno.Equals("false"))
                        {
                            retorno = "0";
                        }
                        else if ((Compilador.tipod.Equals("Double") && retorno.Contains("'")) || (Compilador.tipod.Equals("Int") && retorno.Contains("'")))
                        {
                            try
                            {
                                retorno = Convert.ToString((int)Convert.ToChar(retorno.Replace("'", "")));
                            }
                            catch (Exception e) {
                            }
                        }
                        else
                        {
                            bool bl = false;
                            foreach (Tabla tab in tabla.pila)
                            {
                                foreach (Simbolos sb in tab.tabla)
                                {
                                    if (sb.nombre.Equals(numero[0].Replace("'","")))
                                    {
                                        bl = true;
                                        if (sb.tipo.Equals("Char"))
                                        {
                                            retorno = Convert.ToString((int)Convert.ToChar(sb.valor.Replace("\'", "")));
                                        }
                                        else
                                        {
                                            retorno = sb.valor;
                                        }
                                    }
                                    if (bl) { break; }
                                }
                                if (bl) { break; }
                            }

                        }
                    }

                    return retorno;

                case 2:
                    if (root.ChildNodes.ElementAt(0).ToString().Substring(0, 1).Equals("!"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(1), tabla, tablam).Equals("true"))
                        {
                            return "false";
                        }
                        return "true";
                    }
                    else
                    {

                        return Convert.ToString(-1 * Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(1), tabla, tablam)));
                    }
                case 3:
                    if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("&&"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") && Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true"))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals(">="))
                    {
                        if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) >= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("||"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true"))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("|&"))
                    {
                        if ((Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true")) || (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("false") || Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("false")))
                        {
                            return "false";
                        }
                        return "true";
                    }

                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("<="))
                    {

                        if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) <= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                        {
                            return "true";
                        }
                        return "false";

                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1).Equals("~"))
                    {

                        if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Trim().ToLower() == Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Trim().ToLower())
                        {
                            return "true";
                        }
                        return "false";

                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("=="))
                    {
                        try
                        {
                            if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) == Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                            {
                                return "true";
                            }
                            return "false";
                        }
                        catch (Exception e)
                        {
                            if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam) == Expresion(root.ChildNodes.ElementAt(2), tabla, tablam))
                            {
                                return "true";
                            }
                            return "false";


                        }
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("!="))
                    {
                        try
                        {
                            if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) != Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                            {
                                return "true";
                            }
                            return "false";
                        }
                        catch (Exception e)
                        {
                            if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam) != Expresion(root.ChildNodes.ElementAt(2), tabla, tablam))
                            {
                                return "true";
                            }
                            return "false";


                        }
                    }
                    else
                    {
                        switch (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1))
                        {

                            case "+":
                                try
                                {

                                    return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) + Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)));
                                }
                                catch (Exception e)
                                {

                                    String b1 = "";
                                    String b2 = "";

                                    if (root.ChildNodes.ElementAt(0).ToString().Equals("Expd"))
                                    {
                                        b1 = Expresion(root.ChildNodes.ElementAt(0), tabla, tablam);
                                    }
                                    if (root.ChildNodes.ElementAt(2).ToString().Equals("Expd"))
                                    {

                                        b2 = Expresion(root.ChildNodes.ElementAt(2), tabla, tablam);
                                    }



                                    String a11 = "";
                                    String a22 = "";
                                    String c1 = "";
                                    String c2 = "";


                                    String[] a1 = root.ChildNodes.ElementAt(0).ToString().Split('(');
                                    String[] a2 = root.ChildNodes.ElementAt(2).ToString().Split('(');

                                    if (b1.Equals(""))
                                    {


                                    }
                                    else
                                    {
                                        a11 = b1;


                                    }
                                    if (b1.Equals(""))
                                    {

                                    }
                                    else
                                    {
                                        a22 = b2;

                                    }




                                    if (a11.Equals(""))
                                    {
                                        c1 = a1[0];
                                    }
                                    else
                                    {
                                        c1 = a11;
                                    }

                                    if (a22.Equals(""))
                                    {
                                        c2 = a2[0];
                                    }
                                    else
                                    {
                                        c2 = a22;
                                    }




                                    return c1 + c2;


                                }
                            case "-":

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) - Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)));
                            case "%":

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) % Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)));
                            case "*":


                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) * Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)));
                            case "/":

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) / Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)));
                            case "^":

                                return Convert.ToString(Math.Pow(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)), Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))); ;
                            case "==":
                                try
                                {
                                    if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) == Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                    {
                                        return "true";
                                    }
                                    return "false";
                                }
                                catch (Exception e)
                                {
                                    if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam) == Expresion(root.ChildNodes.ElementAt(2), tabla, tablam))
                                    {
                                        return "true";
                                    }
                                    return "false";


                                }
                            case "!=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) != Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "<":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) < Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "~":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Trim().ToLower() == Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Trim().ToLower())
                                {
                                    return "true";
                                }
                                return "false";
                            case "<=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) <= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                {
                                    return "true";
                                }
                                return "false";
                            case ">":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) > Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                {
                                    return "true";
                                }
                                return "false";
                            case ">=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla, tablam)) >= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla, tablam)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "&&":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") && Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true"))
                                {
                                    return "true";
                                }
                                return "false";

                            case "||":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true"))
                                {
                                    return "true";
                                }
                                return "false";
                            case "|&":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla, tablam).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla, tablam).Equals("true"))
                                {
                                    return "false";
                                }
                                return "true";

                            default:
                                String asa = root.ChildNodes.ElementAt(1).ToString().Substring(0, 1);
                                return Expresion(root.ChildNodes.ElementAt(1), tabla, tablam);

                        }
                    }
            }
            return "";
        }


    }
}