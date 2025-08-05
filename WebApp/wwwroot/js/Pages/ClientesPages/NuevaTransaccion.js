// WebApp/wwwroot/js/Pages/ClientesPages/NuevaTransaccion.js

function NuevaTransaccionController() {
    const ca = new ControlActions();
    this.api = "Transaccion";
    this.apiCuenta = "CuentaCliente";
    this.apiComercio = "Comercio";

    this.clienteId = parseInt($("#hdnClienteId").val() || "0", 10);
    this.email = $("#hdnEmail").val() || "";

    this.init = () => {
        this.loadDropdowns();
        this.bindEvents();
    };

    this.loadDropdowns = () => {
        // Carga cuentas bancarias, evitando duplicados
        ca.GetToApi(
            `${this.apiCuenta}/RetrieveAll?clienteId=${this.clienteId}`,
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
            }
        );

        // Carga comercios
        ca.GetToApi(
            `${this.apiComercio}/RetrieveAll`,
            data => {
                const ddl = $("#ddlComercios").empty();
                data.forEach(c => {
                    ddl.append(new Option(c.nombre, c.id));
                });
            }
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
                `${this.api}/Create?email=${encodeURIComponent(this.email)}`,
                dto,
                () => {
                    window.location.href = "/ClientesPages/ClienteHome";
                }
            );
        });
    };
}

$(document).ready(() => new NuevaTransaccionController().init());
