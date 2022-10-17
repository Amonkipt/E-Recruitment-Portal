﻿function jQueryAjaxPost(form) {
    $.validator.unobstrusive.parse(form);
    alert("Zinaenda")
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                alert("imefika");
                if (response.success) {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true)

                    //succes message
                    Swal({
                        icon: 'info',
                        title: 'Saved',
                        text: response.message
                    });
                }
                else {
                    //error message
                    Swal({
                        icon: 'error',
                        title: 'Failed!',
                        text: response.message
                    });
                }
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processType"] = false;
        }
        $.ajax(ajaxConfig);

    }
    alert("kaesio")
    return false;
}

function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#secondTab").html(response);
        }
    });
}
