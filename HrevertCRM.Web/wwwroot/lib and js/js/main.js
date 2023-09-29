function kontakte(e) {
    var getthevalue = $(e).attr('id');
    //console.log(getthevalue);
    $('#' + getthevalue).bootstrapMaterialDatePicker({
        time: false,
        clearButton: true,
        weekStart: 0
    });
}
function test() {
    //var inputs = document.getElementById("newFiscalPeriod").getElementsByTagName("input");
    //console.log("Test");
}

$(document).ready(function () {
    //$('#materialDatePicker1').bootstrapMaterialDatePicker({
    //    time: false,
    //    clearButton: true,
    //    weekStart: 0
    //});
    //$('#materialDatePicker2').bootstrapMaterialDatePicker({
    //    time: false,
    //    clearButton: true,
    //    weekStart: 0
    //});
    //$('#materialDatePicker3').bootstrapMaterialDatePicker({
    //    time: false,
    //    clearButton: true,
    //    weekStart: 0
    //});
    //$('#materialDatePicker4').bootstrapMaterialDatePicker({
    //    time: false,
    //    clearButton: true,
    //    weekStart: 0
    //});

    $('.date').tooltip();
    $('[data-toggle="tooltip"]').tooltip();
});
//$(function () {
//    $('.sortable').sortable();
//    $('.handles').sortable({
//        handle: 'span'
//    });
//    $('.connected').sortable({
//        connectWith: '.connected'
//    });
//    $('.exclude').sortable({
//        items: ':not(.disabled)'
//    });
//});
