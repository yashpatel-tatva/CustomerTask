
var id = $('input[name="Id"]').val();
$('input[name="Ac"]').on('blur', function () {
    acNo = $('input[name="Ac"]').val();

    $.ajax({
        url: '/Home/CheckACExist',
        data: { id, acNo },
        type: 'POST',
        success: function (res) {
            if (res) {
                $('input[name="Ac"]').val("");
                Swal.fire({
                    position: "top-end",
                    icon: "warning",
                    title: 'A/C Already Existed',
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        }
    })
})

$('input[name="Name"]').on('blur', function () {
    var name = $('input[name="Name"]').val();
    $.ajax({
        url: '/Home/CheckCompanyNameExist',
        data: { id, name },
        type: 'POST',
        success: function (res) {
            if (res) {
                $('input[name="Name"]').val("");
                Swal.fire({
                    position: "top-end",
                    icon: "warning",
                    title: 'Company With this Name Already Existed',
                    showConfirmButton: false,
                    timer: 1500
                });
            }
        }
    })
})

$('#addsubmit').on('click', function () {
    var form = $('#form');

    var valid = formvalid();

    $('input').on('input', function () {
        formvalid()
    })

    if (valid) {
        $.ajax({
            url: '/Home/UpSertCustomer',
            data: form.serialize(),
            type: 'POST',
            success: function (res) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Custemer detail has been saved",
                    showConfirmButton: false,
                    timer: 1500
                });
                acNo = $('input[name="Ac"]').val();
                var isedit = $('#isEdit').val();
                if (isedit == 'No') {
                    var link = document.createElement('a');
                    link.href = '/Home/DetailOfCustomer?acNo=' + acNo;
                    setTimeout(function () {
                        link.click();
                    }, 1000)
                }
                else {
                    setinputfieldreadonly();
                    var contactid = $(document).find('.editthisContact').data('id');
                    getdataofContact(contactid)
                }
            },
            error: function (error) {
                Swal.fire("Form Is InValid")
            }
        })
    } else {
        console.error("invalid form")

    }
})

function checkacexist(acNo) {
    var isexists = true;
    $.ajax({
        url: '/Home/CheckACExist',
        data: { acNo },
        type: 'POST',
        success: function (res) {
            isexists = res;
            return isexists;
        }
    })
}


function formvalid() {
    var valid = true;
    var Ac = $('input[name="Ac"]').val();
    var Name = $('input[name="Name"]').val();
    var Postcode = $('input[name="Postcode"]').val();
    var Country = $('input[name="Country"]').val();
    var Telephone = $('input[name="Telephone"]').val();
    var Relation = $('input[name="Relation"]').val();
    var Currency = $('input[name="Currency"]').val();
    var Address1 = $('input[name="Address1"]').val();
    var Address2 = $('input[name="Address2"]').val();
    var Town = $('input[name="Town"]').val();
    var County = $('input[name="County"]').val();
    var Email = $('input[name="Email"]').val();

    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(Name)) {
        valid = false;
        $('span[name="Name"]').text("Name can only contain letters");
    } else {
        $('span[name="Name"]').text("")
    }
    if (!/^(?=.*\S)[a-zA-Z0-9\s]*$/.test(Postcode)) {
        valid = false;
        $('span[name="Postcode"]').text("Postcode can only contain alphanumeric characters");
    } else { $('span[name="Postcode"]').text("") }
    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(Country)) {
        valid = false;
        $('span[name="Country"]').text("Country can only contain letters");
    } else { $('span[name="Country"]').text("") }
    if (!/^[+]{1}\d{12}$/.test(Telephone)) {
        valid = false;
        $('span[name="Telephone"]').text("Must be in (+911234567890) format");
    } else { $('span[name="Telephone"]').text("") }
    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(Relation)) {
        valid = false;
        $('span[name="Relation"]').text("Relation can only contain letters");
    } else { $('span[name="Relation"]').text("") }
    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(Currency)) {
        valid = false;
        $('span[name="Currency"]').text("Currency can only contain letters");
    } else { $('span[name="Currency"]').text("") }
    if (Address1 === '') {
        valid = false;
        $('span[name="Address1"]').text("Address1 is required");
    } else { $('span[name="Address1"]').text("") }
    if (Address2 === '') {
        valid = false;
        $('span[name="Address2"]').text("Address2 is required");
    } else { $('span[name="Address2"]').text("") }
    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(Town)) {
        valid = false;
        $('span[name="Town"]').text("Town can only contain letters");
    } else { $('span[name="Town"]').text("") }
    if (!/^(?=.*\S)[a-zA-Z\s]*$/.test(County)) {
        valid = false;
        $('span[name="County"]').text("County can only contain letters");
    } else { $('span[name="County"]').text("") }
    if (!/^\S+@\S+\.\S+$/.test(Email)) {
        valid = false;
        $('span[name="Email"]').text("Invalid Email Address");
    } else { $('span[name="Email"]').text("") }
    if (!/^[A-Z]{2}\d{5}$/.test(Ac)) {
        valid = false;
        $('span[name="Ac"]').text("Ac must start with two uppercase letters followed by five digits.");
    } else { $('span[name="Ac"]').text("") }
    return valid;
}

function setinputfieldreadonly() {
    $('.invoiceedit').prop('readonly', true);
    $('#saveandcancelinvoice').addClass('d-none');
    $('#mailinglist').addClass('readonly');
    $('.addfield').addClass('d-none')
    $('#OpenGroupDialog').val('false')
    document.getElementById('editGroupmodel').style.display = 'none';
    const urlParams = new URLSearchParams(window.location.search);
    const acNo = urlParams.get('acNo');
    putdata(acNo)
    $('.editerror').text('');
}