<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="CambiarPassword.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Banorte.CambiarPassword" %>


<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function ActualizarNombreArchivo(oFileInput, sTargetID) {
            var arrTemp = oFileInput.value.split('\\');
            $('#' + sTargetID).val(arrTemp[arrTemp.length - 1]).change();
        }

        function showLoader() {
            Page_ClientValidate();
            if (Page_IsValid) {
                $('#MainContent_pnlLoaderHeader').css('display', 'none');
                $('#MainContent_pnlLoaderFooter').css('display', 'none');
                $get('<%= uppMainContent.ClientID %>').style.display = 'block';
                $('#MainContent_lblCargarFacturaResultados').html('');




                return true;
            }
        }
    </script>


    <asp:UpdatePanel ID="udpMainContentCombo" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <br />
                    <h3><b>Cambio de Contraseña</b></h3>
                    <br />                       
                    <p>Por favor tenga en cuenta que los campos marcados con un (*) son obligatorios.</p>
                    <br />                
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="udpMainContent" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div class="row">
                <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8 col-sm-offset-1 col-sm-10 col-xs-12">
                    <asp:Panel runat="server" ID="pnlCodigoProveedor">
                        <div class="row">
                            <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                               
                                <label class="control-label">Contraseña Actual * </label>

                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:TextBox runat="server" ID="txtContrasenaActual" CssClass="form-control text-left" TextMode="Password"   MaxLength="20" Width="100%" placeholder="Contraseña Actual"></asp:TextBox>
                                           
                                <asp:RequiredFieldValidator ID="rfvContrasenaActual" CssClass="error error-align" runat="server" ControlToValidate="txtContrasenaActual" ErrorMessage="* Obligatorio" />
                            </div>
                            <br />
                        </div>
                        <br />
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Contraseña Nueva * </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-8">
                            <asp:TextBox runat="server" CssClass="form-control text-left" ID="txtContrasenaNueva" TextMode="Password" MaxLength="20" Width="100%" placeholder="Contraseña Nueva"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvContrasenaNueva" CssClass="error error-align" runat="server" ControlToValidate="txtContrasenaNueva" ErrorMessage="* Obligatorio" />                               
                        </div>                       
                    </div>
                    <br />                  
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Confirmar Contraseña * </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-8">
                            <asp:TextBox runat="server" CssClass="form-control text-left" ID="txtConfirmarContrasena" TextMode="Password"  MaxLength="20" Width="100%" placeholder="Confirmar Contraseña"></asp:TextBox>                            
                            <asp:CompareValidator ID="cvContrasenna" runat="server" ControlToValidate="txtConfirmarContrasena" CssClass="error error-align" ControlToCompare="txtContrasenaNueva" ErrorMessage="Las contraseñas deben ser iguales" ToolTip="Las contraseñas deben ser iguales" />
                            <asp:RequiredFieldValidator ID="rfvConfirmarContrasena" CssClass="error error-align" runat="server" ControlToValidate="txtConfirmarContrasena" ErrorMessage="* Obligatorio" /> 
                        </div>                       
                    </div>
                   <br />
                    <div class="row">
                        <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12 text-center">
                            <br />                           
                            <asp:Button runat="server" ID="btnAceptar" CssClass="btn btn-primary" Text="Aceptar" Width="80%"   OnClientClick="showLoader();" OnClick="btnAceptar_Click" ></asp:Button>
                              
                        </div>  
                    </div>
                    <br />
 
                    <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12 text-center">
                        <ul class="listaResultado">
                            <li>
                                <ul class="list-inline">

                                    <li>
                                        <br />
                                        <asp:Label runat="server" ID="lblCambioContrasena" CssClass="control-label textoArchivo">Cambio de Contraseña </asp:Label>
                                    </li>
                                     <li>
                                        <span id="imagenGuardarConntrasenaOk" runat="server" class="glyphicon glyphicon-ok text-success"></span>
                                        <span id="imagenGuardarConntrasenaNotOk" runat="server" class="glyphicon glyphicon-remove text-danger"></span>
                                    </li>
                                <li>
                                    <li>
                                        <br />
                                        <asp:Label runat="server" ID="lblGuardarContrasena" CssClass="control-label textoArchivo"></asp:Label>
                                        <asp:Label runat="server" ID="lblErrorGuardarContrasena" CssClass="control-label textoArchivo"></asp:Label>
                                    </li>
                                  
                                    <li>
                                        
                                    </li>
                                </ul>
                            </li>
                          </ul>
                    </div>

                </div>

            </div>
          
            
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAceptar" />
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
