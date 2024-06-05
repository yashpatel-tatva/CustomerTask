﻿$('.nav-tabs').append('<li class="nav-item" role="presentation">                    <button class="nav-link fw-bolder" id="detailtab" data-bs-toggle="tab" data-bs-target="#detail" type="button" role="tab" aria-selected="false"><i class="fas fa-th-list"></i>  Details</button>                </li>')
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
            console.log(response)
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
            getcontactlist(search)
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
            //document.getElementById('editgroupdialog').show();
        }
    })
})


/// -------------------- Contact --------------------------------///


$('#issubscribeornot').on('click', function () {
    if (!$(this).hasClass('readonly')) {
        $('#maillistdrop').removeClass('d-none')
    }
})
$('#fullname').on('click', function () {
    if ($(this).prop('readonly')) {
        $('#contactsdrop').removeClass('d-none')
    }
})
$('.editcontact').on('click', function () {
    var c = $('#contactid').val();
    openedit(c)
})
function openedit(contactid) {
    document.getElementById('contactsdrop').classList.add('d-none')
    if (contactid == 0) {
        $('.disable').val("")
        $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
        $('#contactid').val(0)
    }
    $('.savecancelcontact').removeClass('d-none')
    $('.disable').not('#issubscribeornot').prop('readonly', false);
    $('#issubscribeornot').removeClass('readonly');
}

function closeedit() {
    var def = $('#firstcontactid').val()
    if (def == 0) {
        $('.disable').val("")
        $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
    }
    else {
        getdataofcontact(def)
    }
    $('.savecancelcontact').addClass('d-none')
    $('.disable').prop('readonly', true);
    $('#issubscribeornot').addClass('readonly');
}

$('.addthiscontact').on('click', function () {
    var Id = $('#contactid').val();
    var Name = $('#fullname').val();
    var Username = $('#username').val();
    var Telephone = $('#contacttelephone').val();
    var Email = $('#contactemail').val();
    var MailingList = $('input[name="issubscribe"]:checked').val();
    var Isdelete = false;
    var CustomerId = $('#thisid').val();


    var usernameRegex = /^(?=.*[a-zA-Z])(?!\s*$).+/;
    var nameRegex = /^(?!\s*$)[a-zA-Z\s]*$/;
    var telephoneRegex = /^(?!\s*$)\+91\d{10}$/;
    var emailRegex = /^(?!\s*$)[^\s@]+@[^\s@]+\.[^\s@]+$/;


    if (!usernameRegex.test(Username)) {
        Swal.fire('Invalid Username');
        return;
    }
    if (!nameRegex.test(Name)) {
        Swal.fire('Invalid Name');
        return;
    }
    if (!telephoneRegex.test(Telephone)) {
        Swal.fire('Invalid Telephone');
        return;
    }
    if (!emailRegex.test(Email)) {
        Swal.fire('Invalid Email');
        return;
    }
    $.ajax({
        url: '/Home/AddThisContact',
        data: { Id, Username, Name, Telephone, Email, MailingList, Isdelete, CustomerId },
        type: 'POST',
        success: function (res) {
            $('#contactid').val(res);
            closeedit()
            getdataofcontact(res); putdata(acno); getcontactlist(search)
            Toast.fire({
                icon: "success",
                title: "Contact Information Saved"
            });
        }
    })
})

$('input[type="checkbox"][name="issubscribe"]').on('change', function () {
    $('input[type="checkbox"][name="issubscribe"]').not(this).prop('checked', false);
    var maillist = ($('input[name="issubscribe"]:checked').val());
    $('#issubscribeornot').val(maillist);
    $('#closemaillist').click()
});

function getdataofcontact(contactid) {
    $.ajax({
        url: '/Home/GetContactDataById',
        data: { contactid },
        type: 'POST',
        success: function (res) {
            console.log(res)
            $('#contactid').val(res.id);
            $('#firstcontactid').val(res.id);
            $('#fullname').val(res.name);
            $('#username').val(res.username);
            $('#contacttelephone').val(res.telephone);
            $('#contactemail').val(res.email);
            $('#issubscribeornot').val(res.mailingList);
            $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
            $('input[type="checkbox"][value="' + res.mailingList + '"]').prop('checked', true);
            $('#fullnamespan').text(res.name);
            document.getElementById('contactsdrop').classList.add('d-none')
        }
    })
}


$(document).on('click', '.editthiscontact', function () {
    var contactid = $(this).data('id');
    getdataofcontact(contactid);
    openedit(contactid)
})

$(document).on('click', '.deletethiscontact', function () {
    var contactid = $(this).data('id');
    $.ajax({
        url: '/Home/DeletthisContact',
        data: { contactid },
        type: 'POST',
        success: function () {
            getcontactlist(search);
            Toast.fire({
                icon: "success",
                title: "Removed from Contacts"
            });
        }
    })
})


function getcontactlist(search) {
    var CustomerId = $('#thisid').val();
    $.ajax({
        url: '/Home/GetContactListOfCustomer',
        data: { CustomerId, search },
        type: 'POST',
        success: function (res) {
            $('.contactlist').html(res);
        }
    })
}

var search = $('#contactsearch').val();

$('#contactsearch').on('input', function () {
    search = $('#contactsearch').val();
    getcontactlist(search)
})











var Toast = Swal.mixin({
    toast: true,
    position: "top-end",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.onmouseenter = Swal.stopTimer;
        toast.onmouseleave = Swal.resumeTimer;
    }
});