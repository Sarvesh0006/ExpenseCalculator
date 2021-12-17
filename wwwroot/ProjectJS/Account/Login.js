$('body').on('click', '#btn_login', function () {
    $(this).attr('disabled');
    let UserName = $('#txt_userid').val();
    let Password = $('#txt_password').val();
    if (UserName == '') {
        alert("Enter User Name..");
        $('#txt_userid').focus();
        $(this).removeAttr('disabled');
        return false;
    }
    if (Password == '') {
        alert("Enter Password..");
        $('#txt_password').focus();
        $(this).removeAttr('disabled');
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
            debugger
            $(this).removeAttr('disabled');
            if (data.responseCode == 200) {
                window.sessionStorage.setItem('uname', data.UserName);
                window.location.href = '/Home/Dashboard';
            }
            else {
                alert(data.responseMessgae);
            }
        },
        error: function (xhr) {
            $(this).removeAttr('disabled');
            console.log(xhr);
        }
    });
});
