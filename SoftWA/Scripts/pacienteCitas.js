function mostrarModalConfirmacionReserva(idCita, especialidad, medico, fecha, hora, precio, hfId) {
    try {
        document.getElementById('modalConfirmEspecialidad').textContent = especialidad;
        document.getElementById('modalConfirmMedico').textContent = medico;
        document.getElementById('modalConfirmFecha').textContent = fecha;
        document.getElementById('modalConfirmHorario').textContent = hora;
        document.getElementById('modalConfirmPrecio').textContent = "s/ " + parseFloat(precio).toFixed(2);
        const hiddenField = document.getElementById(hfId);
        if (hiddenField) {
            hiddenField.value = idCita;
        }
        const ModalElementoConfirmar = document.getElementById('confirmarReservaModal');
        const ModalConfirmar = new bootstrap.Modal(ModalElementoConfirmar);
        ModalConfirmar.show();
    } catch (e) {
        console.error("Error en mostrarModalConfirmacionReserva:", e);
    }
    return false; 

function cerrarModalConfirmacion() {
    const modalElement = document.getElementById('confirmarReservaModal');
    if (modalElement) {
        const modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        }
    }
}