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
    var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    user.firstName = $('#firstname').val();
    user.lastName = $('#lastname').val();
    user.email = $('#email').val();
    user.password = $('#password').val();
    user.mobile = $('#mobile').val();
    user.gender = $('input[name=Gender]:checked').val();
    var validationMsg = "";
    debugger;
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
    if (user.mobile == '' || user.mobile.length < 10 || user.mobile.length > 10) {
        if (validationMsg.length > 1) {
            validationMsg += ", ";
        }
        validationMsg += "Mobile";
    }
    if (validationMsg.length > 1) {
        alert("Please enter valid " + validationMsg + ". this all are required fields");
        return false;
    }
    $.ajax({
        url: "RegisterUser",
        type: "POST",
        data: { user: user },
        dataType: "json",
        cache: false,
        success: function (dataResult) {
            window.location.href = "/event/UserEventList?type=" + 1;
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
                alert("This email address is already exist! Try another email.");
                return false;
            }
            getUserModel();
        }
    });
}

function LoginUser() {
    var email = $('#email').val();
    var password = $('#password').val();
    $.ajax({
        url: 'GetLoginDetailByEmail',
        type: 'GET',
        data: { email: email, password: password },
        success: function (data) {
            debugger
            if (data != undefined) {
                window.location.href = "/event/UserEventList?type="+1;
            } else {
                alert("enter valid email and password")
            }
        }
    });
}

