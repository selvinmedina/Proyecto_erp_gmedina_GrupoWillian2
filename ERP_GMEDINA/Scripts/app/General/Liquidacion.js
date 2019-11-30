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
						cargarSpinnerFecha.html();
						cargarSpinnerFecha.hide();
						var mes = 0;
						var anio = 0;
						var dias = 0;

						console.log(data);

						mes = data.sMeses;
						anio = data.sAnios;

						//Metodo 1

						if (data.sDias < 0) {
							dias = data.sDias + 30;
						} else {
							dias = data.sDias;
						}
						mostrarTiempoTrabajado(dias, mes, anio);

						//Metodo 2
						// while (data >= 360) {
						// 	++anio;
						// 	data -= 360;
						// }

						// while (data >= 30) {
						// 	++mes;
						// 	data -= 30;
						// }
						//mostrarTiempoTrabajado(data, mes, anio);
					},
					(enviar) => {
						cargarSpinnerFecha.html(spinner());
						cargarSpinnerFecha.show();
					}
				);
			}
		});

	_ajax(
		null,
		'Liquidacion/GetEmpleadosAreas',
		'GET',
		(dataa) => {
			console.log(dataa);
			$('.select2_demo_3').select2({
				data: [
					{ id: 0, text: 'selvin' },
					{ id: 0, text: 'Walter' }
				]
				// placeholder: 'Seleccione un empleado',
				// language: {
				// 	noResults: function() {
				// 		return 'Resultados no encontrados';
				// 	},
				// 	searching: function() {
				// 		return 'Buscando...';
				// 	}
				// },
				// ajax: {
				// 	url: 'Liquidacion/GetEmpleadosAreas',
				// 	type: 'GET',
				// 	processResults: function(data) {
				// 		// Transforms the top-level key of the response object from 'items' to 'results'
				// 		return {
				// 			results: data.items
				// 		};
				// 	}
				// }
			});
		},
		() => {
			console.log('enviando..');
		}
	);

	var dfd = {
		results: [
			{
				text: 'informatica',
				children: [
					{ id: 1, text: 'Kevin Caballero' },
					{ id: 2, text: 'Juan sanchez' },
					{ id: 3, text: 'Elder sanchez' },
					{ id: 4, text: 'C sanchez' },
					{ id: 5, text: 'D sanchez' },
					{ id: 6, text: 'E sanchez' },
					{ id: 7, text: 'Keneth Sanchez' },
					{ id: 8, text: 'Andrea Flores' },
					{ id: 9, text: 'Nicol Hernandez' },
					{ id: 10, text: 'Hernan Lopez' },
					{ id: 11, text: 'Alejandra Nuñez' },
					{ id: 12, text: 'Maria Florez' },
					{ id: 13, text: 'Marlom Flores' }
				]
			}
		]
	};
	function validarCampos() {
		var todoBien = true;

		//Validar que no este vacio el campo de fecha de despido
		if (inputFechaFin.val() == '') {
			todoBien = false;
		}
		return todoBien;
	}
});

function mostrarTiempoTrabajado(dDias, dMeses, dAnios) {
	$('#h3Dias').html(dDias);
	$('#h3Meses').html(dMeses);
	$('#h3Anios').html(dAnios);
	$('#tiempoTrabajado').show();
}
