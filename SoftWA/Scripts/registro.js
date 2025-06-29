function initializeRegistrationForm(ids) {
    const hdnField = document.getElementById(ids.hdnFieldId);
    const btnDNI = document.getElementById(ids.btnDniId);
    const btnCE = document.getElementById(ids.btnCeId);
    const dniContainer = document.getElementById(ids.dniContainerId);
    const ceContainer = document.getElementById(ids.ceContainerId);
    const txtDNI = document.getElementById(ids.txtDniId);
    const txtCE = document.getElementById(ids.txtCeId);
    function selectDocumentType(docType) {
        hdnField.value = docType;
        const isDni = (docType === 'DNI');

        btnDNI.classList.toggle('active', isDni);
        btnCE.classList.toggle('active', !isDni);
        dniContainer.style.display = isDni ? 'block' : 'none';
        ceContainer.style.display = isDni ? 'none' : 'block';
        txtDNI.disabled = !isDni;
        txtCE.disabled = isDni;

        if (isDni) txtCE.value = '';
        else txtDNI.value = '';
    }

    btnDNI.addEventListener('click', () => selectDocumentType('DNI'));
    btnCE.addEventListener('click', () => selectDocumentType('CE'));

    selectDocumentType(hdnField.value);
}