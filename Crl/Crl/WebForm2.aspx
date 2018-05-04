<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Crl.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
                <asp:Button ID="Button1" runat="server" Text="Abrir" />
    <asp:Button ID="Button2" runat="server" Text="Guardar" />
    <asp:Button ID="Button3" runat="server" Text="Crear pestaña" />
    
    <asp:Button ID="Button4" runat="server" Text="Eliminar pestaña" />
    <asp:Button ID="Button5" runat="server" Text="Compilar" OnClick="Button5_Click" />
            <br>
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="518px" Width="514px" OnTextChanged="TextBox1_TextChanged" >Importar aritmeticas.clr ;
Importar relacionales.clr ;
Definir 2 ;
Definir &quot;/home/user1/pictures/reportes&quot; ;

Char a=49;
Char b=&#39;1&#39;;

Double a2=8;
Int a3= 20;
Int prueba1(Int a, String b, Bool c){}
String prueba2(){}
Vacio prueba3(){}
Bool prueba4(){}

Principal(){
Int a = llamada(100);
 

Selecciona( a * 2 ) { 
10 :  
200: Print &quot;aaaaaaaaa&quot;  
Defecto : 
} 
Si(a&gt;99){
Int a1 = 99;

Si(a1&gt;99){
Int a1 = 99;
}Sino{
Si(a2==8){
Print &quot;Entro prro&quot;
}
}

}Sino{
Int a1 = 99;
}

}

Int llamada(Int a){
Retorno a;
}
</asp:TextBox>
    
    <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" Height="170px" Width="514px" OnTextChanged="TextBox1_TextChanged" ></asp:TextBox>
    
            <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="154px" Width="514px" ></asp:TextBox>
        </div>
    </form>
</body>
</html>
