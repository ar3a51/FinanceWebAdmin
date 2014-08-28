function ajaxSend(url,
                           data,
                           dataType,
                           traditional,
                           successFn,
                           completeFn
                           ) {

    $.ajax({
        type: "post",
        url: url, //"/EventMonitoringAdmin/user/add",
        data: data, //user,
        dataType: dataType, //"json",
        traditional: traditional,
        success: successFn,     // success         
        complete: completeFn,
        beforeSend: function (xhr)
        {
            xhr.withCredentials = true;
        }

    });

}


