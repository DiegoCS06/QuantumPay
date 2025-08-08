function CuentaClienteController() {
    const ca = new ControlActions();
    this.Api = "CuentaCliente";

    this.init = function () {
        this.loadTable();
        $('#btnCreateCuenta').click(() => this.create());
        $('#btnUpdateCuenta').click(() => this.update());
        $('#btnDeleteCuenta').click(() => this.delete());
    };

    this.getClienteId = function () {
        // now pulls the real hidden input
        return parseInt($('#hdnClienteId').val(), 10);
    };

    this.loadTable = function () {
        const url = ca.GetUrlApiService(`${this.Api}/RetrieveAll`);
        $('#tblCuentaCliente').DataTable({
            destroy: true,
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
                },
            },
            columns: [
                { data: 'banco' },
                { data: 'tipoCuenta' },
                { data: 'numeroCuenta' },
                { data: 'saldo' }
            ]
        });

        $('#tblCuentaCliente tbody')
            .off('click', 'tr')
            .on('click', 'tr', function () {
                var dto = $('#tblCuentaCliente').DataTable().row(this).data();
                $('#txtId').val(dto.id);
                $('#txtNumeroCuenta').val(dto.numeroCuenta);
                $('#selectBanco').val(dto.banco);
                $('#selectTipoCuenta').val(dto.tipoCuenta);
                $('#txtSaldo').val(dto.saldo);
            });
    };

    this.create = function () {
        const dto = {
            clienteId: this.getClienteId(),
            numeroCuenta: $('#txtNumeroCuenta').val(),
            banco: $('#selectBanco').val(),
            tipoCuenta: $('#selectTipoCuenta').val(),
            saldo: parseFloat($('#txtSaldo').val() || 0)
        };
        ca.PostToAPI(`${this.Api}/Create`, dto, () => this.loadTable());
    };  // ← make sure this closing brace+semicolon is here!

    this.update = function () {
        const dto = {
            id: parseInt($('#txtId').val(), 10),
            numeroCuenta: $('#txtNumeroCuenta').val(),
            banco: $('#selectBanco').val(),
            tipoCuenta: $('#selectTipoCuenta').val(),
            saldo: parseFloat($('#txtSaldo').val() || 0)
        };
        ca.PutToAPI(`${this.Api}/Update`, dto, () => this.loadTable());
    };

    this.delete = function () {
        const id = parseInt($('#txtId').val(), 10);
        ca.DeleteToAPI(`${this.Api}/Delete/${id}`, {}, () => this.loadTable());
    };
}

$(document).ready(function () {
    new CuentaClienteController().init();
});
