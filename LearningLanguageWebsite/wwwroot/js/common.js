async function makePostRequest(url = '', data = {}) {
    const response = await fetch(url, {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            'X-Csrf-Token-Value': csrfToken
        },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify(data)
    });
    if (response.status > 299 || response.status < 200) {
        return { errorCode: response.status, text: await response.text(), error: 'status_not_success' };
    }
    return response.json();
}

async function makeGetRequest(url = '') {
    const response = await fetch(url, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        redirect: 'follow',
        headers: {
            'X-Csrf-Token-Value': csrfToken
        },
        referrerPolicy: 'no-referrer'
    });
    if (response.status > 299 || response.status < 200) {
        return { errorCode: response.status, text: await response.text(), error: 'status_not_success' };
    }
    return response.json();
}

function showMessagesFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const successMessage = urlParams.get('success');
    const errorMessage = urlParams.get('error');

    switch (true) {
        case urlParams.has('success'):
            toastr.success(successMessage); //TODO TRANSLATE
            break;
        case urlParams.has('error'):
            toastr.error(errorMessage); //TODO TRANSLATE
            break;
        default:
            break;
    }
}

function translateCode(code) {
    return code;
}

(function () {
    const currentAction = window.location.href.substring(window.location.href.lastIndexOf('/') + 1).toLocaleLowerCase();
    document.querySelectorAll('ul.navbar-nav').forEach(x => {
        x.querySelectorAll('a').forEach(a => {
            const action = a.href.substring(a.href.lastIndexOf('/') + 1).toLocaleLowerCase();
            if (action == currentAction) {
                a.classList.add('active');
            }
        });
    });
})();