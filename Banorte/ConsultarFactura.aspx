<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="ConsultarFactura.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Banorte.ConsultarFactura" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showLoader() {
            Page_ClientValidate();
            if (Page_IsValid) {
                $('#MainContent_pnlLoaderHeader').css('display', 'none');
                $('#MainContent_pnlLoaderFooter').css('display', 'none')
                $('#MainContent_udpLoader').css('display', 'block');
                $('#MainContent_lblResultados').html('');
                $('#MainContent_lblHayRegistro').html('');
                $('#MainContent_btnEnviar').disabled = true;
                return true;
            }
            else {
                $('#MainContent_btnEnviar').disabled = false;
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="udpMainContent" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <br />
                    <h3><b>Consulta de estatus</b></h3>
                    <br />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-lg-offset-2 col-lg-8 col-md-offset-2 col-md-8 col-sm-offset-1 col-sm-10 col-xs-12">
                    <div class="row">
                        <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-offset-1 col-xs-5">
                            <label class="control-label">Fecha de Factura</label>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-5">
                            <div class="form-group">
                                <div class='input-group date' id='dtpFechaDesde'>
                                    <asp:TextBox runat="server" ID="txtFechaDesde" CssClass="form-control text-right"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix visible-sm visible-xs"></div>
                        <div class="col-lg-1 col-md-1 col-sm-offset-1 col-sm-3 col-xs-offset-1 col-xs-5">
                            <label class="control-label">Hasta</label>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-5">
                            <div class="form-group">
                                <div class='input-group date' id='dtpFechaHasta'>
                                    <asp:TextBox runat="server" ID="txtFechaHasta" CssClass="form-control text-right"></asp:TextBox>
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Panel runat="server" ID="pnlCodigoProveedor">
                            <div class="row">
                                <div class="col-lg-offset-1 col-lg-3 col-md-offset-1 col-md-3 col-sm-offset-1 col-sm-3 col-xs-12">
                                    <label class="control-label">Código proveedor * </label>

                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                    <asp:TextBox runat="server" ID="txtCodigoProveedor" CssClass="form-control text-left" MaxLength="20" Width="100%" placeholder="Código proveedor"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCodigoProveedor" CssClass="error error-align" runat="server" ControlToValidate="txtCodigoProveedor" ErrorMessage="* Obligatorio" />
                                </div>
                                <br />
                            </div>
                            <br />
                        </asp:Panel>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-offset-4 col-lg-4 col-md-offset-4 col-md-4 col-sm-offset-4 col-sm-4  col-xs-offset-3 col-xs-6 text-center">
                            <asp:Button runat="server" ID="btnConsultar" CssClass="btn btn-primary" Text="Continuar" Width="100%" OnClick="btnConsultar_Click" OnClientClick="showLoader()"></asp:Button>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">

                            <asp:Label runat="server" ID="lblRazonSocial" class="control-label text-center titulo-consulta"></asp:Label>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 center-block">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptFacturas" runat="server" OnItemDataBound="rptFacturas_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="table table-striped table-bordered table-hover">
                                            <tr>
                                                <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center bg-default"><b><span>Factura</span></b></td>
                                                <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center bg-default"><b><span>Estatus</span></b></td>
                                                <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center bg-default"><b><span>Fecha Registro</span></b></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-left">
                                                <%# DataBinder.Eval(Container.DataItem, "XBLNR") %> 
                                            </td>
                                            <%--<div class="clearfix visible-xs"></div>--%>
                                            <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-left">
                                                <%--<%# DataBinder.Eval(Container.DataItem, "STATU") %> --%>
                                                <%# DataBinder.Eval(Container.DataItem, "TXTST") %> 
                                            </td>
                                            <td class="col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center">

                                                <asp:Label ID="lblDATRE" runat="server" Text='<%# Eval("DATRE")%>'></asp:Label>
                                                <%--(int)DataBinder.Eval(Container, "DataItem.Active") == 0 ? "Active" : "Inactive"--%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table> 
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                            <nav>
                                <ul class="pagination pagination-centered pagination-sm" runat="server" id="olPaginador">
                                    <li class="page-item">
                                        <asp:LinkButton ID="lblPagPrimero" runat="server" CssClass="btn btn-sm btn-primary" ToolTip="Primero"
                                            OnClick="lblPagPrimero_Click"><i class="glyphicon glyphicon-fast-backward" aria-hidden="true"></i></asp:LinkButton>
                                    </li>
                                    <li class="page-item">
                                        <asp:LinkButton ID="lblPagPrevio" runat="server" CssClass="btn btn-sm btn-primary" ToolTip="Anterior"
                                            OnClick="lblPagPrevio_Click"><i class="glyphicon glyphicon-step-backward" aria-hidden="true"></i></asp:LinkButton>
                                    </li>
                                    <asp:Repeater ID="rptPaginador" runat="server"
                                        OnItemCommand="rptPaginador_ItemCommand"
                                        OnItemDataBound="rptPaginador_ItemDataBound"
                                        OnItemCreated="rptPaginador_ItemCreated">
                                        <ItemTemplate>
                                            <li class="page-item">
                                                <asp:LinkButton ID="lblPagSelecionada" runat="server" CssClass="btn btn-sm btn-primary page-link"
                                                    CommandArgument='<%# Eval("PageIndex") %>'
                                                    CommandName="newPage"
                                                    Text='<%# Eval("PageText") %>'>                                                   
                                                </asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <li class="page-item">
                                        <asp:LinkButton ID="lblPagSiguiente" runat="server" CssClass="btn btn-sm btn-primary" ToolTip="Siguiente"
                                            OnClick="lblPagSiguiente_Click"><i class="glyphicon glyphicon-step-forward" aria-hidden="true"></i></asp:LinkButton></li>
                                    <li class="page-item">
                                        <asp:LinkButton ID="lblPagUltimo" runat="server" CssClass="btn btn-sm btn-primary" ToolTip="Último"
                                            OnClick="lblPagUltimo_Click"><i class="glyphicon glyphicon-fast-forward" aria-hidden="true"></i></asp:LinkButton></li>
                                    <li class="sr-only">
                                        <a>
                                            <asp:Label ID="lblPaginaActual" runat="server" Text=""></asp:Label>
                                        </a>
                                    </li>
                                </ul>
                            </nav>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Panel runat="server" ID="pnlHayRegistros" CssClass="row" Visible="false">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <br />
                    <asp:Label runat="server" ID="lblHayRegistro" CssClass="control-label error">No existen Registros</asp:Label>
                </div>
            </asp:Panel>

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <label runat="server" id="lblResultados" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
            <asp:PostBackTrigger ControlID="rptFacturas" />
            <asp:AsyncPostBackTrigger ControlID="lblPagPrimero" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lblPagPrevio" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lblPagSiguiente" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lblPagUltimo" EventName="Click" />
            <asp:PostBackTrigger ControlID="rptPaginador" />
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
    <script src="Scripts/datepicker_config.js"></script>
</asp:Content>
