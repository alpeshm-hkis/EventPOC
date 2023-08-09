$(document).ready(function () {
    $(document).on("keyup", function (e) {
        if (window.location.pathname == '/home/login') {
            if (e.which == 13) // the enter key ascii code
            {
                LoginUser();
            }
        }
    });
});

// Get User default model for Create User form
function getUserModel() {
    $.ajax({
        url: "UserModel",
        type: "GET",
        async: false,
        cache: false,
        success: function (dataResult) {
            // Call register user method for register user data
            registerUser(dataResult);        
        }
    });
}

// Add new User Detail in DB

function registerUser(user) {
    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    user.firstName = $('#firstname').val();
    user.lastName = $('#lastname').val();
    user.email = $('#email').val();
    user.password = $('#password').val();

    // Validation for User detail
    var validationMsg = "";
    ;
    if (user.firstName == "") {
        validationMsg = "First Name";
    }
    if (user.lastName == "") {
        if (validationMsg.length > 1) {
            validationMsg += ", ";
        }
        validationMsg += "Last Name";
    }
    if (user.email == '' && !user.email.match(validRegex)) {
        if (validationMsg.length > 1) {
            validationMsg += ", ";
        }
        validationMsg += "Email";
    }
    if (user.password == '' || user.password.length < 1) {
        if (validationMsg.length > 1) {
            validationMsg += ", ";
        }
        validationMsg += "Password";
    }
    if (validationMsg.length > 1) {
        alert("Please enter valid " + validationMsg + ". this all are required fields");
        return false;
    }
    $.ajax({
        url: "RegisterUser",
        type: "POST",
        data: { user: user },
        success: function (dataResult) {
            //If User succesfully Created
            if (dataResult != null) {
                $('#confirm').modal("show");
                $($('#contextMsg').parent()).prepend("<i class='bi bi-check-circle-fill' style='color: #ff0000; font-size: 60px;'></i>");
                $('#contextMsg').text("User Created Successfully!");
                $("#success").on('click', function (e) {
                    $($('#contextMsg').parent()).first().remove();
                    window.location.href = "/event/UserEventList?type=" + 1;
                });
            } else {
                $('#confirm').modal("show");
                $('#contextMsg').text("User does not created!")
                $("#success").on('click', function (e) {
                    $('#confirm').modal('hide');
                });
            }            
        }
    });
}


// Call Email validation method for validate dublicate email
function emailValidate() {
    var email = $('#email').val();
    $.ajax({
        url: "GetEmailValidate",
        type: "GET",
        data: {
            email: email
        },
        cache: false,
        success: function (dataResult) {
            // If email is already exist then return false
            if (dataResult == true) {
                $('#confirm').modal("show");
                $($($('#confirm').children()).children()).css('width', '600px');
                $($($('#confirm').children()).children()).css('margin-left', '100px');
                $('#contextMsg').text("This email address is already exist!Please try another email.");
                $("#success").on('click', function (e) {
                    $('#confirm').modal('hide');
                    $($($('#confirm').children()).children()).removeAttr('style');
                    return false;
                });
            } else {
                getUserModel();
            }            
            
        }
    });
}

// Call Login method for login user
function LoginUser() {
    var email = $('#email').val();
    var password = $('#password').val();
    if (email.length > 0 && password.length > 0) {

        $.ajax({
            url: 'GetLoginDetailByEmail',
            type: 'GET',
            data: { email: email, password: password },
            success: function (data) {
                if (data != undefined) {
                    window.location.href = "/event/UserEventList?type=" + 1;
                } else {
                    $('#confirm').modal("show");
                    $('#contextMsg').text("please enter valid email and password.");
                    $("#success").on('click', function (e) {
                        $('#confirm').modal('hide');
                        return false;
                    });
                }
            }
        });
    } else {
        $('#confirm').modal("show");
        $('#contextMsg').text("please enter valid email and password.");
        $("#success").on('click', function (e) {
            $('#confirm').modal('hide');
        });
    }
}

