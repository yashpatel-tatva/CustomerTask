var btncount = 0;
var count = 0;
var orderbycolumnname = "Ac";
var orderby = true;
var gotodrop = $('#gotodrop');


var searchbycolumns = {
    Ac: "",
    Name: "",
    Postcode: "",
    Country: "",
    Telephone: "",
    Relation: "",
    Currency: ""
}

$('.nav-link').removeClass('active')
$('#listtab').addClass('active')
$('#detaildiv').hide()

function filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns) {
    //filtercount(search);
    $.ajax({
        url: '/Home/GetCustomerListView',
        data: JSON.stringify({ search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns }),
        contentType: 'application/json',
        type: 'POST',
        success: function (res) {
            $('.datadiv').html(res);
            filtercount();
            var stateObj = { pagesize, currentpage, search };
            history.pushState(stateObj, "", "/Home/Index?pagesize=" + pagesize + "&currentpage=" + currentpage + "&search=" + search)
        }
    })
    $('.pagebtn_1').data('id', currentpage);
    $('.pagebtn_1').text(currentpage);
}

function filtercount() {
    count = $('#totalrecords').val();
    printbuttons(count)
    btncount = Math.ceil(count / pagesize);
    $('#totalpages').text(btncount);
    if (btncount == 0) {
        $('#datainfo').text('No Record Found');
    }
    else if (btncount < currentpage) {
        if (currentpage != 1) {
            currentpage = 1;
            filterdata(search, currentpage, pagesize)
        }
    }
    else if (btncount > currentpage) {
        $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + (((currentpage) * pagesize)) + ' out of ' + count);
    }
    else {
        $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + count + ' out of ' + count);
    }

    gotodrop.attr('max', btncount);

    //var gotodrop = $('#gotodrop');
    //gotodrop.empty();
    //gotodrop.append($('<option>', {
    //    value: 1,
    //    text: 1
    //}))
    //for (var i = 5; i <= btncount; i = i + 5) {
    //    gotodrop.append($('<option>', {
    //        value: i,
    //        text: i
    //    }))
    //}
}


filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)


$('#search').on('input', function () {
    search = $(this).val()
    currentpage = 1;
    orderbycolumnname = null;
    $('.notselect').find('i').removeClass('fa-sort-up fa-sort-down');
    $('.notselect').find('i').addClass('fa-sort');
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
})

$('#pagesize').on('change', function () {
    pagesize = $(this).val()
    currentpage = 1;
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
})
$('#gotodrop').on('change', function () {
    if ($(this).val() == null || $(this).val() == "") {
        $(this).val(1);
    }
    else if ($(this).val() > btncount) {
        $(this).val(1);
    }
    currentpage = $(this).val()
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
})

$('.dataTables_paginate').on('click', '.paginate_button', function () {
    currentpage = $(this).data('id');
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
});

$('.paginate_Previousbutton').on('click', function () {
    if (currentpage > 1) {
        currentpage = parseInt(currentpage) - 1;
        filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (btncount > currentpage) {
        currentpage = parseInt(currentpage) + 1;
        filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
    }
});

$('.paginate_first').on('click', function () {
    if (currentpage != 1) {
        currentpage = 1;
        filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
    }
});
$('.paginate_last').on('click', function () {
    if (btncount != currentpage) {
        currentpage = btncount;
        filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns)
    }
});


$(document).on('dblclick', '.customerrow', function () {

    var acNo = $(this).data('id');

    var link = document.createElement('a');
    link.href = '/Home/DetailOfCustomer?acNo=' + acNo;

    link.click();
});

$(document).on('click', '.customerrow', function () {
    $(this).toggleClass('selectthisrow')
});

$('.notselect').on('click', function () {
    orderbycolumnname = $(this).data('name');
    console.log(orderbycolumnname)
    var icon = $(this).find('i');

    $(this).find('i').removeClass('fa-sort')
    $('.notselect').not(this).removeClass("order");
    $('.notselect').not(this).find('i').removeClass('fa-sort-up fa-sort-down');
    $('.notselect').not(this).find('i').addClass('fa-sort');

    if ($(this).hasClass("order")) {
        if ($(this).hasClass("asc")) {
            orderby = false;
            $(this).removeClass("asc").addClass("dsc");
            icon.removeClass("fa-sort-up").addClass("fa-sort-down");
        }
        else {
            orderby = true;
            $(this).removeClass("dsc").addClass("asc");
            icon.removeClass("fa-sort-down").addClass("fa-sort-up");
        }
    }
    else {
        orderby = true;
        $(this).addClass("order asc");
        icon.addClass("fa-sort-up");
    }
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns);
});


$('.searchfilter').on('click', function () {
    //search = ""; $('#search').val("")
    searchbyfilter();
})

$('.clearfilter').on('click', function () {
    $('.form-control').val("");
    searchbyfilter();
})

function searchbyfilter() {
    var ac = $('input[name="Ac"]').val();
    var name = $('input[name="Name"]').val();
    var postcode = $('input[name="Postcode"]').val();
    var country = $('input[name="Country"]').val();
    var telephone = $('input[name="Telephone"]').val();
    var relationship = $('input[name="Relation"]').val();
    var currency = $('input[name="Currency"]').val();
    searchbycolumns = {
        Ac: ac,
        Name: name,
        Postcode: postcode,
        Country: country,
        Telephone: telephone,
        Relation: relationship,
        Currency: currency
    }
    orderbycolumnname = null;
    $('.notselect').find('i').removeClass('fa-sort-up fa-sort-down');
    $('.notselect').find('i').addClass('fa-sort');
    currentpage = 1;
    filterdata(search, currentpage, pagesize, orderbycolumnname, orderby, searchbycolumns);

}





function printbuttons(data) {
    buttoncount = Math.ceil(data / pagesize);
    $('.dataTables_paginate').html("");
    var btn = Math.min(5, buttoncount);
    var j = currentpage;
    if (buttoncount == 0) {
        $('.dataTables_paginate').html("No Records Found");
    }
    else if (buttoncount <= 5) {
        for (var i = 1; i <= btn; i++) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + i + '">' + i + '</div > ');
        }
    }
    else {
        if (currentpage >= buttoncount - 3) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 3) + '">' + (buttoncount - 3) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 2) + '">' + (buttoncount - 2) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (buttoncount - 1) + '">' + (buttoncount - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else if (currentpage < 4) {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 2 + '">' + 2 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 3 + '">' + 3 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 4 + '">' + 4 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" >' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
        else {
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + 1 + '">' + 1 + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage - 1) + '">' + (currentpage - 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + currentpage + '">' + currentpage + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + (currentpage + 1) + '">' + (currentpage + 1) + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_buttonNaN" data-id="' + '...' + '">' + '...' + '</div > ');
            $('.dataTables_paginate').append('<div class="paginate_button" data-id="' + buttoncount + '">' + buttoncount + '</div > ');
        }
    }
    if (currentpage == 1) {
        $('.paginate_Previousbutton').addClass("d-none");
        $('.paginate_first').addClass("d-none");
    }
    else {
        $('.paginate_Previousbutton').removeClass("d-none");
        $('.paginate_first').removeClass("d-none");
    }
    if (buttoncount == currentpage) {
        $('.paginate_Nextbutton').addClass("d-none");
        $('.paginate_last').addClass("d-none");
    }
    else {
        $('.paginate_Nextbutton').removeClass("d-none");
        $('.paginate_last').removeClass("d-none");
    }



    $('.paginate_button').removeClass("current");
    $('.paginate_button[data-id="' + currentpage + '"]').addClass("current");

}