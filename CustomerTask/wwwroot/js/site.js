$('.nav-tabs').append('<li class="nav-item" role="presentation">                    <button class="nav-link fw-bolder" id="detailtab" data-bs-toggle="tab" data-bs-target="#detail" type="button" role="tab" aria-selected="false"><i class="fas fa-th-list"></i>  Details</button>                </li>')
$('.nav-link').removeClass('active')
$('#detailtab').addClass('active')
$('.searchdiv').hide()
$('#listdiv').hide()
var AccountNoDrop = $('#acdrop');
var acno = $('#acdrop').val();
function putdata(acno) {
    $.ajax({
        url: '/Home/GetInfoOfAC',
        data: { acno },
        type: 'POST',
        success: function (response) {
            console.log(response);
            try {
                $('#customerid').text(response.ac);
                $('#customername').text(response.name);
                $('#company').text(response.name);
                $('#address1').text(response.address1);
                $('#address2').text(response.address2);
                $('#town').text(response.town);
                $('#county').text(response.county);
                $('#postcode').text(response.postcode);
                $('#country').text(response.country);
                $('#telephone').text(response.telephone);
                $('#email').text(response.email);
                $('#thisid').val(response.id);
                if (response.issubscribe) {
                    $('#mailinglist').text("Subscribed");
                }
                else {
                    $('#mailinglist').text("Unsubscribed");
                }
                $('.editthis').attr('data-id', response.ac)
            }
            catch {
                $('.recorddiv').hide();
                $('#customerid').text('0');
                $('#customername').text("No Records Found");
            }

        }
    })
}
putdata(acno);
$('.editthis').on('click', function () {
    var id = $('#thisid').val();
    $.ajax({
        url: '/Home/OpenDetailForm',
        data: { id },
        success: function (res) {
            $('.popup').html(res)
            $('#adddialog').addClass('foredit');
            document.getElementById('adddialog').show();
        }
    })
})


$('#OpenGroupDialog').on('click', function () {
    var id = $('#thisid').val();
    $.ajax({
        url: '/Home/OpenGroupModal',
        data: { id },
        type: 'POST',
        success: function (res) {
            $('#editgroupmodel').html(res);
            document.getElementById('editgroupdialog').show();
        }
    })
})