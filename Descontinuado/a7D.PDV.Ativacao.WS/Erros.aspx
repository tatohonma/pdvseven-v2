<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Erros.aspx.cs" Inherits="a7D.PDV.Ativacao.WS.Erros" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ultimos Erros PDV7</title>
    <meta http-equiv="refresh" content="300">
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="lblInfo" />
        <div>
            <asp:GridView runat="server" ID="gv" />
        </div>
    </form>
</body>
</html>
