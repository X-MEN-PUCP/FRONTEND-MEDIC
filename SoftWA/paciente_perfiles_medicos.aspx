<%@ Page Title="Perfiles de Médicos" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" CodeBehind="paciente_perfiles_medicos.aspx.cs" Inherits="SoftWA.paciente_perfiles_medicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Perfiles de Médicos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col">
                <h2><i class="fa-solid fa-user-doctor me-2"></i>Conozca a Nuestros Médicos</h2>
                <hr />
            </div>
        </div>
        <asp:UpdatePanel ID="updFiltros" runat="server">
            <ContentTemplate>
                <div class="card shadow-sm mb-4">
                    <div class="card-body">
                        <div class="row g-3 align-items-end">
                            <div class="col-md-5">
                                <label for="<%=txtFiltroNombre.ClientID %>" class="form-label">Filtrar por Nombre o Especialidad:</label>
                                <asp:TextBox ID="txtFiltroNombre" runat="server" CssClass="form-control" placeholder="Ej: Dr. Pérez o Cardiología"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="<%=ddlOrdenarPor.ClientID %>" class="form-label">Ordenar por:</label>
                                <asp:DropDownList ID="ddlOrdenarPor" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="Nombre" Text="Nombre (A-Z)"></asp:ListItem>
                                    <asp:ListItem Value="Especialidad" Text="Especialidad (A-Z)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 d-grid">
                                <asp:Button ID="btnAplicarFiltros" runat="server" Text="Aplicar" CssClass="btn btn-primary" OnClick="btnAplicarFiltros_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="updResultados" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="phNoMedicos" runat="server" Visible="false">
                    <div class="alert alert-info" role="alert">
                        <i class="fa-solid fa-circle-info me-2"></i>No se encontraron médicos que coincidan con los criterios de búsqueda.
                    </div>
                </asp:PlaceHolder>
                <asp:Repeater ID="rptPerfilesMedicos" runat="server" OnItemCommand="rptPerfilesMedicos_ItemCommand">
                    <HeaderTemplate>
                        <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 shadow-sm medico-perfil-card">
                                <div class="card-body text-center">
                                    <h5 class="card-title mt-2"><%# Eval("Usuario.nombres") %> <%# Eval("Usuario.apellidoPaterno") %></h5>
                                    <p class="card-text text-muted mb-1">
                                        <i class="fa-solid fa-stethoscope me-1"></i> <%# Eval("Especialidad.nombreEspecialidad") %>
                                    </p>
                                    <p class="card-text small">
                                        <strong>Código CMP:</strong> <%# Eval("Usuario.codMedico") ?? "N/A" %>
                                    </p>
                                </div>
                                <div class="card-footer bg-light p-0">
                                     <asp:LinkButton ID="btnReservarConMedico" runat="server" 
                                         CssClass="btn btn-primary w-100 rounded-bottom" 
                                         CommandName="Reservar" 
                                         CommandArgument='<%# Eval("Usuario.idUsuario") + ";" + Eval("Especialidad.idEspecialidad") %>'>
                                         <i class="fa-solid fa-calendar-check me-2"></i>Reservar Cita
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
                <asp:AsyncPostBackTrigger ControlID="btnAplicarFiltros" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <style>
        .medico-perfil-card {
            transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
            border-left: 5px solid #0d6efd;
        }
        .medico-perfil-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;
        }
        .btn.rounded-bottom {
             border-top-left-radius: 0;
             border-top-right-radius: 0;
        }
    </style>
</asp:Content>