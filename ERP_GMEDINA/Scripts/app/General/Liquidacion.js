$.getScript("../Scripts/app/General/SerializeDate.js")

//Funcion generica para reutilizar AJAX
function _ajax(params, uri, type, callback, enviar) {
    $.ajax({
        url: uri,
        type: type,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        beforeSend: function () {
            enviar();
        },
        success: function (data) {
            callback(data);
        }
    });
}

$('#data_5 .input-daterange').datepicker({
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    separator: ' hasta ',
    format: "yyyy/mm/dd"
});

$(document).ready(function () {
    $('#btnEnviarFechas').click(() => {
        var fechaIncio = $('#fechaInicio').val()
        var fechaFin = $('#fechaFin').val();

        _ajax({
            fechaInicio: fechaIncio,
            fechaFin: fechaFin
        },
                "/Liquidacion/GetFechaInicioFechaFin",
                "POST",
                (data) => {

                    var dDia, dMes, dAnio, dFechaFin;

                    [dFechaFin, dDia, dMes, dAnio] = FechaFormato(data.dFechaIncio)
                    console.log('Dia: ' + dDia);
                    console.log('Mes: ' + dMes);
                    console.log('Año: ' + dAnio);
                    console.log('Fecha complea: ' + dFechaFin);
                },
                enviar => { console.log('Enviando'); });
    });
});

