<%@ Page Title="Agenda del Día" Language="C#" MasterPageFile="~/SoftMA_Doctor.Master" AutoEventWireup="true" CodeBehind="doctor_agenda.aspx.cs" Inherits="SoftWA.doctor_agenda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Agenda del Doctor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row mb-3 align-items-center">
            <div class="col-md-8">
                <h2><i class="fa-solid fa-calendar-day me-2"></i>Agenda de Citas</h2>
                <p class="text-muted">Listado de sus próximas citas programadas.</p>
            </div>
        </div>
        <hr />

        <asp:UpdatePanel ID="updAgendaPrincipal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoAgenda" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No tiene citas pendientes en su agenda.
                    </div>
                </asp:PlaceHolder>

                <asp:Panel ID="pnlProximaCita" runat="server" Visible="false">
                    <div class="row justify-content-center mb-4">
                        <div class="col-lg-8 col-md-10">
                            <h3 class="text-center mb-3"><i class="fa-solid fa-star me-2 text-warning"></i>Próxima Cita a Atender</h3>
                            <div class="card shadow-lg cita-card-proxima">
                                <div class="card-body">
                                    <h4 class="card-title">
                                        <i class="fa-solid fa-user-injured me-2"></i>
                                        <asp:Literal ID="ltlProximaPacienteNombre" runat="server" />
                                    </h4>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-calendar-check me-2 text-success"></i>
                                        <strong>Fecha:</strong> <asp:Literal ID="ltlProximaFecha" runat="server" />
                                    </p>
                                    <p class="card-text mb-1">
                                        <i class="fa-solid fa-clock me-2 text-primary"></i>
                                        <strong>Horario:</strong> <asp:Literal ID="ltlProximaHorario" runat="server" />
                                    </p>
                                    <p class="card-text mb-2">
                                        <i class="fa-solid fa-stethoscope me-2" style="color: #5bd3c5;"></i>
                                        <strong>Especialidad:</strong> <asp:Literal ID="ltlProximaEspecialidad" runat="server" />
                                    </p>
                                    <div class="text-end mt-3">
                                        <asp:LinkButton ID="btnAtenderProximaCita" runat="server" CssClass="btn btn-info" OnClick="btnAtenderProximaCita_Click">
                                            <i class="fa-solid fa-file-medical me-1"></i>Registrar Atención
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr id="hrSeparadorCitas" runat="server" visible="false" class="my-4" />
                </asp:Panel>

                <asp:Panel ID="pnlSiguientesCitas" runat="server" Visible="false">
                    <h4 class="mb-3"><i class="fa-solid fa-list-ol me-2"></i>Siguientes Citas Programadas</h4>
                    <asp:Repeater ID="rptSiguientesCitas" runat="server" OnItemCommand="rptSiguientesCitas_ItemCommand">
                        <HeaderTemplate>
                            <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="col">
                                <div class="card h-100 shadow-sm cita-card">
                                    <div class="card-body">
                                        <h5 class="card-title">
                                            <i class="fa-solid fa-user me-2"></i>
                                            <%# Eval("paciente.nombres") %> <%# Eval("paciente.apellidoPaterno") %>
                                        </h5>
                                        <p class="card-text mb-1">
                                            <i class="fa-solid fa-calendar-day me-2 text-success"></i>
                                            <strong>Fecha:</strong> <%# DateTime.Parse(Eval("fechaCita").ToString()).ToString("dddd, dd 'de' MMMM 'de' yyyy") %>
                                        </p>
                                        <p class="card-text mb-1">
                                            <i class="fa-solid fa-clock me-2 text-primary"></i>
                                            <strong>Horario:</strong> <%# ((DateTime)Eval("turno.horaInicio")).ToString("HH:mm") + " - " + ((DateTime)Eval("turno.horaFin")).ToString("HH:mm") %>
                                        </p>
                                        <p class="card-text mb-2">
                                            <i class="fa-solid fa-stethoscope me-2" style="color: #5bd3c5;"></i>
                                            <strong>Especialidad:</strong> <%# Eval("especialidad.nombreEspecialidad") %>
                                        </p>
                                        <div class="text-end mt-2">
                                            <asp:LinkButton ID="btnAtenderSiguienteCita" runat="server" CommandName="AtenderCita"
                                                CommandArgument='<%# Eval("idCita") %>' CssClass="btn btn-sm btn-outline-info">
                                                <i class="fa-solid fa-file-medical me-1"></i>Atender
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <style>
        .cita-card, .cita-card-proxima {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            border-left: 5px solid #5bd3c5;
        }
        .cita-card:hover, .cita-card-proxima:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .cita-card-proxima {
             border-left-width: 7px;
             border-left-color: #48a999;
        }
        .cita-card .card-title i, .cita-card-proxima .card-title i {
            color: #5bd3c5;
        }
        .cita-card-proxima .card-title i {
            color: #48a999;
        }
         .badge.bg-primary {
            background-color: #5bd3c5 !important;
            color: white !important;
        }
    </style>
</asp:Content>
