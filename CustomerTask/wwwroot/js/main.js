
var tab = $('.nav-link.active').attr('id');
$('#listtab').on('click', function () {

    var link = document.createElement('a');
    link.href = '/Home/Index';

    link.click();
})
$('.deletecustomer').on('click', function () {
    ids = [];
    if (tab == "listtab") {
        if ($('.customerrow').hasClass('selectthisrow')) {
            $('.selectthisrow').each(function () {
                var id = $(this).data('name');
                ids.push(id);
            });
        } else {
            Swal.fire({
                position: "top-end",
                icon: "warning",
                title: "Select At Least One",
                showConfirmButton: false,
                timer: 500
            });
            return;
        }
    }
    else {
        var acid = $('input[name="Id"]').val();
        ids.push(acid)
    }
    $.ajax({
        url: '/Home/DeleteCustomerlist',
        data: { ids },
        type: 'POST',
        success: function () {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Deleted",
                showConfirmButton: false,
                timer: 1500
            });
        }
    })
    setTimeout(function () {
        location.reload();
    }, 1000)
})

$('.addcustomer').on('click', function () {
    var link = document.createElement('a');
    link.href = "/Home/OpenDetailForm?id=0"
    link.click();

})
function hideThisDialog() {
    document.getElementById('adddialog').close();
}