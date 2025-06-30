function mostrarModalConfirmacionReserva(idCita, especialidad, medico, fecha, horario, precio, hfId) {
    try {
        document.getElementById(hfId).value = idCita;
        const setText = (id, text) => {
            const el = document.getElementById(id);
            if (el) {
                el.textContent = text || 'N/A';
            } else {
                console.error(`Elemento con ID '${id}' no fue encontrado en el modal.`);
            }
        };

        setText('modalConfirmEspecialidad', especialidad);
        setText('modalConfirmMedico', medico);
        setText('modalConfirmFecha', fecha);
        setText('modalConfirmHorario', horario);
        setText('modalConfirmPrecio', "S/. " + precio);

        const modalElement = document.getElementById('confirmarReservaModal');
        if (modalElement) {
            const myModal = new bootstrap.Modal(modalElement);
            myModal.show();
        } else {
            console.error("El elemento del modal 'confirmarReservaModal' no fue encontrado.");
        }
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