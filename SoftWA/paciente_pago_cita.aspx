<%@ Page Title="Realizar Pago de Cita" Language="C#" MasterPageFile="~/SoftMA_Paciente.Master" AutoEventWireup="true" CodeBehind="paciente_pago_cita.aspx.cs" Inherits="SoftWA.paciente_pago_cita" %>

<asp:Content ID="Content1" ContentPlaceHolderID="chpTitulo" runat="server">
    Realizar Pago de Cita
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphContenido" runat="server">
    <div class="container mt-4">
        <div class="row justify-content-center">
            <div class="col-md-8 col-lg-6">
                <div class="card shadow-sm">
                    <div class="card-header">
                        <h2 class="mb-0"><i class="fa-solid fa-credit-card me-2"></i>Detalles del Pago</h2>
                    </div>
                    <div class="card-body">
                        <asp:PlaceHolder ID="phDetallesPago" runat="server">
                            <p><strong>Descripción:</strong> <asp:Literal ID="ltlDescripcionCita" runat="server"></asp:Literal></p>
                            <p><strong>Monto a Pagar:</strong> S/. <asp:Literal ID="ltlMontoPagar" runat="server"></asp:Literal></p>
                            <hr />
                            <h4>Ingrese los datos de su tarjeta:</h4>
                            
                            <div class="mb-3">
                                <label for="<%=txtNumeroTarjeta.ClientID %>" class="form-label">Número de Tarjeta:</label>
                                <div class="input-group">
                                    <span class="input-group-text"><i class="fa-regular fa-credit-card"></i></span>
                                    <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control" placeholder="XXXX XXXX XXXX XXXX" MaxLength="19"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvNumeroTarjeta" runat="server" ControlToValidate="txtNumeroTarjeta"
                                    ErrorMessage="El número de tarjeta es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revNumeroTarjeta" runat="server" ControlToValidate="txtNumeroTarjeta"
                                    ValidationExpression="^\d{4}[\s-]?\d{4}[\s-]?\d{4}[\s-]?\d{4}$"
                                    ErrorMessage="Formato de tarjeta inválido (ej: 1111 1111 1111 1111)." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RegularExpressionValidator>
                            </div>

                            <div class="row">
                                <div class="col-md-7 mb-3">
                                    <label for="<%=txtFechaExpiracion.ClientID %>" class="form-label">Fecha de Expiración (MM/AA):</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="fa-solid fa-calendar-days"></i></span>
                                        <asp:TextBox ID="txtFechaExpiracion" runat="server" CssClass="form-control" placeholder="MM/AA" MaxLength="5"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvFechaExpiracion" runat="server" ControlToValidate="txtFechaExpiracion"
                                        ErrorMessage="La fecha de expiración es obligatoria." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revFechaExpiracion" runat="server" ControlToValidate="txtFechaExpiracion"
                                        ValidationExpression="^(0[1-9]|1[0-2])\/?([0-9]{2})$"
                                        ErrorMessage="Formato inválido (MM/AA)." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-5 mb-3">
                                    <label for="<%=txtCVV.ClientID %>" class="form-label">CVV:</label>
                                    <div class="input-group">
                                        <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                                        <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="XXX" MaxLength="4" TextMode="Password"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvCVV" runat="server" ControlToValidate="txtCVV"
                                        ErrorMessage="El CVV es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCVV" runat="server" ControlToValidate="txtCVV"
                                        ValidationExpression="^\d{3,4}$"
                                        ErrorMessage="CVV debe tener 3 o 4 dígitos." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                             <div class="mb-3">
                                <label for="<%=txtNombreTitular.ClientID %>" class="form-label">Nombre del Titular:</label>
                                 <div class="input-group">
                                     <span class="input-group-text"><i class="fa-solid fa-user-tie"></i></span>
                                    <asp:TextBox ID="txtNombreTitular" runat="server" CssClass="form-control" placeholder="Como aparece en la tarjeta"></asp:TextBox>
                                 </div>
                                <asp:RequiredFieldValidator ID="rfvNombreTitular" runat="server" ControlToValidate="txtNombreTitular"
                                    ErrorMessage="El nombre del titular es obligatorio." CssClass="text-danger small" Display="Dynamic" ValidationGroup="PagoCita"></asp:RequiredFieldValidator>
                            </div>
                            <div class="mb-3">
                                <label for="<%= txtCorreo.ClientID %>" class="form-label">Correo electrónico (para enviar resumen)</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="correo@ejemplo.com" ValidationGroup="PagoCita" TextMode="Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" ControlToValidate="txtCorreo" ErrorMessage="El correo electrónico es requerido." Display="Dynamic" CssClass="text-danger" ValidationGroup="PagoCita"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo" ErrorMessage="Formato de correo electrónico inválido." Display="Dynamic" CssClass="text-danger" ValidationGroup="PagoCita" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <asp:ValidationSummary ID="vsPagoCita" runat="server" CssClass="text-danger mt-2" HeaderText="Por favor corrija los siguientes errores:" ValidationGroup="PagoCita"/>
                            
                            <div class="d-grid gap-2 mt-4">
                                <asp:Button ID="btnPagar" runat="server" Text="Pagar Ahora" CssClass="btn btn-primary btn-lg" OnClick="btnPagar_Click" ValidationGroup="PagoCita"/>
                                <asp:Button ID="btnCancelarPago" runat="server" Text="Cancelar Pago" CssClass="btn btn-outline-secondary" OnClick="btnCancelarPago_Click" CausesValidation="false"/>
                            </div>
                        </asp:PlaceHolder>
                        
                        <asp:PlaceHolder ID="phMensajeResultado" runat="server" Visible="false">
                             <div id="divMensajeResultado" runat="server" class="alert text-center" role="alert">
                                <h4 id="hdrMensajeResultado" runat="server" class="alert-heading"></h4>
                                <p id="pMensajeResultado" runat="server"></p>
                                <asp:Panel ID="pnlResumenCita" runat="server" CssClass="card mt-3 mb-3 bg-light" Visible="false">
                                    <div class="card-body text-start">
                                        <h5 class="card-title text-center mb-3">Resumen de su Cita</h5>
                                        <p><strong><i class="fa-solid fa-info me-3 text-primary"></i>Código de Operación:</strong> <asp:Literal ID="ltlResumenCodOperacion" runat="server"></asp:Literal></p>
                                        <p><strong><i class="fa-solid fa-credit-card me-2 text-primary"></i>Número de tarjeta:</strong> <asp:Literal ID="ltlResumenTarjeta" runat="server"></asp:Literal></p>
                                        <div class="row mb-2">
                                            <div class="col-sm-5 col-6 fw-bold">Tipo de Tarjeta:</div>
                                            <div class="col-sm-7 col-6">
                                                <asp:Literal ID="ltlResumenTipoTarjeta" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <p><strong><i class="fa-solid fa-stethoscope me-2 text-primary"></i>Especialidad:</strong> <asp:Literal ID="ltlResumenEspecialidad" runat="server"></asp:Literal></p>
                                        <p><strong><i class="fa-solid fa-user-doctor me-2 text-primary"></i>Médico:</strong> <asp:Literal ID="ltlResumenMedico" runat="server"></asp:Literal></p>
                                        <p><strong><i class="fa-solid fa-calendar-day me-2 text-success"></i>Fecha:</strong> <asp:Literal ID="ltlResumenFecha" runat="server"></asp:Literal></p>
                                        <p><strong><i class="fa-solid fa-clock me-2 text-warning"></i>Horario:</strong> <asp:Literal ID="ltlResumenHorario" runat="server"></asp:Literal></p>
                                        <p><strong><i class="fa-solid fa-money-bill-wave me-2 text-success"></i>Monto Pagado:</strong> S/. <asp:Literal ID="ltlResumenMonto" runat="server"></asp:Literal></p>
                                        <p><i class="text-message"></i>El comprobante de pago fue enviado a <asp:Literal ID="ltlResumenMensaje" runat="server"></asp:Literal></p>
                                    </div>
                                </asp:Panel>
                                
                                <hr id="hrMensajeResultado" runat="server" visible="false"/>
                                <asp:Button ID="btnVolverInicio" runat="server" Text="Volver al Inicio" CssClass="btn btn-primary mt-2" OnClick="btnVolverInicio_Click" Visible="false"/>
                                <asp:Button ID="btnVerMisCitas" runat="server" Text="Ver Mis Citas" CssClass="btn btn-info mt-2 ms-2" OnClick="btnVerMisCitas_Click" Visible="false"/>
                             </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src='<%= ResolveUrl("~/Scripts/pagoCita.js") %>' type="text/javascript"></script>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            const elementIds = {
                cardNumberId: '<%= txtNumeroTarjeta.ClientID %>',
                expiryDateId: '<%= txtFechaExpiracion.ClientID %>'
            };
            initializePaymentForm(elementIds);
        });
    </script>
</asp:Content>