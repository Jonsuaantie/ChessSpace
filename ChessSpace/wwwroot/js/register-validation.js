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


$(function () {
    // Custom password rule
    $.validator.addMethod("strongPassword", function (value, element) {
        return this.optional(element)
            || /[A-Z]/.test(value)      // at least one uppercase
            && /[0-9]/.test(value)      // at least one number
            && /[^A-Za-z0-9]/.test(value) // at least one special char
            && value.length >= 8;
    }, "Password must be at least 8 characters, contain a number, an uppercase letter, and a special character.");

    $("#registerForm").validate({
        rules: {
            UserName: "required",
            Email: {
                required: true,
                email: true,
                remote: {
                    url: "/LoginRegister/IsEmailAvailable",
                    type: "POST",
                    data: {
                        email: function () {
                            return $("#registerForm input[name='Email']").val();
                        }
                    }
                }
            },
            Password: {
                required: true,
                strongPassword: true
            }
        },
        messages: {
            UserName: "Please enter a username",
            Email: {
                required: "Please enter an email address",
                email: "Please enter a valid email address",
                remote: "An account with this email already exists."
            },
            Password: {
                required: "Please enter a password"
            }
        },
        errorClass: "text-properties",
        validClass: "text-properties",
        onkeyup: function(element) { $(element).valid(); },
        onfocusout: function(element) { $(element).valid(); }
    });

    // Disable submit button until form is valid
    const $form = $("#registerForm");
    const $submit = $form.find('button[type="submit"]');
    $submit.prop('disabled', true);

    $form.on('keyup change', function () {
        $submit.prop('disabled', !$form.valid());
    });
});