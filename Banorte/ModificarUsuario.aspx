<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModificarUsuario.aspx.cs" Inherits="Banorte.ModificarUsuario" %>

<%@ PreviousPageType VirtualPath="~/ConsultarUsuario.aspx" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
       function showLoader() {
            Page_ClientValidate();
            if (Page_IsValid) {
                $('#MainContent_pnlLoaderHeader').css('display', 'none');
                $('#MainContent_pnlLoaderFooter').css('display', 'none');
                $get('<%= uppMainContent.ClientID %>').style.display = 'block';
                $('#MainContent_lblModificarUsuario').html('');
                $('#MainContent_lblModificarUsuario').val('');
                $('#MainContent_lblModificarUsuarioResultados').html('');
                $('#MainContent_lblModificarUsuarioResultados').val('');
                return true;
            }
        }
    </script>

    <asp:UpdatePanel ID="udpMainContent" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <br />
                    <h3><b>Editar Usuario</b></h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-offset-9 col-lg-3 col-md-offset-9 col-md-3 col-sm-offset-9 col-sm-3 col-xs-offset-9 col-xs-3 text-center">
                    
                    <asp:LinkButton runat="server" ID="lknEditar" CssClass="btn btn-primary" ToolTip="Volver a la consulta" PostBackUrl="~/ConsultarUsuario.aspx" CausesValidation="false" >
                        <i class="glyphicon glyphicon-arrow-left" aria-hidden="true"></i>
                    </asp:LinkButton>                    
                    
                    <asp:LinkButton runat="server" ID="lknReCargar" CssClass="btn btn-primary" ToolTip="Recargar" OnClientClick="window.location.reload();" CausesValidation="false" >
                        <i class="glyphicon glyphicon-refresh" aria-hidden="true"></i>
                    </asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <p>Por favor tenga en cuenta que los campos marcados con un (*) son obligatorios.</p>
                    <br />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-offset-2 col-lg-9 col-md-offset-2 col-md-8 col-sm-offset-1 col-sm-10 col-xs-12">
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Login * </label>
                        </div>
                        <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox runat="server" ID="txtLogin" CssClass="form-control" autocomplete="off">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="error" runat="server" ControlToValidate="txtLogin" AutoCompleteType="None" InitialValue="" ErrorMessage="* Obligatorio" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Contraseña * </label>
                        </div>
                        <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox runat="server" ID="txtClave" TextMode="Password"  autocomplete="off" CssClass="form-control" ReadOnly="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvClave" CssClass="error" runat="server" ControlToValidate="txtClave" InitialValue="" ErrorMessage="* Obligatorio" />
                        </div>
                    </div>
                   <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Confirmar Contraseña * </label>
                        </div>
                          
                           <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox runat="server" CssClass="form-control text-left" TextMode="Password" ID="txtConfirmarClave" autocomplete="off"  ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContrasenna" CssClass="error" runat="server" ControlToValidate="txtConfirmarClave" InitialValue="" ErrorMessage="* Obligatorio" />
                            <asp:CompareValidator ID="cvContrasenna" runat="server" ControlToValidate="txtConfirmarClave" CssClass="error error-align" ControlToCompare="txtClave" ErrorMessage="Las contraseñas deben ser iguales" ToolTip="Las contraseñas deben ser iguales" />
                            <asp:CustomValidator ID="cvlContrasenna" runat="server" CssClass="error" ErrorMessage="* Obligatorio" OnServerValidate="txtConfirmaClave_ServerValidate"></asp:CustomValidator>
                        </div>

                          <div class="col-lg-offset-0 col-lg-2 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                              <label class="control-label">
                                <asp:CheckBox runat="server" ID="chkCambioContrasena" AutoPostBack="true" OnCheckedChanged="chkCambioContrasena_CheckedChanged" />
                                &nbsp;&nbsp;Cambiar<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Contraseña
                            </label>
                        </div>
                                              
                    </div>
                    <br>
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Tipo de Usuario
                            </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <label class="control-label">
                            <asp:CheckBox ID="chkEsProveedor" Checked="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkEsProveedor_CheckedChanged" />
                            &nbsp;&nbsp;Proveedor
                            </label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Código Proveedor *
                            </label>
                        </div>
                        <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox ID="txtCodigoProveedor" runat="server" CssClass="form-control" ReadOnly="true">
                            </asp:TextBox>
                            <asp:CustomValidator ID="cvlCodigoProveedor" runat="server" CssClass="error" ErrorMessage="* Obligatorio" OnServerValidate="txtCodigoProveedor_ServerValidate"></asp:CustomValidator>
                        </div>
                        <br />
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Razón Social
                            </label>
                        </div>
                        <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" ReadOnly="true">
                            </asp:TextBox>
                            <asp:CustomValidator ID="cvlRazonSocial" runat="server" CssClass="error" ErrorMessage="* Obligatorio" OnServerValidate="txtRazonSocial_ServerValidate"></asp:CustomValidator>
                        </div>
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Tipo de Usuario
                            </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <label class="control-label">
                            <asp:CheckBox ID="chkEsSuperUsuario" runat="server" />
                            &nbsp;&nbsp; Administrador
                            </label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Bloqueado
                            </label>
                        </div>
                        <div class="col-lg-3 col-md-5 col-sm-6 col-xs-12">
                            <asp:DropDownList ID="ddlTiposBloqueo" runat="server" CssClass="form-control" Width="100%">
                                <asp:ListItem  Text="Si" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-offset-0 col-lg-2 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            N° Intentos Conexión</label>
                        </div>
                        <div class="col-lg-offset-0 col-lg-1 col-md-6 col-sm-6 col-xs-12">
                            <asp:TextBox ID="txtNroIntentos" runat="server" CssClass="form-control text-right" ReadOnly="true" Text="0">
                            </asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">
                            Cambio de Clave Obligatorio
                            </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <label class="control-label">
                            <asp:CheckBox ID="chkCambiarClave" runat="server" />
                            &nbsp;&nbsp; Si
                            </label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12 text-center">
                            <br />
                            <asp:Button ID="btnEnviar" runat="server" CssClass="btn btn-primary" OnClick="btnEnviar_Click" OnClientClick="showLoader();" Text="Enviar" Width="80%" />
                        </div>
                    </div>
                    </br>
                </div>
                <br />
            <br />
            <div class="row">
                <div class="col-lg-offset-5 col-lg-7 col-md-offset-4 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12">
                    <ul class="listaResultado">
                        <li>
                            <ul class="list-inline">
                                <li>
                                    <br />
                                    <asp:Label runat="server" ID="lblModificarUsuario" CssClass="control-label textoArchivo">Editar </asp:Label></li>
                                <li>
                                    <i id="icoOK" runat="server" class="glyphicon glyphicon-ok text-success"></i>
                                    <i id="icoNOTOK" runat="server" visible="false" class="glyphicon glyphicon-remove text-danger"></i>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblModificarUsuarioResultados" CssClass="control-label textoMensaje"></asp:Label>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
        </ContentTemplate>
        <Triggers>
<%--            <asp:PostBackTrigger ControlID="chkEsProveedor" />
            <asp:PostBackTrigger ControlID="chkCambioContrasena" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="uppMainContent" runat="server" AssociatedUpdatePanelID="udpMainContent">
        <ProgressTemplate>
            <asp:UpdatePanel ID="udpLoader" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="Background"></div>
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div id="Progress">
                                <asp:Panel runat="server" ID="pnlLoaderHeader" class="modal-header">
                                    <button id="btnHeaderClose" runat="server" type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h6 class="modal-title">
                                        <b>
                                            <asp:Label ID="lblModalTitle" runat="server" Text=""></asp:Label></b>
                                    </h6>
                                </asp:Panel>
                                <div class="modal-body">
                                    <h6>
                                        <p class="text-center">
                                            <b>
                                                <asp:Label ID="lblModalBody" runat="server" Text="Procesando Datos, Espere por favor..."></asp:Label>
                                            </b>
                                        </p>
                                    </h6>
                                </div>
                                <asp:Panel runat="server" ID="pnlLoaderFooter" class="modal-footer" Style="text-align: right;">
                                    <button id="btnFooterClose" runat="server" class="btn btn-info btn-sm" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                                </asp:Panel>
                                <br />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
<asp:Content ID="ScriptContent" ContentPlaceHolderID="BootomScriptContent" runat="server">
</asp:Content>
