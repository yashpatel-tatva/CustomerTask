var id = $('input[name="Id"]').val();
$('input[name="Ac"]').on('blur', function () {
    acno = $('input[name="Ac"]').val();

    $.ajax({
        url: '/Home/CheckACExist',
        data: { id, acno },
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
    console.log($('#isedit').val());
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

    var inputs = form.find('input[required]');
    var allFilled = true;
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].value === '') {
            allFilled = false;
            break;
        }
    }
    allFilled = !checkacexist()
    if (allFilled) {
        if (true) {
            $.ajax({
                url: '/Home/AddCustomer',
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
                    var acno = $('input[name="Ac"]').val();


                    var link = document.createElement('a');
                    link.href = '/Home/DetailOfCustomer?acno=' + acno;
                    setTimeout(function () {

                        link.click();
                    }, 1000)
                },
                error: function (error) {
                    Swal.fire("Form Is InValid")
                }
            })
        } else {
            console.error("invalid form")
            
        }
    } else {
        Swal.fire({
            position: "top-end",
            icon: "warning",
            title: 'Please fill out all required fields.',
            showConfirmButton: false,
            timer: 1500
        });
    }
})

function checkacexist(acno) {
    var isexists = true;
    $.ajax({
        url: '/Home/CheckACExist',
        data: { acno },
        type: 'POST',
        success: function (res) {
            isexists = res;
            console.log(isexists)
            return isexists;
        }
    })
}