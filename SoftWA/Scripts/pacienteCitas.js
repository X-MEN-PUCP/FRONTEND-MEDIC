function mostrarModalConfirmacionReserva(citaId) {
    var hf = document.querySelector('[id$="hfModalCitaId"]');
    if (hf) {
        hf.value = citaId;
    }

    var myModal = new bootstrap.Modal(document.getElementById('confirmarReservaModal'), {
        keyboard: false,
        backdrop: 'static'
    });
    myModal.show();

    return false;
}
function cerrarModalConfirmacion() {
    var myModalEl = document.getElementById('confirmarReservaModal');
    if (myModalEl) {
        var myModal = bootstrap.Modal.getInstance(myModalEl);
        if (myModal) {
            myModal.hide();
        }
    }
}