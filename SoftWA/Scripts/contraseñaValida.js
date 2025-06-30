function initializePasswordValidator(passwordInputId, requirementsContainerId) {
    const passwordInput = document.getElementById(passwordInputId);
    const requirementsContainer = document.getElementById(requirementsContainerId);

    if (!passwordInput || !requirementsContainer) return;

    const requirements = [
        { id: 'length', text: 'Al menos 8 caracteres', regex: /.{8,}/ },
        { id: 'uppercase', text: 'Una letra mayúscula (A-Z)', regex: /[A-Z]/ },
        { id: 'lowercase', text: 'Una letra minúscula (a-z)', regex: /[a-z]/ },
        { id: 'number', text: 'Un número (0-9)', regex: /[0-9]/ },
        { id: 'symbol', text: 'Un símbolo (!@#$...)', regex: /[^A-Za-z0-9]/ }
    ];

    let html = '<ul class="list-unstyled password-requirements">';
    requirements.forEach(req => {
        html += `<li id="${req.id}" class="invalid"><i class="fas fa-times-circle me-2"></i>${req.text}</li>`;
    });
    html += '</ul>';
    requirementsContainer.innerHTML = html;
    requirementsContainer.style.display = 'none';

    const validatePassword = () => {
        const password = passwordInput.value;
        requirements.forEach(req => {
            const requirementLi = document.getElementById(req.id);
            if (req.regex.test(password)) {
                requirementLi.className = 'valid';
                requirementLi.innerHTML = `<i class="fas fa-check-circle"></i>${req.text}`;
            } else {
                requirementLi.className = 'invalid';
                requirementLi.innerHTML = `<i class="fas fa-times-circle"></i>${req.text}`;
            }
        });
    };

    passwordInput.addEventListener('focus', () => {
        requirementsContainer.style.display = 'block';
    });

    passwordInput.addEventListener('blur', () => {
        if (passwordInput.value.length === 0) {
            requirementsContainer.style.display = 'none';
        }
    });

    passwordInput.addEventListener('input', validatePassword);
}
function togglePasswordVisibility(inputId, eyeIcon) {
    const input = document.getElementById(inputId);
    if (!input || !eyeIcon) return;

    if (input.type === 'password') {
        input.type = 'text';
        eyeIcon.classList.remove('fa-eye');
        eyeIcon.classList.add('fa-eye-slash');
    } else {
        input.type = 'password';
        eyeIcon.classList.remove('fa-eye-slash');
        eyeIcon.classList.add('fa-eye');
    }
}