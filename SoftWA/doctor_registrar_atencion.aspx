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

        <!-- mensajess -->
        <asp:UpdatePanel ID="updMensajes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:PlaceHolder ID="phMensaje" runat="server" Visible="false">
                    <div id="divAlert" runat="server" role="alert">
                        <asp:Literal ID="ltlMensaje" runat="server"></asp:Literal>
                    </div>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Acordeon -->
        <div class="accordion" id="accordionAtencion">
            <!-- 1. Epicrisis  -->
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
                            <!-- Peso -->
                            <div class="col-md-3">
                                <label class="form-label">Peso (kg) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPeso" runat="server" CssClass="form-control" TextMode="Number" step="0,1" min="0,1" max="500"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPeso" runat="server" 
                                    ControlToValidate="txtPeso" 
                                    ErrorMessage="El peso es obligatorio" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rvPeso" runat="server" 
                                    ControlToValidate="txtPeso" 
                                    MinimumValue="0,1" 
                                    MaximumValue="500" 
                                    Type="Double" 
                                    ErrorMessage="El peso debe estar entre 0.1 y 500 kg" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RangeValidator>
                            </div>

                            <!-- Talla -->
                             <div class="col-md-3"><label class="form-label">Talla (m)</label><asp:TextBox ID="txtTalla" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox></div>

                            <!-- Presión Arterial -->
                            <div class="col-md-3">
                                <label class="form-label">Presión Arterial <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtPresion" runat="server" CssClass="form-control" placeholder="Ej: 120/80" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPresion" runat="server" 
                                    ControlToValidate="txtPresion" 
                                    ErrorMessage="La presión arterial es obligatoria" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPresion" runat="server" 
                                    ControlToValidate="txtPresion" 
                                    ValidationExpression="^([5-9][0-9]|[1-2][0-9][0-9]|300)\/([3-9][0-9]|1[0-9][0-9]|200)$" 
                                    ErrorMessage="Formato inválido. Use formato: 120/80 (valores entre 50-300 / 30-200)" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RegularExpressionValidator>
                            </div>

                            <!-- Temperatura -->
                            <div class="col-md-3"><label class="form-label">Temperatura (°C)</label><asp:TextBox ID="txtTemperatura" runat="server" CssClass="form-control" TextMode="Number" step="0.1"></asp:TextBox></div>

                            <!-- Motivo de Consulta -->
                            <div class="col-md-12">
                                <label class="form-label">Motivo de Consulta <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtMotivoConsulta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMotivoConsulta" runat="server" 
                                    ControlToValidate="txtMotivoConsulta" 
                                    ErrorMessage="El motivo de consulta es obligatorio" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RequiredFieldValidator>
                                <small class="form-text text-muted">Máximo 500 caracteres</small>
                            </div>

                            <!-- Tratamiento -->
                            <div class="col-md-12">
                                <label class="form-label">Tratamiento y Evolución <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtTratamiento" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="1000"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTratamiento" runat="server" 
                                    ControlToValidate="txtTratamiento" 
                                    ErrorMessage="El tratamiento es obligatorio" 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" 
                                    ValidationGroup="Epicrisis">
                                </asp:RequiredFieldValidator>
                                <small class="form-text text-muted">Máximo 1000 caracteres</small>
                            </div>

                            <!-- Recomendaciones -->
                            <div class="col-md-12">
                                <label class="form-label">Recomendaciones Finales</label>
                                <asp:TextBox ID="txtRecomendaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500"></asp:TextBox>
                                <small class="form-text text-muted">Máximo 500 caracteres (opcional)</small>
                            </div>
                        </div>

                        <!-- Resumen de validación -->
                        <div class="mt-3">
                            <asp:ValidationSummary ID="vsEpicrisis" runat="server" 
                                ValidationGroup="Epicrisis" 
                                CssClass="alert alert-danger" 
                                HeaderText="Por favor, corrija los siguientes errores:" 
                                DisplayMode="BulletList" 
                                ShowSummary="true">
                            </asp:ValidationSummary>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="d-flex justify-content-end mt-3">
                    <button type="button" class="btn btn-outline-secondary" onclick="validarYAvanzar('Epicrisis', 'collapseEpicrisis', 'collapseDiagnostico')">
                        Siguiente <i class="fa-solid fa-chevron-right ms-1"></i>
                    </button>
                </div>
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
                                <div id="panelAgregarDiagnostico" runat="server">
                                    <div class="input-group mb-3">
                                        <asp:DropDownList ID="ddlDiagnosticos" runat="server" CssClass="form-control select2-diagnosticos" Width="50%"></asp:DropDownList>
                                        <asp:Button ID="btnAgregarDiagnostico" runat="server" Text="Agregar" CssClass="btn btn-outline-primary" OnClick="btnAgregarDiagnostico_Click" />
                                    </div>
                                    <div class="mb-3">
                                        <label for="<%=txtObservacionDiagnostico.ClientID%>" class="form-label">Observación del Diagnóstico:</label>
                                        <asp:TextBox ID="txtObservacionDiagnostico" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                    <hr />
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
                                            <asp:LinkButton ID="btnQuitarDiagnostico" runat="server" 
                                            CssClass="btn btn-outline-danger btn-sm" 
                                            CommandName="Quitar" 
                                            CommandArgument='<%# Eval("diagnostico.idDiagnostico") %>'
                                            Visible='<%# !EsModoVista %>'>
                                                <i class="fa-solid fa-trash-alt"></i> Quitar
                                            </asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate></ul></FooterTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="d-flex justify-content-end mt-3">
                            <button type="button" class="btn btn-outline-secondary" onclick="avanzarAcordeon('collapseDiagnostico', 'collapseExamenes')">
                                Siguiente <i class="fa-solid fa-chevron-right ms-1"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 3. Exámenes -->
<div class="accordion-item">
    <h2 class="accordion-header" id="headingExamenes">
        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExamenes" aria-expanded="false" aria-controls="collapseExamenes">
            <i class="fas fa-vial me-2"></i>Solicitud de Exámenes
        </button>
    </h2>
    <div id="collapseExamenes" class="accordion-collapse collapse" aria-labelledby="headingExamenes">
        <div class="accordion-body">
            <asp:UpdatePanel ID="updExamenes" runat="server">
                <ContentTemplate>
                    <div id="panelAgregarExamen" runat="server">
                        <div class="input-group mb-3">
                            <asp:DropDownList ID="ddlExamenes" runat="server" CssClass="form-control select2-busqueda" Width="50%"></asp:DropDownList>
                            <asp:Button ID="btnAgregarExamen" runat="server" Text="Agregar" CssClass="btn btn-outline-primary" OnClick="btnAgregarExamen_Click" />
                        </div>
                        <div class="mb-3">
                            <label for="<%=txtObservacionExamen.ClientID%>" class="form-label">Indicaciones / Observaciones del Examen:</label>
                            <asp:TextBox ID="txtObservacionExamen" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                        <hr />
                    </div>
                    <hr />
                    <h6>Exámenes Solicitados</h6>
                    <asp:Repeater ID="rptExamenesAgregados" runat="server" OnItemCommand="rptExamenesAgregados_ItemCommand">
                        <HeaderTemplate><ul class="list-group"></HeaderTemplate>
                        <ItemTemplate>
                            <li class="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong><%# Eval("examen.nombreExamen") %></strong>
                                    <small class="d-block text-muted"><%# Eval("observaciones") %></small>
                                </div>
                                <asp:LinkButton ID="btnQuitarExamen" runat="server" 
                                        CssClass="btn btn-outline-danger btn-sm"
                                        CommandName="Quitar" 
                                        CommandArgument='<%# Eval("examen.idExamen") %>'
                                        Visible='<%# !EsModoVista %>'>
                                    <i class="fa-solid fa-trash-alt"></i> Quitar
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate></ul></FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
                        <div class="d-flex justify-content-end mt-3">
                            <button type="button" class="btn btn-outline-secondary" onclick="avanzarAcordeon('collapseExamenes', 'collapseInterconsultas')">
                                Siguiente <i class="fa-solid fa-chevron-right ms-1"></i>
                            </button>
                        </div>
        </div>
    </div>
</div>

<!-- 4. Interconsultas -->
<div class="accordion-item">
    <h2 class="accordion-header" id="headingInterconsultas">
        <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseInterconsultas" aria-expanded="false" aria-controls="collapseInterconsultas">
            <i class="fas fa-user-md me-2"></i>Solicitud de Interconsultas
        </button>
    </h2>
    <div id="collapseInterconsultas" class="accordion-collapse collapse" aria-labelledby="headingInterconsultas">
        <div class="accordion-body">
            <asp:UpdatePanel ID="updInterconsultas" runat="server">
                <ContentTemplate>
                    <div id="panelAgregarInterconsulta" runat="server">
                        <div class="input-group mb-3">
                            <asp:DropDownList ID="ddlEspecialidadInterconsulta" runat="server" CssClass="form-control select2-busqueda" Width="50%"></asp:DropDownList>
                            <asp:Button ID="btnAgregarInterconsulta" runat="server" Text="Agregar" CssClass="btn btn-outline-primary" OnClick="btnAgregarInterconsulta_Click" />
                        </div>
                        <div class="mb-3">
                            <label for="<%=txtRazonInterconsulta.ClientID%>" class="form-label">Razón de la Interconsulta:</label>
                            <asp:TextBox ID="txtRazonInterconsulta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                        <hr />
                    </div>
                    <hr />
                    <h6>Interconsultas Solicitadas</h6>
                    <asp:Repeater ID="rptInterconsultasAgregadas" runat="server" OnItemCommand="rptInterconsultasAgregadas_ItemCommand">
                        <HeaderTemplate><ul class="list-group"></HeaderTemplate>
                        <ItemTemplate>
                            <li class="list-group-item d-flex justify-content-between align-items-center">
                                <div>
                                    <strong><%# Eval("especialidadInterconsulta.nombreEspecialidad") %></strong>
                                    <small class="d-block text-muted"><%# Eval("razonInterconsulta") %></small>
                                </div>
                                <asp:LinkButton ID="btnQuitarInterconsulta" runat="server" 
                                    CommandName="Quitar" 
                                    CommandArgument='<%# Eval("especialidadInterconsulta.idEspecialidad") %>' 
                                    CssClass="btn btn-sm btn-outline-danger"
                                    Visible='<%# !EsModoVista %>'>
                                    <i class="fas fa-trash-alt"></i>
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate></ul></FooterTemplate>
                    </asp:Repeater>
                </ContentTemplate>
            </asp:UpdatePanel>
                    <div class="d-flex justify-content-end mt-3">
                    <button type="button" class="btn btn-outline-primary" onclick="avanzarAcordeon('collapseInterconsultas', 'collapseEpicrisis')">
                        <i class="fa-solid fa-rotate-left me-1"></i> Volver al inicio
                    </button>
                </div>
        </div>
    </div>
</div>

        </div>
    </div>
    <script>
    function avanzarAcordeon(actualId, siguienteId) {
        var actual = document.getElementById(actualId);
        var siguiente = document.getElementById(siguienteId);
        var bsActual = bootstrap.Collapse.getOrCreateInstance(actual);
        bsActual.hide();
        var bsSiguiente = bootstrap.Collapse.getOrCreateInstance(siguiente);
        bsSiguiente.show();
    }
    </script>
    <script type="text/javascript">
        function inicializarSelect2() {
            $('.select2-busqueda').select2({
                placeholder: "Escriba para buscar...",
                allowClear: true
            });

            $('.select2-diagnosticos').select2({
                placeholder: "Buscar diagnóstico por nombre o CIE-10...",
                allowClear: true
            });
        }

        $(document).ready(function () {
            inicializarSelect2();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (e.get_error() == null) {
                    inicializarSelect2();
                }
            });
        }
    </script>

    <script type="text/javascript">
        function validarYAvanzar(validationGroup, actualId, siguienteId) {
            if (typeof Page_ClientValidate === 'function') {
                var isValid = Page_ClientValidate(validationGroup);
                if (!isValid) {
                    toastr.error('Por favor complete todos los campos obligatorios correctamente.');
                    return false;
                }
            }

            avanzarAcordeon(actualId, siguienteId);
            return true;
        }

        function validarPresionArtierial(input) {
            var valor = input.value;
            var patron = /^([5-9][0-9]|[1-2][0-9][0-9]|300)\/([3-9][0-9]|1[0-9][0-9]|200)$/;

            if (valor && !patron.test(valor)) {
                input.setCustomValidity('Formato inválido. Use formato: 120/80');
            } else {
                input.setCustomValidity('');
            }
        }

        function actualizarContador(textareaId, maxLength, contadorId) {
            var textarea = document.getElementById(textareaId);
            var contador = document.getElementById(contadorId);
            var restantes = maxLength - textarea.value.length;
            contador.textContent = restantes + ' caracteres restantes';

            if (restantes < 50) {
                contador.className = 'form-text text-warning';
            } else if (restantes < 20) {
                contador.className = 'form-text text-danger';
            } else {
                contador.className = 'form-text text-muted';
            }
        }

        $(document).ready(function () {
            $('#<%= txtPresion.ClientID %>').on('blur', function () {
            validarPresionArtierial(this);
        });

        $('#<%= txtMotivoConsulta.ClientID %>').on('input', function () {
            actualizarContador(this.id, 500, 'contador-motivo');
        });

        $('#<%= txtTratamiento.ClientID %>').on('input', function () {
            actualizarContador(this.id, 1000, 'contador-tratamiento');
        });

        $('#<%= txtRecomendaciones.ClientID %>').on('input', function () {
            actualizarContador(this.id, 500, 'contador-recomendaciones');
        });
    });
    </script>
</asp:Content>