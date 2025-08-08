function ComercioViewController() {
    this.ViewName = "Comercio";
    this.ApiEndPointName = "Comercio";

    //Metodo Constructor
    this.InitView = function () {
        console.log("Comercio init view --> ok");
        this.LoadTable();

        //Asignar eventos a los botones
        $('#btnCreate').click(function () {
            var vc = new ComercioViewController();
            vc.Create();
        });
        $('#btnUpdate').click(function () {
            var vc = new ComercioViewController();
            vc.Update();
        });
        $('#btnDelete').click(function () {
            var vc = new ComercioViewController();
            vc.Delete();
        });
    }

    //Metodo cargar tabla
    this.LoadTable = function () {


        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);


        /*
    {
    "nombre": "Vindi San Rafael de Heredia",
    "idCuenta": 1,
    "estadoSolicitud": "aprobada",
    "id": 3
  }
      <th>ID</th>
                        <th>Cuenta</th>
                        <th>Nombre</th>
                        <th>Estado Solicitud</th>
    */
        var columns = [];
        columns[0] = { 'data': 'id' }; //ID
        columns[1] = { 'data': 'idCuenta' }
        columns[2] = { 'data': 'nombre' }
        columns[3] = { 'data': 'estadoSolicitud' }

        $('#tblcomercio').dataTable({
            "ajax": {
                url: urlService,
                "dataSrc": ""
            },
            columns: columns
        });
        //asignar eventos de carga de datos o bindin segun la tabla
        $('#tblcomercio tbody').on('click', 'tr', function () {

            //Extraemos la fila y los datos de la tabla
            var row = $(this).closest("tr");

            //extraemos el DTO
            //Esto nos devuelve el json de la fila seleccionada por el usuario
            //Segun la data devuelta por el API
            var comercioDTO = $('#tblcomercio').DataTable().row(this).data();

            //Binding en el form
            $('#txtId').val(comercioDTO.id);
            $('#txtIdCuenta').val(comercioDTO.idCuenta);
            $('#txtNombre').val(comercioDTO.nombre);
            $('#txtEstadoSolicitud').val(comercioDTO.estadoSolicitud);
        })
    }
    this.fillForm = function (cuentaComercioDTO) {
        $('#txtIdCuenta').val(cuentaComercioDTO.id);
        $('#txtNombreUsuario').val(cuentaComercioDTO.nombreUsuario);
        $('#txtCedulaJuridica').val(cuentaComercioDTO.cedulaJuridica);
        $('#txtTelefono').val(cuentaComercioDTO.telefono);
        $('#txtCorreoElectronico').val(cuentaComercioDTO.correoElectronico);
        $('#txtDireccion').val(cuentaComercioDTO.direccion);
    }
    this.Create = function () {
        var comercioDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        comercioDTO.id = $('#txtId').val(); 
        comercioDTO.idCuenta = $('#txtIdCuenta').val();


        //Valores capturados en pantalla
        comercioDTO.nombre = $('#txtNombre').val();
        comercioDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();

        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";

        ca.PutToAPI(urlService, comercioDTO, function () {
            //Recargar la tabla
            $('#tblcomercio').DataTable().ajax.reload();
        })
    }
    this.Update = function () {
        var comercioDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        comercioDTO.id = $('#txtId').val();
        comercioDTO.idCuenta = $('#txtIdCuenta').val();


        //Valores capturados en pantalla
        comercioDTO.nombre = $('#txtNombre').val();
        comercioDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();

        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";

        ca.PutToAPI(urlService, comercioDTO, function () {
            //Recargar la tabla
            $('#tblcomercio').DataTable().ajax.reload();
        })
    }
    this.Delete = function () {
        var comercioDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        comercioDTO.id = $('#txtId').val();
        comercioDTO.idCuenta = $('#txtIdCuenta').val();


        //Valores capturados en pantalla
        comercioDTO.nombre = $('#txtNombre').val();
        comercioDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();

        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";

        ca.PutToAPI(urlService, comercioDTO, function () {
            //Recargar la tabla
            $('#tblcomercio').DataTable().ajax.reload();
        })
    }
}
$(document).ready(function () {
    var vc = new ComercioViewController();
    vc.InitView();
})