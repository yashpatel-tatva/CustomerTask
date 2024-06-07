$('.nav-tabs').append('<li class="nav-item" role="presentation">                    <button class="nav-link fw-bolder" id="detailtab" data-bs-toggle="tab" data-bs-target="#detail" type="button" role="tab" aria-selected="false"><i class="fas fa-th-list"></i>  Details</button>                </li>')
$('.nav-link').removeClass('active')
$('#detailtab').addClass('active')
$('.searchdiv').hide()
$('#listdiv').hide()
var AccountNoDrop = $('#acdrop');
var acNo = $('#acdrop').val();
function putdata(acNo) {
    $.ajax({
        url: '/Home/GetInfoOfAC',
        data: { acNo },
        type: 'POST',
        success: function (response) {
            try {
                $('#customerId').text(response.ac);
                $('#customername').text(response.name);
                $('#company').val(response.name);
                $('#address1').val(response.address1);
                $('input[name="Ac"]').val(response.ac);
                $('#address2').val(response.address2);
                $('#town').val(response.town);
                $('#county').val(response.county);
                $('#postcode').val(response.postcode);
                $('#country').val(response.country);
                $('#telephone').val(response.telephone);
                $('#email').val(response.email);
                $('#thisid').val(response.id);
                $('#relation').val(response.relation);
                $('#currency').val(response.currency);
                if (response.issubscribe == null) {
                    $('input[name="IssubscribeInvoice"]').prop('checked', false)
                    $('#mailinglist').val("");
                }
                else if (response.issubscribe) {
                    $('#mailinglist').val("Subscribed");
                    $('input[name="IssubscribeInvoice"]').prop('checked', false)
                    $('input[name="IssubscribeInvoice"][value="Subscribed"]').prop('checked', true)
                }
                else if (!response.issubscribe) {
                    $('#mailinglist').val("Unsubscribed");
                    $('input[name="IssubscribeInvoice"]').prop('checked', false)
                    $('input[name="IssubscribeInvoice"][value="Unsubscribed"]').prop('checked', true)
                }
                $('.editthis').attr('data-id', response.ac)
            }
            catch {
                $('.recorddiv').hide();
                $('#customerId').text('0');
                $('#customername').text("No Records Found");
            }
            getContactlist(search)
        }
    })
}
putdata(acNo);
$('.editthis').on('click', function () {
    //var id = $('#thisid').val();
    //$.ajax({
    //    url: '/Home/OpenDetailForm',
    //    data: { id },
    //    success: function (res) {
    //        $('.popup').html(res)
    //        $('#adddialog').addClass('foredit');
    //        document.getElementById('adddialog').show();
    //    }
    //})
    $('.invoiceedit').prop('readonly', false);
    $('#saveandcancelinvoice').removeClass('d-none');
    $('#mailinglist').removeClass('readonly');
    $('.addfield').removeClass('d-none')
    $('#OpenGroupDialog').val('true')
    document.getElementById('editGroupmodel').style.display = 'none';
})


$('#OpenGroupDialog').on('click', function () {
    var id = $('#thisid').val();
    var isEdit = $(this).val();
    $.ajax({
        url: '/Home/OpenGroupModal',
        data: { id, isEdit },
        type: 'POST',
        success: function (res) {
            $('#editGroupmodel').html(res);
            $('#editGroupmodel').show();
            //document.getElementById('editGroupdialog').show();
        }
    })
})
function openGroupmodal() {
    var id = $('#thisid').val();
    var isEdit = $(this).val();
    $.ajax({
        url: '/Home/OpenGroupModal',
        data: { id, isEdit },
        type: 'POST',
        success: function (res) {
            $('#editGroupmodel').html(res);
            $('#editGroupmodel').show();
            //document.getElementById('editGroupdialog').show();
        }
    })
}

/// -------------------- Contact --------------------------------///


$('#issubscribeornot').on('click', function () {
    if (!$(this).hasClass('readonly')) {
        $('#maillistdrop').show()

    }
})
$('#mailinglist').on('click', function () {
    if (!$(this).hasClass('readonly')) {
        $('#maillistinvoicedrop').show()
    }
})
$('#fullname').on('click', function () {
    if ($(this).prop('readonly')) {
        $('#Contactsdrop').show()
    }
})
$('#fullname').on('dblclick', function () {
    $('#Contactsdrop').show()
})
$('.editContact').on('click', function () {
    var c = $('#contactId').val();
    openedit(c)
})
function openedit(contactId) {
    //document.getElementById('Contactsdrop').style.display = 'none';
    $('.addContactbtn').show();
    $('.editthisContact').show();
    $('.deletethisContact').show();
    if (contactId == 0) {
        $('.disable').val("")
        $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
        $('#contactId').val(0)
        document.getElementById('Contactsdrop').style.display = 'none';
    }
    $('.savecancelContact').removeClass('d-none')
    $('.disable').not('#issubscribeornot').prop('readonly', false);
    $('#issubscribeornot').removeClass('readonly');
}

function closeedit() {
    var def = $('#firstcontactId').val()
    if (def == 0) {
        $('.disable').val("")
        $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
    }
    else {
        getdataofContact(def)
    }
    $('.savecancelContact').addClass('d-none')
    $('.disable').prop('readonly', true);
    $('#issubscribeornot').addClass('readonly');
    $('.addContactbtn').hide();
    $('.editthisContact').hide();
    $('.deletethisContact').hide();
    $('.err').text('')
}

$('.AddCustomerContact').on('click', function () {
    var Id = $('#contactId').val();
    var Name = $('#fullname').val();
    var Username = $('#username').val();
    var Telephone = $('#Contacttelephone').val();
    var Email = $('#Contactemail').val();
    var MailingList = $('input[name="issubscribe"]:checked').val();
    var Isdelete = false;
    var customerId = $('#thisid').val();


    if (validcontact()) {
        $.ajax({
            url: '/Home/UpSertCustomerContact',
            data: { Id, Username, Name, Telephone, Email, MailingList, Isdelete, customerId },
            type: 'POST',
            success: function (res) {
                $('#contactId').val(res);
                closeedit()
                getdataofContact(res); putdata(acNo); getContactlist(search)
                Toast.fire({
                    icon: "success",
                    title: "Contact Information Saved"
                });
            }
        })
    }
})


$('.disable').on('input', function () {
    validcontact();
})

function validcontact() {
    var isvalid = true;
    var usernameRegex = /^(?=.*[a-zA-Z0-9])(?!\s*$).+/;
    var nameRegex = /^(?!\s*$)[a-zA-Z\s]*$/;
    var telephoneRegex = /^(?!\s*$)\+91\d{10}$/;
    var emailRegex = /^(?!\s*$)[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var Name = $('#fullname').val();
    var Username = $('#username').val();
    var Telephone = $('#Contacttelephone').val();
    var Email = $('#Contactemail').val();
    if (!usernameRegex.test(Username)) {
        isvalid = false;
        $('label[name="username"]').text('Latters & Digits only');
    } else {
        $('label[name="username"]').text('');
    }
    if (!nameRegex.test(Name)) {
        isvalid = false;
        $('label[name="fullname"]').text('Only Latters allowed');
    } else {
        $('label[name="fullname"]').text('');
    }
    if (!telephoneRegex.test(Telephone)) {
        isvalid = false;
        $('label[name="telephone"]').text('Must be in (+911234567890) format');
    } else {
        $('label[name="telephone"]').text('');
    }
    if (!emailRegex.test(Email)) {
        isvalid = false;
        $('label[name="email"]').text('Invalid Email');
    } else {
        $('label[name="email"]').text('');
    }
    return isvalid;
}


$('input[type="checkbox"][name="issubscribe"]').on('change', function () {
    $('input[type="checkbox"][name="issubscribe"]').not(this).prop('checked', false);
    var maillist = ($('input[name="issubscribe"]:checked').val());
    $('#issubscribeornot').val(maillist);
    $('#closemaillist').click()
});
$('input[type="checkbox"][name="IssubscribeInvoice"]').on('change', function () {
    $('input[type="checkbox"][name="IssubscribeInvoice"]').not(this).prop('checked', false);
    var maillist = ($('input[name="IssubscribeInvoice"]:checked').val());
    $('#mailinglist').val(maillist);
    document.getElementById('maillistinvoicedrop').style.display = 'none';
});

$('#closemaillistinvoice').on('click', function () {
    document.getElementById('maillistinvoicedrop').style.display = 'none';

})
function getdataofContact(contactId) {
    $.ajax({
        url: '/Home/GetContactDataById',
        data: { contactId },
        type: 'POST',
        success: function (res) {
            $('#contactId').val(res.id);
            $('#firstcontactId').val(res.id);
            $('#fullname').val(res.name);
            $('#username').val(res.username);
            $('#Contacttelephone').val(res.telephone);
            $('#Contactemail').val(res.email);
            $('#issubscribeornot').val(res.mailingList);
            $('input[type="checkbox"][name="issubscribe"]').prop('checked', false);
            $('input[type="checkbox"][name="issubscribe"][value="' + res.mailingList + '"]').prop('checked', true);
            $('#fullnamespan').text(res.name);
            document.getElementById('Contactsdrop').style.display = 'none';

        }
    })
}


$(document).on('click', '.editthisContact', function () {
    var contactId = $(this).data('id');
    getdataofContact(contactId);
    openedit(contactId)
})

$(document).on('click', '.deletethisContact', function () {
    var contactId = $(this).data('id');
    $.ajax({
        url: '/Home/DeletthisContact',
        data: { contactId },
        type: 'POST',
        success: function () {
            getContactlist(search);
            Toast.fire({
                icon: "success",
                title: "Removed from Contacts"
            });
            var def = $('#firstcontactId').val()
            getdataofContact(def)
            document.getElementById('Contactsdrop').style.display = 'none';
        }
    })
})


function getContactlist(search) {
    var customerId = $('#thisid').val();
    $.ajax({
        url: '/Home/GetContactListCustomer',
        data: { customerId, search },
        type: 'POST',
        success: function (res) {
            $('.Contactlist').html(res);
        }
    })
}

var search = $('#Contactsearch').val();

$('#Contactsearch').on('input', function () {
    search = $('#Contactsearch').val();
    getContactlist(search)
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