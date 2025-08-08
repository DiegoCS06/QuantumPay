function ClientesViewController() {
    this.ViewName = "Cliente";
    this.ApiEndPointName = "Cliente";

    //Metodo Constructor
    this.InitView = function () {
        console.log("Institucion Bancaria init view --> ok");
        this.LoadTable();

    }

    //Metodo cargar tabla
    this.LoadTable = function () {


        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        var columns = [];
        columns[0] = { 'data': 'id' }; 
        columns[1] = { 'data': 'nombre' }
        columns[2] = { 'data': 'apellido' }
        columns[3] = { 'data': 'cedula' }
        columns[4] = { 'data': 'correo' }
        columns[5] = { 'data': 'direccion' }
        columns[7] = { 'data': 'fechaNacimiento' }
        columns[6] = { 'data': 'telefono' }
        columns[8] = { 'data': 'iban' }

        $('#tblcliente').dataTable({
            "ajax": {
                url: urlService,
                "dataSrc": ""
            },
            columns: columns
        });
        
    }
}
$(document).ready(function () {
    var vc = new ClientesViewController();
    vc.InitView();
})