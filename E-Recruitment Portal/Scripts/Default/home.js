//Add Line Function
$('#NewEducation').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Add Line Function
$('#NewExperience').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Add Line Function
$('#NewReferee').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Add Line Function
$('#NewMembership').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Add Line Function
$('#NewHobbies').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Add Line Function
$('#NewProfessional').on('click', function (e) {
    e.preventDefault();
    $('#AddLine').prop('hidden', false);
});

//Delete line function
$(document).ready(function () {
    $('.delete').on('click', function (e, data) {
        if (!data) {
            handleDelete(e, 1);
        } else {
            window.location = $(this).attr('href');
        }
    });
});



function handleDelete(e, stop) {
    if (stop) {
        event.preventDefault();
        swal({
            title: "Are you sure?",
            text: "You will not be able to recover this file again!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#2E354E",
            confirmButtonText: "Yes, Continue!",
            cancelButtonText: "No, Cancel",
            closeOnConfirm: false,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {
                $(e.target).trigger('click', {});
            } else {
                swal("Cancelled", "You have Cancelled this Action!", "error");
            }
        });
    }
};