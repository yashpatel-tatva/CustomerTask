var customerid = $('#thiscustomerid').val()
var search = $('#groupsearch').val();
getgroups(customerid, search);


$('.addgroupbtn').on('click', function () {
    var customerid = $('#thisid').val();
    var groupid = 0;
    $.ajax({
        url: '/Home/AddGroupModal',
        data: { customerid, groupid },
        type: 'POST',
        success: function (res) {
            $('#addgroupmodel').html(res)
            document.getElementById('addgroupdialog').show();

        }
    })
})

$(document).on('click', '.deletethisgroup', function () {
    var customerid = $('#thiscustomerid').val();
    var groupid = $(this).data('id');
    $.ajax({
        url: '/Home/DeleteGroupFromCustomer',
        data: { customerid, groupid },
        type: 'POST',
        success: function (res) {
            $('#editgroupmodel').html(res);
            Toast.fire({
                icon: "success",
                title: "Group Removed"
            });            putdata(acno)
        }
    })
})

$(document).on('click', '.editthisgroup', function () {
    var customerid = $('#thisid').val();
    var groupid = $(this).data('id');
    $.ajax({
        url: '/Home/AddGroupModal',
        data: { customerid, groupid },
        type: 'POST',
        success: function (res) {
            $('#addgroupmodel').html(res)
            document.getElementById('addgroupdialog').show(); putdata(acno)
        }
    })
})

$('.cancelsupplier').on('click', function () {
    $('.groupdetail').html('');
    $('.grouplist').removeClass('d-none');
    $('.infoofmodal').html('Name')
    $('.savecancel').addClass('d-none');
    $('.groupdetail').addClass('d-none');
})

$('.savesupplier').on('click', function () {
    var removefromgroup = []
    var addtogroup = []
    var groupid = $('#groupid').val();
    $('input[name="supplierofgroup"]:not(:checked)').each(function () {
        var id = $(this).val();
        removefromgroup.push(id);
    });
    $('input[name="nullsupplier"]:checked').each(function () {
        var id = $(this).val();
        addtogroup.push(id);
    });
    $.ajax({
        url: '/Home/EditSupplierinGroup',
        data: { groupid, removefromgroup, addtogroup },
        type: 'POST',
        success: function () {
            $('.groupdetail').html('');
            $('.grouplist').removeClass('d-none');
            $('.infoofmodal').html('Name')
            $('.savecancel').addClass('d-none');
            $('.groupdetail').addClass('d-none');
        }
    })
})


function closeeditmodal() {
    var adddialog = document.getElementById('addgroupdialog');
    if (adddialog == null) {
        document.getElementById('editgroupdialog').remove();
    }
    else if (adddialog.open) {
        adddialog.classList.add('border-danger');
    } else {
        document.getElementById('editgroupdialog').remove();
    }
}


$('#groupsearch').on('input', function () {
    if ($('.groupdetail').hasClass('d-none')) {
        var search = $('#groupsearch').val();
        getgroups(customerid, search);
    }
    else {
        var groupid = $('#groupid').val();
        var search = $('#groupsearch').val();
        getsuppliers(groupid, search);
    }
})

function getsuppliers(groupid, search) {
    $.ajax({
        url: '/Home/GroupSupplierModel',
        data: { groupid, search },
        type: 'POST',
        success: function (res) {
            $('.groupdetail').html(res);
            $('.grouplist').addClass('d-none');
            $('.infoofmodal').html($('#groupname').val())
            $('.savecancel').removeClass('d-none');
            $('.groupdetail').removeClass('d-none');
        }
    })
}

function getgroups(customerid, search) {
    customerid = parseInt(customerid)
    $.ajax({
        url: '/Home/CustomerGroupData',
        data: { customerid, search },
        type: 'POST',
        success: function (res) {
            $('.grouplist').html(res);
        }
    })
}

function clearfilter() {
    $('input[name="groupin"]').prop('checked', false);
    $.ajax({
        url: '/Home/UnSelectAllGroupInCustomer',
        data: { customerid },
        type: 'POST',
        success: function () {
            Toast.fire({
                icon: "success",
                title: "All Removed from selection"
            });
            $('#OpenGroupDialog').text("No Selected");
        }
    })
}
$(document).on('change', 'input[name="groupin"]', function () {
    //console.log("her");
    var groupid = $(this).val();
    var gname = $(this).data('name')
    var isselect = $(this).is(":checked");
    $.ajax({
        url: '/Home/SelectGroupInCustomer',
        data: { groupid, isselect },
        type: 'POST',
        success: function () {
            if (isselect) {
                Toast.fire({
                    icon: "success",
                    title: gname + " Add in selection"
                });
            }
            else {
                Toast.fire({
                    icon: "success",
                    title: gname + " Removed from selection"
                });
            }
            var c = $(document).find('input[name="groupin"]:checked').length;
            var n = c - 1;
            var grname = $(document).find('input[name="groupin"]:checked').data('name')
            if (c == 0) {
                $('#OpenGroupDialog').text("No Selected");
            }
            else if (c == 1) {
                $('#OpenGroupDialog').text(grname);
            }
            else {
                $('#OpenGroupDialog').text(grname + " + " + n + " more....");
            }
        }
    })
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