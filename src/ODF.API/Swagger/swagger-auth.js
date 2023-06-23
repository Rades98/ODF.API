document.addEventListener('DOMContentLoaded', function () {
    let loginButton = document.createElement('button');
    loginButton.setAttribute('id', 'custom-auth-link');
    loginButton.innerHTML = 'Login';
    loginButton.style.cursor = 'pointer';
    loginButton.style.padding = '10px';
    loginButton.style.backgroundColor = '#4CAF50';
    loginButton.style.color = 'white';
    loginButton.style.border = 'none';
    loginButton.style.borderRadius = '3px';
    loginButton.style.cursor = 'pointer';
    loginButton.addEventListener('click', showAuthModal);

    let logoutButton = document.createElement('button');
    logoutButton.setAttribute('id', 'logout-button');
    logoutButton.innerHTML = 'Logout';
    logoutButton.style.cursor = 'pointer';
    logoutButton.style.padding = '10px';
    logoutButton.style.backgroundColor = '#FF0000';
    logoutButton.style.color = 'white';
    logoutButton.style.border = 'none';
    logoutButton.style.borderRadius = '3px';
    logoutButton.style.cursor = 'pointer';
    logoutButton.style.marginLeft = '10px';

    setTimeout(appendTopBar, 1000);

    function appendTopBar() {
        let authBar = document.querySelector('.swagger-ui .main');
        if (authBar) {
            console.log('Append child');
            authBar.style.display = 'flex';
            authBar.style.justifyContent = 'flex-end';
            authBar.style.alignItems = 'center';

            let spacer = document.createElement('div');
            spacer.style.flexGrow = '1';

            authBar.appendChild(spacer);
            authBar.appendChild(loginButton);
            authBar.appendChild(logoutButton);
        }
    }

    function showAuthModal() {
        let overlay = createOverlay();
        let form = createAuthForm();
        let closeButton = createCloseButton();

        form.appendChild(closeButton);
        overlay.appendChild(form);
        document.body.appendChild(overlay);

        closeButton.addEventListener('click', function () {
            document.body.removeChild(overlay);
        });
    }

    function createOverlay() {
        let overlay = document.createElement('div');
        overlay.setAttribute('id', 'auth-overlay');
        overlay.style.position = 'fixed';
        overlay.style.top = '0';
        overlay.style.left = '0';
        overlay.style.width = '100%';
        overlay.style.height = '100%';
        overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
        overlay.style.display = 'flex';
        overlay.style.justifyContent = 'center';
        overlay.style.alignItems = 'center';

        return overlay;
    }

    function createAuthForm() {
        let formContainer = document.createElement('div');
        formContainer.setAttribute('id', 'auth-form-container');
        formContainer.style.backgroundColor = '#f8f8f8';
        formContainer.style.borderRadius = '5px';
        formContainer.style.padding = '20px';
        formContainer.style.boxShadow = '0 2px 4px rgba(0, 0, 0, 0.2)';
        formContainer.style.width = '300px';

        let form = document.createElement('form');
        form.setAttribute('id', 'auth-form');

        let usernameLabel = document.createElement('label');
        usernameLabel.setAttribute('for', 'username');
        usernameLabel.innerHTML = 'Username: ';
        usernameLabel.style.marginBottom = '10px';
        usernameLabel.style.fontWeight = 'bold';

        let usernameInput = document.createElement('input');
        usernameInput.setAttribute('type', 'text');
        usernameInput.setAttribute('id', 'username');
        usernameInput.setAttribute('name', 'username');
        usernameInput.style.display = 'block';
        usernameInput.style.marginBottom = '10px';
        usernameInput.style.width = '100%';
        usernameInput.style.padding = '8px';
        usernameInput.style.border = '1px solid #ccc';
        usernameInput.style.borderRadius = '3px';

        let passwordLabel = document.createElement('label');
        passwordLabel.setAttribute('for', 'password');
        passwordLabel.innerHTML = 'Password: ';
        passwordLabel.style.marginBottom = '10px';
        passwordLabel.style.fontWeight = 'bold';

        let passwordInput = document.createElement('input');
        passwordInput.setAttribute('type', 'password');
        passwordInput.setAttribute('id', 'password');
        passwordInput.setAttribute('name', 'password');
        passwordInput.style.display = 'block';
        passwordInput.style.marginBottom = '10px';
        passwordInput.style.width = '100%';
        passwordInput.style.padding = '8px';
        passwordInput.style.border = '1px solid #ccc';
        passwordInput.style.borderRadius = '3px';

        let submitButton = document.createElement('button');
        submitButton.setAttribute('type', 'button');
        submitButton.setAttribute('id', 'auth-submit');
        submitButton.innerHTML = 'Submit';
        submitButton.style.width = '100%';
        submitButton.style.padding = '10px';
        submitButton.style.backgroundColor = '#4CAF50';
        submitButton.style.color = 'white';
        submitButton.style.border = 'none';
        submitButton.style.borderRadius = '3px';
        submitButton.style.cursor = 'pointer';

        form.appendChild(usernameLabel);
        form.appendChild(usernameInput);
        form.appendChild(passwordLabel);
        form.appendChild(passwordInput);
        form.appendChild(submitButton);

        formContainer.appendChild(form);

        return formContainer;
    }

    function createCloseButton() {
        let closeButton = document.createElement('button');
        closeButton.setAttribute('type', 'button');
        closeButton.innerHTML = 'Close';
        closeButton.style.padding = '5px 10px';
        closeButton.style.backgroundColor = '#f8f8f8';
        closeButton.style.border = 'none';
        closeButton.style.borderRadius = '3px';
        closeButton.style.cursor = 'pointer';
        closeButton.style.marginBottom = '10px';
        closeButton.style.color = '#777';

        return closeButton;
    }

    document.addEventListener('click', function (event) {
        if (event.target && event.target.id === 'auth-submit') {
            login();
        } else if (event.target && event.target.id === 'logout-button') {
            logout();
        }
    });

    function login() {
        let username = document.getElementById('username').value;
        let password = document.getElementById('password').value;

        let xhr = new XMLHttpRequest();
        xhr.open('POST', '/cz/user', true);
        xhr.setRequestHeader('Content-Type', 'application/json');

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {

                    // Hide the modal dialog
                    let overlay = document.getElementById('auth-overlay');
                    if (overlay) {
                        document.body.removeChild(overlay);
                    }

                    return; // Exit the function to prevent showing the alert
                } else {
                    document.getElementById('username').value = '';
                    document.getElementById('password').value = '';

                    alert('Error: ' + xhr.responseText);
                }
            }
        };

        let formData = {
            userName: username,
            password: password
        };
        xhr.send(JSON.stringify(formData));
    }

    function logout() {
        let xhr = new XMLHttpRequest();
        xhr.open('DELETE', '/cz/user', true);

        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    alert('Logout successful!');
                    return;
                } else {
                    alert('Error: ' + xhr.responseText);
                }
            }
        };

        xhr.send();
    }
});
