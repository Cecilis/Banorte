Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
    $('#dtpFechaDesde').datetimepicker({
        format: 'DD/MM/YYYY',
        locale: 'es',
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    });
    $('#dtpFechaHasta').datetimepicker({
        useCurrent: false, //Important! See issue #1075
        format: 'DD/MM/YYYY',
        locale: 'es',
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    });

    $("#dtpFechaDesde").on("dp.change", function (e) {
        $('#dtpFechaHasta').data("DateTimePicker").minDate(e.date);
    });

    $("#dtpFechaHasta").on("dp.change", function (e) {
        $('#dtpFechaDesde').data("DateTimePicker").maxDate(e.date);
    });

});