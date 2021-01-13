<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Banorte.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <asp:PlaceHolder runat="server"></asp:PlaceHolder>

    <script src="Scripts/moment.min.js"></script>
    <script src="Scripts/jquery-3.1.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/respond.min.js"></script>
    <script src="Scripts/block_click.js"></script>
    <webopt:BundleReference runat="server" Path="~/Content/Login/css" />

    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>

    <div class="container">
        <br />
        <br />
        <div class="row">
            <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8  col-sm-offset-2 col-sm-8 col-xs-offset-1 col-xs-10 form-login-outer">
                <form id="frmLogin" runat="server">
                    <div class="row">
                        <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8  col-sm-offset-2 col-sm-8 col-xs-offset-2 col-xs-8">
                            <br />
                            <img src="Images/servicios.png" class="img-responsive" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8  col-sm-offset-1 col-sm-10 col-xs-offset-1 col-xs-10 form-login form-login-error-message">
                            <br />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsuario" ErrorMessage="* Usuario es obligatorio" />
                            <div class="clearfix"></div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="* Contraseña es obligatoria" />
                            <div class="clearfix"></div>
                            <label runat="server" id="lblResultados" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8  col-sm-offset-1 col-sm-10 col-xs-offset-1 col-xs-10 form-login">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <img src="Images/usuario.png" />
                                </span>
                                <asp:TextBox runat="server" ID="txtUsuario" placeholder="Usuario" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8  col-sm-offset-1 col-sm-10 col-xs-offset-1 col-xs-10 form-login">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <img src="Images/candado.png" />
                                </span>
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" placeholder="Contraseña" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8  col-sm-offset-1 col-sm-10 col-xs-offset-1 col-xs-10 form-login">
                            <asp:ImageButton runat="server" CssClass="btn btn-lg" ImageUrl="~/Images/aceptar.png" Width="80%" ID="btnAceptar" OnClick="btnAceptar_Click" />
                            <br />
                            <br />
                        </div>
                    </div>
                </form>
                <div class="login-triangle-down"></div>
            </div>
            <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8  col-sm-offset-2 col-sm-8 col-xs-offset-1 col-xs-10 form-login-outer">
                <div class="row">
                    <div class="col-lg-offset-1 col-lg-10 col-md-offset-1 col-md-10  col-sm-offset-1 col-sm-10 col-xs-offset-0 col-xs-12 form-login-footer">
                        <br />
                        <p>
                            &copy; <%: DateTime.Now.Year %>. Afore XXI Banorte S.A. de C.V.
                        <br />
                            Derechos Reservados
                        </p>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
