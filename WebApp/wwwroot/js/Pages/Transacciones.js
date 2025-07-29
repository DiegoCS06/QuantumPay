
function TransaccionesViewController() {
    const ca = new ControlActions();
    const self = this;
    this.Api = "Transaccion";

    this.initView = function () {
        this.bindFilterPlaceholder();
        this.loadTable();
        this.bindEvents();
        console.log("Transacciones init → OK");
    };

    this.bindFilterPlaceholder = function () {
        $('#transaccionFiltro').on('change', function () {
            const tipo = $(this).val();
            const $input = $('#filtroValor');
            if (tipo === 'banco') {
                $input.prop('disabled', false).attr('placeholder', 'Ingrese IBAN');
            } else if (tipo === 'comercio') {
                $input.prop('disabled', false).attr('placeholder', 'Ingrese ID de Comercio');
            } else {
                $input.val('').prop('disabled', true).attr('placeholder', 'IBAN o ID de Comercio');
            }
        }).trigger('change');
    };

    this.loadTable = function () {
        const filtro = $('#transaccionFiltro').val();
        const valor = $('#filtroValor').val().trim();
        let endpoint = `${this.Api}/RetrieveAll`;

        if (filtro === 'banco' && valor) {
            endpoint = `${this.Api}/RetrieveByBanco?iban=${encodeURIComponent(valor)}`;
        } else if (filtro === 'comercio' && valor) {
            endpoint = `${this.Api}/RetrieveByComercio?idComercio=${encodeURIComponent(valor)}`;
        }

        const url = ca.GetUrlApiService(endpoint);

        if (!$.fn.dataTable.isDataTable('#tblTransacciones')) {
            $('#tblTransacciones').DataTable({
                processing: true,
                ajax: { url, dataSrc: '' },
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

    this.bindEvents = function () {
        $('#btnBuscar').on('click', () => this.loadTable());

        $('#transaccionFiltro').on('change', function () {
            if (this.value === 'all') {
                $('#filtroValor').val('');
            }
        });

        $('#btnCreate').on('click', () => {
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

        $('#btnUpdate').on('click', () => {
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

        $('#btnDelete').on('click', () => {
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

$(document).ready(() => new TransaccionesViewController().initView());
