<%@ Page Title="" Language="C#" MasterPageFile="~/SoftMA_Admin.Master" AutoEventWireup="true" CodeBehind="admin_gestionar_doctores.aspx.cs" Inherits="SoftWA.admin_gestionar_doctores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Gestión de Doctores
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="updGestionDoctores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container mt-4">
                <div class="row mb-3 align-items-center">
                    <div class="col-md-9">
                        <h2><i class="fa-solid fa-user-doctor me-2"></i>Administración de Doctores</h2>
                        <p class="text-muted">Agregar, editar o eliminar doctores del sistema.</p>
                    </div>
                    <div class="col-md-3 text-md-end">
                        <asp:Button ID="btnShowAddPanel" runat="server" Text="Agregar Doctor"
                            CssClass="btn btn-primary" OnClick="btnShowAddPanel_Click" />
                    </div>
                </div>
                <hr/>
                <asp:Panel ID="pnlAddEditDoctor" runat="server" Visible="false" CssClass="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0">
                            <asp:Label ID="lblFormTitle" runat="server" Text="Agregar Nuevo Doctor"></asp:Label>
                        </h5>
                    </div>
                    <div class="card-body">
                        <asp:HiddenField ID="hfDoctorId" runat="server" Value="0" />
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="<%=txtNombre.ClientID%>" class="form-label">Nombre:</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                                    ErrorMessage="El nombre es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="AddEditDoctor"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label for="<%=txtApellido.ClientID%>" class="form-label">Apellido:</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                                    ErrorMessage="El apellido es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="AddEditDoctor"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label for="<%=ddlEspecialidadAddEdit.ClientID%>" class="form-label">Especialidad:</label>
                                <asp:DropDownList ID="ddlEspecialidadAddEdit" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-- Seleccione --" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="ddlEspecialidadAddEdit"
                                     InitialValue="" ErrorMessage="Seleccione una especialidad." CssClass="text-danger small" Display="Dynamic" ValidationGroup="AddEditDoctor"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label for="<%=txtEmail.ClientID%>" class="form-label">Email:</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="100" TextMode="Email"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                     ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                     ErrorMessage="Ingrese un email válido." CssClass="text-danger small" Display="Dynamic" ValidationGroup="AddEditDoctor"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-6">
                                <label for="<%=txtTelefono.ClientID%>" class="form-label">Teléfono (Opcional):</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mt-3 text-end">
                             <asp:Button ID="btnGuardarDoctor" runat="server" Text="Guardar"
                                 CssClass="btn btn-success me-2" OnClick="btnGuardarDoctor_Click" ValidationGroup="AddEditDoctor"/>
                             <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                                 OnClick="btnCancelar_Click" CausesValidation="false"/>
                        </div>
                         <asp:ValidationSummary ID="vsAddEditDoctor" runat="server" CssClass="text-danger mt-2" HeaderText="Por favor corrija los siguientes errores:" ValidationGroup="AddEditDoctor"/>
                    </div>
                </asp:Panel>
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros y Ordenamiento</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-4">
                                <label for="<%=txtFiltroNombreApellido.ClientID %>" class="form-label">Buscar por Nombre/Apellido:</label>
                                <asp:TextBox ID="txtFiltroNombreApellido" runat="server" CssClass="form-control" placeholder="Ingrese nombre o apellido"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlFiltroEspecialidad.ClientID %>" class="form-label">Filtrar por Especialidad:</label>
                                <asp:DropDownList ID="ddlFiltroEspecialidad" runat="server" CssClass="form-select" 
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Text="-- Todas las Especialidades --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlOrdenarDoctores.ClientID %>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarDoctores" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="ID Doctor (Ascendente)" Value="IdAsc"></asp:ListItem>
                                    <asp:ListItem Text="ID Doctor (Descendente)" Value="IdDesc"></asp:ListItem>
                                    <asp:ListItem Text="Nombre Completo (A-Z)" Value="NombreAsc"></asp:ListItem>
                                    <asp:ListItem Text="Nombre Completo (Z-A)" Value="NombreDesc"></asp:ListItem>
                                    <asp:ListItem Text="Especialidad (A-Z)" Value="EspecialidadAsc"></asp:ListItem>
                                    <asp:ListItem Text="Especialidad (Z-A)" Value="EspecialidadDesc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 text-md-end align-self-end">
                                <asp:Button ID="btnAplicarFiltros" runat="server" Text="Aplicar" CssClass="btn btn-primary w-100" OnClick="btnAplicarFiltros_Click" />
                            </div>
                             <div class="col-12 text-md-start">
                                <asp:LinkButton ID="lnkLimpiarFiltros" runat="server" OnClick="lnkLimpiarFiltros_Click" CssClass="btn btn-link px-0" CausesValidation="false">Limpiar todos los filtros</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:ListView ID="lvDoctores" runat="server"
                    ItemPlaceholderID="itemPlaceholder"
                    OnItemCommand="lvDoctores_ItemCommand"
                    OnItemDeleting="lvDoctores_ItemDeleting"
                    DataKeyNames="IdDoctor"> 
                    <LayoutTemplate>
                         <div class="table-responsive">
                            <table class="table table-hover table-sm align-middle">
                                <thead class="table-light">
                                    <tr>
                                        <th>ID</th>
                                        <th>Nombre Completo</th>
                                        <th>Especialidad</th>
                                        <th>Email</th>
                                        <th>Teléfono</th>
                                        <th class="text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                         <asp:PlaceHolder ID="phNoDoctores" runat="server" Visible="false">
                             <div class="alert alert-info text-center">
                                No se encontraron doctores que coincidan con los criterios de búsqueda.
                             </div>
                         </asp:PlaceHolder>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("IdDoctor") %></td>
                            <td><%# Eval("NombreCompleto") %></td> 
                            <td><%# Eval("NombreEspecialidad") %></td>
                            <td><%# Eval("Email") %></td>
                            <td><%# Eval("Telefono") %></td>
                            <td class="text-center">
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("IdDoctor") %>'
                                    CssClass="btn btn-sm btn-outline-primary me-1" ToolTip="Editar">
                                    <i class="fa-solid fa-pencil"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("IdDoctor") %>'
                                    CssClass="btn btn-sm btn-outline-danger" ToolTip="Eliminar"
                                    OnClientClick='<%# Eval("IdDoctor", "return confirm(\"¿Está seguro de que desea eliminar al doctor con ID {0}?\");") %>'>
                                    <i class="fa-solid fa-trash-can"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                         <div class="alert alert-info text-center">
                            No hay doctores registrados en el sistema. Haga clic en "Agregar Doctor" para comenzar.
                         </div>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltros" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkLimpiarFiltros" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>