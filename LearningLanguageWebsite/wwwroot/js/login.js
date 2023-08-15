/**
 * Submits the registration data.
 */
function submitRegister() {
    const checkboxes = document.querySelectorAll('.form-check-input');
    const selectedIds = [];

    checkboxes.forEach(checkbox => {
        if (checkbox.checked) {
            selectedIds.push(checkbox.id);
        }
    });

    makePostRequest(registerAccountUrl, {
        username: document.getElementById('userName').value,
        email: document.getElementById('userEmail').value,
        password: document.getElementById('userPassword').value,
        languageid: selectedIds
    })
        .then(data => {
            if (data.error) {
                toastr.error(translateCode(data.error));
                return;
            }

            window.location = loginHomeUrl + '?success=account_created';
        })
        .catch(error => {
            toastr.error(error.toString());
        });
}

/**
 * Submits the login data.
 */
function loginSubmit() {
    makePostRequest(loginAccountUrl, {
        email: document.getElementById('userEmail').value,
        password: document.getElementById('userPassword').value,
        rememberme: document.getElementById('rememberMe').checked
    })
        .then(data => {
            if (data.error) {
                toastr.error(translateCode(data.error));
                return;
            }

            window.location = homeUrl;
        })
        .catch(error => {
            toastr.error(error.toString());
        });
}

/**
 * Sends a password reset request.
 */
function resetPassword() {
    const email = document.getElementById('userEmail');

    if (email.checkValidity()) {
        makePostRequest(passwordAccountResetUrl, { email: email.value })
            .then(data => {
                if (data.error) {
                    toastr.error(translateCode(data.error));
                    return;
                }

                toastr.success(translateCode(data.success));
            })
            .catch(error => {
                toastr.error(error.toString());
            });
    }

    email.parentElement.classList.add('was-validated');
}

/**
 * Validates form and determines action based on the submit button's type.
 */
(function initFormValidation() {
    showMessagesFromUrl();

    const forms = document.querySelectorAll('.needs-validation');

    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            event.preventDefault();
            event.stopPropagation();

            if (form.checkValidity()) {
                switch (event.submitter.dataset.buttontype) {
                    case 'login':
                        loginSubmit();
                        break;
                    case 'register':
                        submitRegister();
                        break;
                }
            }

            form.classList.add('was-validated');
        });
    });
})();