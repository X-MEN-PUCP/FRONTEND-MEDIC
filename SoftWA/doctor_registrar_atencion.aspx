<%-- Archivo: doctor_registrar_atencion.aspx --%>
<%@ Page Title="Registrar Atención Médica" Language="C#" MasterPageFile="~/SoftMA_Doctor.Master" AutoEventWireup="true" CodeBehind="doctor_registrar_atencion.aspx.cs" Inherits="SoftWA.doctor_registrar_atencion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Registro de Atención
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container-fluid mt-4">
        <asp:HiddenField ID="hfIdCita" runat="server" />
        <asp:HiddenField ID="hfIdPaciente" runat="server" />
        <asp:HiddenField ID="hfIdHistoria" runat="server" />

        <!-- Encabezado con información del paciente y la cita -->
        <div class="card shadow-sm mb-4">
            <div class="card-body">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h4 class="mb-1">Paciente: <asp:Literal ID="ltlNombrePaciente" runat="server"></asp:Literal></h4>
                        <p class="text-muted mb-0">
                            Cita de <asp:Literal ID="ltlEspecialidad" runat="server"></asp:Literal> |
                            Fecha: <asp:Literal ID="ltlFechaCita" runat="server"></asp:Literal>
                        </p>
                    </div>
                    <div>
                        <asp:Button ID="btnFinalizarAtencion" runat="server" Text="Finalizar y Guardar Atención" CssClass="btn btn-success" OnClick="btnFinalizarAtencion_Click" OnClientClick="return confirm('¿Está seguro de que desea finalizar y guardar toda la información de la atención?');" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Panel de Mensajes -->
        <asp:UpdatePanel ID="updMensajes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phMensaje" runat="server" Visible="false">
                    <div id="divAlert" runat="server" role="alert">
                        <asp:Literal ID="ltlMensaje" runat="server"></asp:Literal>
                    </div>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Acordeón para las secciones de la atención -->
        <div class="accordion" id="accordionAtencion">
            <!-- 1. Epicrisis (Historia Clínica por Cita) -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingEpicrisis">
                    <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseEpicrisis" aria-expanded="true" aria-controls="collapseEpicrisis">
                        <i class="fa-solid fa-notes-medical me-2"></i>Epicrisis / Datos de la Consulta
                    </button>
                </h2>
                <div id="collapseEpicrisis" class="accordion-collapse collapse show" aria-labelledby="headingEpicrisis">
                    <div class="accordion-body">
                        <asp:UpdatePanel ID="updEpicrisis" runat="server">
                            <ContentTemplate>
                                <div class="row g-3">
                                    <div class="col-md-3"><label class="form-label">Peso (kg)</label><asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" TextMode="Number" step="0.1"></asp:TextBox></div>
                                    <div class="col-md-3"><label class="form-label">Talla (m)</label><asp:TextBox ID="txtTalla" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox></div>
                                    <div class="col-md-3"><label class="form-label">Presión Arterial</label><asp:TextBox ID="txtPresion" runat="server" CssClass="form-control" placeholder="Ej: 120/80"></asp:TextBox></div>
                                    <div class="col-md-3"><label class="form-label">Temperatura (°C)</label><asp:TextBox ID="txtTemperatura" runat="server" CssClass="form-control" TextMode="Number" step="0.1"></asp:TextBox></div>
                                    <div class="col-md-12"><label class="form-label">Motivo de Consulta</label><asp:TextBox ID="txtMotivoConsulta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox></div>
                                    <div class="col-md-12"><label class="form-label">Tratamiento y Evolución</label><asp:TextBox ID="txtTratamiento" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox></div>
                                    <div class="col-md-12"><label class="form-label">Recomendaciones Finales</label><asp:TextBox ID="txtRecomendaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox></div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!-- 2. Diagnósticos -->
            <div class="accordion-item">
                 <h2 class="accordion-header" id="headingDiagnostico">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseDiagnostico" aria-expanded="false" aria-controls="collapseDiagnostico">
                        <i class="fa-solid fa-stethoscope me-2"></i>Diagnósticos
                    </button>
                </h2>
                <div id="collapseDiagnostico" class="accordion-collapse collapse" aria-labelledby="headingDiagnostico">
                    <div class="accordion-body">
                        <asp:UpdatePanel ID="updDiagnosticos" runat="server">
                            <ContentTemplate>
                                <!-- Formulario para agregar nuevo diagnóstico -->
                                <div class="row g-3 mb-3 p-3 border rounded">
                                    <div class="col-md-5">
                                        <label class="form-label">Diagnóstico (CIE-10)</label>
                                        <asp:DropDownList ID="ddlDiagnosticos" runat="server" CssClass="form-select"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-5">
                                        <label class="form-label">Observación</label>
                                        <asp:TextBox ID="txtObservacionDiagnostico" runat="server" CssClass="form-control" placeholder="Detalles adicionales..."></asp:TextBox>
                                    </div>
                                    <div class="col-md-2 d-flex align-items-end">
                                        <asp:Button ID="btnAgregarDiagnostico" runat="server" Text="Agregar" CssClass="btn btn-outline-primary w-100" OnClick="btnAgregarDiagnostico_Click" />
                                    </div>
                                </div>
                                <!-- Lista de diagnósticos agregados -->
                                <asp:Repeater ID="rptDiagnosticosAgregados" runat="server" OnItemCommand="rptDiagnosticosAgregados_ItemCommand">
                                     <HeaderTemplate><ul class="list-group"></HeaderTemplate>
                                     <ItemTemplate>
                                        <li class="list-group-item d-flex justify-content-between align-items-center">
                                            <div>
                                                <strong><%# Eval("diagnostico.nombreDiagnostico") %></strong><br/>
                                                <small class="text-muted"><%# Eval("observacion") %></small>
                                            </div>
                                            <asp:LinkButton ID="btnQuitarDiagnostico" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="Quitar" CommandArgument='<%# Eval("diagnostico.idDiagnostico") %>'><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!-- 3. Exámenes -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingExamenes">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExamenes" aria-expanded="false" aria-controls="collapseExamenes">
                        <i class="fa-solid fa-vial-virus me-2"></i>Exámenes
                    </button>
                </h2>
                <div id="collapseExamenes" class="accordion-collapse collapse" aria-labelledby="headingExamenes">
                    <div class="accordion-body">
                        <p class="text-center text-muted">Funcionalidad de solicitud de exámenes en desarrollo.</p>
                        <%-- Aquí iría la lógica para agregar exámenes (similar a diagnósticos) --%>
                    </div>
                </div>
            </div>

            <!-- 4. Interconsultas -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingInterconsultas">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseInterconsultas" aria-expanded="false" aria-controls="collapseInterconsultas">
                        <i class="fa-solid fa-user-doctor me-2"></i>Interconsultas
                    </button>
                </h2>
                <div id="collapseInterconsultas" class="accordion-collapse collapse" aria-labelledby="headingInterconsultas">
                    <div class="accordion-body">
                         <p class="text-center text-muted">Funcionalidad de solicitud de interconsultas en desarrollo.</p>
                        <%-- Aquí iría la lógica para agregar interconsultas --%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>