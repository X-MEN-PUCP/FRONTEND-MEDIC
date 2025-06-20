function initializeRegistrationForm(ids) {
    const hdnField = document.getElementById(ids.hdnFieldId);
    const btnDNI = document.getElementById(ids.btnDniId);
    const btnCE = document.getElementById(ids.btnCeId);
    const dniContainer = document.getElementById(ids.dniContainerId);
    const ceContainer = document.getElementById(ids.ceContainerId);
    const txtDNI = document.getElementById(ids.txtDniId);
    const txtCE = document.getElementById(ids.txtCeId);
    const rfvDNIValidator = document.getElementById(ids.rfvDniId);
    const revDNIValidator = document.getElementById(ids.revDniId);
    const rfvCEValidator = document.getElementById(ids.rfvCeId);
    const revCEValidator = document.getElementById(ids.revCeId);

    function selectDocumentType(docType) {
        hdnField.value = docType;

        const isDni = (docType === 'DNI');
        btnDNI.classList.toggle('active', isDni);
        btnCE.classList.toggle('active', !isDni);
        dniContainer.style.display = isDni ? 'block' : 'none';
        ceContainer.style.display = isDni ? 'none' : 'block';
        txtDNI.disabled = !isDni;
        txtCE.disabled = isDni;
        if (isDni) {
            txtCE.value = '';
        } else {
            txtDNI.value = '';
        }

        if (typeof (ValidatorEnable) === 'function') {
            ValidatorEnable(rfvDNIValidator, isDni);
            ValidatorEnable(revDNIValidator, isDni);
            ValidatorEnable(rfvCEValidator, !isDni);
            ValidatorEnable(revCEValidator, !isDni);
        }
    }

    btnDNI.addEventListener('click', function () {
        selectDocumentType('DNI');
    });

    btnCE.addEventListener('click', function () {
        selectDocumentType('CE');
    });

    if (hdnField && hdnField.value) {
        selectDocumentType(hdnField.value);
    }
}