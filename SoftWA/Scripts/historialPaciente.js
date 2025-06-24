function mostrarDetallesCita(clickedCard) {
    if (!clickedCard) return;

    const detalleModalElement = document.getElementById('modalDetalleCitaHistorial');
    if (!detalleModalElement) return;

    const detalleModal = bootstrap.Modal.getOrCreateInstance(detalleModalElement);

    const setText = (id, text) => {
        const el = document.getElementById(id);
        if (el) el.textContent = text || 'N/A';
    };

    const decodeHtml = (html) => {
        if (!html) return '';
        const txt = document.createElement('textarea');
        txt.innerHTML = html;
        return txt.value;
    };

    setText('modalEspecialidad', clickedCard.dataset.especialidad);
    setText('modalMedico', clickedCard.dataset.medico);
    setText('modalFecha', clickedCard.dataset.fecha);
    setText('modalHorario', clickedCard.dataset.horario);
    setText('modalEstado', clickedCard.dataset.estado);
    setText('modalMotivo', decodeHtml(clickedCard.dataset.motivo));
    setText('modalPresion', clickedCard.dataset.presion);
    setText('modalTemperatura', clickedCard.dataset.temperatura);
    setText('modalPeso', clickedCard.dataset.peso);
    setText('modalTalla', clickedCard.dataset.talla);
    setText('modalEvolucion', decodeHtml(clickedCard.dataset.evolucion));
    setText('modalTratamiento', decodeHtml(clickedCard.dataset.tratamiento));
    setText('modalReceta', decodeHtml(clickedCard.dataset.receta));
    setText('modalRecomendacion', decodeHtml(clickedCard.dataset.recomendacion));
    detalleModal.show();
}