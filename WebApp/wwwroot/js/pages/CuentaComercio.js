function CuentaComercioViewController() {
    this.ViewName = "CuentaComercio";
    this.ApiEndPointName = "CuentaComercio";

    //Metodo Constructor
    this.InitView = function () {
        console.log("User init view --> ok");
        this.LoadTable();

        //Asignar eventos a los botones
        $('#btnCreate').click(function () {
            var vc = new CuentaComercioViewController();
            vc.Create();
        });
        $('#btnUpdate').click(function () {
            var vc = new CuentaComercioViewController();
            vc.Update();
        });
        $('#btnDelete').click(function () {
            var vc = new CuentaComercioViewController();
            vc.Delete();
        });
        $('#btnSearch').click(function () {
            var id = $('#txtSearchComercioId').val();
            if (!id) { return; }

            // Obtenemos la data actual de la tabla
            var data = $('#tblcuentaComercio').DataTable().rows().data().toArray();

            var cuentaComercioDTO = null;
            for (var i = 0; i < data.length; i++) {
                if (data[i].id == id) {
                    cuentaComercioDTO = data[i];
                    break;
                }
            }

            if (cuentaComercioDTO) {
                var vc = new CuentaComercioViewController();
                vc.fillForm(cuentaComercioDTO);
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'ID de Comercio requerido',
                    text: 'Por favor, ingrese un ID válido antes de continuar.',
                    confirmButtonText: 'Aceptar'
                });
            }
            // Disparar búsqueda con Enter
            $('#txtSearchComercioId').keyup(function (e) {
                if (e.key === 'Enter') $('#btnSearch').click();
            });
        }
        );

}

    //Metodo cargar tabla
    this.LoadTable = function () {

        //URL del API a invocar
        //https://localhost:7147/api/User/RetrieveAll

        var ca = new ControlActions();
        var service = this.ApiEndPointName + "/RetrieveAll";
        var urlService = ca.GetUrlApiService(service);

        var cuentaComercioDTO = {};
        cuentaComercioDTO.idcuenta = $('#txtIdCuenta').val(); //ID de la cuenta comercio a consultar
        /*
    {
    "id": 1,
    "nombreUsuario": "sanrafaelherediaVind",
    "contrasena": "vindi12",
    "cedulaJuridica": "402950881",
    "telefono": 12312313,
    "correoElectronico": "sanrafael@comerciosvindi.com",
    "direccion": "san rafael"
  }
     <th>ID Cuenta</th>
                        <th>Nombre Usuario</th>
                        <th>Cédula Jurídica</th>
                        <th>Teléfono</th>
                        <th>Correo Electrónico</th>
                        <th>Dirección</th>
    */
        var columns = [];
        columns[0] = { 'data': 'id' }
        columns[1] = { 'data': 'nombreUsuario' }
        columns[2] = { 'data': 'cedulaJuridica' }
        columns[3] = { 'data': 'telefono' }
        columns[4] = { 'data': 'correoElectronico' }
        columns[5] = { 'data': 'direccion' }

        $('#tblcuentaComercio').dataTable({
            "ajax": {
                url: urlService,
                "dataSrc": ""
            },
            columns: columns
        });
        //asignar eventos de carga de datos o bindin segun la tabla
        $('#tblcuentaComercio tbody').on('click', 'tr', function () {

            //Extraemos la fila y los datos de la tabla
            var row = $(this).closest("tr");

            //extraemos el DTO
            //Esto nos devuelve el json de la fila seleccionada por el usuario
            //Segun la data devuelta por el API
            var cuentaComercioDTO = $('#tblcuentaComercio').DataTable().row(this).data();

            //Binding en el form
            $('#txtIdCuenta').val(cuentaComercioDTO.id);
            $('#txtNombreUsuario').val(cuentaComercioDTO.nombreUsuario);
            $('#txtCedulaJuridica').val(cuentaComercioDTO.cedulaJuridica);
            $('#txtTelefono').val(cuentaComercioDTO.telefono);
            $('#txtCorreoElectronico').val(cuentaComercioDTO.correoElectronico);
            $('#txtDireccion').val(cuentaComercioDTO.direccion);
        })
    }
    this.fillForm = function (cuentaComercioDTO) {
        $('#txtIdCuenta').val(cuentaComercioDTO.id);
        $('#txtNombreUsuario').val(cuentaComercioDTO.nombreUsuario);
        $('#txtCedulaJuridica').val(cuentaComercioDTO.cedulaJuridica);
        $('#txtTelefono').val(cuentaComercioDTO.telefono);
        $('#txtCorreoElectronico').val(cuentaComercioDTO.correoElectronico);
        $('#txtDireccion').val(cuentaComercioDTO.direccion);
    };
    this.Create = function () {
        var cuentaComercioDTO = {};
        cuentaComercioDTO.id = $('#txtId').val();
        cuentaComercioDTO.nombreUsuario = $('#txtNombreUsuario').val();
        cuentaComercioDTO.contrasena = $('#txtContrasena').val();
        cuentaComercioDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        cuentaComercioDTO.telefono = parseInt($('#txtTelefono').val());
        cuentaComercioDTO.correoElectronico = $('#txtCorreoElectronico').val();
        cuentaComercioDTO.direccion = $('#txtDireccion').val();

        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Create";
        ca.PostToAPI(urlService, cuentaComercioDTO, function () {
            $('#tblcuentaComercio').DataTable().ajax.reload();
        })
    }
    this.Update = function () {
        var cuentaComercioDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        cuentaComercioDTO.id = $('#txtIdCuenta').val();

        //Valores capturados en pantalla
        cuentaComercioDTO.nombreUsuario = $('#txtNombreUsuario').val();
        cuentaComercioDTO.contrasena = $('#txtContrasena').val(); //Este campo no se muestra en la pantalla, pero es requerido por el API
        cuentaComercioDTO.cedulaJuridica = $('#txtCedulaJuridica').val();
        cuentaComercioDTO.telefono = $('#txtTelefono').val();
        cuentaComercioDTO.correoElectronico = $('#txtCorreoElectronico').val();
        cuentaComercioDTO.direccion = $('#txtDireccion').val();

        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Update";
        ca.PutToAPI(urlService, cuentaComercioDTO, function () {
            //Recargar la tabla
            $('#tblcuentaComercio').DataTable().ajax.reload();
        })

    }
    this.Delete = function () {
        var cuentaComercioDTO = {};
        //Atributos con valores default, que son coltrolados por el API
        cuentaComercioDTO.id = $('#txtIdCuenta').val();

        //Valores capturados en pantalla
        cuentaComercioDTO.nombreUsuario = $('#txtNombreUsuario').val();
        cuentaComercioDTO.contrasena = $('#txtContrasena').val();
        cuentaComercioDTO.telefono = $('#txtTelefono').val();
        cuentaComercioDTO.correoElectronico = $('#txtCedulaJuridica').val();
        cuentaComercioDTO.direccion = $('#txtDireccion').val();
        

        //Enviar la data al API
        var ca = new ControlActions();
        var urlService = this.ApiEndPointName + "/Delete";
        ca.DeleteToAPI(urlService, cuentaComercioDTO, function () {
            //Recargar la tabla
            $('#tblcuentaComercio').DataTable().ajax.reload();
        })

    }
    this.Search = function () {
        var cuentaComercioDTO = {};
        cuentaComercioDTO.idCuenta = $('#txtIdCuenta').val(); //ID de la cuenta comercio a consultar
        this.LoadTable();
    }
}
$(document).ready(function () {
    var vc = new CuentaComercioViewController();
    vc.InitView();
})