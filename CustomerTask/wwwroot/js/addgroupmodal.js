$('#Groupname').on('input', function () {
    var customerId = $('#thiscustomerIdinadd').val();
    var groupId = $('#thisGroupinadd').val();
    var group = $(this).val();
    $.ajax({
        url: '/Home/CustomerGroupExist',
        data: { customerId, groupId, group },
        type: 'POST',
        success: function (res) {
            if (res) {
                $('#spanoferror').text("Name already in use")
                $('#Groupname').val("");
            }
            else {
                $('#spanoferror').text("")
            }
        }
    })
})

$('.addthisGroup').on('click', function () {
    var customerId = $('#thiscustomerIdinadd').val();
    var groupId = $('#thisGroupinadd').val();
    var group = $('#Groupname').val();
    if (group == null || group == "") {
        Swal.fire('Enter Name');
        return;
    }
    $.ajax({
        url: '/Home/UpSertCustomerGroup',
        data: { customerId, groupId, group },
        type: 'POST',
        success: function (res) {
            $('#editGroupmodel').html(res);
            Toast.fire({
                icon: "success",
                title: "Group Information saved"
            });
            document.getElementById('addgroupdialog').close()
        }
    })
})