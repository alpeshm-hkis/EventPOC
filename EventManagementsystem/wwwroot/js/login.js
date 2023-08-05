$(document).ready(function () {

});

function getUserModel() {
    $.ajax({
        url: "UserModel",
        type: "GET",
        async: false,
        cache: false,
        success: function (dataResult) {
            registerUser(dataResult);        
        }
    });
}

function registerUser(user) {
    user.firstName = $('#firstname').val();
    user.lastName = $('#lastname').val();
    user.email = $('#email').val();
    user.password = $('#password').val();
    user.mobile = $('#mobile').val();
    user.gender = $('input[name=Gender]:checked').val()
    debugger
    $.ajax({
        url: "RegisterUser",
        type: "POST",
        data: { user: user },
        dataType: "json",
        cache: false,
        success: function (dataResult) {
            Response.redirect('index.cshtml');
        }
    });
}

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
            if (dataResult == true) {
                return false;
            }
            getUserModel();
        }
    });
}

function LoginUser() {
    debugger;
    var email = $('#email').val();
    var password = $('#password').val();
    $.ajax({
        url: 'GetLoginDetailByEmail',
        type: 'GET',
        data: { email: email, password: password },
        success: function (data) {
            debugger
            if (data != undefined) {
                Response.redirect('event/index');
                window.location.href = "/event/UserEventList?type="+1;
            } else {
                alert("enter valid email and password")
            }
        }
    });
}
