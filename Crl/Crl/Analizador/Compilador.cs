﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Ast;
using Irony.Parsing;
using System.Drawing;
using System.IO;
using WINGRAPHVIZLib;
using Crl.Graficas;
using Crl.Pila;
using Crl.TablaMetodos;
using System.Text;
using BarcodeLib;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Crl.Analizador
{
    public class Compilador: Grammar
    {
        public static String error;
        public static String tipod;
        public static String tipodmet;
        public static String llamdamet;
        public static String consola;
        public static PilaTS pilax;
        public static ParseTreeNode rai;
        public Simbolos retmet=new Simbolos();
        bool ret;
        Double cont;
        String dup="";
        String ambitots;
        bool ambit=true;
        public bool analizar(String cadena)
        {
            consola = "";
            error += "-";
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode raiz = arbol.Root;
            Tabla tabla = new Tabla();
            TablaM tablam = new TablaM();
            PilaTS pila = new PilaTS();
            rai = raiz;
           
            generarImagen(raiz);
            if (error != "")
            {
                Primera_pasada(raiz.ChildNodes.ElementAt(0), tabla, tablam);
                pila.pila.Push(tabla);
                pilax = pila;
                cont++;
            }
            String ruta = "";
            String datoDefinir = "";

            Segunda_pasada(raiz.ChildNodes.ElementAt(0), pila,tabla,tablam,ruta,datoDefinir);
            if (raiz == null)
            {
                return false;
            }
            return true;
        }
        private void Segunda_pasada(ParseTreeNode root, PilaTS pila,Tabla tabla,TablaM tablam,String ruta,String datoDefnir)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("import_definir_clase"))
            {
                Segunda_pasada(root.ChildNodes.ElementAt(0), pila,tabla, tablam,ruta,datoDefnir);

                switch (root.ChildNodes.ElementAt(1).ToString())
                {
                    case "import":
                        try
                        {
                            String leeri ="";
                            try
                            {
                                String leer = File.ReadAllText("C:\\Users\\arnol\\OneDrive\\Escritorio\\" + root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ToString().Replace(" (Id)", "") + ".clr");
                                leeri = leer;
                            }
                            catch (Exception e)
                            {

                                error += "Error la ruta de la libreria no es correcta \n ";
                                
                            }

                        Gramatica gramatica = new Gramatica();
                        LanguageData lenguaje = new LanguageData(gramatica);
                        Parser parser = new Parser(lenguaje);
                        ParseTree arbol = parser.Parse(leeri);
                        ParseTreeNode raiz = arbol.Root;


                        Primera_pasada(raiz.ChildNodes.ElementAt(0), tabla, tablam);
                        pila.pila.Push(tabla);
                        pilax = pila;
                            String druta = "";
                            String ddatoDefinir = "";

                            Segunda_pasada(raiz.ChildNodes.ElementAt(0), pila, tabla, tablam,druta,ddatoDefinir);
                        }
                        catch (Exception e)
                        {

                            error += "Error en una de las libreras \n ";
                        }

                        break;
                    case "definir":
                        String definir = root.ChildNodes.ElementAt(1).ToString();

                        if (definir.Contains("(Numerod)")) {
                            ruta = definir.Replace(" (Numerod)","");
                        }
                        if (definir.Contains("(TextoS)")) {
                            datoDefnir = definir.Replace(" (TextoS)", "");
                        }
                        break;
                    case "Contenidoclase":
                        Contenido_Principal(root.ChildNodes.ElementAt(1), pila, tablam);
                        break;
                }
            }
            else
            {
                switch (root.ChildNodes.ElementAt(0).ToString())
                {
                    case "import":
                        try{

                            String leeri = "";
                            try
                            {
                                String leer = File.ReadAllText("C:\\Users\\arnol\\OneDrive\\Escritorio\\" + root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).ToString().Replace(" (Id)", "") + ".clr");
                                leeri = leer;
                            }
                            catch (Exception e)
                            {

                                error += "Error la ruta de la libreria no es correcta \n ";

                            }
                            Gramatica gramatica = new Gramatica();
                            LanguageData lenguaje = new LanguageData(gramatica);
                            Parser parser = new Parser(lenguaje);
                            ParseTree arbol = parser.Parse(leeri);
                            ParseTreeNode raiz = arbol.Root;
                            
                          
                            Primera_pasada(raiz.ChildNodes.ElementAt(0), tabla, tablam);
                            pila.pila.Push(tabla);
                            pilax = pila;
                            String druta = "";
                            String ddatoDefinir = "";

                            Segunda_pasada(raiz.ChildNodes.ElementAt(0), pila,tabla, tablam,druta,ddatoDefinir);


                        }
                        catch (Exception e){

                            error += "Error en una de las libreras \n ";

                        }
                        break;
                    case "definir":
                        String definir = root.ChildNodes.ElementAt(0).ToString();

                        if (definir.Contains("(Numerod)"))
                        {
                            ruta = definir.Replace(" (Numerod)", "");
                        }
                        if (definir.Contains("(TextoS)"))
                        {
                            datoDefnir = definir.Replace(" (TextoS)", "");
                        }
                        break;
                    case "Contenidoclase":
                        Contenido_Principal(root.ChildNodes.ElementAt(0), pila, tablam);
                        break;
                }

            }

        }

        private void ImportClase(ParseTreeNode root, PilaTS pila, TablaM tablam) {


        }

        private void Contenido_Principal(ParseTreeNode root, PilaTS pila, TablaM tablam)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidoclase"))
            {
                Contenido_Principal(root.ChildNodes.ElementAt(0), pila, tablam);

                if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Equals("Principal (Keyword)"))
                {
                    ambitots = "Ambito_Principal";
                    Tabla tabla = new Tabla();
                    ret = false;
                    Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(4), pila, tabla,tablam,true, true);
                    ret = false;
                }
            }
            else
            {
                if (root.ChildNodes.ElementAt(0).ToString().Equals("Principal (Keyword)"))
                {
                    ambitots = "Ambito_Principal";
                    Tabla tabla = new Tabla();
                    ret = false;
                    Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4), pila, tabla, tablam,true,true);
                    ret = false;
                }

            }

        }

        //variables para detener
        bool hasta = true;
        bool fo = true;
        bool mientras = true;
        bool selec = true;
        bool defec = false;
        //variables para contnuar
        bool conthasta = true;
        bool contmientras = true;
        bool conti = false;
        bool contfo = true;

        bool continuar = false;
        //retur
        Simbolos simr;
        String retorno;
        int contmos;

        public String tipom;
        private bool Contenido_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla,TablaM tablam,bool verd,bool brek)
        {
            String ass= root.ToString();
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidometfun2"))
            {
               brek = Contenido_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam,verd,brek);
                if (ret) { return brek; }
                if (brek)
                {

                    switch (root.ChildNodes.ElementAt(1).ToString())
                    {
                        case "Variable2":
                            String[] var = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Split(' ');
                            bool salir = true;
                          
                            if (var[1].Equals("(Id)"))
                            {
                                foreach (Tabla tab in pila.pila)
                                {
                                    Tabla tabla2 = new Tabla();
                                    foreach (Simbolos sim in tab.tabla)
                                    {
                                        if (sim.nombre.Equals(var[0]) && salir)
                                        {
                                            salir = false;
                                            switch (sim.tipo)
                                            {
                                                case "Int":
                                                    try
                                                    {
                                                        tipod = "Int";
                                                        sim.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam)));
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Double":
                                                    try
                                                    {
                                                        tipod = "Double";
                                                        sim.valor = Convert.ToString(Convert.ToDouble(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam)));
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "String":
                                                    try
                                                    {
                                                        tipod = "String";
                                                        sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Bool":
                                                    try
                                                    {
                                                        tipod = "";
                                                        if (Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Equals("false"))
                                                        {
                                                            sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Char":
                                                    tipod = "";
                                                    if (Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Contains("'"))
                                                    {
                                                        tipod = "Char";
                                                        try
                                                        {
                                                            sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);
                                                            Char tr = Convert.ToChar(sim.valor.Replace("\'", ""));

                                                        }
                                                        catch (Exception)
                                                        {
                                                        }

                                                        try
                                                        {
                                                            int ti = Convert.ToInt32(sim.valor.Replace("\'", ""));
                                                            sim.valor = Convert.ToString(Convert.ToChar((Char)Convert.ToInt32(ti)));
                                                        }
                                                        catch (Exception f)
                                                        {
                                                            error += "Linea: " + "Columna: " + " Error semantico, no es posible asignar un valor que no sea tipo Char a la variable \"" + sim.nombre + "\" .\n";


                                                        }


                                                    }

                                                    break;
                                            }
                                        }
                                        tabla2.tabla.Add(sim);
                                        if (!salir) { break; }


                                    }
                                    if (!salir) { break; }
                                }

                                break;

                            }
                            pila.pila.Push(tabla);
                            VariableMetodo(root.ChildNodes.ElementAt(1), tabla, pila, tablam);
                            pila.pila.Pop();
                            break;
                        case "SI":
                            pila.pila.Push(tabla);
                            brek = Si_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam,verd, brek);
                            pila.pila.Pop();
                            break;
                        case "Print":
                            pila.pila.Push(tabla);
                            consola = consola+Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam) + " \n ";
                            pila.pila.Pop();
                            break;
                        case "Hasta":
                            pila.pila.Push(tabla);
                            brek = Hasta_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            break;
                        case "Mientras":

                            pila.pila.Push(tabla);
                            brek = Mientras_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();

                            break;

                        case "selecciona":
                            
                            ambitots += "_SubAmbito_Seleccion";
                            pila.pila.Push(tabla);
                            brek = Selecciona_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            ambitots = ambitots.Replace("_SubAmbito_Seleccion", "");

                            break;
                        case "detener":
                            if (verd)
                            {
                                brek = true;
                            }
                            else
                            {
                                error += "Error semantico, no es posble utilizar la sentencia Detener si no es encuentra en un ciclo o en un select\n";
                            }
                            break;
                        case "continua":
                            if (verd)
                            {
                                continuar = true;
                                return false;
                            }
                            else
                            {
                                error += "Error semantico, no es posble utilizar la sentencia Continuar si no es encuentra en un ciclo o en un select\n";
                            }
                            break;
                        case "Para":
                            pila.pila.Push(tabla);
                            brek = Para_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            
                            break;
                        case "retornar":
                            if (root.ChildNodes.ElementAt(1).ChildNodes.Count == 1)
                            {
                                ret = true;
                            }
                            else
                            {
                                tipod = tipodmet;
                                pila.pila.Push(tabla);
                                switch (tipodmet)
                                {
                                    case "Int":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Int";

                                            }
                                        }

                                        break;
                                    case "Double":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToDouble(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }

                                        break;
                                    case "String":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);

                                        break;
                                    case "Bool":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);

                                        if (!(retmet.valor.Equals("true") || retmet.valor.Equals("false") || retmet.valor.Equals("0") || retmet.valor.Equals("1")))
                                        {
                                            error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Bool";

                                        }

                                        break;
                                    case "Char":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Replace("'", "")));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((char)Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }
                                        break;
                                }
                                retmet.ambito = "";
                                retmet.nombre = "llamada";
                                retmet.tipo=""; 
                                ret = true;
                                pila.pila.Pop();
                            }
                            break;
                        case "Llamada":
                            pila.pila.Push(tabla);
                            DatoSB retd = new DatoSB();
                            retd = Llamada_Metodo(root.ChildNodes.ElementAt(1), pila, tabla, tablam, verd, brek);
                            ret = false;
                            brek = retd.brek;
                            pila.pila.Pop();
                            break;
                        case "Mostrar":
                            contmos = 0;
                            pila.pila.Push(tabla);
                            List<String> text = new List<string>();
                            List<String> pos = new List<string>();
                            Mostrar_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3), pila, tabla, tablam, verd, brek, text, pos);
                            String print = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2).ToString().Replace(" (TextoS)", "");
                            int i = 0;
                            while (pos.Count > i)
                            {


                                if (print.Contains(pos[i]))
                                {
                                    print = print.Replace(pos[i], text[i]);
                                }
                                i++;
                            }
                            consola +=print + " \n ";
                            pila.pila.Pop();
                            break;
                        case "Dibujarast":
                            DibujarAST(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2).ToString().Replace(" (Id)", ""), tablam);
                            break;
                        case "Dibujarexp":
                            DibujarExp(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2));
                            break;
                        case "Dibujarts":
                            DibujarTS(tabla);
                            break;
                    }
                }
            }
            else
            {
                if (ret) { return brek; }
                if (brek)
                {

                    switch (root.ChildNodes.ElementAt(0).ToString())
                    {
                        case "Variable2":
                            String[] var = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                            bool salir = true;
                            
                            if (var[1].Equals("(Id)"))
                            {
                                foreach (Tabla tab in pila.pila)
                                {
                                    Tabla tabla2 = new Tabla();
                                    foreach (Simbolos sim in tab.tabla)
                                    {
                                        if (sim.nombre.Equals(var[0]) && salir)
                                        {

                                            salir = false;
                                            switch (sim.tipo)
                                            {
                                                case "Int":
                                                    try
                                                    {
                                                        tipod = "Int";
                                                        sim.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Double":
                                                    try
                                                    {
                                                        tipod = "Double";
                                                        sim.valor = Convert.ToString(Convert.ToDouble(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "String":
                                                    try
                                                    {
                                                        tipod = "String";
                                                        sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Bool":
                                                    try
                                                    {
                                                        tipod = "";
                                                        if (Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Equals("false"))
                                                        {
                                                            sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                case "Char":
                                                    tipod = "";
                                                    if (Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Contains("'"))
                                                    {
                                                        tipod = "Char";
                                                        try
                                                        {
                                                            sim.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);
                                                            Char tr = Convert.ToChar(sim.valor.Replace("\'", ""));

                                                        }
                                                        catch (Exception)
                                                        {
                                                        }

                                                        try
                                                        {
                                                            int ti = Convert.ToInt32(sim.valor.Replace("\'", ""));
                                                            sim.valor = Convert.ToString(Convert.ToChar((Char)Convert.ToInt32(ti)));
                                                        }
                                                        catch (Exception f)
                                                        {
                                                            error += "Linea: " + "Columna: " + " Error semantico, no es posible asignar un valor que no sea tipo Char a la variable \"" + sim.nombre + "\" .\n";


                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                     
                                        tabla2.tabla.Add(sim);
                                        if (!salir) { break; }
                                    }
                                    if (!salir) { break; }
                                }
                               

                                break;

                            }
                            pila.pila.Push(tabla);
                            VariableMetodo(root.ChildNodes.ElementAt(0), tabla, pila, tablam);
                            pila.pila.Pop();
                            break;
                        case "SI":                    
                            pila.pila.Push(tabla);
                            brek = Si_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            break;
                        case "Print":
                            pila.pila.Push(tabla);
                            consola = consola+Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam) + " \n ";
                            pila.pila.Pop();
                            break;
                        case "Hasta":
 
                            pila.pila.Push(tabla);
                            brek = Hasta_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                         

                            break;
                        case "Mientras":

                            pila.pila.Push(tabla);
                            brek = Mientras_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            break;

                        case "selecciona":
                            ambitots += "_SubAmbito_Selecciona";
                            pila.pila.Push(tabla);
                            brek= Selecciona_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            ambitots = ambitots.Replace("_SubAmbito_Selecciona", "");
                            break;
                        case "detener":
                            if (verd)
                            {
                                return false;
                            }
                            else
                            {
                                error += "Error semantico, no es posble utilizar la sentencia Detener si no es encuentra en un ciclo o en un select\n";
                            }
                            break;
                        case "continua":
                            if (verd)
                            {
                                continuar = true;
                                return false;
                            }
                            else
                            {
                                error += "Error semantico, no es posble utilizar la sentencia Continuar si no es encuentra en un ciclo o en un select\n";
                            }
                            break;
                        case "Para":
                            pila.pila.Push(tabla);
                            brek = Para_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            pila.pila.Pop();
                            break;
                        case"Llamada":
                            pila.pila.Push(tabla);
                            DatoSB retd = new DatoSB();
                            retd = Llamada_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek);
                            ret = false;
                            brek =retd.brek;
                            pila.pila.Pop();
                            break;
                        case "retornar":
                            if (root.ChildNodes.ElementAt(0).ChildNodes.Count == 1)
                            {
                                ret = true;
                            }
                            else {
                                pila.pila.Push(tabla);
                                tipod = tipodmet;
                                switch (tipodmet) {
                                    case "Int":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Int";

                                            }
                                        }

                                        break;
                                    case "Double":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToDouble(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }

                                        break;
                                    case "String":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);                                         

                                        break;
                                    case "Bool":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);
                                                      tipod = tipodmet;
                                switch (tipodmet) {
                                    case "Int":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Int";

                                            }
                                        }

                                        break;
                                    case "Double":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam)));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((int)Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }

                                        break;
                                    case "String":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);                                         

                                        break;
                                    case "Bool":
                                        retmet.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);

                                        if (!(retmet.valor.Equals("true") || retmet.valor.Equals("false") || retmet.valor.Equals("0") || retmet.valor.Equals("1"))) {
                                            error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Bool";

                                        }

                                        break;
                                    case "Char":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'","")));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((char)Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }
                                        break;
                                }
                                        if (!(retmet.valor.Equals("true") || retmet.valor.Equals("false") || retmet.valor.Equals("0") || retmet.valor.Equals("1"))) {
                                            error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Bool";

                                        }

                                        break;
                                    case "Char":
                                        try
                                        {
                                            retmet.valor = Convert.ToString(Convert.ToChar(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'","")));
                                        }
                                        catch (Exception e)
                                        {
                                            try
                                            {
                                                retmet.valor = Convert.ToString((char)Convert.ToInt32(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam).Replace("'", "")));
                                            }
                                            catch (Exception)
                                            {
                                                error += "El retorno de la funcion " + llamdamet + " debe de ser ded tipo Double";

                                            }
                                        }
                                        break;
                                }
                                retmet.ambito = "";
                                retmet.nombre = "llamada";
                                retmet.tipo = "";
                                ret = true;
                                pila.pila.Pop();
                            }
                            break;
                        case "Mostrar":
                            contmos = 0;
                            pila.pila.Push(tabla);
                            List<String> text = new List<string>();
                            List<String> pos = new List<string>();
                            Mostrar_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3), pila, tabla, tablam, verd, brek,text,pos);
                            String print = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ToString().Replace(" (TextoS)","");
                            int i = 0;
                            while (pos.Count>i) {


                                if (print.Contains(pos[i])) {
                                    print = print.Replace(pos[i], text[i]);
                                }
                                i++;
                            }
                            consola += print+" \n ";
                            pila.pila.Pop();
                            break;
                        case"Dibujarast":
                            DibujarAST(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ToString().Replace(" (Id)",""),tablam);
                            break;
                        case "Dibujarexp":
                            DibujarExp(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                            break;
                        case "Dibujarts":
                            DibujarTS(tabla);
                            break;
                    }
                }
            }
            return brek;
        }



        int contts;
 
        private void DibujarTS(Tabla tabla)
        {
            String texto = "";
            if (!dup.Equals(ambitots))
            {

                dup = ambitots;
            texto += "-------------------------------------------------------------- \n";
            texto += "Ambito |" + ambitots + "| \n";
            texto += "-------------------------------------------------------------- \n";

            foreach (Simbolos sim in tabla.tabla)
            {
                texto += "-------------------------------------------------------------- \n";
                texto += "Variable " + sim.nombre + "| Valor: " + sim.valor + ", Tipo: " + sim.tipo + "| \n";
                texto += "-------------------------------------------------------------- \n";

            }
            try
            {

                //CREAMOS EL OBJETO IMAGEN
                Bitmap objBmp = new Bitmap(1, 1);
                int Width = 0;
                int Height = 0;
                //LE DAMOS EL FORMATO DE LA FUENTE
                Font objFont = new Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

                Graphics objGraphics = Graphics.FromImage(objBmp);

                Width = (int)objGraphics.MeasureString(texto, objFont).Width;
                Height = (int)objGraphics.MeasureString(texto, objFont).Height;

                objBmp = new Bitmap(objBmp, new Size(Width, Height));

                objGraphics = Graphics.FromImage(objBmp);

                objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                objGraphics.DrawString(texto, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
                objGraphics.Flush();
                objBmp.Save("C:\\Users\\arnol\\OneDrive\\Escritorio\\album\\Tabla" + contts.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                contts++;
            }
            catch (Exception e) { }
        }
        }
        private void DibujarAST(String nombre, TablaM tablam)
        {
            foreach (SimbolosM sim in tablam.tablam)
            {
                if (sim.nombre.Equals(nombre))
                {
                    foreach (Parametros par in sim.parametro) {
                        nombre += "_"+par.tipo;
                    }
                    GrafoAST(sim.raiz,nombre);
                }

            }
        }
        static int contexp;
        private void DibujarExp(ParseTreeNode root)
        {
            String grafoDOT = GraficarEXP.getDOTe(root);

            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            
            img.Save("C:\\Users\\arnol\\OneDrive\\Escritorio\\album\\Expresion" + contexp.ToString()+".png");
            contexp++;
        }
        private static void GrafoAST(ParseTreeNode root,String nombre)
        {

            String grafoDOT = GraficarAST.getDOT(root.ChildNodes.ElementAt(6));

            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);

            img.Save("C:\\Users\\arnol\\OneDrive\\Escritorio\\album\\"+nombre+".png");

            //DEVOLVER STRING BASE 64 DE IMIAGEN
            //    return img.ToBase64String();
        }
        private bool Mostrar_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek, List<String> text, List<String> pos) {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidomostrar")) {
                Mostrar_Metodo(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek, text, pos);

                pos.Add("{" + contmos.ToString() + "}");
                text.Add(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila,tablam));
                contmos++;
            }
            else {

                pos.Add("{"+contmos.ToString()+"}");
                text.Add(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam));
                contmos++;
            }
            return brek;
        }
        public DatoSB Llamada_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek) {
            String nombre =root.ChildNodes.ElementAt(0).ToString().Replace(" (Id)","");
            String act = ambitots;
            ambitots = "Ambito_"+nombre;
            bool erro = false;
            DatoSB ret = new DatoSB();
            List<SimbolosM> pr = new List<SimbolosM>();
            foreach (SimbolosM sim in tablam.tablam) {
                if (nombre.Equals(sim.nombre)) {
                    pr.Add(sim);
                    tipodmet = sim.tipo;
                    erro = true;
                }
            }

            if (erro)
            {
                SimbolosM simbol = new SimbolosM();
                if (root.ChildNodes.ElementAt(2).ChildNodes.Count != 0)
                {
                    Obtener_parametros(root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek, simbol);
                }
                    bool igual = false;

                foreach (SimbolosM param in pr)
                {
                    if (root.ChildNodes.ElementAt(2).ChildNodes.Count != 0)
                    {
                        igual = Comparar_Parametros(param, simbol);
                        if (igual)
                        {
                            int i = 0;
                            Tabla tab = new Tabla();

                            while (simbol.parametro.Count > i)
                            {
                                Simbolos simpar = new Simbolos();

                                simpar.nombre = simbol.parametro[i].nombre;
                                simpar.valor = simbol.parametro[i].valor;
                                simpar.tipo = simbol.parametro[i].tipo;
                                simpar.ambito = "";
                                tab.tabla.Add(simpar);
                                i++;

                            }



                            ret.brek = Contenido_Metodo(param.raiz.ChildNodes.ElementAt(6), pila, tab, tablam, verd, true);
                            ret.retorno = retmet;
                            break;
                        }
                    }
                    else {
                        Tabla tab = new Tabla();
                        ret.brek = Contenido_Metodo(param.raiz.ChildNodes.ElementAt(6), pila, tab, tablam, verd, true);
                        ret.retorno = retmet;
                        break;
                    }
                }

            }
            else {
                error += "No se eocnotro el metodo o funcion " + nombre + "\n";
            }
            ambitots = act;
            return ret;
        }
        private bool Comparar_Parametros(SimbolosM tabla, SimbolosM actual) {
            int i = 0;
            if (actual.parametro.Count == tabla.parametro.Count)
            {
                while (actual.parametro.Count > i)
                {
                    try
                    {
                        switch (tabla.parametro[i].tipo)
                        {
                            case "Int":
                                actual.parametro[i].nombre = tabla.parametro[i].nombre;
                                actual.parametro[i].tipo = "Int";
                                actual.parametro[i].valor = Convert.ToString(Convert.ToInt32(actual.parametro[i].valor));

                                break;
                            case "Double":
                                actual.parametro[i].nombre = tabla.parametro[i].nombre;
                                actual.parametro[i].tipo = "Double";
                                actual.parametro[i].valor = Convert.ToString(Convert.ToDouble(actual.parametro[i].valor));

                                break;
                            case "String":
                                actual.parametro[i].nombre = tabla.parametro[i].nombre;
                                actual.parametro[i].tipo = "Double";
                                actual.parametro[i].valor = actual.parametro[i].valor;
                                break;
                            case "Char":
                                actual.parametro[i].nombre = tabla.parametro[i].nombre;
                                actual.parametro[i].tipo = "Char";
                                actual.parametro[i].valor = Convert.ToString(Convert.ToChar(actual.parametro[i].valor.Replace("'", "")));
                                break;
                            case "Bool":
                                actual.parametro[i].nombre = tabla.parametro[i].nombre;
                                actual.parametro[i].tipo = "Bool";
                                if (actual.parametro[i].valor.Equals("true") || actual.parametro[i].valor.Equals("1") || actual.parametro[i].valor.Equals("0") || actual.parametro[i].valor.Equals("false"))
                                {
                                    actual.parametro[i].valor = actual.parametro[i].valor;
                                }
                                else
                                {
                                    error = "Los parametros no coinsiden con los tipos de dato en el metodo o funcion " + tabla.nombre;
                                    return false;
                                }
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        error = "Los parametros no coinsiden con los tipos de dato en el metodo o funcion " + tabla.nombre;
                        return false;
                    }

                    i++;
                }
            }
            else { return false; }

            return true;
        }
        private bool Obtener_parametros(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek,SimbolosM simbol) {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidollamada2"))
            {
                brek = Obtener_parametros(root.ChildNodes.ElementAt(0), pila, tabla, tablam, verd, brek,simbol);
                Parametros par = new Parametros();
                par.nombre = "";
                par.tipo = "";
                tipod = "";
                par.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(1), pila, tablam);
                simbol.parametro.Add(par);
            }
            else {
                Parametros par = new Parametros();
                par.nombre = "";
                par.tipo = "";
                tipod = "";
                par.valor = Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(0), pila, tablam);
                simbol.parametro.Add(par);
            }

            return brek;
        }
        private bool Para_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek) {

            if (root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).ToString().Equals("Double (Keyword)"))
            {
                Simbolos simb = new Simbolos();
                simb.nombre = root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(1).ToString().Replace(" (Id)", "");
                simb.tipo = root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).ToString().Replace(" (Keyword)", "");
                simb.ambito = "";

                try {
                    tipod = "Double";
                    simb.valor = Convert.ToString(Convert.ToDouble(Calculadorametodo.ResolverOperacion(root.ChildNodes.ElementAt(2),pila, tablam)));
                    Tabla tb = new Tabla();
                    tb.tabla.Add(simb);
                    pila.pila.Push(tb);
                    bool entro = true;
                    Tabla tab = new Tabla();
                    tipod = "";
                    while (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1")) {
                        if (entro) {
                            entro = false;
                            ambitots += "_SubAmbito_Para";
                            brek = Contenido_Metodo(root.ChildNodes.ElementAt(7), pila, tb, tablam, true, brek);
                            ambitots = ambitots.Replace("_SubAmbito_Para", "");
                            pila.pila.Pop();
                            if (continuar)
                            {
                                if (root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0).ToString().Equals("++ (Key symbol)"))
                                {
                                    simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) + 1);

                                    tab.tabla.Add(simb);
                                    pila.pila.Push(tab);
                                }
                                else
                                {
                                    simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) - 1);
                                    tab.tabla.Add(simb);
                                    pila.pila.Push(tab);
                                }
                                brek = true;
                                continuar = false;
                                continue;
                            }
                            if (!brek)
                            {
                                brek = true;
                                break;
                            }
                            if (root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0).ToString().Equals("++ (Key symbol)"))
                            {
                                simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) + 1);
                                
                                tab.tabla.Add(simb);
                                pila.pila.Push(tab);
                            }
                            else
                            {
                                simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) - 1);
                                tab.tabla.Add(simb);
                                pila.pila.Push(tab);
                            }
                        }
                        else {
                            
                            tab = new Tabla();
                            ambitots += "_SubAmbito_Para";
                            brek = Contenido_Metodo(root.ChildNodes.ElementAt(7), pila, tab, tablam, true, brek);
                            ambitots = ambitots.Replace("_SubAmbito_Para", "");
                            pila.pila.Pop();
                            if (continuar)
                            {
                                if (root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0).ToString().Equals("++ (Key symbol)"))
                                {
                                    simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) + 1);
                                    tab.tabla.Add(simb);
                                    pila.pila.Push(tab);
                                }
                                else
                                {
                                    simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) - 1);
                                    tab.tabla.Add(simb);
                                    pila.pila.Push(tab);
                                }
                                brek = true;
                                continuar = false;
                                continue;
                            }
                            if (!brek)
                            {
                                brek = true;
                                break;
                            }
                            if (root.ChildNodes.ElementAt(4).ChildNodes.ElementAt(0).ToString().Equals("++ (Key symbol)"))
                            {
                                simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) + 1);
                                tab.tabla.Add(simb);
                                pila.pila.Push(tab);
                            }
                            else
                            {
                                simb.valor = Convert.ToString(Convert.ToDouble(simb.valor) - 1);
                                tab.tabla.Add(simb);
                                pila.pila.Push(tab);
                            }
                        }
                        ambit = false;
                    }

                    ambit = true;

                } catch (Exception e) {
                    error = "Error semantico, se debe asignar un dato de tipo Double a la variable " + root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(1).ToString().Replace("(Id)", "") +"\n";

                }

            }
            else {
                error ="Error semantico, la variable "+ root.ChildNodes.ElementAt(2).ChildNodes.ElementAt(1).ToString().Replace("(Id)","") + " debe de ser de tipo Double\n";
            }
            return brek;
        }
        private bool Selecciona_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam,bool verd,bool brek)
        {
            Simbolos simb = new Simbolos();
            try {
                tipod = "Double";
                simb.valor = Convert.ToString(Convert.ToDouble( Calculadorametodo.ResolverOperacion(root, pila, tablam)));
                simb.tipo = "Double";
                simb.ambito = "Select";
                simb.nombre = "sele";
                brek = ContenidoSelecciona(root.ChildNodes.ElementAt(5).ChildNodes.ElementAt(0),pila,tabla,tablam,simb,true, brek);
                brek = true;

            }
            catch (Exception e) {
                tipod = "Double";
                simb.valor = Calculadorametodo.ResolverOperacion(root, pila, tablam).Replace("'","");
                simb.tipo = "Double";
                simb.ambito = "Select";
                simb.nombre = "sele";
                brek = ContenidoSelecciona(root.ChildNodes.ElementAt(5).ChildNodes.ElementAt(0), pila, tabla, tablam, simb,true, brek);
                brek = true;
            }
            return brek;
        }
        private bool ContenidoSelecciona(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam,Simbolos simb, bool verd, bool brek)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidoswitch1"))
            {
                ContenidoSelecciona(root.ChildNodes.ElementAt(0), pila, tabla, tablam, simb,true, brek);
                if (root.ChildNodes.ElementAt(1).ChildNodes.Count == 3&& brek)
                {
                    if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2).ChildNodes.Count == 3)
                    {

                       brek= Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2).ChildNodes.ElementAt(2), pila, tabla, tablam,true,brek);

                        
                    }
                }
                else
                {

                    if (simb.tipo.Equals("Double") && brek)
                    {
                        String[] dato = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                        try
                        {
                            if (Convert.ToString(Convert.ToDouble(dato[0])).Equals(simb.valor))
                            {
                                brek = Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2), pila, tabla, tablam,true, brek);
                                
                            }
                            else
                            {
                                if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.Count > 2)
                                {

                                    brek = Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            error += "El dato en el case del Switch no es de tipo Double";
                        }
                    }
                    else
                    {
                        String[] dato = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                        if (Convert.ToString(Convert.ToDouble(dato[0])).Equals(simb.valor) && brek)
                        {

                            brek = Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                        }
                        else
                        {
                            if (root.ChildNodes.ElementAt(1).ChildNodes.Count == 4)
                            {
                                if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.Count == 3)
                                {
                                    brek = Contenido_Metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                if (root.ChildNodes.ElementAt(0).ChildNodes.Count == 3 && brek)
                {
                    if (root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count == 3)
                    {

                        brek = Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                        

                    }
                }
                else
                {

                    if (simb.tipo.Equals("Double") && brek)
                    {
                        String[] dato = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                        try
                        {
                            if (Convert.ToString(Convert.ToDouble(dato[0])).Equals(simb.valor))
                            {
                                brek = Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                                
                            }
                            else
                            {
                                if (root.ChildNodes.ElementAt(0).ChildNodes.Count == 4)
                                {
                                    if (root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ChildNodes.Count == 3)
                                    {
                                        brek = Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                                        
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            error += "El dato en el case del Switch no es de tipo Double";
                        }
                    }
                    else
                    {
                        String[] dato = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                        if (Convert.ToString(Convert.ToDouble(dato[0])).Equals(simb.valor) && brek)
                        {
                            brek = Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                        }
                        else
                        {
                            if (root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ChildNodes.Count > 2 && brek)
                            {
                                brek = Contenido_Metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ChildNodes.ElementAt(2), pila, tabla, tablam, true, brek);
                            }
                        }
                    }
                }
            }
            return brek;
        }
        private bool Mientras_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek)
        {
            tipod = "";
            if (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("false"))
            {
                tipod = "";
                while (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true"))
                {
                    Tabla tb = new Tabla();
                    defec = true;
                    ambitots += "_SubAmbito_Mientras";

                    brek = Contenido_Metodo(root.ChildNodes.ElementAt(5), pila, tb, tablam, true, brek);
                    ambitots = ambitots.Replace("_SubAmbito_Mientras", "");

                    if (continuar) {
                        brek = true;
                        continuar = false;
                        continue;
                    }

                    if (!brek) {
                        brek = true;
                        break;
                    }
                    ambit = false;
                }
                ambit = true;
            }
            else
            {
                error += "La exprecion en el Si no es Booleana";
            }
            return brek;
        }
        private bool Hasta_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek)
        {
            tipod = "";
            if (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("false"))
            {
                tipod = "";
                while (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("false"))
                {
                   
                    Tabla tb = new Tabla();
                    defec = true;
                    ambitots += "_SubAmbito_Hasta";
                    brek = Contenido_Metodo(root.ChildNodes.ElementAt(5), pila, tb, tablam,true, brek);
                    ambitots = ambitots.Replace("_SubAmbito_Hasta", "");
                    brek = true;
                    if (continuar)
                    {
                        brek = true;
                        continuar = false;
                        continue;
                    }
                    if (!brek)
                    {
                        brek = true;
                        break;
                    }
                    ambit = false;
                }
                ambit = true;
            }
          
            else
            {
                error += "La exprecion en el Si no es Booleana";
            }
            return brek;
        }
        private bool Si_Metodo(ParseTreeNode root, PilaTS pila, Tabla tabla, TablaM tablam, bool verd, bool brek)
        {
            tipod = "";
            if (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("0") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("false"))
            {
                tipod = "";
               // String asdd=Calculadorametodo.ResolverOperacion(root, pila, tablam);
                if (Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("1") || Calculadorametodo.ResolverOperacion(root, pila, tablam).Equals("true"))
                {
                    Tabla tb = new Tabla();
                    ambitots += "_SubAmbito_Si";
                    brek = Contenido_Metodo(root.ChildNodes.ElementAt(5), pila, tb, tablam,verd,brek);

                    ambitots = ambitots.Replace("_SubAmbito_Si", "");
                }
                else
                {
                    if (root.ChildNodes.Count == 8)
                    {
                        if (root.ChildNodes.ElementAt(7).ChildNodes.Count > 3)
                        {
                            Tabla tb = new Tabla();
                            ambitots += "_SubAmbito'_Sino";
                            brek = Contenido_Metodo(root.ChildNodes.ElementAt(7).ChildNodes.ElementAt(2), pila, tb, tablam,verd, brek);

                            ambitots = ambitots.Replace("_SubAmbito'_Sino", "");
                        }
                    }

                }
            }
            else
            {
                error += "La exprecion en el Si no es Booleana";
            }
            return brek;
        }
        static String stat_var2;
        static String stat_val2;
        private void VariableMetodo(ParseTreeNode root, Tabla tabla,PilaTS pila,TablaM tablam)
        {
            switch (root.ChildNodes.ElementAt(1).ToString())
            {
                case "Idl":
                    String[] tipo = root.ChildNodes.ElementAt(0).ToString().Split(' ');

                    stat_var2 = tipo[0];
                    tipod = tipo[0];
                    stat_val2 = Calculadorametodo.ResolverOperacion(root, pila, tablam);


                    ListaVariablemrtodo(root.ChildNodes.ElementAt(1), tabla,pila);

                    break;

                default:
                    String[] tipo1 = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                    String[] variable = root.ChildNodes.ElementAt(1).ToString().Split(' ');
                    Simbolos simbol = new Simbolos();
                    bool err = true;

                    foreach (Simbolos sim in tabla.tabla)
                    {

                        if (sim.nombre.Equals(variable[0]))
                        {
                            error += "Linea: " + "Columna: " + " Error sntactico la varable \"" + variable[0] + "\" ya existe.\n";
                            err = false;
                        }

                    }

                    if (err)
                    {

                        switch (tipo1[0])
                        {
                            case "Int":
                                tipod = "Int";
                                try
                                {

                                    int tr = Convert.ToInt32(Calculadorametodo.ResolverOperacion(root, pila, tablam));
                                    simbol.valor = Convert.ToString(tr);
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        int ti = Convert.ToInt32(variable[0].Replace("\'", ""));
                                        simbol.nombre = variable[0];
                                        simbol.tipo = variable[0];
                                        simbol.valor = Convert.ToString((Char)Convert.ToInt32(variable[0]));
                                        simbol.ambito = "";
                                        tabla.tabla.Add(simbol);
                                    }
                                    catch (Exception f)
                                    {
                                        error += "Linea: " + "Columna: " + " Error sintactico, el tipo de dato no es valido \"" + variable[0] + "\" .\n";

                                    }

                                }
                                break;
                            case "Double":
                                tipod = "Double";
                                try
                                {
                                    Double tr = Convert.ToDouble(Calculadorametodo.ResolverOperacion(root, pila, tablam));
                                    simbol.valor = Convert.ToString(tr);
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Double la variable \"" + variable[0] + "\".\n";
                                }
                                break;
                            case "Bool":
                                tipod = "Bool";
                                try
                                {

                                    simbol.valor = Calculadorametodo.ResolverOperacion(root, pila, tablam);
                                    if (simbol.valor.Equals("0") || simbol.valor.Equals("1") || simbol.valor.Equals("false") || simbol.valor.Equals("true"))
                                    {
                                        simbol.nombre = variable[0];
                                        simbol.tipo = tipo1[0];
                                        simbol.ambito = "";
                                        tabla.tabla.Add(simbol);
                                    }
                                    else
                                    {
                                        simbol.valor = Calculadorametodo.ResolverOperacion(root, pila, tablam);

                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + variable[0] + "\".\n";

                                    }
                                }
                                catch (Exception e)
                                {
                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + variable[0] + "\".\n";

                                }
                                break;
                            case "String":
                                tipod = "String";
                                simbol.nombre = variable[0];
                                simbol.tipo = tipo1[0];
                                simbol.valor = Calculadorametodo.ResolverOperacion(root, pila, tablam);
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);

                                break;
                            case "Char":
                                tipod = "Char";
                                try
                                {
                                    simbol.valor = Calculadorametodo.ResolverOperacion(root, pila, tablam);

                                    Char tr = Convert.ToChar(simbol.valor.Replace("\'", ""));
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        int ti = Convert.ToInt32(simbol.valor.Replace("\'", ""));
                                        simbol.nombre = variable[0];
                                        simbol.tipo = tipo1[0];
                                        simbol.ambito = "";
                                        tabla.tabla.Add(simbol);
                                    }
                                    catch (Exception f)
                                    {
                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Char la variable \"" + variable[0] + "\".\n";

                                    }

                                }
                                break;

                        }
                        break;

                }else
                {
                error += "";
                        break;
                }

            }
                    
            
        }
        private void ListaVariablemrtodo(ParseTreeNode root, Tabla tabla, PilaTS pila)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Idl"))
            {

                ListaVariablemrtodo(root.ChildNodes.ElementAt(0), tabla, pila);
                String[] variable = root.ChildNodes.ElementAt(1).ToString().Split(' ');
                Simbolos simbol = new Simbolos();
                bool err = true;

                foreach (Simbolos sim in tabla.tabla)
                {

                    if (sim.nombre.Equals(variable[0]))
                    {
                        error += "Linea: " + "Columna: " + " Error sntactico la varable \"" + variable[0] + "\" ya existe.\n";
                        err = false;
                    }

                }

                if (err)
                {
                    switch (stat_var2)
                    {
                        case "Int":
                            tipod = "Int";
                            try
                            {
                                simbol.nombre = variable[0];

                                int tr = Convert.ToInt32(stat_val2);
                                simbol.valor = stat_val2;
                                simbol.tipo = stat_var2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Int la variable \"" + stat_val2 + "\".\n";

                            }

                            break;
                        case "Double":
                            tipod = "Double";
                            try
                            {
                                simbol.nombre = variable[0];

                                Double tr = Convert.ToDouble(stat_val2);
                                simbol.valor = stat_val2;
                                simbol.tipo = stat_var2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Double la variable \"" + stat_val2 + "\".\n";
                            }
                            break;
                        case "Bool":
                            tipod = "Bool";
                            try
                            {

                                simbol.nombre = variable[0];
                                if (stat_val2.Equals("0") || stat_val2.Equals("1") || stat_val2.Equals("false") || stat_val2.Equals("true"))
                                {
                                    simbol.valor = stat_val2;

                                    simbol.tipo = stat_var2;
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                else
                                {

                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + stat_val2 + "\".\n";

                                }
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + stat_val2 + "\".\n";

                            }
                            break;
                        case "String":
                            tipod = "String";
                            simbol.nombre = variable[0];
                            simbol.tipo = stat_var2;
                            simbol.valor = stat_val2;
                            simbol.ambito = "";
                            tabla.tabla.Add(simbol);

                            break;
                        case "Char":
                            tipod = "Char";
                            try
                            {
                                simbol.nombre = variable[0];

                                Char tr = Convert.ToChar(stat_val2.Replace("\'", ""));
                                simbol.tipo = stat_var2;
                                simbol.valor = stat_val2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];

                                    int ti = Convert.ToInt32(stat_val2.Replace("\'", ""));
                                    simbol.tipo = stat_var2;
                                    simbol.valor = Convert.ToString((Char)Convert.ToInt32(stat_val2));
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Char la variable \"" + stat_val2 + "\".\n";

                                }

                            }
                            break;

                    }
                }

            }
            else
            {
                String[] variable = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                Simbolos simbol = new Simbolos();
                bool err = true;

                foreach (Simbolos sim in tabla.tabla)
                {

                    if (sim.nombre.Equals(variable[0]))
                    {
                        error += "Linea: " + "Columna: " + " Error sntactico la varable \"" + variable[0] + "\" ya existe.\n";
                        err = false;
                    }

                }

                if (err)
                {
                    switch (stat_var2)
                    {
                        case "Int":
                            tipod = "Int";
                            try
                            {

                                int tr = Convert.ToInt32(stat_val2);
                                simbol.valor = stat_val2;
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Int la variable \"" + stat_val2 + "\".\n";

                            }

                            break;
                        case "Double":
                            tipod = "Double";
                            try
                            {
                                Double tr = Convert.ToDouble(stat_val2);
                                simbol.valor = stat_val2;
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Double la variable \"" + stat_val2 + "\".\n";
                            }
                            break;
                        case "Bool":
                            tipod = "Bool";
                            try
                            {
                                if (stat_val2.Equals("0") || stat_val2.Equals("1") || stat_val2.Equals("false") || stat_val2.Equals("true"))
                                {
                                    simbol.valor = stat_val2;

                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var2;
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                else
                                {

                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + stat_val2 + "\".\n";

                                }
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + stat_val2 + "\".\n";


                            }
                            break;
                        case "String":
                            tipod = "String";
                            simbol.nombre = variable[0];
                            simbol.tipo = stat_var2;
                            simbol.valor = stat_val2;
                            simbol.ambito = "";
                            tabla.tabla.Add(simbol);

                            break;
                        case "Char":
                            tipod = "Char";
                            try
                            {

                                Char tr = Convert.ToChar(stat_val2.Replace("\'", ""));
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var2;
                                simbol.valor = stat_val2;
                                simbol.ambito = "";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    int ti = Convert.ToInt32(stat_val2.Replace("\'", ""));
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var2;
                                    simbol.valor = Convert.ToString((Char)Convert.ToInt32(stat_val2));
                                    simbol.ambito = "";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Char la variable \"" + stat_val2 + "\".\n";

                                }

                            }
                            break;

                    }
                }
            }
        }


        //primera pasada
        private void Primera_pasada(ParseTreeNode root, Tabla tabla, TablaM tablam)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("import_definir_clase"))
            {
                Primera_pasada(root.ChildNodes.ElementAt(0), tabla, tablam);

                switch (root.ChildNodes.ElementAt(1).ToString())
                {
                    case "import":
                        Contenido_clase(root.ChildNodes.ElementAt(1), tabla, tablam);
                        break;
                    case "definir":
                        break;
                    case "Contenidoclase":
                        Contenido_clase(root.ChildNodes.ElementAt(1), tabla, tablam);
                        break;
                }
            }
            else
            {
                switch (root.ChildNodes.ElementAt(0).ToString())
                {
                    case "import":
                        Contenido_clase(root.ChildNodes.ElementAt(0), tabla, tablam);

                        break;
                    case "definir":
                        break;
                    case "Contenidoclase":
                        Contenido_clase(root.ChildNodes.ElementAt(0), tabla, tablam);
                        break;
                }

            }

        }

        //contenido clase

        private void Contenido_clase(ParseTreeNode root, Tabla tabla, TablaM tablam)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Contenidoclase"))
            {
                Contenido_clase(root.ChildNodes.ElementAt(0), tabla, tablam);

                switch (root.ChildNodes.ElementAt(1).ToString())
                {
                    case "Metodo":
                        if (!root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Equals("Principal (Keyword)"))
                        {
                            SimbolosM simbol = new SimbolosM();
                            String[] tipo = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Split(' ');
                            simbol.tipo = tipo[0];
                            String[] nombre = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ToString().Split(' ');
                            simbol.no = cont.ToString();
                            simbol.nombre = nombre[0];
                            simbol.raiz = root.ChildNodes.ElementAt(1);
                            if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.Count != 0)
                            {
                                Parametros_metodo(root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.ElementAt(0), tablam, simbol);
                            }
                            tablam.tablam.Add(simbol);

                        }
                        break;
                    case "Variable":
                        VariableClase(root.ChildNodes.ElementAt(1), tabla);
                        break;

                }
            }
            else
            {
                switch (root.ChildNodes.ElementAt(0).ToString())
                {
                    case "Metodo":
                        if (!root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Equals("Principal (Keyword)"))
                        {
                            SimbolosM simbol = new SimbolosM();
                            String[] tipo = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                            simbol.tipo = tipo[0];
                            String[] nombre = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).ToString().Split(' ');
                            simbol.nombre = nombre[0];
                            tablam.tablam.Add(simbol);
                            if (root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(3).ChildNodes.Count != 0)
                            {
                                Parametros_metodo(root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ChildNodes.ElementAt(0), tablam, simbol);
                            }
                        }
                        break;
                    case "Variable":
                        VariableClase(root.ChildNodes.ElementAt(0), tabla);
                        break;

                }

            }

        }

        static String stat_var;
        static String stat_val;
        private void VariableClase(ParseTreeNode root, Tabla tabla)
        {
            switch (root.ChildNodes.ElementAt(1).ToString())
            {
                case "Idl":
                    String[] tipo = root.ChildNodes.ElementAt(0).ToString().Split(' ');

                    stat_var = tipo[0];
                    tipod = tipo[0]; 
                    stat_val = Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla);


                    ListaVariable(root.ChildNodes.ElementAt(1), tabla);

                    break;

                default:
                    String[] tipo1 = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                    String[] variable = root.ChildNodes.ElementAt(1).ToString().Split(' ');
                    Simbolos simbol = new Simbolos();
                    bool err = true;

                    foreach (Simbolos sim in tabla.tabla)
                    {

                        if (sim.nombre.Equals(variable[0]))
                        {
                            error += "Linea: " + "Columna: " + " Error Semantico la variable \"" + variable[0] + "\" ya existe.\n";
                            err = false;
                        }

                    }

                    if (err)
                    {
                        
                        switch (tipo1[0])
                        {
                            case "Int":
                                tipod = "Int";
                                try
                                {

                                    simbol.valor = Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla);
                                    int tr = Convert.ToInt32(simbol.valor);
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        simbol.nombre = variable[0];
                                        simbol.tipo = stat_var;
                                        simbol.valor = Convert.ToString((int)Convert.ToChar(simbol.valor));
                                        simbol.ambito = "global";
                                        tabla.tabla.Add(simbol);
                                    }
                                    catch (Exception f)
                                    {
                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Int la variable \"" + variable[0] + "\".\n";

                                    }

                                }
                                break;
                            case "Double":
                                tipod = "Double";
                                try
                                {
                                    simbol.valor = Convert.ToString(Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla));

                                    Double tr = Convert.ToDouble(simbol.valor);
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        simbol.nombre = variable[0];
                                        simbol.tipo = stat_var;
                                        simbol.valor = Convert.ToString((int)Convert.ToChar(simbol.valor));
                                        simbol.ambito = "global";
                                        tabla.tabla.Add(simbol);
                                    }
                                    catch (Exception f)
                                    {
                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Int la variable \"" + variable[0] + "\".\n";

                                    }
                                }
                                break;
                            case "Bool":
                                tipod = "Bool";
                                try
                                {

                                    simbol.valor = Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla);
                                    if (simbol.valor.Equals("0") || simbol.valor.Equals("1") || simbol.valor.Equals("false") || simbol.valor.Equals("true"))
                                    {
                                        simbol.nombre = variable[0];
                                        simbol.tipo = tipo1[0];
                                        simbol.ambito = "global";
                                        tabla.tabla.Add(simbol);
                                    }
                                    else
                                    {
                                        
                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + variable[0] + "\".\n";

                                    }
                                }
                                catch (Exception e)
                                {
                                    error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Bool la variable \"" + variable[0] + "\".\n";

                                }
                                break;
                            case "String":
                                tipod = "String";
                                simbol.nombre = variable[0];
                                simbol.tipo = tipo1[0];
                                simbol.valor = Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla);
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);

                                break;
                            case "Char":
                                tipod = "Char";
                                try
                                {
                                    simbol.valor = Calculadora.ResolverOperacion(root.ChildNodes.ElementAt(2), tabla);

                                    Char tr = Convert.ToChar(simbol.valor.Replace("\'", ""));
                                    simbol.nombre = variable[0];
                                    simbol.tipo = tipo1[0];
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        int ti = Convert.ToInt32(simbol.valor.Replace("\'", ""));
                                        simbol.nombre = variable[0];
                                        simbol.valor = Convert.ToString((Char)ti);
                                        simbol.tipo = tipo1[0];
                                        simbol.ambito = "global";
                                        tabla.tabla.Add(simbol);
                                    }
                                    catch (Exception f)
                                    {
                                        error += "Linea: " + "Columna: " + " Error Semantico, se ntenta signar un dato que no es de tipo Char la variable \""+ variable[0] +"\".\n";

                                    }

                                }
                                break;

                        }
                        break;

                    }
                    else
                    {
                        error += "";
                        break;
                    }
            }
        }
        private void ListaVariable(ParseTreeNode root, Tabla tabla)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Idl"))
            {

                ListaVariable(root.ChildNodes.ElementAt(0), tabla);
                String[] variable = root.ChildNodes.ElementAt(1).ToString().Split(' ');
                Simbolos simbol = new Simbolos();
                bool err = true;

                foreach (Simbolos sim in tabla.tabla)
                {

                    if (sim.nombre.Equals(variable[0]))
                    {
                        error += "Linea: " + "Columna: " + " Error sntactico la varable \"" + variable[0] + "\" ya existe.\n";
                        err = false;
                    }

                }

                if (err)
                {
                    switch (stat_var)
                    {
                        case "Int":
                            tipod = "Int";
                            try
                            {
                                simbol.nombre = variable[0];

                                int tr = Convert.ToInt32(stat_val);
                                simbol.valor = stat_val;
                                simbol.tipo = stat_var;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((int)Convert.ToChar(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Int a la variable\"" + variable[0] + "\" .\n";
                                }
                            }

                            break;
                        case "Double":
                            tipod = "Double";
                            try
                            {
                                simbol.nombre = variable[0];

                                Double tr = Convert.ToDouble(stat_val);
                                simbol.valor = stat_val;
                                simbol.tipo = stat_var;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((int)Convert.ToChar(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Double a la variable\"" + variable[0] + "\" .\n";
                                }
                            }
                            break;
                        case "Bool":
                            tipod = "Bool";
                            try
                            {

                                simbol.nombre = variable[0];
                                if (stat_val.Equals("0") || stat_val.Equals("1") || stat_val.Equals("false") || stat_val.Equals("true"))
                                {
                                    simbol.valor = stat_val;

                                    simbol.tipo = stat_var;
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                else
                                {

                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Bool a la variable\"" + variable[0] + ".\n";

                                }
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Bool a la variable\"" + variable[0] + "\" .\n";

                            }
                            break;
                        case "String":
                            tipod = "String";
                            simbol.nombre = variable[0];
                            simbol.tipo = stat_var;
                            simbol.valor = stat_val;
                            simbol.ambito = "global";
                            tabla.tabla.Add(simbol);

                            break;
                        case "Char":
                            tipod = "Char";
                            try
                            {
                                simbol.nombre = variable[0];

                                Char tr = Convert.ToChar(stat_val.Replace("\'", ""));
                                simbol.tipo = stat_var;
                                simbol.valor = stat_val;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];

                                    int ti = Convert.ToInt32(stat_val.Replace("\'", ""));
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((Char)Convert.ToInt32(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Char a la variable\"" + variable[0] + "\" .\n";

                                }

                            }
                            break;

                    }
                }

            }
            else
            {
                String[] variable = root.ChildNodes.ElementAt(0).ToString().Split(' ');
                Simbolos simbol = new Simbolos();
                bool err = true;

                foreach (Simbolos sim in tabla.tabla)
                {

                    if (sim.nombre.Equals(variable[0]))
                    {
                        error += "Linea: " + "Columna: " + " Error sntactico la varable \"" + variable[0] + "\" ya existe.\n";
                        err = false;
                    }

                }

                if (err)
                {
                    switch (stat_var)
                    {
                        case "Int":
                            tipod = "Int";
                            try
                            {

                                int tr = Convert.ToInt32(stat_val);
                                simbol.valor = stat_val;
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((int)Convert.ToChar(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Int a la variable\"" + variable[0] + "\" .\n";
                                }
                            }

                            break;
                        case "Double":
                            tipod = "Double";
                            try
                            {
                                Double tr = Convert.ToDouble(stat_val);
                                simbol.valor = stat_val;
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((int)Convert.ToChar(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, no se esta asignando un dato tipo Double a la variable\"" + variable[0] + "\" .\n";
                                }
                            }
                            break;
                        case "Bool":
                            tipod = "Bool";
                            try
                            {
                                if (stat_val.Equals("0") || stat_val.Equals("1") || stat_val.Equals("false") || stat_val.Equals("true"))
                                {
                                    simbol.valor = stat_val;

                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                else
                                {

                                    error += "Linea: " + "Columna: " + " Error sintactico, el tipo de dato no es valido \"" + stat_val + "\" .\n";

                                }
                            }
                            catch (Exception e)
                            {
                                error += "Linea: " + "Columna: " + " Error sintactico, el tipo de dato no es valido \"" + stat_val + "\" .\n";

                            }
                            break;
                        case "String":
                            tipod = "String";
                            simbol.nombre = variable[0];
                            simbol.tipo = stat_var;
                            simbol.valor = stat_val;
                            simbol.ambito = "global";
                            tabla.tabla.Add(simbol);

                            break;
                        case "Char":
                            tipod = "Char";
                            try
                            {

                                Char tr = Convert.ToChar(stat_val.Replace("\'", ""));
                                simbol.nombre = variable[0];
                                simbol.tipo = stat_var;
                                simbol.valor = stat_val;
                                simbol.ambito = "global";
                                tabla.tabla.Add(simbol);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    int ti = Convert.ToInt32(stat_val.Replace("\'", ""));
                                    simbol.nombre = variable[0];
                                    simbol.tipo = stat_var;
                                    simbol.valor = Convert.ToString((Char)Convert.ToInt32(stat_val));
                                    simbol.ambito = "global";
                                    tabla.tabla.Add(simbol);
                                }
                                catch (Exception f)
                                {
                                    error += "Linea: " + "Columna: " + " Error sintactico, el tipo de dato no es valido \"" + stat_val + "\" .\n";

                                }

                            }
                            break;

                    }
                }
            }
        }
        private void Parametros_metodo(ParseTreeNode root, TablaM tablam, SimbolosM simbol)
        {
            if (root.ChildNodes.ElementAt(0).ToString().Equals("Parametros2"))
            {
                Parametros_metodo(root.ChildNodes.ElementAt(0), tablam, simbol);
                Parametros param = new Parametros();
                String[] tipo = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Split(' ');
                param.tipo = tipo[0];
                String[] nombre = root.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ToString().Split(' ');
                param.nombre = nombre[0];
                simbol.parametro.Add(param);

            }
            else
            {
                String a = root.ChildNodes.ElementAt(0).ToString();

                Parametros param = new Parametros();
                String[] tipo = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ');
                param.tipo = tipo[0];
                String[] nombre = root.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).ToString().Split(' ');
                param.nombre = nombre[0];
                simbol.parametro.Add(param);

            }
        }

        //imagen arbol
        public static String getImage(ParseTreeNode raiz)
        {
            String grafoDOT = ControlDOT.getDOT(raiz);

            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);
            byte[] imageBytes = Convert.FromBase64String(img.ToBase64String());
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            Image imagen = Image.FromStream(ms, true);
            return grafoDOT;
        }
        private static string generarImagen(ParseTreeNode raiz)
        {
            String grafoDOT = ControlDOT.getDOT(raiz);

            WINGRAPHVIZLib.DOT dot = new WINGRAPHVIZLib.DOT();
            WINGRAPHVIZLib.BinaryImage img = dot.ToPNG(grafoDOT);

            img.Save("C:\\Users\\arnol\\OneDrive\\Escritorio\\grafo.png");

            //DEVOLVER STRING BASE 64 DE IMIAGEN
            return img.ToBase64String();
        }

    }
}