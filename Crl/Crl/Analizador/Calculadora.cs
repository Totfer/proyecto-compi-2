using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;
using Crl.Pila;

namespace Crl.Analizador
{
    public class Calculadora
    {
        public static String dato;
        public static String ResolverOperacion(ParseTreeNode root, Tabla tabla)
        {

            String retorno = root.ToString();

            foreach (ParseTreeNode hijo in root.ChildNodes)
            {
                String a = hijo.ToString();
                if (hijo.ToString().Equals("Expd") && !root.ToString().Equals("Expd"))
                {
                    String resultado = Expresion(hijo, tabla);
                    dato = resultado;
                    retorno = resultado;
                }

                ResolverOperacion(hijo,tabla);


            }
            return retorno;
        }


        public static String Expresion(ParseTreeNode root, Tabla tabla)
        {
            String ass = root.ToString();
            switch (root.ChildNodes.Count)
            {

                case 1:
                    String[] numero = (root.ChildNodes.ElementAt(0).ToString().Split(' '));
                    String retorno="";
                    if (numero.Count() > 1) {
                        for (int i=0; i<numero.Count()-1;i++) {
                            retorno +=numero[i];

                        }

                    }
                    if (retorno.Equals("asignar"))
                    {
                        retorno = "";
                    }
                    else if (Sintactico.tipod.Equals("String"))
                    {
                        foreach (Simbolos sb in tabla.tabla)
                        {
                            
                            if (sb.nombre.Equals(numero[0]))
                            {
                                retorno = sb.valor;
                            }
                        }
                        
                        retorno = "`" + retorno + "`";

                    }
                    else if (Sintactico.tipod.Equals("Bool"))
                    {
                        foreach (Simbolos sb in tabla.tabla)
                        {
                            if (sb.nombre.Equals(numero[0]))
                            {
                                retorno = sb.valor;
                            }
                        }
                        if (retorno.Equals("0")) {
                            retorno = "false";
                        }else if (retorno.Equals("1"))
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
                    else if ((Sintactico.tipod.Equals("Double") && retorno.Contains("'")) || (Sintactico.tipod.Equals("Int") && retorno.Contains("'")))
                    {
                        retorno = Convert.ToString((int)Convert.ToChar(retorno.Replace("'", "")));
                    }
                    else
                    {
                        foreach (Simbolos sb in tabla.tabla)
                        {
                            if (sb.nombre.Equals(numero[0]))
                            {

                                if (sb.tipo.Equals("Char"))
                                {
                                    retorno = Convert.ToString((int)Convert.ToChar(sb.valor.Replace("\'", "")));
                                }
                                else
                                {
                                    retorno = sb.valor;
                                }
                            }
                        }


                    }
                        
                    return retorno;

                case 2:
                    if (root.ChildNodes.ElementAt(0).ToString().Substring(0, 1).Equals("!"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(1), tabla).Equals("true"))
                        {
                            return "false";
                        }
                        return "true";
                    }
                    else
                    {

                        return Convert.ToString(-1 * Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(1), tabla)));
                    }
                case 3:
                    if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("&&"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") && Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals(">="))
                    {
                        if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) >= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("||"))
                    {
                        if (Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))
                        {
                            return "true";
                        }
                        return "false";
                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("|&"))
                    {
                        if ((Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))||(Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("false") || Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("false")))
                        {
                            return "false";
                        }
                        return "true";
                    }

                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("<="))
                    {

                        if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) <= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                        {
                            return "true";
                        }
                        return "false";

                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 1).Equals("~"))
                    {

                        if (Expresion(root.ChildNodes.ElementAt(0), tabla).Trim().ToLower() == Expresion(root.ChildNodes.ElementAt(2), tabla).Trim().ToLower())
                        {
                            return "true";
                        }
                        return "false";

                    }
                    else if (root.ChildNodes.ElementAt(1).ToString().Substring(0, 2).Equals("=="))
                    {
                        try
                        {
                            if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) == Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                            {
                                return "true";
                            }
                            return "false";
                        }
                        catch (Exception e)
                        {
                            if (Expresion(root.ChildNodes.ElementAt(0), tabla) == Expresion(root.ChildNodes.ElementAt(2), tabla))
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
                            if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) != Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                            {
                                return "true";
                            }
                            return "false";
                        }
                        catch (Exception e)
                        {
                            if (Expresion(root.ChildNodes.ElementAt(0), tabla) != Expresion(root.ChildNodes.ElementAt(2), tabla))
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

                                    return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) + Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)));
                                }
                                catch (Exception e)
                                {

                                    String b1 = "";
                                    String b2 = "";

                                    if (root.ChildNodes.ElementAt(0).ToString().Equals("Expd"))
                                    {
                                        b1 = Expresion(root.ChildNodes.ElementAt(0), tabla);
                                    }
                                    if (root.ChildNodes.ElementAt(2).ToString().Equals("Expd"))
                                    {

                                        b2 = Expresion(root.ChildNodes.ElementAt(2), tabla);
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

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) - Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)));
                            case "%":

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) % Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)));
                            case "*":


                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) * Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)));
                            case "/":

                                return Convert.ToString(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) / Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)));
                            case "^":

                                return Convert.ToString(Math.Pow(Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)), Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))); ;
                            case "==":
                                try
                                {
                                    if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) == Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                    {
                                        return "true";
                                    }
                                    return "false";
                                }
                                catch (Exception e)
                                {
                                    if (Expresion(root.ChildNodes.ElementAt(0), tabla) == Expresion(root.ChildNodes.ElementAt(2), tabla))
                                    {
                                        return "true";
                                    }
                                    return "false";


                                }
                            case "!=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) != Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "<":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) < Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "~":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla).Trim().ToLower() == Expresion(root.ChildNodes.ElementAt(2), tabla).Trim().ToLower())
                                {
                                    return "true";
                                }
                                return "false";
                            case "<=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) <= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                {
                                    return "true";
                                }
                                return "false";
                            case ">":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) > Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                {
                                    return "true";
                                }
                                return "false";
                            case ">=":

                                if (Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(0), tabla)) >= Convert.ToDouble(Expresion(root.ChildNodes.ElementAt(2), tabla)))
                                {
                                    return "true";
                                }
                                return "false";
                            case "&&":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") && Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))
                                {
                                    return "true";
                                }
                                return "false";

                            case "||":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))
                                {
                                    return "true";
                                }
                                return "false";
                            case "|&":

                                if (Expresion(root.ChildNodes.ElementAt(0), tabla).Equals("true") || Expresion(root.ChildNodes.ElementAt(2), tabla).Equals("true"))
                                {
                                    return "false";
                                }
                                return "true";

                            default:
                                String asa = root.ChildNodes.ElementAt(1).ToString().Substring(0, 1);
                                return Expresion(root.ChildNodes.ElementAt(1), tabla);

                        }
                    }
            }
            return "";
        }

    }
}
