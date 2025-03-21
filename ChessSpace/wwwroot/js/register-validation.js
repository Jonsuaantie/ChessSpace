document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('registerForm');
    if (form) {
        form.addEventListener('submit', function (event) {
            var password = document.querySelector('input[name="Password"]').value;
            var passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

            if (!passwordPattern.test(password)) {
                event.preventDefault();
                alert('Password must be at least 8 characters long, contain at least one number, one uppercase letter, and one special character.');
            }
        });
    }
});