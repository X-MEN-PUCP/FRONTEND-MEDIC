<%@ Page Title="Próximas Citas" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" CodeBehind="paciente_proximas_citas.aspx.cs" Inherits="SoftWA.paciente_proximas_citas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Próximas Citas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row mb-3 align-items-center">
            <div class="col-md-7">
                <h2><i class="fa-solid fa-calendar-check me-2"></i>Sus Próximas Citas</h2>
            </div>
            <div class="col-md-5">
                <asp:UpdatePanel ID="updFiltroEspecialidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="input-group">
                            <asp:Label AssociatedControlID="ddlFiltroEspecialidad" Text="Filtrar por Especialidad:" CssClass="input-group-text" runat="server" />
                            <asp:DropDownList ID="ddlFiltroEspecialidad" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEspecialidad_SelectedIndexChanged" AppendDataBoundItems="true">
                                <asp:ListItem Text="-- Todas --" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <hr />

        <asp:UpdatePanel ID="updCitas" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoCitas" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No tiene citas programadas próximamente con los filtros seleccionados.
                        <a href="paciente_cita_reserva.aspx" class="alert-link ms-2">Reserve una nueva cita aquí</a>.
                    </div>
                </asp:PlaceHolder>
                <asp:Literal ID="ltlMensajeAccion" runat="server" EnableViewState="false"></asp:Literal>
                <asp:Repeater ID="rptProximasCitas" runat="server" OnItemCommand="rptProximasCitas_ItemCommand" OnItemDataBound="rptProximasCitas_ItemDataBound">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm cita-card <%# GetCardBorderClass(Eval("EstadoCita").ToString()) %>">
                                <div class="card-body">
                                    <div class="d-flex justify-content-between align-items-start">
                                        <h5 class="card-title mb-0"><i class="fa-solid fa-stethoscope me-2"></i><%# Eval("NombreEspecialidad") %></h5>
                                        <span class="badge <%# GetEstadoBadgeClass(Eval("EstadoCita").ToString()) %>"><%# Eval("EstadoCita") %></span>
                                    </div>
                                    <hr class="my-2">
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
                                    <p class="card-text">
                                        <i class="fa-solid fa-money-bill-1-wave me-2 text-info"></i>
                                        <strong>Precio:</strong> S/. <%# Eval("Precio", "{0:N2}") %>
                                    </p>
                                </div>
                                <div class="card-footer bg-light text-end"> 
                                    <asp:LinkButton ID="btnPagarAhora" runat="server"
                                        CommandName="Pagar"
                                        CommandArgument='<%# Eval("IdCita") %>'
                                        CssClass="btn btn-sm btn-success me-2"
                                        ToolTip="Pagar esta cita ahora" Visible="false">
                                        <i class="fa-solid fa-dollar-sign me-1"></i>Pagar Ahora
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCancelarCita" runat="server"
                                        CommandName="Cancelar"
                                        CommandArgument='<%# Eval("IdCita") %>'
                                        CssClass="btn btn-sm btn-outline-danger"
                                        ToolTip="Cancelar esta cita" Visible="false"
                                        OnClientClick="return confirm('¿Está seguro de que desea cancelar esta cita? Si ha pagado la cita se devolvera el pago en el plazo de 3 dias habiles');">
                                        <i class="fa-solid fa-trash-can me-1"></i>Cancelar Cita
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
                 <asp:AsyncPostBackTrigger ControlID="ddlFiltroEspecialidad" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <style>
        .cita-card {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
        }
        .cita-card-confirmada { border-left: 5px solid #198754;}
        .cita-card-pendiente { border-left: 5px solid #ffc107;}

        .cita-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .card-title i {
            color: #5bd3c5;
        }
    </style>
</asp:Content>