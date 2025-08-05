function TransaccionesClienteController() {
    const ca = new ControlActions();
    this.api = "Transaccion";

    this.init = function () {
        this.loadTable();
    };

    this.getClienteId = function () {
        return $('#hdnClienteId').val();
    };

    this.loadTable = function () {
        const url = ca.GetUrlApiService(`${this.api}/RetrieveByCliente?clienteId=${this.getClienteId()}`);
        $('#tblTransaccionesCliente').DataTable({
            destroy: true,
            ajax: { url: url, dataSrc: '' },
            columns: [
                { data: 'id' },
                { data: 'fecha' },
                { data: 'monto' },
                { data: 'metodoPago' },
                { data: 'idCuentaBancaria' },
                { data: 'idCuentaComercio' }
            ]
        });
    };
}

$(document).ready(function () {
    new TransaccionesClienteController().init();
});
