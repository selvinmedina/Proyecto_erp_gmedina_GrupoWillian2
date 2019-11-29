//Funcion generica para reutilizar AJAX
function _ajax(params, uri, type, callback, enviar) {
	$.ajax({
		url: uri,
		type: type,
		dataType: 'json',
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify(params),
		beforeSend: function() {
			enviar();
		},
		success: function(data) {
			callback(data);
		}
	});
}
var fecha = new Date();
const inputFechaFin = $('#fechaFin');

//Mostrar el spinner
function spinner() {
	return `<div class="sk-spinner sk-spinner-wave">
                <div class="sk-rect1"></div>
                <div class="sk-rect2"></div>
                <div class="sk-rect3"></div>
                <div class="sk-rect4"></div>
                <div class="sk-rect5"></div>
             </div>`;
}

const cargarSpinnerFecha = $('#cargarSpinner');
$(document).ready(function() {
	$('#datepicker .input-group.date').datepicker({
		todayBtn: 'linked',
		keyboardNavigation: false,
		forceParse: false,
		calendarWeeks: true,
		autoclose: true,
		format: 'yyyy/mm/dd'
	});
	function validarCampos() {
		var todoBien = true;

		//Validar que no este vacio el campo de fecha de despido
		if (inputFechaFin.val() == '') {
			todoBien = false;
		}

		return todoBien;
	}
	$(inputFechaFin).change(() => {
		console.clear();

		let fechaFin = inputFechaFin.val();

		if (validarCampos()) {
			_ajax(
				{
					idEmpleado: 1,
					fechaFin: fechaFin
				},
				'/Liquidacion/GetFechaInicioFechaFin',
				'POST',
				(data) => {
					console.log(data);
					cargarSpinnerFecha.hide();
					mostrarTiempoTrabajado(data.sDias, data.sMeses, data.sAnios);
				},
				(enviar) => {
					cargarSpinnerFecha.html(spinner());
					cargarSpinnerFecha.show();
				}
			);
		}
	});
});

function mostrarTiempoTrabajado(dDias, dMeses, dAnios) {
	$('#h3Dias').html(dDias);
	$('#h3Meses').html(dMeses);
	$('#h3Anios').html(dAnios);
	$('#tiempoTrabajado').show();
}
