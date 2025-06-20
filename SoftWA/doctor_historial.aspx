<%@ Page Title="Historial de Citas" Language="C#" MasterPageFile="~/SoftMA_Doctor.Master" AutoEventWireup="true" CodeBehind="doctor_historial.aspx.cs" Inherits="SoftWA.doctor_historial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Historial de Citas Atendidas
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row mb-3 align-items-center">
            <div class="col-md-6">
                <h2><i class="fa-solid fa-book-medical me-2"></i>Historial de Citas Atendidas</h2>
                <p class="text-muted">Listado de sus citas previamente atendidas.</p>
            </div>
            <div class="col-md-6 text-md-end">
                 <asp:Label ID="lblDoctorInfo" runat="server" CssClass="badge bg-info text-dark p-2 fs-6" ToolTip="Doctor actualmente viendo el historial"></asp:Label>
            </div>
        </div>
        <hr />
        <asp:UpdatePanel ID="updFiltrosHistorialDoctor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros y Orden</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-4">
                                <label for="<%=txtFechaDesdeHist.ClientID%>" class="form-label">Desde:</label>
                                <asp:TextBox ID="txtFechaDesdeHist" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=txtFechaHastaHist.ClientID%>" class="form-label">Hasta:</label>
                                <asp:TextBox ID="txtFechaHastaHist" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=ddlOrdenarPorHist.ClientID%>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarPorHist" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Fecha (Más Recientes Primero)" Value="FechaDesc"></asp:ListItem>
                                    <asp:ListItem Text="Fecha (Más Antiguas Primero)" Value="FechaAsc"></asp:ListItem>
                                    <asp:ListItem Text="Nombre Paciente (A-Z)" Value="NombrePacienteAsc"></asp:ListItem>
                                    <asp:ListItem Text="Nombre Paciente (Z-A)" Value="NombrePacienteDesc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 text-md-end mt-3">
                                <asp:Button ID="btnLimpiarFiltrosHistDoc" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary me-2" OnClick="btnLimpiarFiltrosHistDoc_Click" CausesValidation="false"/>
                                <asp:Button ID="btnAplicarFiltrosHistDoc" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnAplicarFiltrosHistDoc_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updHistorialRepeater" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoHistorial" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No tiene historial de atención que coincida con los filtros.
                    </div>
                </asp:PlaceHolder>

                <asp:Repeater ID="rptHistDoctor" runat="server" OnItemCommand="rptHistDoctor_ItemCommand">
                    <HeaderTemplate>
                        <ul class="list-group shadow-sm">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="list-group-item list-group-item-action agenda-item">
                            <div class="d-flex w-100 justify-content-between">
                                <h5 class="mb-1">
                                   <i class="fa-solid fa-user-injured me-2 text-primary"></i> <%# Eval("NombrePaciente") %>
                                </h5>
                                <p class="text-muted"><%# Eval("FechaCita", "{0:dd MMM yyyy}") %></p>
                            </div>
                            <p class="mb-1">
                                <i class="fa-solid fa-clock me-2 text-success"></i>
                                <strong>Horario: </strong><%# Eval("DescripcionHorario") %>
                                <br />
                                <i class="fa-solid fa-stethoscope me-2 text-info"></i>
                                <strong>Especialidad: </strong><%# Eval("NombreEspecialidad") %>
                            </p>
                             <div class="mt-2 text-end">
                                 <asp:LinkButton ID="btnVerResultados" runat="server" CommandName="VerResultados"
                                                CommandArgument='<%# Eval("IdCita") %>' CssClass="btn btn-sm btn-outline-primary"
                                                 ToolTip="Ver/Registrar Resultados (Próximamente)">
                                                <i class="fa-solid fa-file-medical me-1"></i>Resultados
                                </asp:LinkButton>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                         <asp:Literal ID="ltlNoResultadosMsg" runat="server" Visible="false">
                            <div class="alert alert-light text-center mt-3">No se encontraron citas en el historial con los criterios seleccionados.</div>
                        </asp:Literal>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltrosHistDoc" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnLimpiarFiltrosHistDoc" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <style>
        .agenda-item {
            transition: background-color 0.2s ease-in-out;
            border-left: 5px solid #6c757d;
            margin-bottom: 0.5rem; 
        }
        .agenda-item:hover {
            background-color: #f8f9fa; 
        }
    </style>
</asp:Content>