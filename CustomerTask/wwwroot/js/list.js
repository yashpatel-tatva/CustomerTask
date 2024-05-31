var btncount = 0;
var count = 0;
var columnname = "Ac";

$('.nav-link').removeClass('active')
$('#listtab').addClass('active')
$('#detaildiv').hide()

function filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort) {
    filtercount(search);
    $.ajax({
        url: '/Home/GetCustomerListView',
        data: { search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort },
        type: 'GET',
        success: function (res) {

            if (btncount == 0) {
                $('#datainfo').text('No Record Found');

            }
            else if (btncount > currentpage) {
                $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + (((currentpage) * pagesize)) + ' out of ' + count);
            }
            else {
                $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + count + ' out of ' + count);
            }
            $('.datadiv').html(res);
            var stateObj = { pagesize, currentpage, search };
            history.pushState(stateObj, "", "/Home/Index?pagesize=" + pagesize + "&currentpage=" + currentpage + "&search=" + search)
        }
    })
    $('.pagebtn_1').data('id', currentpage);
    $('.pagebtn_1').text(currentpage);
}

function filtercount(search) {
    $.ajax({
        url: '/Home/GetCustomerCount',
        data: { search },
        type: 'POST',
        success: function (res) {
            count = res;
            btncount = Math.ceil(res / pagesize);
            if (btncount == 0) {
                if (currentpage != 1) {
                    filterdata(search, 1, pagesize)
                }
            }
            else if (btncount > currentpage) {
                $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + (((currentpage) * pagesize)) + ' out of ' + count);
            }
            else {
                $('#datainfo').text('Showing ' + (((currentpage - 1) * pagesize) + 1) + ' - ' + count + ' out of ' + count);
            }
            var gotodrop = $('#gotodrop');
            gotodrop.empty();
            gotodrop.append($('<option>', {
                value: 1,
                text: 1
            }))
            for (var i = 5; i <= btncount; i = i + 5) {
                gotodrop.append($('<option>', {
                    value: i,
                    text: i
                }))
            }
        }
    })
}
filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)


$('#search').on('input', function () {
    search = $(this).val()
    currentpage = 1;
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
})

$('#pagesize').on('change', function () {
    pagesize = $(this).val()
    currentpage = 1;
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
})
$('#gotodrop').on('change', function () {
    currentpage = $(this).val()
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
})



$('.paginate_Previousbutton').on('click', function () {
    if (currentpage > 1) {
        currentpage = parseInt(currentpage) - 1;
        filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
    }
});
$('.paginate_Nextbutton').on('click', function () {
    if (btncount > currentpage) {
        currentpage = parseInt(currentpage) + 1;
        filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
    }
});

$('.paginate_first').on('click', function () {
    if (currentpage != 1) {
        currentpage = 1;
        filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
    }
});
$('.paginate_last').on('click', function () {
    if (btncount != currentpage) {
        currentpage = btncount;
        filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
    }
});


$(document).on('dblclick', '.customerrow', function () {

    var acno = $(this).data('id');

    var link = document.createElement('a');
    link.href = '/Home/DetailOfCustomer?acno=' + acno;

    link.click();
});

$(document).on('click', '.customerrow', function () {
    if ($(this).hasClass('selectthisrow')) {
        $(this).removeClass('selectthisrow');
    }
    else {
        $(this).addClass('selectthisrow');
    }
});




$('#acsortth').on('click', function () {
    columnname = "Ac";
    removeothersortingthanthis($(this).id)
    switch (acsort) {
        case 0:
            acsort = 1;
            $('#acsorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            acsort = 2;

            $('#acsorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            acsort = 0;

            $('#acsorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#namesortth').on('click', function () {
    columnname = "Name";
    removeothersortingthanthis($(this).id)
    switch (namesort) {
        case 0:
            namesort = 1;

            $('#namesorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            namesort = 2;

            $('#namesorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            namesort = 0;

            $('#namesorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#pcsortth').on('click', function () {
    columnname = "Postcode";
    removeothersortingthanthis($(this).id)
    switch (pcsort) {
        case 0:
            pcsort = 1;

            $('#pcsorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            pcsort = 2;

            $('#pcsorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            pcsort = 0;

            $('#pcsorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#cntrysortth').on('click', function () {
    columnname = "Country";
    removeothersortingthanthis($(this).id)
    switch (cntrysort) {
        case 0:
            cntrysort = 1;

            $('#cntrysorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            cntrysort = 2;

            $('#cntrysorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            cntrysort = 0;

            $('#cntrysorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#tpsortth').on('click', function () {
    columnname = "Telephone"
    removeothersortingthanthis($(this).id)
    switch (tpsort) {
        case 0:
            tpsort = 1;

            $('#tpsorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            tpsort = 2;

            $('#tpsorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            tpsort = 0;

            $('#tpsorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#rlsnsortth').on('click', function () {
    columnname = "Relation";
    removeothersortingthanthis($(this).id)
    switch (rlsnsort) {
        case 0:
            rlsnsort = 1;

            $('#rlsnsorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            rlsnsort = 2;

            $('#rlsnsorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            rlsnsort = 0;

            $('#rlsnsorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});

$('#currsortth').on('click', function () {
    columnname = "Currency";
    removeothersortingthanthis($(this).id)
    switch (currsort) {
        case 0:
            currsort = 1;

            $('#currsorticn').removeClass("bi-arrow-down bi-arrow-right").addClass("bi-arrow-up");
            break;
        case 1:
            currsort = 2;

            $('#currsorticn').removeClass("bi-arrow-up bi-arrow-right").addClass("bi-arrow-down");
            break;
        default:
            currsort = 0;

            $('#currsorticn').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right");
    }
    filterdata(search, currentpage, pagesize, acsort, namesort, pcsort, cntrysort, tpsort, rlsnsort, currsort)
});



function removeothersortingthanthis(id) {
    $('.bi').removeClass("bi-arrow-up bi-arrow-down").addClass("bi-arrow-right")
    switch (id) {
        case "acsortth":
            namesort = 0; pcsort = 0; cntrysort = 0; tpsort = 0; rlsnsort = 0; currsort = 0;
            break;
        case "namesortth":
            acsort = 0; pcsort = 0; cntrysort = 0; tpsort = 0; rlsnsort = 0; currsort = 0;
            break;
        case "pcsortth":
            acsort = 0; namesort = 0; cntrysort = 0; tpsort = 0; rlsnsort = 0; currsort = 0;
            break;
        case "cntrysortth":
            acsort = 0; namesort = 0; pcsort = 0; tpsort = 0; rlsnsort = 0; currsort = 0;
            break;
        case "tpsortth":
            acsort = 0; namesort = 0; pcsort = 0; cntrysort = 0; rlsnsort = 0; currsort = 0;
            break;
        case "rlsnsortth":
            acsort = 0; namesort = 0; pcsort = 0; cntrysort = 0; tpsort = 0; currsort = 0;
            break;
        case "currsortth":
            acsort = 0; namesort = 0; pcsort = 0; cntrysort = 0; tpsort = 0; rlsnsort = 0;
            break;
    }
}