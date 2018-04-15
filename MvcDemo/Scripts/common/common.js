/*
功能說明:顯示開窗Dialog
參數說明:請傳入物件, 參數有title, partialView width, height
*/
function showDialog(title, width, height) {
    try {

        $("#dialog").dialog({
            modal: true, //model:遮罩預設False
            title: title,
            width: width,
            height: height
        });
    }
    catch(err)
    {
        alert('開窗畫面讀取失敗!請聯絡系統管理員');
    }
}

/*
功能說明:隱藏開窗Dialog
*/
function hideDialog() {
    $("#dialog").dialog('close');
}

/*
功能說明:讀取開窗Dialog要用到的PartialView
參數說明:Router 呼叫的Action
         Data 要傳遞的參數(物件型態)
         OnSuccess callBack Function
*/
function loadDialogPartial(Router,Data,OnSuccess) {
    $('#dialog').html('');
    $.ajax({
        url: Router,
        type: 'Post',
        cache: false,
        dataType: 'html',
        data: Data
    }).done(function (result) {
        $('#dialog').html(result);
        OnSuccess();
    });


}