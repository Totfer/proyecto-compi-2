<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Crl.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,700" rel="stylesheet">
	
	<!-- Animate.css -->
	<link rel="stylesheet" href="css/animate.css">
	<!-- Icomoon Icon Fonts-->
	<link rel="stylesheet" href="css/icomoon.css">
	<!-- Themify Icons-->
	<link rel="stylesheet" href="css/themify-icons.css">
	<!-- Bootstrap  -->
	<link rel="stylesheet" href="css/bootstrap.css">

	<!-- Magnific Popup -->
	<link rel="stylesheet" href="css/magnific-popup.css">

	<!-- Owl Carousel  -->
	<link rel="stylesheet" href="css/owl.carousel.min.css">
	<link rel="stylesheet" href="css/owl.theme.default.min.css">

	<!-- Theme style  -->
	<link rel="stylesheet" href="css/style.css">

	<!-- Modernizr JS -->
	<script src="js/modernizr-2.6.2.min.js"></script>
	<!-- FOR IE9 below -->
	<!--[if lt IE 9]>
	<script src="js/respond.min.js"></script>
	<![endif]-->




</head>
<body>
    <div class="gtco-cover gtco-cover-sm" id="gtco-header" role="banner" style="background-image: url(images/img_1.jpg)">
       <div class="row">
    <form id="form1" runat="server">
        <div class="row">
    <div class="col-md-2">
                 <div class="col-md-2">
    <asp:Button ID="Button7" runat="server" Text="Agregar Pestaña"  CssClass="btn btn-primary"/>
                </div>
                 <div class="col-md-2">
    <asp:Button ID="Button8" runat="server" Text="Eliminar Pestaña"  CssClass="btn btn-primary"/>
                </div>
<br>

        <div class="col-md-4 col-md-push-1 animate-box fadeInRight animated-fast" data-animate-effect="fadeInRight">
							<div class="form-wrap">
								<div class="tab">
									<ul class="tab-menu">
										<li class="gtco-first active"><a href="#" data-tab="signup">Sign up</a></li>
										<li class="gtco-second"><a href="#" data-tab="login">Login</a></li>
									</ul>
									<%--	<div class="tab-content-inner active" data-content="sign up">
 --%>                                              <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="518px" Width="1527px" OnTextChanged="TextBox1_TextChanged" CssClass="form-control"  placeholder="Write us something" rows="10" cols="30">Importar aritmeticas.clr ;
        
											<%--</div>

	--%>									

								</div>
							</div>
						</div>

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
     <br> 
        <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" Height="170px" Width="514px" OnTextChanged="TextBox1_TextChanged" CssClass="form-control"  placeholder="Write us something" rows="10" cols="30"></asp:TextBox>
        <br>    
         <div class="col-md-2">
        <asp:Button ID="Button1" runat="server" Text="Abrir"  CssClass="btn btn-primary"/>
        </div>
            <div class="col-md-2">
    <asp:Button ID="Button2" runat="server" Text="Guardar"  CssClass="btn btn-primary"/>
                </div>
            <div class="col-md-2">
    <asp:Button ID="Button3" runat="server" Text="Crear pestaña"  CssClass="btn btn-primary"/>
    </div>
            <div class="col-md-2">
    <asp:Button ID="Button4" runat="server" Text="Eliminar pestaña"  CssClass="btn btn-primary"/>
                </div>
            <div class="col-md-2">
    <asp:Button ID="Button5" runat="server" Text="Compilar" OnClick="Button5_Click"  CssClass="btn btn-primary"/>
                </div>
            <div class="col-md-2">
    <asp:Button ID="Button6" runat="server" Text="Abrir album" OnClick="Button6_Click"  CssClass="btn btn-primary"/>
            </div><br />
    </div>
      
      
            </div>
    </form>

    </div>
</body>
</html>
