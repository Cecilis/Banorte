<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarUsuario.aspx.cs" Inherits="Banorte.ConsultarUsuario" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showLoader() {
            Page_ClientValidate();
            if (Page_IsValid) {
                $('#MainContent_pnlLoaderHeader').css('display', 'none');
                $('#MainContent_pnlLoaderFooter').css('display', 'none')
                $('#MainContent_udpLoader').css('display', 'block');
                $('#MainContent_lblResultados').val('');
                $('#MainContent_lblHayRegistro').val('');
                $('#MainContent_btnEnviar').disabled = true;
                return true;
            }
            else {
                $('#MainContent_btnEnviar').disabled = false;
                return false;
            }
        }
        function ChangeIcon(obj) {
            //alert($(obj));
        }
    </script>

    <asp:UpdatePanel ID="udpMainContent" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                    <br />
                    <h3><b>Consulta de usuarios</b></h3>
                    <br />
                    <asp:HiddenField runat="server" ID="hdnIDUsuarioEnEdicion" Value="0" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-lg-offset-0 col-lg-12 col-md-offset-1 col-md-10 col-sm-offset-1 col-sm-10 col-xs-12">
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 center-block">
                            <div class="table-responsive">
                                <asp:Repeater ID="rptUsuarios" runat="server" OnItemCommand="rptUsuarios_ItemCommand" OnItemCreated="rptUsuarios_ItemCreated">
                                    <HeaderTemplate>
                                        <table class="table table-condensed table-striped table-bordered table-hover table-sm">
                                            <tr class="table-header">
                                                <td class="hidden-lg hidden-md col-sm-1 col-xs-1 text-center bg-default"></td>
                                                <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center bg-default"><b>Usuario</b></br>
                                                    <%--                                                    <asp:TextBox runat="server" ID="txtFltUsuario" Width="80%" CssClass="form-control input-sm"  OnTextChanged="txtFltUsuario_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                                    &nbsp;&nbsp;--%>
                                                    <asp:LinkButton ID="lnkLoginASC" runat="server" ToolTip="Ascendente" CommandName="login" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkLoginDESC" runat="server" ToolTip="Descendente" CommandName="login" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center bg-default"><b>Proveedor</b></br>
                                                    <asp:LinkButton ID="lnkEsProveedorASC" runat="server" ToolTip="Ascendente" CommandName="es_proveedor" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkEsProveedorDESC" runat="server" ToolTip="Descendente" CommandName="es_proveedor" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>                                                    
                                                </td>
                                                <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center bg-default"><b>Código Proveedor</b></br>
                                                    <asp:LinkButton ID="lnkCodProvASC" runat="server" ToolTip="Ascendente" CommandName="codigoProveedor" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="LnkCodProvDESC" runat="server" ToolTip="Descendente" CommandName="codigoProveedor" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center bg-default"><b>Razón Social</b></br>
                                                    <asp:LinkButton ID="lnkRazonSocialASC" runat="server" ToolTip="Ascendente" CommandName="razonSocial" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkRazonSocialDESC" runat="server" ToolTip="Descendente" CommandName="razonSocial" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center bg-default"><b>Administrador</b></br>                                              
                                                    <asp:LinkButton ID="lnkSuperUsuarioASC" runat="server" ToolTip="Ascendente" CommandName="esSuperUsuario" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkSuperUsuarioDESC" runat="server" ToolTip="Descendente" CommandName="esSuperUsuario" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center bg-default"><b>Cambiar Clave</b></br>
                                                    <asp:LinkButton ID="lnkCambiarClaveASC" runat="server" ToolTip="Ascendente" CommandName="cambiarClave" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkCambiarClaveDESC" runat="server" ToolTip="Descendente" CommandName="cambiarClave" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center bg-default"><b>Bloqueo</b></br>
                                                    <asp:LinkButton ID="lnkTipoBloqueoASC" runat="server" ToolTip="Ascendente" CommandName="bloqueado" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkTipoBloqueoDESC" runat="server" ToolTip="Descendente" CommandName="bloqueado" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center bg-default"><b>N° Fallos</b></br>
                                                    <asp:LinkButton ID="lnkNroFallosASC" runat="server" ToolTip="Ascendente" CommandName="nroIntentos" CommandArgument="ASC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-up pull-right"></i></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkNroFallosDESC" runat="server" ToolTip="Descendente" CommandName="nroIntentos" CommandArgument="DESC" OnClick="lnkOrdenarPor_Click"><i class="glyphicon glyphicon-chevron-down pull-right"></i></asp:LinkButton></b>
                                                </td>
                                                <td class="col-lg-1 col-md-1 hidden-sm hidden-xs text-center bg-default"></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="hidden-lg hidden-md col-sm-1 col-xs-1 text-center">
                                                <asp:LinkButton runat="server" ID="lknEditarXS" CssClass="btn btn-sm btn-primary" ToolTip="Editar" CommandName="E" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'>
                                                    <i class="glyphicon glyphicon-pencil boton-table" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                                <br />
                                            </td>
                                            <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-left">
                                                <%# DataBinder.Eval(Container.DataItem, "login") %> 
                                            </td>
                                            <%--<div class="clearfix visible-xs"></div>--%>
                                            <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center">
                                                <%# (bool)DataBinder.Eval(Container, "DataItem.es_proveedor") == true ? "Si" : "No"%>
                                            </td>
                                            <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center">
                                                <asp:Label ID="lblCodigoProveedor" runat="server" Text='<%# Eval("codigoProveedor")%>'></asp:Label>
                                            </td>
                                            <td class="col-lg-2 col-md-2 col-sm-2 col-xs-2 text-center">
                                                <%# DataBinder.Eval(Container.DataItem, "razonSocial") %> 
                                            </td>
                                            <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center">
                                                <%# (bool)DataBinder.Eval(Container, "DataItem.esSuperUsuario") == true ? "Si" : "No"%>
                                            </td>
                                            <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center">
                                                <%# (bool)DataBinder.Eval(Container, "DataItem.cambiarClave") == true ? "Si" : "No"%>
                                            </td>
                                            <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center">
                                                <%# Convert.ToInt32(DataBinder.Eval(Container, "DataItem.bloqueado")) == 0 ? "No" : "Si" %>
                                            </td>
                                            <td class="col-lg-1 col-md-1 col-sm-1 col-xs-1 text-center">
                                                <%# DataBinder.Eval(Container.DataItem, "nroIntentos") %> 
                                            </td>
                                            <td class="col-lg-1 col-md-1 hidden-sm hidden-xs text-center">
                                                <asp:LinkButton runat="server" ID="lknEditar" CssClass="btn btn-sm btn-primary" ToolTip="Editar" CommandName="E" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'>
                                                    <i class="glyphicon glyphicon-pencil boton-table" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                                <br />
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
                                            OnClick="lblPagPrevio_Click"><i class="glyphicon glyphicon-chevron-left" aria-hidden="true"></i></asp:LinkButton>
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
                                            OnClick="lblPagSiguiente_Click"><i class="glyphicon glyphicon-chevron-right" aria-hidden="true"></i></asp:LinkButton></li>
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
            <br />
            <div class="row">
                <div class="col-lg-offset-5 col-lg-7 col-md-offset-4 col-md-8 col-sm-offset-3 col-sm-6 col-xs-12">
                    <ul class="listaResultado">
                        <li>
                            <ul class="list-inline">
                                <li>
                                    <br />
                                    <asp:Label runat="server" ID="lblConsultarUsuario" CssClass="control-label textoArchivo">Consultar </asp:Label></li>
                                <li>
                                    <i id="icoOK" runat="server" class="glyphicon glyphicon-ok text-success"></i>
                                    <i id="icoNOTOK" runat="server" visible="false" class="glyphicon glyphicon-remove text-danger"></i>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblConsultarUsuarioResultados" CssClass="control-label textoMensaje"></asp:Label>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rptUsuarios" />
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
</asp:Content>
