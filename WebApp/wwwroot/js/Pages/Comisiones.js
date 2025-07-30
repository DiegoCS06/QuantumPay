function ComisionesViewController() {
    const ca = new ControlActions();
    const self = this;
    this.Api = "Comision";

    this.initView = function () {
        this.loadTable();
        this.bindEvents();
    };

    this.loadTable = function () {
        const ca = new ControlActions();
        const url = ca.GetUrlApiService(this.Api + "/RetrieveAll");
        if (!$.fn.dataTable.isDataTable("#tblComisiones")) {
            $("#tblComisiones").DataTable({
                processing: true,
                ajax: { url: url, dataSrc: "" },
                columns: [
                    { data: "id" },
                    { data: "idInstitucionBancaria" },
                    { data: "idCuentaComercio" },
                    { data: "porcentaje" },
                    { data: "montoMaximo" }
                ]
            });
        } else {
            $("#tblComisiones").DataTable().ajax.url(url).load();
        }
    };

    this.bindEvents = function () {
        const ca = new ControlActions();
        const self = this;

        $("#btnCreate").click(function () {
            const dto = {
                idInstitucionBancaria: parseInt($("#txtIdInstitucionBancaria").val(), 10),
                idCuentaComercio: parseInt($("#txtIdCuentaComercio").val(), 10) || null,
                porcentaje: parseFloat($("#txtPorcentaje").val()),
                montoMaximo: parseFloat($("#txtMontoMaximo").val())
            };
            ca.PostToAPI(self.Api + "/Create", dto, function () { self.loadTable(); });
        });

        $("#btnUpdate").click(function () {
            const dto = {
                id: parseInt($("#txtId").val(), 10),
                idInstitucionBancaria: parseInt($("#txtIdInstitucionBancaria").val(), 10),
                idCuentaComercio: parseInt($("#txtIdCuentaComercio").val(), 10) || null,
                porcentaje: parseFloat($("#txtPorcentaje").val()),
                montoMaximo: parseFloat($("#txtMontoMaximo").val())
            };
            ca.PutToAPI(self.Api + "/Update", dto, function () { self.loadTable(); });
        });

        $("#btnDelete").click(function () {
            const id = parseInt($("#txtId").val(), 10);
            ca.DeleteToAPI(self.Api + "/Delete/" + id, {}, function () { self.loadTable(); });
        });

        $("#tblComisiones tbody").on("click", "tr", function () {
            const data = $("#tblComisiones").DataTable().row(this).data();
            $("#txtId").val(data.id);
            $("#txtIdInstitucionBancaria").val(data.idInstitucionBancaria);
            $("#txtIdCuentaComercio").val(data.idCuentaComercio);
            $("#txtPorcentaje").val(data.porcentaje);
            $("#txtMontoMaximo").val(data.montoMaximo);
        });
    };
}

$(document).ready(function () {
    new ComisionesViewController().initView();
});
