//Funcion generica para reutilizar AJAX
function _ajax(params, uri, type, callback, enviar) {
	$.ajax({
		url: uri,
		type: type,
		dataType: 'json',
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify(params),
		beforeSend: function () {
			enviar();
		},
		success: function (data) {
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
$(document).ready(function () {
	$('#datepicker .input-group.date')
		.datepicker({
			todayBtn: 'linked',
			keyboardNavigation: false,
			forceParse: false,
			calendarWeeks: true,
			autoclose: true,
			format: 'yyyy/mm/dd'
		})
		.on('changeDate', function (e) {
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
						anio = data.sAnios

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

	$(".select2_demo_3").select2({
		'placeholder': "Seleccione un empleado",
		'allowClear': true,
		"language": {
			"noResults": function () {
				return "Resultados no encontrados";
			},
			'searching': function () {
				return "Buscando...";
			}
		},
	});


	// $(".js-data-example-ajax").select2({
	// 	multiple: true,
	// 	minimumInputLength: 1,
	// 	ajax: {
	// 		url: "https://api.myjson.com/bins/55p57",
	// 		dataType: 'json',
	// 		delay: 250,
	// 		data: function (params) {
	// 			return {
	// 				q: params.term, // search term
	// 			};
	// 		}
	// 	},
	// 	//escapeMarkup: function (markup) { return markup; }, // let our custom formatter work

	// });



	var id = {
		"data" : {
			'text': 'Mountain Time Zone',
			'children': [
			  {
				'id': 'CA',
				'text': 'California'
			  },
			  {
				'id': 'CO',
				'text': 'Colorado'
			  }
			]
		}
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
