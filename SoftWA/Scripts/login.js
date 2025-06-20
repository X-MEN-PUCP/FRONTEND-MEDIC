function initializeLoginForm(ids) {
    const hdnField = document.getElementById(ids.hdnFieldId);
    const btnDNI = document.getElementById(ids.btnDniId);
    const btnCE = document.getElementById(ids.btnCeId);
    const txtDocumento = document.getElementById(ids.txtDocumentoId);
    const revDNIValidator = document.getElementById(ids.revDniId);
    const revCEValidator = document.getElementById(ids.revCeId);

    if (!btnDNI || !btnCE) {
        console.error("No se pudieron encontrar los botones del toggle. Revisa los IDs.");
        return;
    }

    function selectDocumentType(docType) {
        hdnField.value = docType;
        txtDocumento.value = '';

        const isDni = (docType === 'DNI');
        btnDNI.classList.toggle('active', isDni);
        btnCE.classList.toggle('active', !isDni);

        if (isDni) {
            txtDocumento.setAttribute('placeholder', 'Ingrese su DNI');
            txtDocumento.setAttribute('maxlength', '8');
        } else {
            txtDocumento.setAttribute('placeholder', 'Ingrese su Carnet de Extranjería');
            txtDocumento.setAttribute('maxlength', '12');
        }

        if (typeof (ValidatorEnable) === 'function') {
            ValidatorEnable(revDNIValidator, isDni);
            ValidatorEnable(revCEValidator, !isDni);
        }

        txtDocumento.focus();
    }

    btnDNI.addEventListener('click', function () {
        selectDocumentType('DNI');
    });

    btnCE.addEventListener('click', function () {
        selectDocumentType('Carnet de Extranjeria');
    });

    if (hdnField && hdnField.value) {
        selectDocumentType(hdnField.value);
    }
    const togglePasswordButton = document.getElementById('togglePassword');
    const togglePasswordIcon = document.getElementById('togglePasswordIcon');
    const passwordField = document.getElementById(ids.txtPasswordId);
    if (togglePasswordButton && passwordField) {
        togglePasswordButton.addEventListener('click', function () {
            const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordField.setAttribute('type', type);
            togglePasswordIcon.classList.toggle('fa-eye');
            togglePasswordIcon.classList.toggle('fa-eye-slash');
        });
    }
}