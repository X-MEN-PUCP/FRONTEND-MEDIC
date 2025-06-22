function initializeGlobalLoadingIndicators(spinnerId, ajaxDelayInMs, fullPageNavSelector) {
    const spinnerElement = document.getElementById(spinnerId);

    if (!spinnerElement) {
        console.error(`Elemento spinner con ID '${spinnerId}' no fue encontrado.`);
        return;
    }
    const navLinks = document.querySelectorAll(fullPageNavSelector);
    navLinks.forEach(function (link) {
        link.addEventListener('click', function (event) {
            spinnerElement.style.display = 'block';
        });
    });

    if (typeof (Sys) !== 'undefined' && typeof (Sys.WebForms) !== 'undefined' && typeof (Sys.WebForms.PageRequestManager) !== 'undefined') {
        const prm = Sys.WebForms.PageRequestManager.getInstance();
        let requestTimer;

        prm.add_beginRequest(function (sender, args) {
            requestTimer = setTimeout(function () {
                spinnerElement.style.display = 'block';
            }, ajaxDelayInMs);
        });

        prm.add_endRequest(function (sender, args) {
            clearTimeout(requestTimer);
            spinnerElement.style.display = 'none';
        });
    }
}