function initializeAjaxLoadingSpinner(spinnerId, delayInMs) {
    if (typeof (Sys) === 'undefined' || typeof (Sys.WebForms) === 'undefined' || typeof (Sys.WebForms.PageRequestManager) === 'undefined') {
        console.error("ASP.NET AJAX PageRequestManager no está disponible. El spinner de carga no funcionará.");
        return;
    }

    const prm = Sys.WebForms.PageRequestManager.getInstance();
    let requestTimer;

    prm.add_beginRequest(function (sender, args) {
        requestTimer = setTimeout(function () {
            const spinnerElement = document.getElementById(spinnerId);
            if (spinnerElement) {
                spinnerElement.style.display = 'block';
            }
        }, delayInMs);
    });

    prm.add_endRequest(function (sender, args) {
        clearTimeout(requestTimer);
        const spinnerElement = document.getElementById(spinnerId);
        if (spinnerElement) {
            spinnerElement.style.display = 'none';
        }
    });
}