using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;


namespace Crl.Analizador
{
    public class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {
            // a duni le gusta el pene
            #region ER;
            RegexBasedTerminal Numerod = new RegexBasedTerminal("Numerod", "[0-9]+");
            RegexBasedTerminal Numeroe = new RegexBasedTerminal("Numeroe", "[0-9]+");
            RegexBasedTerminal Id = new RegexBasedTerminal("Id", "[A-Za-zñÑ][_0-9A-Za-z]*");
            var string_literal = TerminalFactory.CreateCSharpString("Cadena");
            var comentario_line = new CommentTerminal("comentario_line", "!!", "\r", "\n", "\u2085", "\u2028", "\u2029");
            var comentario_multi = new CommentTerminal("comentario_multi", "<<", ">>");
            var Texto = new CommentTerminal("TextoS", "\"", "\"");
            var Char = new CommentTerminal("TextoC", "'", "'");
            var Espacio = new CommentTerminal("Espacio", " ", "\r", "\n", "\t");

            NonGrammarTerminals.Add(comentario_line);
            NonGrammarTerminals.Add(comentario_multi);
            NonGrammarTerminals.Add(Espacio);

            #endregion

            #region Palabras_reservadas

            //tipos de dato

            MarkReservedWords("Double");
            var Double_r = ToTerm("Double");

            MarkReservedWords("String");
            var String_r = ToTerm("String");

            MarkReservedWords("Int");
            var Int_r = ToTerm("Int");

            MarkReservedWords("Bool");
            var Bool_r = ToTerm("Bool");

            MarkReservedWords("Char");
            var Char_r = ToTerm("Char");

            MarkReservedWords("Vacio");
            var Vacio_r = ToTerm("Vacio");

            //operadores aritmeticos

            MarkReservedWords("-");
            var menos_r = ToTerm("-");

            MarkReservedWords("+");
            var mas_r = ToTerm("+");

            MarkReservedWords("/");
            var dividir_r = ToTerm("/");

            MarkReservedWords("*");
            var por_r = ToTerm("*");

            MarkReservedWords("^");
            var pow_r = ToTerm("^");

            MarkReservedWords("%");
            var modulo_r = ToTerm("%");

            MarkReservedWords("(");
            var par_a_r = ToTerm("(");

            MarkReservedWords(")");
            var par_b_r = ToTerm(")");

            MarkReservedWords("++");
            var mas_mas_r = ToTerm("++");

            MarkReservedWords("--");
            var menos_menos_r = ToTerm("--");

            //operaciones relacionales

            MarkReservedWords("==");
            var igual_igual_r = ToTerm("==");

            MarkReservedWords("!=");
            var diferente_r = ToTerm("!=");

            MarkReservedWords("<");
            var menor_r = ToTerm("<");

            MarkReservedWords("<=");
            var menor_igual_r = ToTerm("<=");

            MarkReservedWords(">");
            var mayor_r = ToTerm(">");

            MarkReservedWords(">=");
            var mayor_igual_r = ToTerm(">=");

            MarkReservedWords("~");
            var diferencia_r = ToTerm("~");

            //expreciones logicas

            MarkReservedWords("&&");
            var and_r = ToTerm("&&");

            MarkReservedWords("||");
            var or_r = ToTerm("||");

            MarkReservedWords("!");
            var not_r = ToTerm("!");

            MarkReservedWords("|&");
            var xor_r = ToTerm("|&");

            //tipos de dato

            MarkReservedWords("true");
            var true_r = ToTerm("true");

            MarkReservedWords("false");
            var false_r = ToTerm("false");

            //encabezado 

            MarkReservedWords("Importar");
            var importar_r = ToTerm("Importar");

            MarkReservedWords("Definir");
            var definr_r = ToTerm("Definir");

            //=

            MarkReservedWords("=");
            var igual_r = ToTerm("=");

            MarkReservedWords(";");
            var puntocoma_r = ToTerm(";");

            MarkReservedWords(":");
            var dospuntos_r = ToTerm(":");

            MarkReservedWords(",");
            var coma_r = ToTerm(",");

            MarkReservedWords("{");
            var corchete_a_r = ToTerm("{");

            MarkReservedWords("}");
            var corchete_b_r = ToTerm("}");

            MarkReservedWords(".");
            var punto_r = ToTerm(".");

            
            //principal
            MarkReservedWords("Principal");
            var Principal_r = ToTerm("Principal");

            MarkReservedWords("Retorno");
            var retorno_r = ToTerm("Retorno");

            //si
            MarkReservedWords("Si");
            var si_r = ToTerm("Si");

            MarkReservedWords("Sino");
            var sino_r = ToTerm("Sino");

            //seleccionar
            MarkReservedWords("Selecciona");
            var Selecciona_r = ToTerm("Selecciona");

            MarkReservedWords("Defecto");
            var Defecto_r = ToTerm("Defecto");

            //para
            MarkReservedWords("Para");
            var Para_r = ToTerm("Para");

            //hasta
            MarkReservedWords("Hasta");
            var hasta_r = ToTerm("Hasta");

            //mientras
            MarkReservedWords("Mientras");
            var Mientras_r = ToTerm("Mientras");

            //mostrar
            MarkReservedWords("Mostrar");
            var Mostrar_r = ToTerm("Mostrar");
            MarkReservedWords("Print");
            var Print_r = ToTerm("Print");

            //dibujarast
            MarkReservedWords("DibujarAST");
            var DibujarAST_r = ToTerm("DibujarAST");

            //dibujarexp
            MarkReservedWords("DibujarEXP");
            var DibujarEXP_r = ToTerm("DibujarEXP");

            //dibujarst
            MarkReservedWords("DibujarTS");
            var DibujarTS_r = ToTerm("DibujarTS");

            //detener
            MarkReservedWords("Detener");
            var Detener_r = ToTerm("Detener");

            //continua
            MarkReservedWords("Continuar");
            var Continua_r = ToTerm("Continuar");

            
            this.RegisterOperators(20, Associativity.Left, mas_r, menos_r);
            this.RegisterOperators(30, Associativity.Left, por_r, dividir_r,modulo_r);
            this.RegisterOperators(40, Associativity.Left, pow_r);
            this.RegisterOperators(10, Associativity.Left, diferente_r, igual_igual_r, mayor_r, menor_r, menor_igual_r, mayor_igual_r, diferencia_r);
            this.RegisterOperators(4, Associativity.Left, or_r, xor_r);
            this.RegisterOperators(3, Associativity.Left, and_r);
            this.RegisterOperators(5, Associativity.Right, not_r);

            
            // this.MarkPunctuation("(",")");


            #endregion

            #region No Terminales
            NonTerminal clr = new NonTerminal("clr");
            NonTerminal Defecto = new NonTerminal("Defecto");
            NonTerminal continua = new NonTerminal("continua");

            NonTerminal import_definir_clase = new NonTerminal("import_definir_clase");
            NonTerminal import_definir_clase_lista = new NonTerminal("import_definir_clase_lista");
            
            NonTerminal import = new NonTerminal("import");
            NonTerminal definir = new NonTerminal("definir");

            NonTerminal Contenido = new NonTerminal("Contenido");
            NonTerminal Contenido2 = new NonTerminal("Contenido2");
            NonTerminal Declaracion = new NonTerminal("Declaracion");
            NonTerminal Tipod = new NonTerminal("Tipod");
            NonTerminal Asignar = new NonTerminal("Asignar");
            NonTerminal Expd = new NonTerminal("Expd");
            NonTerminal Vacio = new NonTerminal("Vacio");
            NonTerminal Idl = new NonTerminal("Idl");
            NonTerminal Idl2 = new NonTerminal("Idl2");
            NonTerminal Metodo = new NonTerminal("Metodo");
            NonTerminal Contenidometfun = new NonTerminal("Contenidometfun");
            NonTerminal Contenidometfun2 = new NonTerminal("Contenidometfun2");
            NonTerminal Contenidometfun3 = new NonTerminal("Contenidometfun3");
            NonTerminal Parametros = new NonTerminal("Parametros");
            NonTerminal Parametros2 = new NonTerminal("Parametros2");
            NonTerminal Parametros3 = new NonTerminal("Parametros3");

            NonTerminal Si = new NonTerminal("SI");
            NonTerminal Sino = new NonTerminal("SINO");

            NonTerminal Dibujarexp = new NonTerminal("Dibujarexp");
            NonTerminal Dibujarts = new NonTerminal("Dibujarts");

            NonTerminal Hasta = new NonTerminal("Hasta");
            NonTerminal Mostrar = new NonTerminal("Mostrar");
            NonTerminal Print = new NonTerminal("Print");
            NonTerminal Dibujarast = new NonTerminal("Dibujarast");
            NonTerminal Contenidomostrar = new NonTerminal("Contenidomostrar");
            NonTerminal Contenidomostrarlista = new NonTerminal("Contenidomostrarlista");
            NonTerminal Mientras = new NonTerminal("Mientras");
            NonTerminal Hacer = new NonTerminal("HACER");
            NonTerminal Salir = new NonTerminal("salir");
            NonTerminal Condicion = new NonTerminal("Condicion");
            NonTerminal Condicion2 = new NonTerminal("Condicion2");

            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal Llamada2 = new NonTerminal("Llamada2");
            NonTerminal Llamada3 = new NonTerminal("Llamada3");
            NonTerminal Contenidollamada = new NonTerminal("Contenidollamada");
            NonTerminal Contenidollamada2 = new NonTerminal("Contenidollamada2");
            NonTerminal Contenidollamada3 = new NonTerminal("Contenidollamada3");
   
            NonTerminal Detener = new NonTerminal("detener");
            NonTerminal asignacionpara = new NonTerminal("asignacionpara");

            NonTerminal Contenidoswitch = new NonTerminal("Contenidoswitch");
            NonTerminal Contenidoswitch2 = new NonTerminal("Contenidoswitch1");
            NonTerminal Contenidoswitch3 = new NonTerminal("Contenidoswitch2");
            NonTerminal Variable = new NonTerminal("Variable");
            NonTerminal Variable2 = new NonTerminal("Variable2");

            NonTerminal Clase = new NonTerminal("Clase");
            NonTerminal Contenidoclase = new NonTerminal("Contenidoclase");
            NonTerminal Contenidoclase2 = new NonTerminal("Contenidoclase2");
            NonTerminal Declaclase = new NonTerminal("Declaclase");

            NonTerminal Comill = new NonTerminal("Comill");

            NonTerminal Declaracion2 = new NonTerminal("Declaracion2");
            NonTerminal Asignar2 = new NonTerminal("Asignar2");
            
            NonTerminal Para = new NonTerminal("Para");
            NonTerminal decrementoincremento = new NonTerminal("decrementoincremento");
            NonTerminal selecciona = new NonTerminal("selecciona");
            NonTerminal retornar = new NonTerminal("retornar");

            MarkTransient(Contenidoclase2);
            MarkTransient(Idl2);
            MarkTransient(Contenidometfun);
            MarkTransient(Contenidometfun3);


            MarkTransient(import_definir_clase_lista);
            MarkTransient(Vacio);
            MarkTransient(Tipod);
            MarkPunctuation("[");

            MarkPunctuation("var");

            MarkPunctuation("=");


            MarkPunctuation("]");
            MarkPunctuation(";");
            MarkPunctuation(",");

            #endregion

            #region Gramatica


            clr.Rule = import_definir_clase;

            import_definir_clase.Rule = import_definir_clase + import_definir_clase_lista
                    | import_definir_clase_lista;

            import_definir_clase_lista.Rule = import
                    | Contenidoclase
                    | definir;


            //Import
            import.Rule = importar_r + Id + punto_r + Id + puntocoma_r;
            //--------------------

            //definir
            definir.Rule = definr_r + Numerod + puntocoma_r
                    | definr_r + Texto + puntocoma_r;

            //------------------------------


            //clase...
          
            Contenidoclase.Rule = Contenidoclase + Contenidoclase2
                    | Contenidoclase2;

            
            Contenidoclase2.Rule = Variable
                | Metodo;
            //--------------------------------------------------------------


            //varables de clase

            Variable.Rule = Tipod + Id + Asignar + puntocoma_r
                         | Tipod + Idl + Asignar + puntocoma_r;
            //--------------------------------

            //asignar
            Asignar.Rule = Vacio
                | igual_r + Expd;
            //------------------------------

            
            //metodos funcones y principal
            Metodo.Rule = Principal_r + par_a_r + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r
                    | Tipod + Id + par_a_r + Parametros + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r;
            //--------------------------------

            //parmetros
            Parametros.Rule = Parametros2
                | Vacio;

            
            Parametros2.Rule = Parametros2 + coma_r + Parametros3
                | Parametros3;

            
            Parametros3.Rule = Tipod + Id;
            //-----------------------------------------

            //contenido metodo...
            Contenidometfun.Rule = Contenidometfun2
                    | Vacio;

            Contenidometfun2.Rule = Contenidometfun2 + Contenidometfun3
                        | Contenidometfun3;


            Contenidometfun3.Rule = Si
                    | selecciona
                    | Hasta
                    | Llamada + puntocoma_r
                    | retornar
                    | Variable2
                    | Mientras
                    | Mostrar
                    | Print
                    | Dibujarast
                    | Dibujarexp 
                    | Dibujarts
                    | continua
                    | Para
                    | Detener;

            //--------------------------

            //print

            Print.Rule = Print_r +  Expd ;
            //------------------

            //detener
            Detener.Rule = Detener_r + puntocoma_r;
            //------------------

            //continuar
            continua.Rule = Continua_r + puntocoma_r;
            //------------------

            //Dibujarexp
            Dibujarexp.Rule = DibujarEXP_r + par_a_r + Expd + par_b_r +puntocoma_r;
            //--------------------------

            //Dibujarts
            Dibujarts.Rule = DibujarTS_r + par_a_r + par_b_r + puntocoma_r;
            //--------------------------


            //Dibujarast
            Dibujarast.Rule = DibujarAST_r + par_a_r + Id + par_b_r + puntocoma_r;
            //--------------------------

            //Mostrar
            Mostrar.Rule = Mostrar_r + par_a_r + Texto + Contenidomostrar + par_b_r + puntocoma_r;

            Contenidomostrar.Rule = Contenidomostrar + Contenidomostrarlista
                    | Contenidomostrarlista;

            Contenidomostrarlista.Rule = coma_r + Expd
                    |Vacio;

            //------------------------------

            //Para
            Para.Rule = Para_r + par_a_r + asignacionpara + puntocoma_r + Expd + puntocoma_r + decrementoincremento + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r;

            asignacionpara.Rule = Double_r + Id + igual_r + Expd
                    | Id;

            decrementoincremento.Rule =mas_mas_r
                    |menos_menos_r;
            //-------------------------

            //retorno
            retornar.Rule = retorno_r + Expd + puntocoma_r
                        |retorno_r + puntocoma_r;

            //--------------------------------

            //selecciona
            selecciona.Rule = Selecciona_r + par_a_r + Expd + par_b_r + corchete_a_r + Contenidoswitch + corchete_b_r;


            Contenidoswitch.Rule = Contenidoswitch2
                    | Vacio;

            Contenidoswitch2.Rule = Contenidoswitch2 + Contenidoswitch3
                    | Contenidoswitch3 ;

            Contenidoswitch3.Rule = Comill + dospuntos_r + Contenidometfun + Defecto;


            Comill.Rule = Texto
                    | Numerod;

            Defecto.Rule = Defecto_r + dospuntos_r + Contenidometfun
                | Vacio
                ;

            //---------------------------

            //variables
           Variable2.Rule = Tipod + Id + igual_r + Expd + puntocoma_r
                    | Tipod + Idl + puntocoma_r
                    | Id + igual_r + Expd + puntocoma_r;
            //------------------------------------

            
       
            //llamada
            Llamada.Rule = Id + par_a_r + Contenidollamada + par_b_r ;
            
            Contenidollamada.Rule = Contenidollamada2
                    | Vacio;

            Contenidollamada2.Rule = Contenidollamada2 + coma_r + Contenidollamada3
                    | Contenidollamada3;

            Contenidollamada3.Rule = Expd;
            //----------------------------------
            
            //hasta
            Hasta.Rule = hasta_r + par_a_r + Expd + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r;
            //------------------------

            //mientras
            Mientras.Rule = Mientras_r + par_a_r + Expd + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r;
            //------------------------
            
            //si y sino
            Si.Rule = si_r + par_a_r + Expd + par_b_r + corchete_a_r + Contenidometfun + corchete_b_r + Sino;

            
            Sino.Rule = sino_r + corchete_a_r + Contenidometfun + corchete_b_r
                   | Vacio;
            //--------------------------------

            
            //Expre
            Expd.Rule = Expd + mas_r + Expd
                      | Expd + menos_r + Expd
                      | Expd + por_r + Expd
                      | Expd + dividir_r + Expd
                      | Expd + pow_r + Expd
                      | Expd + modulo_r + Expd
                      | Expd + menor_r + Expd
                      | Expd + menor_igual_r + Expd
                      | Expd + mayor_r + Expd
                      | Expd + mayor_igual_r + Expd
                      | Expd + igual_igual_r + Expd
                      | Expd + diferente_r + Expd
                      | Expd + diferencia_r + Expd
                      | Expd + or_r + Expd
                      | Expd + xor_r + Expd
                      | Expd + and_r + Expd
                      | not_r + Expd
                      | par_a_r + Expd + par_b_r
                      | menos_r + Expd
                      | Numerod
                      | Numeroe
                      | Char
                      | Id
                      | Llamada
                      | Texto
                      | true_r
                      | false_r;
            //---------------------------------------------

            
            Idl.Rule = Idl + "," + Idl2
                | Idl2;


            Idl2.Rule = Id;

            Tipod.Rule = Double_r
                  | String_r
                  | Bool_r
                  | Int_r
                  | Char_r
                  | Vacio_r;

            Vacio.Rule = Empty;

            
            #endregion

            #region Preferencias
            this.Root = clr;

            #endregion



        }
    }
}