function ClienteHomePage() {
    const ca = new ControlActions();
    this.apiCuenta = "CuentaCliente";
    this.apiTrans = "Transaccion";

    this.init = function () {
        this.loadCuentas();
        this.loadTransacciones();

        // Aquí va el listener del botón Nueva Transacción
        $('#btnNuevaTransaccion').click(() => {
            const id = this.getClienteId();
            const email = $('#hdnClienteEmail').val();
            window.location.href = `/ClientesPages/NuevaTransaccion?clienteId=${id}&email=${encodeURIComponent(email)}`;
        });
    };

    this.getClienteId = function () {
        return $('#hdnClienteId').val();
    };

    this.loadCuentas = function () {
        const url = ca.GetUrlApiService(`${this.apiCuenta}/RetrieveAll?clienteId=${this.getClienteId()}`);
        $('#tblCuentasHome').DataTable({
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
                }
            },
            columns: [
                { data: 'banco' },
                { data: 'tipoCuenta' },
                { data: 'numeroCuenta' },
                { data: 'saldo' }
            ]
        });
    };

    this.loadTransacciones = function () {
        const url = ca.GetUrlApiService(`${this.apiTrans}/RetrieveAll`);
        $('#tblUltimasTransacciones').DataTable({
            destroy: true,
            ajax: {
                url: url, dataSrc: '',
                headers: { 'Authorization': 'Bearer ' + userToken }
            },
            columns: [
                { data: 'fecha' },
                { data: 'metodoPago' },
                { data: 'monto' },
                { data: 'id' }
            ]
        });
    };
}

$(document).ready(() => new ClienteHomePage().init());
