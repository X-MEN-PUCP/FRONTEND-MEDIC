<%-- Archivo: admin_gestionar_usuarios.aspx --%>
<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/SoftMA_Admin.Master" AutoEventWireup="true" CodeBehind="admin_gestionar_usuarios.aspx.cs" Inherits="SoftWA.admin_gestionar_usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Gestión de Usuarios y Roles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="updGestionUsuarios" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <!-- titulo -->
                <div class="d-sm-flex align-items-center justify-content-between mb-4">
                    <h1 class="h3 mb-0 text-gray-800">Gestión de Usuarios y Roles</h1>
                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalAgregarUsuario">
                        <i class="fas fa-plus-circle me-2"></i>Agregar Nuevo Usuario
                    </button>
                </div>
                <asp:PlaceHolder ID="phMensaje" runat="server" Visible="false">
                    <div id="divAlert" runat="server" role="alert" class="alert alert-dismissible fade show">
                        <asp:Literal ID="ltlMensaje" runat="server"></asp:Literal>
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                </asp:PlaceHolder>

                <!--filtros -->
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h6 class="m-0 font-weight-bold text-primary">Filtros de Búsqueda</h6>
                    </div>
                    <div class="card-body">
                        <div class="row align-items-end">
                            <div class="col-md-4 mb-3">
                                <label for="<%= txtFiltroNombre.ClientID %>">Nombre, Apellido o Documento:</label>
                                <asp:TextBox ID="txtFiltroNombre" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3 mb-3">
                                <label for="<%= ddlFiltroRol.ClientID %>">Filtrar por Rol:</label>
                                <asp:DropDownList ID="ddlFiltroRol" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-3 mb-3">
                                <label for="<%= ddlOrdenarUsuarios.ClientID %>">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarUsuarios" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="IdAsc" Text="ID (Ascendente)"></asp:ListItem>
                                    <asp:ListItem Value="IdDesc" Text="ID (Descendente)"></asp:ListItem>
                                    <asp:ListItem Value="NombreAsc" Text="Nombre (A-Z)"></asp:ListItem>
                                    <asp:ListItem Value="NombreDesc" Text="Nombre (Z-A)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 mb-3">
                                <asp:Button ID="btnAplicarFiltros" runat="server" Text="Buscar" CssClass="btn btn-primary w-100" OnClick="btnAplicarFiltros_Click" />
                                <asp:LinkButton ID="lnkLimpiarFiltros" runat="server" OnClick="lnkLimpiarFiltros_Click" CssClass="btn btn-link w-100" CausesValidation="false">Limpiar</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- tabla -->
                <div class="card shadow mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:ListView ID="lvUsuarios" runat="server"
                                OnItemCommand="lvUsuarios_ItemCommand"
                                OnItemDataBound="lvUsuarios_ItemDataBound"
                                DataKeyNames="IdUsuario">
                                <LayoutTemplate>
                                    <table class="table table-striped table-hover" id="dataTable" width="100%" cellspacing="0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>ID</th>
                                                <th>Nombre Completo</th>
                                                <th>Documento</th>
                                                <th>Correo</th>
                                                <th>Roles</th>
                                                <th>Estado</th>
                                                <th class="text-center">Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("IdUsuario") %></td>
                                        <td><%# Eval("NombreCompleto") %></td>
                                        <td><%# Eval("TipoDocumento") %>: <%# Eval("NumDocumento") %></td>
                                        <td><%# Eval("Correo") %></td>
                                        <td><%# Eval("RolesConcatenados") %></td>
                                        <td class="text-center">
                                            <asp:Literal ID="ltlEstado" runat="server"></asp:Literal>
                                        </td>
                                        <td class="text-center">
                                            <asp:LinkButton ID="btnGestionarRoles" runat="server" CommandName="GestionarRoles" CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-sm btn-info me-2" ToolTip="Gestionar Roles">
                                                <i class="fas fa-user-shield"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnToggleStatus" runat="server" CommandName="ToggleStatus" CommandArgument='<%# Eval("IdUsuario") %>' CssClass="btn btn-sm">
                                                <asp:Literal ID="ltlIcono" runat="server" />
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        No se encontraron usuarios que coincidan con los filtros de búsqueda.
                                    </div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- gestionar roles -->
            <div class="modal fade" id="modalGestionarRoles" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalLabel">Gestionar Roles de Usuario</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="hfUsuarioIdModal" runat="server" />
                            <p>Usuario: <strong><asp:Literal ID="ltlNombreUsuarioModal" runat="server"></asp:Literal></strong></p>
                            
                            <h6>Roles Asignados</h6>
                            <asp:Repeater ID="rptRolesActuales" runat="server" OnItemCommand="rptRoles_ItemCommand">
                                <ItemTemplate>
                                    <span class="badge bg-primary me-2 fs-6">
                                        <%# Eval("NombreRol") %>
                                        <asp:LinkButton ID="btnQuitarRol" runat="server" CommandName="QuitarRol" CommandArgument='<%# Eval("IdRol") %>' CssClass="text-white ms-1" ToolTip="Quitar Rol">
                                            <i class="fas fa-times-circle"></i>
                                        </asp:LinkButton>
                                    </span>
                                </ItemTemplate>
                            </asp:Repeater>
                            <hr />
                            <h6>Asignar Nuevo Rol</h6>
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlRolesDisponibles" runat="server" CssClass="form-select"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnAsignarRol" runat="server" Text="Asignar" CssClass="btn btn-success w-100" OnClick="btnAsignarRol_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="modalAgregarUsuario" tabindex="-1" aria-labelledby="modalAgregarLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalAgregarLabel">Agregar Nuevo Usuario</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" onclick="limpiarFormulario()"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Nombres*</label>
                                    <asp:TextBox ID="txtNombresNuevo" runat="server" CssClass="form-control" ValidationGroup="NuevoUsuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNombres" runat="server" ControlToValidate="txtNombresNuevo" ErrorMessage="Nombres es requerido." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Apellido Paterno*</label>
                                    <asp:TextBox ID="txtApellidoPaternoNuevo" runat="server" CssClass="form-control" ValidationGroup="NuevoUsuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvApellidoPaterno" runat="server" ControlToValidate="txtApellidoPaternoNuevo" ErrorMessage="Apellido Paterno es requerido." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Tipo Documento*</label>
                                    <asp:DropDownList ID="ddlTipoDocumentoNuevo" runat="server" CssClass="form-select" ValidationGroup="NuevoUsuario">
                                        <asp:ListItem Value="DNI">DNI</asp:ListItem>
                                        <asp:ListItem Value="CE">Carnet de Extranjería</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Número Documento*</label>
                                    <asp:TextBox ID="txtNumDocumentoNuevo" runat="server" CssClass="form-control" ValidationGroup="NuevoUsuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNumDocumento" runat="server" ControlToValidate="txtNumDocumentoNuevo" ErrorMessage="Número de documento es requerido." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                             <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Correo Electrónico*</label>
                                    <asp:TextBox ID="txtCorreoNuevo" runat="server" CssClass="form-control" TextMode="Email" ValidationGroup="NuevoUsuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreoNuevo" ErrorMessage="Correo es requerido." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Contraseña Temporal*</label>
                                    <asp:TextBox ID="txtContrasenhaNuevo" runat="server" CssClass="form-control" TextMode="Password" ValidationGroup="NuevoUsuario"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvContrasenha" runat="server" ControlToValidate="txtContrasenhaNuevo" ErrorMessage="Contraseña es requerida." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Rol Principal*</label>
                                    <asp:DropDownList ID="ddlRolNuevo" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlRolNuevo_SelectedIndexChanged" ValidationGroup="NuevoUsuario"></asp:DropDownList>
                                </div>
                                <%-- Panel para la especialidad, visible solo para médicos --%>
                                <asp:Panel ID="pnlEspecialidadNuevo" runat="server" Visible="false" CssClass="col-md-6 mb-3">
                                    <label class="form-label">Especialidad del Médico*</label>
                                    <asp:DropDownList ID="ddlEspecialidadNuevo" runat="server" CssClass="form-select" ValidationGroup="NuevoUsuario"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="ddlEspecialidadNuevo" InitialValue="0" ErrorMessage="Especialidad es requerida para médicos." CssClass="text-danger" ValidationGroup="NuevoUsuario" Display="Dynamic"></asp:RequiredFieldValidator>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" onclick="limpiarFormulario()">Cancelar</button>
                            <asp:Button ID="btnGuardarNuevoUsuario" runat="server" Text="Guardar Usuario" CssClass="btn btn-primary" OnClick="btnGuardarNuevoUsuario_Click" ValidationGroup="NuevoUsuario" />
                        </div>
                    </div>
                </div>
            </div>
            
            <script type="text/javascript">
                function limpiarFormulario() {
                    // Limpia los campos del formulario de creación de usuario al cerrar el modal
                    document.getElementById('<%= txtNombresNuevo.ClientID %>').value = '';
                    document.getElementById('<%= txtApellidoPaternoNuevo.ClientID %>').value = '';
                    document.getElementById('<%= txtNumDocumentoNuevo.ClientID %>').value = '';
                    document.getElementById('<%= txtCorreoNuevo.ClientID %>').value = '';
                    document.getElementById('<%= txtContrasenhaNuevo.ClientID %>').value = '';
                    document.getElementById('<%= ddlRolNuevo.ClientID %>').selectedIndex = 0;
                    // Oculta el panel de especialidad
                    var panelEspecialidad = document.getElementById('<%= pnlEspecialidadNuevo.ClientID %>');
                    if (panelEspecialidad) {
                        panelEspecialidad.style.display = 'none';
                    }
                    // Limpia los mensajes de validación (opcional, pero buena práctica)
                    if (typeof (Page_ClientValidate) == 'function') {
                        var validators = Page_Validators;
                        for (var i = 0; i < validators.length; i++) {
                            if (validators[i].validationGroup == 'NuevoUsuario') {
                                ValidatorUpdateDisplay(validators[i]);
                            }
                        }
                    }
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>