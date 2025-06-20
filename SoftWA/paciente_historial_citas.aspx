<%@ Page Title="Historial de Citas" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" CodeBehind="paciente_historial_citas.aspx.cs" Inherits="SoftWA.paciente_historial_citas" %>
<%@ Import Namespace="System.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Historial de Citas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h2><i class="fa-solid fa-book-medical me-2"></i>Historial Clínico</h2>
                <hr />
            </div>
        </div>

        <asp:UpdatePanel ID="updFiltrosHistorial" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa-solid fa-filter me-2"></i>Filtros de Búsqueda</h5>
                    </div>
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-3">
                                <label for="<%=txtFechaDesde.ClientID%>" class="form-label">Desde:</label>
                                <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=txtFechaHasta.ClientID%>" class="form-label">Hasta:</label>
                                <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlEspecialidadHistorial.ClientID%>" class="form-label">Especialidad:</label>
                                <asp:DropDownList ID="ddlEspecialidadHistorial" runat="server" CssClass="form-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidadHistorial_SelectedIndexChanged" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-- Todas --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label for="<%=ddlMedicoHistorial.ClientID%>" class="form-label">Médico:</label>
                                <asp:DropDownList ID="ddlMedicoHistorial" runat="server" CssClass="form-select" AppendDataBoundItems="true" Enabled="false">
                                    <asp:ListItem Text="-- Todos --" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-12 text-md-end mt-3">
                                <asp:Button ID="btnLimpiarFiltrosHistorial" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary me-2" OnClick="btnLimpiarFiltrosHistorial_Click" CausesValidation="false"/>
                                <asp:Button ID="btnAplicarFiltrosHistorial" runat="server" Text="Aplicar Filtros" CssClass="btn btn-primary" OnClick="btnAplicarFiltrosHistorial_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updHistorialCitas" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoHistorial" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No cuenta con historial clínico que coincida con los filtros aplicados.
                    </div>
                </asp:PlaceHolder>

                <asp:Repeater ID="rptHistorial" runat="server">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4" id="historialCardContainer">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm cita-card-historial" style="cursor: pointer;"
                                 data-especialidad="<%# Eval("NombreEspecialidad") %>"
                                 data-medico="<%# Eval("NombreMedico") %>"
                                 data-fecha='<%# Eval("FechaCita", "{0:dddd, dd 'de' MMMM 'de' yyyy}") %>'
                                 data-horario='<%# Eval("DescripcionHorario") %>'
                                 data-estado="<%# Eval("Estado") %>"
                                 data-observaciones="<%# HttpUtility.HtmlEncode((string)Eval("ObsMedicas") ?? "") %>">
                                <div class="card-body">
                                    <h5 class="card-title"><i class="fa-solid fa-stethoscope me-2"></i><%# Eval("NombreEspecialidad") %></h5>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-user-doctor me-2 text-primary"></i>
                                        <strong>Médico:</strong> <%# Eval("NombreMedico") %>
                                    </p>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-calendar-day me-2 text-success"></i>
                                        <strong>Fecha:</strong> <%# Eval("FechaCita", "{0:dddd, dd 'de' MMMM 'de' yyyy}") %>
                                    </p>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-clock me-2 text-warning"></i>
                                        <strong>Horario:</strong> <%# Eval("DescripcionHorario") %>
                                    </p>
                                </div>
                                <div class="card-footer bg-transparent border-top-0 text-end">
                                    <small class="text-muted"><em>Click para ver detalles</em></small>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltrosHistorial" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnLimpiarFiltrosHistorial" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlEspecialidadHistorial" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleCitaHistorial" tabindex="-1" aria-labelledby="modalDetalleCitaHistorialLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #5bd3c5; color: white;">
                    <h5 class="modal-title" id="modalDetalleCitaHistorialLabel"><i class="fa-solid fa-circle-info me-2"></i>Detalle de Cita Atendida</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-2">
                        <strong><i class="fa-solid fa-stethoscope me-2"></i>Especialidad:</strong>
                        <span id="modalEspecialidad"></span>
                    </div>
                    <div class="mb-2">
                        <strong><i class="fa-solid fa-user-doctor me-2 text-primary"></i>Médico:</strong>
                        <span id="modalMedico"></span>
                    </div>
                    <div class="mb-2">
                        <strong><i class="fa-solid fa-calendar-day me-2 text-success"></i>Fecha:</strong>
                        <span id="modalFecha"></span>
                    </div>
                    <div class="mb-2">
                        <strong><i class="fa-solid fa-clock me-2 text-warning"></i>Horario:</strong>
                        <span id="modalHorario"></span>
                    </div>
                     <div class="mb-2">
                        <strong><i class="fa-solid fa-notes-medical me-2 text-info"></i>Estado:</strong>
                        <span id="modalEstado"></span>
                    </div>
                    <hr/>
                    <div class="mt-2">
                        <strong><i class="fa-solid fa-comment-medical me-2 text-secondary"></i>Observaciones Médicas:</strong>
                        <p id="modalObservaciones" class="mt-1" style="white-space: pre-wrap;"></p>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <style>
        .cita-card-historial {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            border-left: 5px solid #5bd3c5;
        }
        .cita-card-historial:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .cita-card-historial .card-title i {
            color: #5bd3c5;
        }
        .modal-body strong {
            display: inline-block;
            min-width: 150px;
        }
    </style>

    <script type="text/javascript">
        function setupHistorialCardClicks() {
            const historialContainer = document.getElementById('historialCardContainer');
            const detalleModalElement = document.getElementById('modalDetalleCitaHistorial');
            if (!historialContainer || !detalleModalElement)
                return;
            const detalleModal = new bootstrap.Modal(detalleModalElement);

            historialContainer.addEventListener('click', function (event) {
                let clickedCard = event.target;
                while (clickedCard && !clickedCard.classList.contains('cita-card-historial')) {
                    clickedCard = clickedCard.parentElement;
                }

                if (clickedCard) {
                    document.getElementById('modalEspecialidad').textContent = clickedCard.dataset.especialidad || 'N/A';
                    document.getElementById('modalMedico').textContent = clickedCard.dataset.medico || 'N/A';
                    document.getElementById('modalFecha').textContent = clickedCard.dataset.fecha || 'N/A';
                    document.getElementById('modalHorario').textContent = clickedCard.dataset.horario || 'N/A';
                    document.getElementById('modalEstado').textContent = clickedCard.dataset.estado || 'N/A';
                    
                    const observacionesEncoded = clickedCard.dataset.observaciones || '';
                    const tempDiv = document.createElement('div');
                    tempDiv.innerHTML = observacionesEncoded;
                    document.getElementById('modalObservaciones').textContent = tempDiv.textContent || 'Sin observaciones.';


                    detalleModal.show();
                }
            });
        }

        document.addEventListener('DOMContentLoaded', setupHistorialCardClicks);

        if (typeof (Sys) !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            const prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function (sender, args) {
                if (args.get_panelsUpdated().some(panel => panel.id === '<%= updHistorialCitas.ClientID %>')) {
                    setupHistorialCardClicks();
                }
            });
        } else {
            setTimeout(setupHistorialCardClicks, 0);
        }

    </script>
</asp:Content>