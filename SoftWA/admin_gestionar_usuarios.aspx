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
                                            <asp:LinkButton ID="btnResetPassword" runat="server"
                                                CommandName="ResetPassword" CommandArgument='<%# Eval("IdUsuario") %>'
                                                ToolTip="Restablecer Contraseña" CssClass="btn btn-link btn-sm p-0"
                                                OnClientClick="return confirm('¿Está seguro de que desea restablecer la contraseña de este usuario a su valor por defecto?');">
                                                <i class="fas fa-key text-warning"></i>
                                            </asp:LinkButton>
                                        </td></tr></ItemTemplate><EmptyDataTemplate>
                                            
                                    <div class="alert alert-info">
                                        No se encontraron usuarios que coincidan con los filtros de búsqueda. </div></EmptyDataTemplate></asp:ListView></div></div></div></div><!-- gestionar roles --><div class="modal fade" id="modalGestionarRoles" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalLabel">Gestionar Roles de Usuario</h5><button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="hfUsuarioIdModal" runat="server" />
                            <p>Usuario: <strong><asp:Literal ID="ltlNombreUsuarioModal" runat="server"></asp:Literal></strong></p><h6>Roles Asignados</h6><asp:Repeater ID="rptRolesActuales" runat="server" OnItemCommand="rptRoles_ItemCommand">
                                <ItemTemplate>
                                    <span class="badge bg-primary me-2 fs-6">
                                        <%# Eval("NombreRol") %>
                                        <asp:LinkButton ID="btnQuitarRol" runat="server" CommandName="QuitarRol" CommandArgument='<%# Eval("IdRol") %>' CssClass="text-white ms-1" ToolTip="Quitar Rol">
                                            <i class="fas fa-times-circle"></i>
                                        </asp:LinkButton></span></ItemTemplate></asp:Repeater><hr /><h6>Asignar Nuevo Rol</h6><div class="row">
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlRolesDisponibles" runat="server" CssClass="form-select"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnAsignarRol" runat="server" Text="Asignar" CssClass="btn btn-success w-100" OnClick="btnAsignarRol_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button></div></div></div></div><!-- Modificar el modal para usar UpdatePanel específico --><div class="modal fade" id="modalAgregarUsuario" tabindex="-1" aria-labelledby="modalAgregarUsuarioLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-lg modal-dialog-scrollable">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalAgregarUsuarioLabel">Agregar Nuevo Usuario</h5><button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" onclick="confirmarCerrarModal()"></button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="updModalAgregar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row g-3">
                            <!-- Fila para Nombres y Apellido Paterno -->
                            <div class="col-md-6">
                                <label for="<%= txtNombresNuevo.ClientID %>" class="form-label">Nombres</label> <asp:TextBox ID="txtNombresNuevo" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvNombres" runat="server" ErrorMessage="Los nombres son requeridos."
                                    ControlToValidate="txtNombresNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label for="<%= txtApellidoPaternoNuevo.ClientID %>" class="form-label">Apellido Paterno</label> <asp:TextBox ID="txtApellidoPaternoNuevo" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvApellidoPaterno" runat="server" ErrorMessage="El apellido paterno es requerido."
                                    ControlToValidate="txtApellidoPaternoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <!-- Fila para Apellido Materno y Tipo de Documento -->
                            <div class="col-md-6">
                                <label for="<%= txtApellidoMaternoNuevo.ClientID %>" class="form-label">Apellido Materno</label> <asp:TextBox ID="txtApellidoMaternoNuevo" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvApellidoMaterno" runat="server" ErrorMessage="El apellido materno es requerido."
                                    ControlToValidate="txtApellidoMaternoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-md-3">
                                <label for="<%= ddlTipoDocumentoNuevo.ClientID %>" class="form-label">Tipo Doc.</label> <asp:DropDownList ID="ddlTipoDocumentoNuevo" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="DNI">DNI</asp:ListItem><asp:ListItem Value="CE">CE</asp:ListItem></asp:DropDownList></div><div class="col-md-3">
                                <label for="<%= txtNumDocumentoNuevo.ClientID %>" class="form-label">N° Documento</label> <asp:TextBox ID="txtNumDocumentoNuevo" runat="server" CssClass="form-control" MaxLength="12" />
                                <asp:RequiredFieldValidator ID="rfvNumDocumento" runat="server" ErrorMessage="El número de documento es requerido."
                                    ControlToValidate="txtNumDocumentoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <!-- Fila para Correo y Celular -->
                            <div class="col-md-6">
                                <label for="<%= txtCorreoNuevo.ClientID %>" class="form-label">Correo Electrónico</label> <asp:TextBox ID="txtCorreoNuevo" runat="server" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ErrorMessage="El correo es requerido."
                                    ControlToValidate="txtCorreoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label for="<%= txtCelularNuevo.ClientID %>" class="form-label">Celular</label> <asp:TextBox ID="txtCelularNuevo" runat="server" CssClass="form-control" MaxLength="9" />
                                <asp:RequiredFieldValidator ID="rfvCelular" runat="server" ErrorMessage="El celular es requerido."
                                    ControlToValidate="txtCelularNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>

                            <!-- Fila para Fecha de Nacimiento y Género -->
                            <div class="col-md-6">
                                <label for="<%= txtFechaNacimientoNuevo.ClientID %>" class="form-label">Fecha de Nacimiento</label> <asp:TextBox ID="txtFechaNacimientoNuevo" runat="server" CssClass="form-control" TextMode="Date" />
                                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ErrorMessage="La fecha de nacimiento es requerida."
                                    ControlToValidate="txtFechaNacimientoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-md-6">
                                <label for="<%= ddlGeneroNuevo.ClientID %>" class="form-label">Género</label> <asp:DropDownList ID="ddlGeneroNuevo" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="" Text="-- Seleccione --" />
                                    <asp:ListItem Value="MASCULINO">Masculino</asp:ListItem><asp:ListItem Value="FEMENINO">Femenino</asp:ListItem><asp:ListItem Value="OTRO">Otro</asp:ListItem></asp:DropDownList><asp:RequiredFieldValidator InitialValue="" ID="rfvGenero" runat="server" ErrorMessage="El género es requerido."
                                    ControlToValidate="ddlGeneroNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            
                            <hr class="my-3"/>
                            
                            <!-- Fila para Selección de Rol -->
                            <div class="col-12">
                                <label for="<%= ddlRolNuevo.ClientID %>" class="form-label fw-bold">Rol del Nuevo Usuario</label> <asp:DropDownList ID="ddlRolNuevo" runat="server" CssClass="form-select" 
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRolNuevo_SelectedIndexChanged" />
                                <asp:RequiredFieldValidator InitialValue="0" ID="rfvRolNuevo" runat="server" ErrorMessage="Debe seleccionar un rol."
                                    ControlToValidate="ddlRolNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            
                            <!-- Panel Dinámico para campos de Médico -->
                            <asp:Panel ID="pnlCamposMedico" runat="server" Visible="false" CssClass="row g-3 mt-2">
                                <div class="col-md-6">
                                    <label for="<%= txtCodMedicoNuevo.ClientID %>" class="form-label">Código de Médico (CMP)</label> <asp:TextBox ID="txtCodMedicoNuevo" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvCodMedico" runat="server" Enabled="false" ErrorMessage="El código de médico es requerido."
                                        ControlToValidate="txtCodMedicoNuevo" ValidationGroup="NuevoUsuario" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Especialidades</label> <div class="border rounded p-2" style="max-height: 150px; overflow-y: auto;">
                                        <asp:CheckBoxList ID="chkEspecialidadesNuevo" runat="server" CssClass="w-100" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>

                        <!-- Resumen de Validación -->
                        <asp:ValidationSummary ID="vsNuevoUsuario" runat="server" ValidationGroup="NuevoUsuario"
                            CssClass="alert alert-danger mt-3" HeaderText="Por favor, corrija los siguientes errores:" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRolNuevo" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" onclick="limpiarFormularioAlCerrar()">Cancelar</button><asp:Button ID="btnGuardarNuevoUsuario" runat="server" Text="Guardar Usuario" 
                    OnClick="btnGuardarNuevoUsuario_Click" CssClass="btn btn-primary" ValidationGroup="NuevoUsuario" />
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    
    var formularioModificado = false;

    function confirmarCerrarModal() {
        if (formularioModificado) {
            if (confirm('¿Está seguro de que desea cerrar? Se perderán los datos ingresados.')) {
                limpiarFormularioAlCerrar();
                $('#modalAgregarUsuario').modal('hide');
            }
        } else {
            $('#modalAgregarUsuario').modal('hide');
        }
    }

    function limpiarFormularioAlCerrar() {
        $('#<%= txtNombresNuevo.ClientID %>').val('');
        $('#<%= txtApellidoPaternoNuevo.ClientID %>').val('');
        $('#<%= txtApellidoMaternoNuevo.ClientID %>').val('');
        $('#<%= txtNumDocumentoNuevo.ClientID %>').val('');
        $('#<%= txtCorreoNuevo.ClientID %>').val('');
        $('#<%= txtCelularNuevo.ClientID %>').val('');
        $('#<%= txtFechaNacimientoNuevo.ClientID %>').val('');
        $('#<%= ddlGeneroNuevo.ClientID %>').val('');
        $('#<%= ddlRolNuevo.ClientID %>').val('0');
        $('#<%= txtCodMedicoNuevo.ClientID %>').val('');

        $('#<%= chkEspecialidadesNuevo.ClientID %> input[type="checkbox"]').prop('checked', false);

        $('#<%= pnlCamposMedico.ClientID %>').hide();

        $('.text-danger').hide();
        $('.alert-danger').hide();

        formularioModificado = false;
    }

    $(document).ready(function () {
        $('#modalAgregarUsuario input, #modalAgregarUsuario select, #modalAgregarUsuario textarea').on('change keyup', function () {
            formularioModificado = true;
        });

        $('#modalAgregarUsuario').on('show.bs.modal', function () {
            formularioModificado = false;
        });

        $('#modalAgregarUsuario').on('hidden.bs.modal', function () {
            limpiarFormularioAlCerrar();
        });
    });

    function forzarCierreModal() {
        formularioModificado = false;
        $('#modalAgregarUsuario').modal('hide');
        $('.modal-backdrop').remove();
        $('body').removeClass('modal-open');
    }

    document.addEventListener('DOMContentLoaded', function () {
        var modalAgregar = document.getElementById('modalAgregarUsuario');
        if (modalAgregar) {
            modalAgregar.addEventListener('shown.bs.modal', function () {
                inicializarModal();
            });
        }
    });
</script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>