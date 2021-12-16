
$('body').on('click', '#btn_add', function () {
    debugger
    $(this).attr('disabled');
    let Title = $('#txt_title').val();
    let Amount = $('#txt_amount').val();
    let Date = $('#txt_date').val();
    if (Title == "") {
        alert("Enter Title..");
        $('#txt_title').focus();
        return false;
    }
    if (Amount == "") {
        alert("Enter Amount..");
        $('#txt_amount').focus();
        return false;
    }
    if (Date == "") {
        alert("Enter Date..");
        $('#txt_date').focus();
        return false;
    }

    let obj = new Object();
    obj.Title = Title.toString();
    obj.Amount = Amount.toString();
    obj.Date = Date.toString();
    if(confirm('Are you shure ?')) {
        $.ajax({
            url: '/home/InsertExpence',
            type: 'post',
            data: JSON.stringify(obj),
            dataType: 'json',
            contentType: 'application/json charset=UTF-8',
            success: function (data) {
                if (data.responseCode == 200) {
                    alert('Successfully Done..');
                }
                else {
                    alert(data.responseMessage);
                }
            },
            error: function (xhr) {
                console.log(xhr);
            }
        });
    }
    $(this).removeAttr('disabled');
});