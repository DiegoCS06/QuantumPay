// WebApp/wwwroot/js/Pages/ClientesPages/NuevaTransaccion.js

function NuevaTransaccionController() {
    const ca = new ControlActions();
    this.api = "Transaccion";
    this.apiCuenta = "CuentaCliente";
    this.apiComercio = "Comercio";

    this.userToken = userToken;
    this.clienteId = parseInt($("#hdnClienteId").val() || "0", 10);
    this.nombreCliente = $("#hdnNombreCliente").val() || "";
    
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
                metodoPago: $("#txtMetodoPago").val(),
                nombreCliente: this.nombreCliente,
                codigoIdentidadInstitucionBancaria: codigoIdentidadInstitucionBancaria
            };

            $.ajax({
                url: "/api/Transaccion/Create",
                type: "POST",
                data: JSON.stringify(dto),
                contentType: "application/json",
                headers: { "Authorization": "Bearer " + this.userToken },
                success: function(response) {
                    window.location.href = "/ClientesPages/ClienteHome";
                },
                error: function(response) {
                    // handle error as above
                }
            });
        });
    };
}

$(document).ready(() => new NuevaTransaccionController().init());

// Add these variables at the top, populated from Razor or AJAX
var nombreCliente = '@Model.NombreCliente'; // Razor injects this
var codigoIdentidadInstitucionBancaria = '';

$("#ddlComercios").change(async function() {
    var comercioId = $(this).val();
    // Fetch the commerce details to get the bank code
    const response = await fetch(`/api/Comercio/GetByCuentaId/${comercioId}`);
    if (response.ok) {
        const comercio = await response.json();
        codigoIdentidadInstitucionBancaria = comercio.codigoIdentidadInstitucionBancaria;
    }
});
