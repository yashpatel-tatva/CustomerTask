
var tab = $('.nav-link.active').attr('id');
$('#listtab').on('click', function () {

    var link = document.createElement('a');
    link.href = '/Home/Index';

    link.click();
})
$('.deletecustomer').on('click', function () {
    if (tab == "listtab") {
               ids = [];
        if ($('.customerrow').hasClass('selectthisrow')) {
            $('.selectthisrow').each(function () {
                var id = $(this).data('name');
                ids.push(id);
            });
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
        } else {
            Swal.fire({
                position: "top-end",
                icon: "warning",
                title: "Select At Least One",
                showConfirmButton: false,
                timer: 500
            });
        }
    }
    else {
        var acno = $('#acdrop').val();
        deletethis(acno);
        setTimeout(function () {
            location.reload();
        }, 1000)
    }
})




$('.addcustomer').on('click', function () {
    //$.ajax({
    //    url: '/Home/OpenDetailForm',
    //    data: { id: 0 },
    //    success: function (res) {
    //        $('.popup').html(res)
    //        $('#adddialog').addClass('foradd');
    //        document.getElementById('adddialog').show();
    //    }
    //}) 

    var link = document.createElement('a');
    link.href = "/Home/OpenDetailForm?id=0"
    link.click();

})
function hideThisDialog() {
    document.getElementById('adddialog').close();
}
function deletethis(acno) {
    $.ajax({
        url: '/Home/DeleteCustomer',
        data: { acno },
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
}
