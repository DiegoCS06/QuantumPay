function BancoViewController() {
    this.ViewName = "InstitucionBancaria";
    this.ApiEndPointName = "InstitucionBancaria";

    //Metodo Constructor
    this.InitView = function () {
        console.log("Institucion Bancaria init view --> ok");
        this.LoadTable();

        //Asignar eventos a los botones
        $('#btnCreate').click(function () {
            var vc = new BancoViewController();
            vc.Create();
        });
        $('#btnUpdate').click(function () {
            var vc = new BancoViewController();
            vc.Update();
        });
        $('#btnDelete').click(function () {
            var vc = new BancoViewController();
            vc.Delete();
        });
    }

    //Metodo cargar tabla
    this.LoadTable = function () {

        //URL del API a invocar
        //https://localhost:5001/api/InstitucionBancaria/RetrieveAll

        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);


        /*
    {
    "codigoIdentidad": 1,
    "codigoIBAN": 1,
    "cedulaJuridica": "1313415123",
    "direccionSedePrincipal": "Los Yoses, San Jose",
    "telefono": 12345678,
    "correoElectronico": "bn@gov.cr",
    "estadoSolicitud": "aprobada",
    "id": 2
  }
      <th>ID</th>
                        <th>Código Identidad</th>
                        <th>Código IBAN</th>
                        <th>Cédula Jurídica</th>
                        <th>Dirección</th>
                        <th>telefono</th>
                        <th>Estado Solicitud</th>
                        <th>Correo Electrónico</th>
    */
        var columns = [];
        columns[0] = { 'data': 'id' }; //ID
        columns[1] = { 'data': 'codigoIdentidad' }
        columns[2] = { 'data': 'cedulaJuridica' }
        columns[3] = { 'data': 'direccionSedePrincipal' }
        columns[4] = { 'data': 'telefono' }
        columns[5] = { 'data': 'correoElectronico' }
        columns[6] = { 'data': 'estadoSolicitud' }

        $('#tblinstitucionBancaria').dataTable({
            "ajax": {
                url: urlService,
                "dataSrc": "",
                headers: {
                    'Authorization': 'Bearer ' + userToken 
                }
            },            
            columns: columns
        });
        //asignar eventos de carga de datos o bindin segun la tabla
        $('#tblinstitucionBancaria tbody').on('click', 'tr', function () {

            //Extraemos la fila y los datos de la tabla
            var row = $(this).closest("tr");

            //extraemos el DTO
            //Esto nos devuelve el json de la fila seleccionada por el usuario
            //Segun la data devuelta por el API
            var bancoDTO = $('#tblinstitucionBancaria').DataTable().row(this).data();

            //Binding en el form
            $('#txtId').val(bancoDTO.id);
            $('#txtCodigoIdentidad').val(bancoDTO.codigoIdentidad);
            $('#txtCedulaJuridica').val(bancoDTO.cedulaJuridica);
            $('#txtDireccionSedePrincipal').val(bancoDTO.direccionSedePrincipal);
            $('#txtTelefono').val(bancoDTO.telefono);
            $('#txtCorreoElectronico').val(bancoDTO.correoElectronico);
            $('#txtEstadoSolicitud').val(bancoDTO.estadoSolicitud);
            
        })
    }
    this.Create = function () {
        var bancoDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        bancoDTO.id = 0; //ID del banco
        bancoDTO.codigoIdentidad = $('#txtCodigoIdentidad').val();


        //Valores capturados en pantalla
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();
        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";

        ca.PostToAPI(urlService, bancoDTO, function () {
            //Recargar la tabla
            $('#tblinstitucionBancaria').DataTable().ajax.reload();
        })
    }
    this.Update = function () {
        var bancoDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        bancoDTO.id = $('#txtId').val(); //ID del banco
        bancoDTO.codigoIdentidad = $('#txtCodigoIdentidad').val();
        

        //Valores capturados en pantalla
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();
        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";

        ca.PutToAPI(urlService, bancoDTO, function () {
            //Recargar la tabla
            $('#tblinstitucionBancaria').DataTable().ajax.reload();
        })
    }
    this.Delete = function () {
        var bancoDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        bancoDTO.id = $('#txtId').val(); //ID del banco
        bancoDTO.codigoIdentidad = $('#txtCodigoIdentidad').val();


        //Valores capturados en pantalla
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();
        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";

        ca.DeleteToAPI(urlService, bancoDTO, function () {
            //Recargar la tabla
            $('#tblinstitucionBancaria').DataTable().ajax.reload();
        })
    }
}
$(document).ready(function () {
    var vc = new BancoViewController();
    vc.InitView();
})