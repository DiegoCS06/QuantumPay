// wwwroot/js/pages/Transacciones.js
function TransaccionesViewController() {
    const ca = new ControlActions();
    const self = this;

    this.Api = "Transaccion";
    this.ApiCuenta = "CuentaCliente";
    this.ApiComercio = "Comercio";

    // Extrae el ID y el email del cliente desde inputs ocultos
    this.getClienteId = () =>
        parseInt($("#hdnClienteId").val() || "0", 10);
    this.getEmail = () =>
        $("#hdnEmail").val() || $("#txtEmail").val() || "";

    // Inicializa la vista
    this.initView = function () {
        this.loadTable();
        this.loadDropdowns();
        this.bindEvents();
        console.log("Transacciones init → OK");
    };

    // Carga el DataTable, con filtro por banco o comercio y header Authorization
    this.loadTable = function () {
        let endpoint = `${this.Api}/RetrieveAll`;
        const filtro = $("#transaccionFiltro").val();
        const valor = $("#filtroValor").val();

        if (filtro === "banco" && valor) {
            endpoint = `${this.Api}/RetrieveByBanco?cuentaId=${valor}`;
        } else if (filtro === "comercio" && valor) {
            endpoint = `${this.Api}/RetrieveByComercio?idComercio=${valor}`;
        }

        const url = ca.GetUrlApiService(endpoint);

        const dtCfg = {
            processing: true,
            ajax: {
                url: url,
                dataSrc: "",
                error: function (xhr, status, err) {
                    console.error("DataTable AJAX error:", status, err);
                },
                columns: [
                    { data: 'id' },
                    { data: 'nombreCliente' }, // Nuevo
                    { data: 'codigoIdentidadInstitucionBancaria' }, // Nuevo
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
            $("#tblTransacciones").DataTable().ajax.url(url).load();
        }
    };

    // Carga los dropdowns de cuentas e IDs de comercio
    this.loadDropdowns = function () {
        const clienteId = this.getClienteId();
        // Cuentas del cliente
        ca.GetToApi(
            `${this.ApiCuenta}/RetrieveAll?clienteId=${clienteId}`,
            data => {
                const ddl = $("#ddlCuentas").empty();
                data.forEach(c => ddl.append(new Option(c.numeroCuenta, c.id)));
            },
            { 'Authorization': 'Bearer ' + userToken }
        );
        // Comercios
        ca.GetToApi(
            `${this.ApiComercio}/RetrieveAll`,
            data => {
                const ddl = $("#ddlComercios").empty();
                data.forEach(c => ddl.append(new Option(c.nombre, c.id)));
            },
            { 'Authorization': 'Bearer ' + userToken }
        );
    };

    // Vincula los eventos de búsqueda, CRUD y selección de fila
    this.bindEvents = function () {
        // Filtrar tabla
        $("#btnBuscar").click(() => this.loadTable());
        $("#transaccionFiltro").change(function () {
            if (this.value === "all") $("#filtroValor").val("");
        });

        // Crear transacción
        $("#btnCreate").click(() => {
            const dto = {
                idCuentaCliente: this.getClienteId(),
                idCuentaBancaria: parseInt($("#ddlCuentas").val(), 10),
                idCuentaComercio: parseInt($("#ddlComercios").val(), 10),
                monto: parseFloat($("#txtMonto").val()),
                comision: parseFloat($("#txtComision").val()),
                descuentoAplicado: parseFloat($("#txtDescuentoAplicado").val()),
                fecha: $("#txtFecha").val(),
                metodoPago: $("#txtMetodoPago").val()
            };
            const email = encodeURIComponent(this.getEmail());
            ca.PostToAPI(
                `${this.Api}/Create?email=${email}`,
                dto,
                () => {
                    window.location.href = "/ClientesPages/ClienteHome";
                },
                { 'Authorization': 'Bearer ' + userToken }
            );
        });

        // Actualizar transacción
        $("#btnUpdate").click(() => {
            const id = parseInt($("#txtId").val(), 10);
            const dto = {
                id,
                idCuentaCliente: this.getClienteId(),
                idCuentaBancaria: parseInt($("#ddlCuentas").val(), 10),
                idCuentaComercio: parseInt($("#ddlComercios").val(), 10),
                monto: parseFloat($("#txtMonto").val()),
                comision: parseFloat($("#txtComision").val()),
                descuentoAplicado: parseFloat($("#txtDescuentoAplicado").val()),
                fecha: $("#txtFecha").val(),
                metodoPago: $("#txtMetodoPago").val()
            };
            ca.PutToAPI(
                `${this.Api}/Update/${id}`,
                dto,
                () => this.loadTable(),
                { 'Authorization': 'Bearer ' + userToken }
            );
        });

        // Eliminar transacción
        $("#btnDelete").click(() => {
            const id = parseInt($("#txtId").val(), 10);
            ca.DeleteToAPI(
                `${this.Api}/Delete/${id}`,
                {},
                () => this.loadTable(),
                { 'Authorization': 'Bearer ' + userToken }
            );
        });

        // Llenar formulario al hacer click en fila
        $("#tblTransacciones tbody").on("click", "tr", function () {
            const data = $("#tblTransacciones").DataTable().row(this).data();
            $("#txtId").val(data.id);
            $("#ddlCuentas").val(data.idCuentaBancaria);
            $("#ddlComercios").val(data.idCuentaComercio);
            $("#txtMonto").val(data.monto);
            $("#txtComision").val(data.comision);
            $("#txtDescuentoAplicado").val(data.descuentoAplicado);
            $("#txtFecha").val(data.fecha);
            $("#txtMetodoPago").val(data.metodoPago);
        });
    };
}

$(document).ready(() => {
    new TransaccionesViewController().initView();
});
