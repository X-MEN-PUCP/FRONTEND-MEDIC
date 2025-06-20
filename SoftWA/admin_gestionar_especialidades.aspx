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
        <asp:UpdatePanel ID="updFiltrosEspecialidades" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros y Ordenamiento</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-5">
                                <label for="<%=txtFiltrarNombre.ClientID%>" class="form-label">Filtrar por Nombre:</label>
                                <asp:TextBox ID="txtFiltrarNombre" runat="server" CssClass="form-control"
                                    AutoPostBack="false"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=ddlOrdenarPor.ClientID%>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarPor" runat="server" CssClass="form-select"
                                    AutoPostBack="false">
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updEspecialidadesRepeater" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoEspecialidad" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No existen especialidades registradas que coincidan con los filtros.
                    </div>
                </asp:PlaceHolder>

                <asp:Repeater ID="rptEspecialidades" runat="server" OnItemCommand="rptEspecialidades_ItemCommand">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm cita-card">
                                <div class="card-body">
                                    <h5 class="card-title"><i class="fa-solid fa-layer-group me-2"></i><%# Eval("NombreEspecialidad") %></h5>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-users me-2 text-primary"></i>
                                        <strong>N° Médicos:</strong> <%# Eval("CantMedicos") %>
                                    </p>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-coins me-2 text-warning"></i>
                                        <strong>Precio Consulta:</strong> S/. <%# Eval("PrecioConsulta", "{0:N2}") %>
                                    </p>
                                </div>
                                <div class="card-footer bg-light text-end">
                                    <asp:LinkButton ID="btnEditarEspecialidad" runat="server"
                                        CssClass="btn btn-link p-1 me-2"
                                        CommandName="EditEspecialidad"
                                        CommandArgument='<%# Eval("ID") %>' ToolTip="Editar Especialidad">
                                        <i class="fa-solid fa-pen" style="color: black; font-size: 1.2em;"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarEspecialidad" runat="server"
                                        CssClass="btn btn-link p-1"
                                        CommandName="EliminarEspecialidad"
                                        CommandArgument='<%# Eval("ID") %>' ToolTip="Eliminar Especialidad"
                                        OnClientClick="return confirm('¿Está seguro de que desea eliminar esta especialidad? Los médicos asociados podrían quedar sin especialidad asignada.');">
                                        <i class="fa-solid fa-trash" style="color: red; font-size: 1.2em;"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                            <div class="col">
                                <asp:LinkButton ID="lnkAgregarNuevaEspecialidadCard" runat="server"
                                    CssClass="card h-100 shadow-sm cita-card-new text-decoration-none"
                                    OnClick="lnkAgregarNuevaEspecialidad_Click" 
                                    ToolTip="Agregar Nueva Especialidad">
                                    <div class="card-body d-flex flex-column justify-content-center align-items-center text-center">
                                        <i class="fa-solid fa-plus fa-3x text-secondary mb-2"></i>
                                        <h5 class="card-title text-secondary">Agregar Nueva</h5>
                                    </div>
                                </asp:LinkButton>
                            </div>
                        </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
                <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltrosEsp" EventName="Click" />
                     <asp:AsyncPostBackTrigger ControlID="btnLimpiarFiltrosEsp" EventName="Click" />
                </Triggers>
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
    </style>
</asp:Content>