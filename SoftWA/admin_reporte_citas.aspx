<%@ Page Title="Reporte de Citas Atendidas" Language="C#" MasterPageFile="~/SoftMA_Admin.Master" AutoEventWireup="true" CodeBehind="admin_reporte_citas.aspx.cs" Inherits="SoftWA.admin_reporte_citas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Reporte de Citas Atendidas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <asp:UpdatePanel ID="updReporteCitas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Filtros del Reporte</h5>
                </div>
                <div class="card-body">
                    <div class="row g-3 align-items-end">
                        <div class="col-md-3">
                            <label for="<%= txtFechaDesdeReporte.ClientID %>" class="form-label">Desde</label>
                            <asp:TextBox ID="txtFechaDesdeReporte" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        <div class="col-md-3">
                            <label for="<%= txtFechaHastaReporte.ClientID %>" class="form-label">Hasta</label>
                            <asp:TextBox ID="txtFechaHastaReporte" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        <div class="col-md-3">
                            <label for="<%= ddlEspecialidadReporte.ClientID %>" class="form-label">Especialidad</label>
                            <asp:DropDownList ID="ddlEspecialidadReporte" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidadReporte_SelectedIndexChanged" />
                        </div>
                        <div class="col-md-3">
                            <label for="<%= ddlDoctorReporte.ClientID %>" class="form-label">Doctor</label>
                            <asp:DropDownList ID="ddlDoctorReporte" runat="server" CssClass="form-select" />
                        </div>
                        <div class="col-md-3">
                            <label for="<%= ddlOrdenarReporte.ClientID %>" class="form-label">Ordenar por</label>
                            <asp:DropDownList ID="ddlOrdenarReporte" runat="server" CssClass="form-select">
                                <asp:ListItem Value="FechaDesc" Text="Fecha (Más recientes primero)"></asp:ListItem>
                                <asp:ListItem Value="FechaAsc" Text="Fecha (Más antiguos primero)"></asp:ListItem>
                                <asp:ListItem Value="PacienteAsc" Text="Paciente (A-Z)"></asp:ListItem>
                                <asp:ListItem Value="PacienteDesc" Text="Paciente (Z-A)"></asp:ListItem>
                                <asp:ListItem Value="DoctorAsc" Text="Doctor (A-Z)"></asp:ListItem>
                                <asp:ListItem Value="DoctorDesc" Text="Doctor (Z-A)"></asp:ListItem>
                                <asp:ListItem Value="EspecialidadAsc" Text="Especialidad (A-Z)"></asp:ListItem>
                                <asp:ListItem Value="EspecialidadDesc" Text="Especialidad (Z-A)"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnAplicarFiltrosReporte" runat="server" Text="Generar Reporte" CssClass="btn btn-primary w-100" OnClick="btnAplicarFiltrosReporte_Click" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnLimpiarFiltrosReporte" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary w-100" OnClick="btnLimpiarFiltrosReporte_Click" CausesValidation="false" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnExportarCSV" runat="server" Text="Exportar a CSV" CssClass="btn btn-success w-100" OnClick="btnExportarCSV_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card mt-4">
                <div class="card-header">
                    <h5 class="mb-0">Resultados</h5>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="d-flex align-items-center">
                                <i class="fa-solid fa-star text-warning me-2 fs-4"></i>
                                <div>
                                    <strong>Especialidad más solicitada:</strong><br />
                                    <asp:Label ID="lblMasSolicitadaEspecialidad" runat="server" Text="N/A"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                             <div class="d-flex align-items-center">
                                 <i class="fa-solid fa-user-doctor text-info me-2 fs-4"></i>
                                 <div>
                                     <strong>Doctor con más citas:</strong><br />
                                    <asp:Label ID="lblMasSolicitadoDoctor" runat="server" Text="N/A"></asp:Label>
                                 </div>
                             </div>
                        </div>
                    </div>
                    
                    <asp:ListView ID="lvCitas" runat="server">
                        <LayoutTemplate>
                            <div class="table-responsive">
                                <table class="table table-striped table-hover">
                                    <thead class="table-light">
                                        <tr>
                                            <th>ID Cita</th>
                                            <th>Paciente</th>
                                            <th>Especialidad</th>
                                            <th>Doctor (CMP)</th>
                                            <th>Fecha</th>
                                            <th>Hora</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </tbody>
                                </table>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("idCita") %></td>
                                <td><%# Eval("paciente") %></td>
                                <td><%# Eval("especialidad") %></td>
                                <td><%# Eval("doctor") %> (<%# Eval("codMedico") %>)</td>
                                <td><%# Convert.ToDateTime(Eval("fechaCita")).ToString("dd/MM/yyyy") %></td>
                                <td><%# Eval("hora") %></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-info text-center">
                                <asp:PlaceHolder ID="phNoReporte" runat="server">
                                    <i class="fa-solid fa-magnifying-glass me-2"></i> No se encontraron citas atendidas para los filtros seleccionados.
                                </asp:PlaceHolder>
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarCSV" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>