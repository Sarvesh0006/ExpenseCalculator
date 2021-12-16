$('body').on('click', '#btn_login', function () {
    let UserName = $('#txt_userid').val();
    let Password = $('#txt_password').val();
    if (UserName == '') {
        alert("Enter User Name..");
        $('#txt_userid').focus();
        return false;
    }
    if (Password == '') {
        alert("Enter User Name..");
        $('#txt_password').focus();
        return false;
    }
    let Obj = new Object();
    Obj.UserName = UserName.toString();
    Obj.Password = Password.toString();
    $.ajax({
        url: '/home/AccountLogin',
        type: 'post',
        data: JSON.stringify(Obj),
        dataType: 'json',
        contentType: 'application/json charset=UTF-8',
        success: function (data) {
            if (data.responseCode == 200) {
                window.sessionStorage.setItem('uname', data.UserName);
                window.location.href = '/Home/Dashboard';
            }
            else {
                alert(data.responseMessage);
            }
        },
        error: function (xhr) {
            console.log(xhr);
        }
    });
});
