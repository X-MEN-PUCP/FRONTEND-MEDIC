<%@ Page Title="Gestionar Especialidades" Language="C#" MasterPageFile="~/SoftMA_Admin.Master" AutoEventWireup="true" CodeBehind="admin_gestionar_especialidades.aspx.cs" Inherits="SoftWA.admin_gestionar_especialidades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Gestionar Especialidades
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card-header mb-3">
            <h2>Administración de Especialidades</h2>
            <p>Agregar, editar o eliminar especialidades del sistema.</p>
        </div>
        
        <asp:UpdatePanel ID="updGestionEspecialidades" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%-- panel de agregar --%>
                <div class="d-flex justify-content-end mb-3">
                    <asp:Button ID="btnShowAddPanel" runat="server" Text="Agregar Especialidad" CssClass="btn btn-primary btn-add-especialidad" OnClick="btnShowAddPanel_Click" />
                </div>

                <%-- Agregar/Editar Especialidad --%>
                <asp:Panel ID="pnlAddEditEspecialidad" runat="server" CssClass="card shadow-sm mb-4" Visible="false">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-layer-group me-2"></i><asp:Label ID="lblFormTitle" runat="server">Agregar Nueva Especialidad</asp:Label></h5>
                    </div>
                    <div class="card-body">
                        <asp:HiddenField ID="hfEspecialidadId" runat="server" Value="0" />
                        <div class="row g-3">
                            <div class="col-md-8">
                                <label for="<%= txtNombreAddEdit.ClientID %>" class="form-label">Nombre de la Especialidad</label>
                                <asp:TextBox ID="txtNombreAddEdit" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombreAddEdit" ErrorMessage="El nombre es obligatorio." Display="Dynamic" CssClass="text-danger" ValidationGroup="AddEditEspecialidad" />
                            </div>
                            <div class="col-md-4">
                                <label for="<%= txtPrecioAddEdit.ClientID %>" class="form-label">Precio de Consulta (S/)</label>
                                <asp:TextBox ID="txtPrecioAddEdit" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvPrecio" runat="server" ControlToValidate="txtPrecioAddEdit" ErrorMessage="El precio es obligatorio." Display="Dynamic" CssClass="text-danger" ValidationGroup="AddEditEspecialidad" />
                                <asp:CompareValidator ID="cvPrecio" runat="server" ControlToValidate="txtPrecioAddEdit" Operator="DataTypeCheck" Type="Currency" ErrorMessage="Ingrese un monto válido." Display="Dynamic" CssClass="text-danger" ValidationGroup="AddEditEspecialidad" />
                            </div>
                        </div>
                        <div class="mt-4 d-flex justify-content-end">
                            <asp:Button ID="btnGuardarEspecialidad" runat="server" Text="Guardar" CssClass="btn btn-primary me-2" OnClick="btnGuardarEspecialidad_Click" ValidationGroup="AddEditEspecialidad" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" CausesValidation="false" />
                        </div>
                        <asp:ValidationSummary ID="vsAddEditEspecialidad" runat="server" ValidationGroup="AddEditEspecialidad" CssClass="alert alert-danger mt-3" HeaderText="Por favor, corrija los siguientes errores:" />
                    </div>
                </asp:Panel>

                <%-- Filtros y Ordenamiento --%>
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros y Ordenamiento</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-5">
                                <label for="<%=txtFiltrarNombre.ClientID%>" class="form-label">Filtrar por Nombre:</label>
                                <asp:TextBox ID="txtFiltrarNombre" runat="server" CssClass="form-control" AutoPostBack="false"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=ddlOrdenarPor.ClientID%>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarPor" runat="server" CssClass="form-select" AutoPostBack="false">
                                    <asp:ListItem Text="Nombre (A-Z)" Value="NombreAsc"></asp:ListItem>
                                    <asp:ListItem Text="Nombre (Z-A)" Value="NombreDesc"></asp:ListItem>
                                    <asp:ListItem Text="Precio (Menor a Mayor)" Value="PrecioAsc"></asp:ListItem>
                                    <asp:ListItem Text="Precio (Mayor a Menor)" Value="PrecioDesc"></asp:ListItem>
                                    <asp:ListItem Text="N° Médicos (Menor a Mayor)" Value="MedicosAsc" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="N° Médicos (Mayor a Menor)" Value="MedicosDesc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 text-md-end">
                                <asp:Button ID="btnLimpiarFiltrosEsp" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary me-2" OnClick="btnLimpiarFiltrosEsp_Click" CausesValidation="false" />
                                <asp:Button ID="btnAplicarFiltrosEsp" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnAplicarFiltrosEsp_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <%-- Contenedor de Especialidades en Cards --%>
                <asp:PlaceHolder ID="phNoEspecialidad" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No existen especialidades registradas que coincidan con los filtros.
                    </div>
                </asp:PlaceHolder>

                <asp:Repeater ID="rptEspecialidades" runat="server" OnItemCommand="rptEspecialidades_ItemCommand" OnItemDataBound="rptEspecialidades_ItemDataBound">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm cita-card">
                                <div class="card-body">
                                    <div class="d-flex justify-content-between align-items-start mb-2">
                                        <h5 class="card-title"><i class="fa-solid fa-layer-group me-2"></i><%# Eval("NombreEspecialidad") %></h5>
                                        <asp:Literal ID="ltlEstado" runat="server"></asp:Literal>
                                    </div>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-users me-2 text-primary"></i>
                                        <strong>N° Médicos:</strong> <%# Eval("CantMedicos") %>
                                    </p>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-coins me-2 text-warning"></i>
                                        <strong>Precio Consulta:</strong> S/. <%# Eval("PrecioConsulta", "{0:N2}") %>
                                    </p>
                                    <small class="text-muted">ID: <%# Eval("ID") %></small>
                                </div>
                                <div class="card-footer bg-light text-end">
                                    <asp:LinkButton ID="btnEditEspecialidad" runat="server"
                                        CssClass="btn btn-link p-1 me-2"
                                        CommandName="EditEspecialidad"
                                        CommandArgument='<%# Eval("ID") %>' ToolTip="Editar Especialidad">
                                        <i class="fa-solid fa-pen" style="color: #007bff; font-size: 1.2em;"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnToggleStatus" runat="server"
                                        CssClass="btn btn-link p-1"
                                        CommandName="ToggleStatus"
                                        CommandArgument='<%# Eval("ID") %>' 
                                        OnClientClick="return confirm('¿Está seguro de que desea cambiar el estado de esta especialidad?');"
                                        ToolTip="Activar/Desactivar">
                                        <i class="fa-solid fa-power-off" style="font-size: 1.2em;"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <style>
        .cita-card {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            border-left: 5px solid #5bd3c5;
        }
        .cita-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .card-title i {
            color: #5bd3c5;
        }

        .cita-card-new {
            border: 2px dashed #adb5bd;
            background-color: #f8f9fa;
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out, background-color 0.2s ease-in-out;
            cursor: pointer;
        }
        .cita-card-new:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.10)!important;
            background-color: #e9ecef;
            border-color: #6c757d;
        }
        .cita-card-new .fa-plus {
            transition: color 0.2s ease-in-out;
        }
        .cita-card-new:hover .fa-plus,
        .cita-card-new:hover .card-title {
            color: #0d6efd !important; 
        }

        /* Estilo para el botón de agregar con icono */
        .btn-add-especialidad::before {
            content: "\f067"; /* Código del icono fa-plus */
            font-family: "Font Awesome 6 Free";
            font-weight: 900;
            margin-right: 8px;
        }

        /* Estilos para los badges de estado */
        .badge {
            font-size: 0.75em;
        }

        /* Estilos para los iconos de acción */
        .btn-link:hover i {
            transform: scale(1.1);
            transition: transform 0.2s ease-in-out;
        }

        /* Estilos adicionales para el formulario */
        .card-header h5 i {
            color: #5bd3c5;
        }
    </style>
</asp:Content>