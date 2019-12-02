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
const cargarSpinnerFecha = $('#cargarSpinner');
const ddlEmpleados = $('#cmbxEmpleados');
const validacionSelectFechaFin = $('#validacionSelectFechaFin');
const validacionSelectEmpleado = $('#validacionSelectEmpleado');

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

$(ddlEmpleados).change(() => {
	const ddlEmpleadosLleno = ddlEmpleados.val() != '';
	if (ddlEmpleadosLleno) validacionSelectEmpleado.hide();

	const fechaFinVal = inputFechaFin.val();
	const ddlEmpleadosVal = ddlEmpleados.val();
	if(ddlEmpleadosLleno && inputFechaFinLlena())
	if (validarCampos()) {
		obtenerDatosEmpleados(ddlEmpleadosVal, fechaFinVal);
	}
});

$(inputFechaFin).change(() => {
	if (inputFechaFinLlena()) validacionSelectFechaFin.hide();
});


$(document).ready(function() {
	$('#datepicker .input-group.date')
		.datepicker({
			todayBtn: 'linked',
			keyboardNavigation: false,
			forceParse: false,
			calendarWeeks: true,
			autoclose: true,
			format: 'yyyy/mm/dd'
		})
		.on('changeDate', function(e) {
			if (validarCampos()) {
				const fechaFinVal = inputFechaFin.val();
				const ddlEmpleadosVal = ddlEmpleados.val();
				obtenerDatosEmpleados(ddlEmpleadosVal, fechaFinVal);
			}
		});

	//Llengar DDL Areas con Empleados
	_ajax(
		null,
		'Liquidacion/GetEmpleadosAreas',
		'GET',
		(data) => {
			$('#cmbxEmpleados').select2({
				placeholder: 'Seleccione un empleado',
				allowClear: true,
				language: {
					noResults: function() {
						return 'Resultados no encontrados.';
					},
					searching: function() {
						return 'Buscando...';
					}
				},
				data: data.results
			});
		},
		() => {}
	);
});
function obtenerDatosEmpleados(idEmpleado, fechaFin) {
	_ajax({
		idEmpleado: idEmpleado,
		fechaFin: fechaFin
	}, '/Liquidacion/GetInfoEmpleado', 'POST', (data) => {
		console.log(data);
		cargarSpinnerFecha.html();
		cargarSpinnerFecha.hide();
		var mes = 0;
		var anio = 0;
		var dias = 0;
		mes = data.sMeses;
		anio = data.sAnios;
		//Metodo 1
		if (data.sDias < 0) {
			dias = data.sDias + 30;
		}
		else {
			dias = data.sDias;
		}
		mostrarTiempoTrabajado(dias, mes, anio);
	}, (enviar) => {
		cargarSpinnerFecha.html(spinner());
		cargarSpinnerFecha.show();
	});
}

function inputFechaFinLlena() {
	return inputFechaFin.val() != '';
}

function validarCampos() {
	var todoBien = true;

	//Validar que el drop down list tenga seleccionado un empleado.
	if (ddlEmpleados.val() == '') {
		todoBien = false;
		validacionSelectEmpleado.show();
	}

	//Validar que no este vacio el campo de fecha de despido.
	if (inputFechaFin.val() == '') {
		todoBien = false;
		validacionSelectFechaFin.show();
	}

	return todoBien;
}

function mostrarTiempoTrabajado(dDias, dMeses, dAnios) {
	$('#h3Dias').html(dDias);
	$('#h3Meses').html(dMeses);
	$('#h3Anios').html(dAnios);
	$('#tiempoTrabajado').show();
}
