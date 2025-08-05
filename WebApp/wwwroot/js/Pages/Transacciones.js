function TransaccionesViewController() {
    const ca = new ControlActions();
    this.Api = "Transaccion";

    this.initView = function () {
        this.loadTable();
        this.bindEvents();
        console.log("Transacciones init → OK");
    };

    this.loadTable = function () {
        const filtro = $('#transaccionFiltro').val();
        const valor = $('#filtroValor').val();
        let endpoint = `${this.Api}/RetrieveAll`;

        if (filtro === "banco" && valor) {
            endpoint = `${this.Api}/RetrieveByBanco?iban=${valor}`;
        } else if (filtro === "comercio" && valor) {
            endpoint = `${this.Api}/RetrieveByComercio?idComercio=${valor}`;
        }

        const url = ca.GetUrlApiService(endpoint);

        if (!$.fn.dataTable.isDataTable('#tblTransacciones')) {
            $('#tblTransacciones').DataTable({
                processing: true,
                ajax: {
                    url: url,
                    dataSrc: function (json) {
                        console.log("Respuesta API →", json);
                        return json;
                    },
                    error: function (xhr, status, error) {
                        console.error("Error AJAX DataTable:", status, error);
                        console.log("Response text:", xhr.responseText);
                    },
                    headers: {
                        'Authorization': 'Bearer ' + userToken 
                    }
                },
                columns: [
                    { data: 'id' },
                    { data: 'idCuentaBancaria' },
                    { data: 'idCuentaComercio' },
                    { data: 'monto' },
                    { data: 'comision' },
                    { data: 'descuentoAplicado' },
                    { data: 'fecha' },
                    { data: 'metodoPago' }
                ]
            });
        } else {
            $('#tblTransacciones').DataTable().ajax.url(url).load();
        }
    };

    this.loadTableId = function(){

        var ca = new ControlActions();
        var service = this.Api + "/RetrieveById";
        var urlService = ca.GetUrlApiService(service);

        columns: [
            { data: 'id' },
            { data: 'IdCuentaCliente'},
            { data: 'idCuentaBancaria' },
            { data: 'idCuentaComercio' },
            { data: 'monto' },
            { data: 'descuentoAplicado' },
            { data: 'fecha' },
            { data: 'metodoPago' }
        ]
        $('#tblTransacciones').DataTable({
            "ajax": {
                url: urlService,
                dataSrc: "",
                headers: {
                    'Authorization': 'Bearer ' + userToken
                }
            }
        });

    }

    this.bindEvents = function () {
        $('#btnBuscar').click(() => this.loadTable());

        $('#transaccionFiltro').change(function () {
            if (this.value === 'all') {
                $('#filtroValor').val('');
            }
        });

        $('#btnCreate').click(() => {
            const dto = {
                idCuentaBancaria: $('#txtIdCuentaBancaria').val(),
                idCuentaComercio: parseInt($('#txtIdCuentaComercio').val(), 10),
                monto: parseFloat($('#txtMonto').val()),
                comision: parseFloat($('#txtComision').val()),
                descuentoAplicado: parseFloat($('#txtDescuentoAplicado').val()),
                fecha: $('#txtFecha').val(),
                metodoPago: $('#txtMetodoPago').val()
            };
            ca.PostToAPI(`${this.Api}/Create`, dto, () => this.loadTable());
        });

        $('#btnUpdate').click(() => {
            const id = parseInt($('#txtId').val(), 10);
            const dto = {
                id,
                idCuentaBancaria: $('#txtIdCuentaBancaria').val(),
                idCuentaComercio: parseInt($('#txtIdCuentaComercio').val(), 10),
                monto: parseFloat($('#txtMonto').val()),
                comision: parseFloat($('#txtComision').val()),
                descuentoAplicado: parseFloat($('#txtDescuentoAplicado').val()),
                fecha: $('#txtFecha').val(),
                metodoPago: $('#txtMetodoPago').val()
            };
            ca.PutToAPI(`${this.Api}/${id}`, dto, () => this.loadTable());
        });

        $('#btnDelete').click(() => {
            const id = parseInt($('#txtId').val(), 10);
            ca.DeleteToAPI(`${this.Api}/Delete/${id}`, {}, () => this.loadTable());
        });

        $('#tblTransacciones tbody').on('click', 'tr', function () {
            const data = $('#tblTransacciones').DataTable().row(this).data();
            $('#txtId').val(data.id);
            $('#txtIdCuentaBancaria').val(data.idCuentaBancaria);
            $('#txtIdCuentaComercio').val(data.idCuentaComercio);
            $('#txtMonto').val(data.monto);
            $('#txtComision').val(data.comision);
            $('#txtDescuentoAplicado').val(data.descuentoAplicado);
            $('#txtFecha').val(data.fecha);
            $('#txtMetodoPago').val(data.metodoPago);
        });
    };
}

$(document).ready(() => {
    new TransaccionesViewController().initView();
});
