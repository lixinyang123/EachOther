﻿function getPassword() {

    var sex = document.querySelector("select").value;
    var url = '/Home/Login/' + sex;

    $.ajax({
        url: url,
        type: 'get',
        dataType: 'text',
        success: function (data) {
            document.querySelector("#pwd").removeAttribute("hidden");
            alert(data);
        },
        error: error => console.log(error)
    });
}