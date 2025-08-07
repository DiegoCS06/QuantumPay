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

        var columns = [];
        columns[0] = { 'data': 'id' }; //ID
        columns[1] = { 'data': 'codigoIdentidad' }
        columns[2] = { 'data': 'cedulaJuridica' }
        columns[3] = { 'data': 'direccionSedePrincipal' }
        columns[4] = { 'data': 'telefono' }
        columns[5] = { 'data': 'correoElectronico' }

        $('#tblinstitucionBancaria').dataTable({
            "ajax": {
                url: urlService,
                "dataSrc": "",
                headers: {
                    'Authorization': 'Bearer ' + userToken // Assuming userToken is defined into the view scope
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
        })
    }
    this.fillForm = function (bancoDTO) {
        $('#txtId').val(bancoDTO.id);
        $('#txtCodigoIdentidad').val(bancoDTO.codigoIdentidad);
        $('#txtCedulaJuridica').val(bancoDTO.cedulaJuridica);
        $('#txtDireccionSedePrincipal').val(bancoDTO.direccionSedePrincipal);
        $('#txtTelefono').val(bancoDTO.telefono);
        $('#txtCorreoElectronico').val(bancoDTO.correoElectronico);
    }
    this.Create = function () {

        var bancoDTO = {};
        bancoDTO.id = $('#txtId').val();
        bancoDTO.codigoIdentidad = $('#txtCodigoIdentidad').val();
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";
        ca.PostToAPI(urlService, bancoDTO, function () {
            $('#tblinstitucionBancaria').DataTable().ajax.reload();
        })
    }
    this.Update = function () {
        var bancoDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        bancoDTO.id = $('#txtId').val(); //ID del banco
        bancoDTO.codigoIdentidad = $('#txtId').val();


        //Valores capturados en pantalla
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val(); //Este es un valor default, controlado por el API
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();

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
        bancoDTO.id = $('#txtId').val();
        bancoDTO.codigoIdentidad = $('#txtCodigoIdentidad').val();

        //Valores capturados en pantalla
        bancoDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        bancoDTO.direccionSedePrincipal = $('#txtDireccionSedePrincipal').val();
        bancoDTO.telefono = $('#txtTelefono').val();
        bancoDTO.estadoSolicitud = $('#txtEstadoSolicitud').val();
        bancoDTO.correoElectronico = $('#txtCorreoElectronico').val();


        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";
        ca.DeleteToAPI(urlService, bancoDTO, function () {
            //Recargar la tabla
            $('#tblcuentaComercio').DataTable().ajax.reload();
        })
    }
    
}


$(document).ready(function () {
    var vc = new BancoViewController();
    vc.InitView();
})