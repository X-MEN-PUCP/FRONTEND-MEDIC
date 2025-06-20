<%@ Page Title="Reporte de Citas" Language="C#" MasterPageFile="~/SoftMA_Admin.Master" AutoEventWireup="true" CodeBehind="admin_reporte_citas.aspx.cs" Inherits="SoftWA.admin_reporte_citas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Reporte de Citas Atendidas
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="updReporteCitas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container mt-4">
                <div class="row mb-3 align-items-center">
                    <div class="col">
                        <h2><i class="fa-solid fa-calendar-check me-2"></i>Reporte de Citas Atendidas</h2>
                        <p class="text-muted">Visualización de citas atendidas y estadísticas.</p>
                    </div>
                </div>
                <hr />
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros y Ordenamiento del Reporte</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-3">
                                <label for="<%=txtFechaDesdeReporte.ClientID%>" class="form-label">Fecha Desde:</label>
                                <asp:TextBox ID="txtFechaDesdeReporte" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=txtFechaHastaReporte.ClientID%>" class="form-label">Fecha Hasta:</label>
                                <asp:TextBox ID="txtFechaHastaReporte" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlEspecialidadReporte.ClientID%>" class="form-label">Especialidad:</label>
                                <asp:DropDownList ID="ddlEspecialidadReporte" runat="server" CssClass="form-select" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidadReporte_SelectedIndexChanged">
                                    <asp:ListItem Text="-- Todas --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlDoctorReporte.ClientID%>" class="form-label">Doctor:</label>
                                <asp:DropDownList ID="ddlDoctorReporte" runat="server" CssClass="form-select" AppendDataBoundItems="true" Enabled="false">
                                    <asp:ListItem Text="-- Todos --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=ddlOrdenarReporte.ClientID %>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarReporte" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Fecha Cita (Más Recientes)" Value="FechaDesc"></asp:ListItem>
                                    <asp:ListItem Text="Fecha Cita (Más Antiguas)" Value="FechaAsc"></asp:ListItem>
                                    <asp:ListItem Text="Paciente (A-Z)" Value="PacienteAsc"></asp:ListItem>
                                    <asp:ListItem Text="Paciente (Z-A)" Value="PacienteDesc"></asp:ListItem>
                                    <asp:ListItem Text="Doctor (A-Z)" Value="DoctorAsc"></asp:ListItem>
                                    <asp:ListItem Text="Doctor (Z-A)" Value="DoctorDesc"></asp:ListItem>
                                    <asp:ListItem Text="Especialidad (A-Z)" Value="EspecialidadAsc"></asp:ListItem>
                                    <asp:ListItem Text="Especialidad (Z-A)" Value="EspecialidadDesc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-auto mt-md-3">
                                <asp:Button ID="btnAplicarFiltrosReporte" runat="server" Text="Aplicar" CssClass="btn btn-primary w-100" OnClick="btnAplicarFiltrosReporte_Click" />
                            </div>
                            <div class="col-md-auto mt-md-3">
                                <asp:Button ID="btnLimpiarFiltrosReporte" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiarFiltrosReporte_Click" CausesValidation="false"/>
                            </div>
                        </div>
                    </div>
                </div>
                <h4><i class="fa-solid fa-list-ul me-2"></i>Detalle de Citas</h4>
                <asp:ListView ID="lvCitas" runat="server" ItemPlaceholderID="itemPlaceholder">
                    <LayoutTemplate>
                         <div class="table-responsive mb-4 shadow-sm">
                            <table class="table table-hover table-sm align-middle">
                                <thead class="table-light">
                                    <tr>
                                        <th>ID Cita</th>
                                        <th>Paciente</th>
                                        <th>Especialidad</th>
                                        <th>CMP</th>
                                        <th>Doctor</th>
                                        <th>Fecha</th>
                                        <th>Hora</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("IdCita") %></td>
                            <td><%# Eval("NombrePaciente") %></td>
                            <td><%# Eval("Especialidad") %></td>
                            <td><%# Eval("CmpDoctor") %></td>
                            <td><%# Eval("NombreDoctor") %></td>
                            <td><%# Eval("FechaCita", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("Horario") %></td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                         <div class="alert alert-info text-center">
                            No hay datos de citas atendidas que coincidan con los filtros aplicados.
                         </div>
                    </EmptyDataTemplate>
                </asp:ListView>

                <hr />
                <h4><i class="fa-solid fa-calculator me-2"></i>Estadísticas Resumen (Basadas en Filtros Aplicados)</h4>
                <div class="card shadow-sm mb-4">
                    <div class="card-body">
                         <div class="row">
                             <div class="col-md-6 mb-2 mb-md-0">
                                 <strong>Especialidad más solicitada:</strong>
                                 <asp:Label ID="lblMasSolicitadaEspecialidad" runat="server" Text="N/A" CssClass="ms-2 fw-bold"></asp:Label>
                             </div>
                             <div class="col-md-6">
                                 <strong>Doctor más solicitado:</strong>
                                 <asp:Label ID="lblMasSolicitadoDoctor" runat="server" Text="N/A" CssClass="ms-2 fw-bold"></asp:Label>
                             </div>
                         </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltrosReporte" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiarFiltrosReporte" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlEspecialidadReporte" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>