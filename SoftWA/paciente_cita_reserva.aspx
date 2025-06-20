<%@ Page Title="Buscar y Reservar Cita" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" CodeBehind="paciente_cita_reserva.aspx.cs" Inherits="SoftWA.paciente_cita_reserva" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Buscar y Reservar Cita
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="card shadow-sm">
            <div class="card-header">
                <h2><i class="fa-solid fa-search me-2"></i>Busque y Reserve su Cita</h2>
            </div>
            <div class="card-body">
                <asp:UpdatePanel ID="updFiltrosCitas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mb-3 align-items-center">
                            <asp:Label ID="lblEspecialidad" runat="server" Text="Especialidad:" CssClass="col-sm-3 col-form-label fw-bold"></asp:Label>
                            <div class="col-sm-9 col-md-6">
                                <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server" ControlToValidate="ddlEspecialidad"
                                    ErrorMessage="Seleccione una especialidad." CssClass="text-danger small" Display="Dynamic" InitialValue="" ValidationGroup="BusquedaCita" />
                            </div>
                        </div>
                        <div class="row mb-3 align-items-center">
                            <asp:Label ID="lblMedico" runat="server" Text="Médico (Opcional):" CssClass="col-sm-3 col-form-label fw-bold"></asp:Label>
                            <div class="col-sm-9 col-md-6">
                                <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" AppendDataBoundItems="true"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                                    <asp:ListItem Text="-- Cualquier Médico --" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <asp:Label ID="lblFecha" runat="server" Text="Fecha (Opcional):" CssClass="col-sm-3 col-form-label fw-bold"></asp:Label>
                            <div class="col-sm-9 col-md-6">
                                <asp:Calendar ID="calFechaCita" runat="server"
                                    OnSelectionChanged="calFechaCita_SelectionChanged"
                                    OnDayRender="calFechaCita_DayRender"
                                    CssClass="table table-bordered table-sm" Width="100%">
                                    <WeekendDayStyle BackColor="#f8f9fa" />
                                    <SelectedDayStyle BackColor="#0d6efd" ForeColor="White" Font-Bold="true" />
                                    <TodayDayStyle BackColor="#cfe2ff" />
                                    <OtherMonthDayStyle ForeColor="LightGray" />
                                    <DayStyle CssClass="text-center" />
                                    <NextPrevStyle Font-Bold="true" ForeColor="#0d6efd" />
                                    <TitleStyle BackColor="#e9ecef" Font-Bold="true" CssClass="py-2" />
                                    <DayHeaderStyle BackColor="#f8f9fa" Font-Bold="true" CssClass="py-1" />
                                </asp:Calendar>
                                <asp:Label ID="lblFechaSeleccionadaInfo" runat="server" CssClass="form-text mt-1" Visible="false"></asp:Label>
                                <asp:Label ID="lblErrorBusqueda" runat="server" CssClass="text-danger small" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlEspecialidad" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlMedico" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="calFechaCita" EventName="SelectionChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="card-footer text-end">
                <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar Filtros" CssClass="btn btn-outline-secondary me-2" OnClick="btnLimpiarFiltros_Click" CausesValidation="false" />
                <asp:Button ID="btnBuscarCitas" CssClass="btn btn-primary" runat="server" Text="Buscar Citas Disponibles" OnClick="btnBuscarCitas_Click" ValidationGroup="BusquedaCita"/>
            </div>
        </div>

        <hr class="my-4" />
        <asp:HiddenField ID="hfModalCitaId" runat="server" Value="0" />
        <asp:UpdatePanel ID="updResultadosCitas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <h3 class="mb-3"><i class="fa-solid fa-calendar-days me-2"></i>Resultados de la Búsqueda</h3>
                <asp:PlaceHolder ID="phNoResultados" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No se encontraron citas disponibles con los criterios seleccionados. Por favor, intente modificar su búsqueda.
                    </div>
                </asp:PlaceHolder>
                <asp:Literal ID="ltlMensajeReserva" runat="server"></asp:Literal>

                <asp:Repeater ID="rptResultadosCitas" runat="server" OnItemDataBound="rptResultadosCitas_ItemDataBound">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm cita-card">
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
                                    <p class="card-text">
                                        <i class="fa-solid fa-clock me-2 text-warning"></i>
                                        <strong>Horario:</strong> <%# Eval("DescripcionHorario") %>
                                    </p>
                                </div>
                                <div class="card-footer bg-light text-end"> 
                                    <asp:LinkButton ID="btnAccionReserva" runat="server" CssClass="btn btn-sm">
                                        
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
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscarCitas" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModalPagarAhora" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModalPagarDespues" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="confirmarReservaModal" tabindex="-1" aria-labelledby="confirmarReservaModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="confirmarReservaModalLabel"><i class="fa-solid fa-circle-question me-2"></i>Confirmar Reserva</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>¿Desea proceder al pago de esta cita ahora o reservarla y pagar más tarde?</p>
                    <p class="text-muted small">Si elige pagar después, tendrá 24 horas para completar el pago. Puede tener hasta 3 citas pendientes de pago.</p>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnModalPagarDespues" runat="server" Text="Reservar y Pagar Después" CssClass="btn btn-secondary" OnClick="btnModalPagarDespues_Click" />
                    <asp:Button ID="btnModalPagarAhora" runat="server" Text="Sí, Pagar Ahora" CssClass="btn btn-primary" OnClick="btnModalPagarAhora_Click" />
                </div>
            </div>
        </div>
    </div>

    <script src='<%= ResolveUrl("~/Scripts/pacienteCitas.js") %>' type="text/javascript"></script>
    <style>
        .cita-card {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            border-left: 5px solid #198754;
        }
        .cita-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .card-title i {
            color: #198754;
        }
        .day-available {
            background-color: #d1e7dd !important; 
            font-weight: bold;
            border-radius: 50% !important;
        }
        .day-available a {
            color: #0f5132 !important; 
        }
        .day-selected {
            background-color: #0d6efd !important;
            color: white !important;
            font-weight: bold;
            border-radius: 5px;
        }
    </style>
</asp:Content>