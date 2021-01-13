<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Http404ErrorPage.aspx.cs" Inherits="SIMLA.Http404ErrorPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:bundlereference runat="server" path="~/Content/Login/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="frmHttp404ErrorPage" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--Para obtener más información sobre cómo agrupar scripts en ScriptManager, consulte http://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Scripts de Framework--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Scripts del sitio--%>
            </Scripts>
        </asp:ScriptManager>
        <br />
        <div class="container col-lg-offset-3 col-lg-6 col-md-offset-3 col-md-6 col-sm-offset-2 col-sm-8 col-xs-offset-1 col-xs-10">
            <div class="col-lg-offset-1 col-lg-10 col-md-offset-1 col-md-10 col-sm-offset-1 col-sm-10 col-xs-offset-0 col-xs-12 fondo">
                <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8 col-sm-offset-2 col-sm-8 col-xs-offset-2 col-xs-8 titulo text-center">
                    <asp:Panel ID="pnlError" runat="server" Style="font-weight: bold; color: #FFFFFF;" Width="100%">
                        <asp:Label ID="lblError" runat="server" Text="Error" />
                    </asp:Panel>
                </div>
                <br />
                <div class="col-lg-offset-1 col-lg-10 col-md-offset-1 col-md-10 col-xs-offset-1 col-xs-10">
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <img alt="" src="../images/error-404.png" />
                            <br />
                            <asp:Label ID="exMessage" runat="server" Font-Bold="true" Font-Size="Large" >
                                Página no encontrada.
                            </asp:Label>
                            <br />
                        </div>
                    </div>
                    <br />
                    <label>Ir a <a href="../Default.aspx" target="_top" style="color: #FFFFFF; font-size: 12px; font-family: Verdana;">Inicio</a></label>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center pie">
                        <br />
                        <p>&copy; <%: DateTime.Now.Year %>. Afore XXI Banorte S.A. de C.V.
                            <br />
                            Derechos Reservados</p>
                        <br />
                        <br />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <span class="visible-xs">SIZE XS</span>
        <span class="visible-sm">SIZE SM</span>
        <span class="visible-md">SIZE MD</span>
        <span class="visible-lg">SIZE LG</span>
    </form>
</body>
</html>
