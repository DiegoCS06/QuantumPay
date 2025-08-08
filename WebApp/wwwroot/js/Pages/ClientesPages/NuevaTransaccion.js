// WebApp/wwwroot/js/Pages/ClientesPages/NuevaTransaccion.js

function NuevaTransaccionController() {
    const ca = new ControlActions();
    this.api = "Transaccion";
    this.apiCuenta = "CuentaCliente";
    this.apiComercio = "Comercio";

    this.userToken = userToken;
    this.clienteId = parseInt($("#hdnClienteId").val() || "0", 10);
    this.userName = $("#hdnEmail").val() || "";
    
    this.init = () => {
        this.loadDropdowns();
        this.bindEvents();
    };

    this.loadDropdowns = () => {
        // Cuentas
        ca.GetToApi(
            `${this.apiCuenta}/RetrieveAll`,
            data => {
                const ddl = $("#ddlCuentas").empty();
                const seen = new Set();
                data.forEach(c => {
                    if (!seen.has(c.id)) {
                        seen.add(c.id);
                        ddl.append(
                            $("<option>", {
                                value: c.id,
                                text: c.numeroCuenta,
                                "data-iban": c.numeroCuenta
                            })
                        );
                    }
                });
            },
            this.userToken 
        );

        // Comercios
        ca.GetToApi(
            `${this.apiComercio}/RetrieveAll`,
            data => {
                const ddl = $("#ddlComercios").empty();
                data.forEach(c => {
                    ddl.append(new Option(c.nombre, c.id));
                });
            },
            this.userToken 
        );
    };

    this.bindEvents = () => {
        $("#btnRealizarPago").click(() => {
            const dto = {
                idCuentaCliente: this.clienteId,
                idCuentaBancaria: parseInt($("#ddlCuentas").val(), 10),
                iban: $("#ddlCuentas option:selected").data("iban"),
                idCuentaComercio: parseInt($("#ddlComercios").val(), 10),
                monto: parseFloat($("#txtMonto").val()),
                comision: 0,
                descuentoAplicado: 0,
                fecha: new Date().toISOString(),
                metodoPago: $("#txtMetodoPago").val()
            };

            ca.PostToAPI(
                `${this.api}/Create?email=${encodeURIComponent(this.userName)}`,
                dto,
                () => {
                    window.location.href = "/ClientesPages/ClienteHome";
                },
                { Authorization: "Bearer " + this.userToken }
            );
        });
    };
}

$(document).ready(() => new NuevaTransaccionController().init());
