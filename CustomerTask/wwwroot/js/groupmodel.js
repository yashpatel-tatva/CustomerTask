var customerId = $('#thiscustomerId').val()
var search = $('#Groupsearch').val();

getGroups(customerId, search);


$('.addGroupbtn').on('click', function () {
    var customerId = $('#thisid').val();
    var groupId = 0;
    $.ajax({
        url: '/Home/AddGroupModal',
        data: { customerId, groupId },
        type: 'POST',
        success: function (res) {
            $('#addGroupmodel').html(res)
            document.getElementById('addgroupdialog').show();

        }
    })
})

$(document).on('click', '.deletethisgroup', function () {
    var customerId = $('#thiscustomerId').val();
    var groupId = $(this).data('id');
    $.ajax({
        url: '/Home/DeleteGroupCustomer',
        data: { customerId, groupId },
        type: 'POST',
        success: function (res) {
            $('#editGroupmodel').html(res);
            Toast.fire({
                icon: "success",
                title: "Group Removed"
            });            putdata(acNo)
        }
    })
})

$(document).on('click', '.editthisgroup', function () {
    var customerId = $('#thisid').val();
    var groupId = $(this).data('id');
    $.ajax({
        url: '/Home/AddGroupModal',
        data: { customerId, groupId },
        type: 'POST',
        success: function (res) {
            $('#addGroupmodel').html(res)
            document.getElementById('addgroupdialog').show(); putdata(acNo)
        }
    })
})

$('.cancelsupplier').on('click', function () {
    $('.Groupdetail').html('');
    $('.Grouplist').removeClass('d-none');
    $('.infoofmodal').html('Name')
    $('.savecancel').addClass('d-none');
    $('.Groupdetail').addClass('d-none');
})

$('.savesupplier').on('click', function () {
    var removeFromGroup = []
    var addToGroup = []
    var groupId = $('#groupId').val();
    $('input[name="supplierofGroup"]:not(:checked)').each(function () {
        var id = $(this).val();
        removeFromGroup.push(id);
    });
    $('input[name="nullsupplier"]:checked').each(function () {
        var id = $(this).val();
        addToGroup.push(id);
    });
    $.ajax({
        url: '/Home/EditSupplierinGroup',
        data: { groupId, removeFromGroup, addToGroup },
        type: 'POST',
        success: function () {
            $('.Groupdetail').html('');
            $('.Grouplist').removeClass('d-none');
            $('.infoofmodal').html('Name')
            $('.savecancel').addClass('d-none');
            $('.Groupdetail').addClass('d-none');
        }
    })
})


function closeeditmodal() {
    var adddialog = document.getElementById('addgroupdialog');
    if (adddialog == null) {
        document.getElementById('editGroupdialog').remove();
        document.getElementById('editGroupmodel').style.display = 'none';
    }
    else if (adddialog.open) {
        adddialog.classList.add('shake');
        adddialog.classList.add('border-danger');
        setTimeout(function () {
            adddialog.classList.remove('shake');
        },1000)
    } else {
        document.getElementById('editGroupdialog').remove();
        document.getElementById('editGroupmodel').style.display = 'none';
    }
}


$('#Groupsearch').on('input', function () {
    if ($('.Groupdetail').hasClass('d-none')) {
        var search = $('#Groupsearch').val();
        getGroups(customerId, search);
    }
    else {
        var groupId = $('#groupId').val();
        var search = $('#Groupsearch').val();
        getsuppliers(groupId, search);
    }
})

function getsuppliers(groupId, search) {
    $.ajax({
        url: '/Home/GroupSupplierModel',
        data: { groupId, search },
        type: 'POST',
        success: function (res) {
            $('.Groupdetail').html(res);
            $('.Grouplist').addClass('d-none');
            $('.infoofmodal').html($('#Groupname').val())
            $('.savecancel').removeClass('d-none');
            $('.Groupdetail').removeClass('d-none');
        }
    })
}

function getGroups(customerId, search) {
    customerId = parseInt(customerId)
    $.ajax({
        url: '/Home/CustomerGroupData',
        data: { customerId, search , isEdit},
        type: 'POST',
        success: function (res) {
            $('.Grouplist').html(res);
        }
    })
}

function clearfilter() {
    $('input[name="groupin"]').prop('checked', false);
    $.ajax({
        url: '/Home/UnSelectAllGroupInCustomer',
        data: { customerId },
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
    var groupId = $(this).val();
    var gname = $(this).data('name')
    var isSelect = $(this).is(":checked");
    $.ajax({
        url: '/Home/SelectGroupInCustomer',
        data: { groupId, isSelect },
        type: 'POST',
        success: function () {
            if (isSelect) {
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