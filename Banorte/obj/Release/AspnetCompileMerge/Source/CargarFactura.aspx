<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CargarFactura.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Banorte.CargarFactura" %>

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
                    <h3><b>Carga de Facturas</b></h3>
                    <br />
                    <p>Por favor tenga en cuenta que los campos marcados con un (*) son obligatorios.</p>
                    <br />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8 col-sm-offset-1 col-sm-10 col-xs-12">
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Concepto * </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:DropDownList runat="server" ID="cmbConcepto" CssClass="form-control" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="error" runat="server" ControlToValidate="cmbConcepto" InitialValue="0" ErrorMessage="* Obligatorio" />
                        </div>
                        <br />
                    </div>

                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Departamento * </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:DropDownList runat="server" ID="cmbDepartamento" CssClass="form-control" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="error" runat="server" ControlToValidate="cmbDepartamento" InitialValue="0" ErrorMessage="* Obligatorio" />
                        </div>
                        <br />
                    </div>
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
                                <label class="control-label">Código proveedor * </label>

                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                <asp:TextBox runat="server" ID="txtCodigoProveedor" CssClass="form-control text-left" onkeypress="return isNumberKey(event)" MaxLength="20" Width="100%" placeholder="Código proveedor"></asp:TextBox>
                                 
                                
                                <asp:RegularExpressionValidator ID="revCodigoProveedor" runat="server"
                                ControlToValidate="txtCodigoProveedor" CssClass="error"
                                ErrorMessage="* Solo se permiten números" ValidationExpression="\d+">
                            </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvCodigoProveedor" CssClass="error error-align" runat="server" ControlToValidate="txtCodigoProveedor" ErrorMessage="* Obligatorio" />
                            </div>
                            <br />
                        </div>
                        <br />
                    </asp:Panel>
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">N° Pedido y Posición </label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-8">
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtNumeroPedido" onkeypress="return isNumberKey(event)" MaxLength="10" Width="100%" placeholder="45000000"></asp:TextBox>

                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-4">
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtPosicion" onkeypress="return isNumberKey(event)" MaxLength="5" Width="100%" placeholder="00000"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-3 col-lg-8 col-md-offset-3 col-md-8 col-sm-offset-3 col-sm-8 col-xs-offset-0 col-xs-12 text-center">
                            <label class="nota-posicion"><span class="text-primary nota-posicion">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Nota:</span> 10 caracteres iniciando con consecutivo 45 * </label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-4 col-lg-6 col-md-offset-4 col-md-6 col-sm-offset-4 col-sm-6 col-xs-8">
                            <%--                            <asp:RequiredFieldValidator ID="rfvNumeroPedido" CssClass="error error-align" runat="server" ControlToValidate="txtNumeroPedido" ErrorMessage="* Obligatorio" />--%>
                            <asp:RegularExpressionValidator ID="revNumeroPedido" runat="server"
                                ControlToValidate="txtNumeroPedido" CssClass="error error-nropedido-align"
                                ErrorMessage="* 10 números iniciando con 45"
                                ValidationExpression="(^(45([0-9]{8}))$)">
                            </asp:RegularExpressionValidator>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-4">
                            <%--                            <asp:RequiredFieldValidator ID="rfvPosicion" CssClass="error" runat="server" ControlToValidate="txtPosicion" ErrorMessage="* Obligatorio" />--%>
                            <asp:RegularExpressionValidator ID="revPosicion" runat="server"
                                ControlToValidate="txtPosicion" CssClass="error"
                                ErrorMessage="* 5 números" ValidationExpression="(^([0-9]{5})$)">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Archivo XML *</label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtArchivoXML" placeholder="Archivo XML"></asp:TextBox>
                                <label class="input-group-btn">
                                    <span class="btn btn-default btn-file-upload btn-file-upload-xml">
                                        <asp:FileUpload ID="fupArchivoXML" runat="server" onchange="ActualizarNombreArchivo(this, 'MainContent_txtArchivoXML');" Style="display: none;" accept=".xml" />
                                    </span>
                                </label>
                            </div>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtArchivoXML"
                                ErrorMessage="* Obligatorio" CssClass="error error-align" />
                            <asp:RegularExpressionValidator ID="revArchivoXML" runat="server" ControlToValidate="txtArchivoXML"
                                ErrorMessage="* Solo se permiten archivos XML" CssClass="error"
                                ValidationExpression="(.*\.([Xx][Mm][Ll])$)">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                            <label class="control-label">Archivo PDF *</label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtArchivoPDF" placeholder="Archivo PDF"></asp:TextBox>
                                <label class="input-group-btn">
                                    <span class="btn btn-default btn-file-upload btn-file-upload-pdf">
                                        <asp:FileUpload ID="fupArchivoPDF" runat="server" onchange="ActualizarNombreArchivo(this, 'MainContent_txtArchivoPDF');" Style="display: none;" accept=".pdf" />
                                    </span>
                                </label>
                            </div>

                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtArchivoPDF"
                                ErrorMessage="* Obligatorio" CssClass="error error-align" />
                            <asp:RegularExpressionValidator ID="revArchivoPDF" runat="server" ControlToValidate="txtArchivoPDF"
                                ErrorMessage="* Solo se permiten archivos PDF" CssClass="error"
                                ValidationExpression="(.*\.([Pp][Dd][Ff])$)">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12 text-center">
                            <br />
                            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-primary" Text="Enviar" Width="80%" OnClick="btnEnviar_Click" OnClientClick="showLoader();"></asp:Button>
                           
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-10 col-md-offset-1 col-md-10 col-sm-offset-1 col-sm-10 col-xs-12 text-center">
                            <br />
                            <asp:Label runat="server" ID="lblCargarFacturaResultados" CssClass="control-label error"></asp:Label>

                        </div>
                    </div>

                </div>

            </div>
             <div class="row">
                <div class="col-lg-offset-3 col-lg-6 col-md-offset-2 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12 text-center">
                    <asp:Label runat="server" ID="lblResultado" CssClass="control-label textoArchivo">RESULTADO</asp:Label>  
                                                                        
                </div>
            </div>
            
            <div class="row">

                <div class="col-lg-offset-5 col-lg-7 col-md-offset-4 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12" >
                    <ul class="listaResultado">
                        <li>
                            <ul class="list-inline">
                                   
                                <li>
                                    <br />
                                    <asp:Label runat="server" ID="lblArchXML" CssClass="control-label textoArchivo">Archivo XML</asp:Label></li>
                                <li>
                                    <span id="imagenArchXMLOk" runat="server" class="glyphicon glyphicon-ok text-success"></span>
                                    <span id="imagenArchXMLNotOk" runat="server" visible="false" class="glyphicon glyphicon-remove text-danger"></span>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblErrorArchXML" CssClass="control-label textoMensaje"></asp:Label>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <ul class="list-inline">
                                <li>
                                    <b><asp:Label runat="server" ID="lblArchPDF" CssClass="control-label textoArchivo">Archivo XML</asp:Label></b></li>
                                <li>
                                    <span id="imagenArchPDFOk" runat="server" visible="false" class="glyphicon glyphicon-ok text-success"></span>
                                    <spani id="imagenArchPDFNotOk" runat="server" class="glyphicon glyphicon-remove text-danger"></spani>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblErrorArchPDF" CssClass="control-label textoMensaje"><b></b></asp:Label>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <ul class="list-inline">
                                <li>
                                    <asp:Label runat="server" ID="lblGuardarFactura" CssClass="control-label textoArchivo">Guardar Factura</asp:Label></li>
                                <li>
                                    <span id="imagenGuardarFacturaPDFOk" runat="server" visible="false" class="glyphicon glyphicon-ok text-success"></span>
                                    <span id="imagenGuardarFacturaPDFNotOk" runat="server" class="glyphicon glyphicon-remove text-danger"></span>
                                </li>
                                <li>
                                    <b><asp:Label runat="server" ID="lblErrorGuardarFactura" CssClass="control-label textoArchivo"></asp:Label></b>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnEnviar" />
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

