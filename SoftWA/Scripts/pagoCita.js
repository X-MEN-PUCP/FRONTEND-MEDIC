function initializePaymentForm(ids) {
    const cardNumberInput = document.getElementById(ids.cardNumberId);
    if (cardNumberInput) {
        cardNumberInput.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            let formattedValue = '';
            for (let i = 0; i < value.length; i++) {
                if (i > 0 && i % 4 === 0) {
                    formattedValue += ' ';
                }
                formattedValue += value[i];
            }
            e.target.value = formattedValue.substring(0, 19);
        });
    }

    const expiryDateInput = document.getElementById(ids.expiryDateId);
    if (expiryDateInput) {
        expiryDateInput.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            let formattedValue = '';
            if (value.length > 2) {
                formattedValue = value.substring(0, 2) + '/' + value.substring(2, 4);
            } else {
                formattedValue = value;
            }
            e.target.value = formattedValue;
        });
    }
}