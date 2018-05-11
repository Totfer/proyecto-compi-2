<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Crl.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type ="text/javascript">
    $(document).ready(function (){
        $('#tabs').tabs();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
                <asp:Button ID="Button1" runat="server" Text="Abrir" />
    <asp:Button ID="Button2" runat="server" Text="Guardar" />
    <asp:Button ID="Button3" runat="server" Text="Crear pestaña" />
    
    <asp:Button ID="Button4" runat="server" Text="Eliminar pestaña" />
    <asp:Button ID="Button5" runat="server" Text="Compilar" OnClick="Button5_Click" />
    <asp:Button ID="Button6" runat="server" Text="Abrir album" OnClick="Button6_Click" />
            <br>
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="518px" Width="514px" OnTextChanged="TextBox1_TextChanged" >Importar aritmeticas.clr ;

Importar relacionales.clr ;
Definir 2 ;
Definir &quot;/home/user1/pictures/reportes&quot; ;

Int a = (2+2)*5^2;
Double b = (1+1)*5^2;
String d = 1+&#39;a&#39;+&quot;b&quot;+false;
Char e = &#39;a&#39;;
Bool f = (true &amp;&amp; 5!=3)&amp;&amp;(5&gt;6||true);


Int a1,a2,a3 = &#39;a&#39;+a;
Double b1,b2,b3 = &#39;a&#39;+b;
String d1,d2,d3 = 2+&#39;c&#39;+&quot;d&quot;+true+d;
Char e1,e2,e3 = &#39;a&#39;;
Bool f1,f2,f3 = f;

Int prueba1(Int a, String b, Bool c){}
String prueba2(){}
Vacio prueba3(){}
Bool prueba4(){}

Principal(){

a = 100;
b = b1+b2;
d = b1+d1+a1;
e = &#39;e1&#39;+&#39;e2&#39;;
f = !f1;
llamada(1,2,3);
Print llamada(1,2,3)

Selecciona( a ) { 
10 :  
100: Print &quot;aaaaaaaaa&quot;  Detener; Print &quot;aaaaaaaaa&quot;
Defecto : 
} 

Int id1 = 0;
	Mientras(id1&lt;6){
		Print id1
		Si(id1==5){
			Int ex = 0;
			Mientras(ex==5){
				ex=ex+1;
				Print &quot;lllll&quot;+ex
			}
			Detener;
			Print &quot;Salir&quot;

			id1=id1+1;
		}
		Sino{
			Int ex=0;
			Mientras(ex!=5){
			ex=ex+1;
				Si(ex==3){
					Detener;
				}
			Print &quot;hallo&quot;+ex
			}
			id1=id1+1;
		}
	}

}

Int llamada(Int a,Int b,Int c){
Print a+b+c
Retorno a;
}</asp:TextBox>
    
    <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" Height="170px" Width="514px" OnTextChanged="TextBox1_TextChanged" ></asp:TextBox>
    
            <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="154px" Width="514px" ></asp:TextBox>


       <div id="tabs" style="width: 400px">
<ul>
    <li><a href="#tab1">Tab 1</a></li>
    <li><a href="#tab1">Tab 1</a></li>
</ul>
           <div id="tab1"> contenido1</div>
           <div id="tab2"> contenido2</div>
</div>
            </div>
    </form>
</body>
</html>
