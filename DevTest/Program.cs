using CoreApp;
using DataAccess.CRUD;
using DataAccess.DAOs;
using DTOs;

public class Program
{

    public static void Main(string[] args)
    {
        Console.WriteLine("Seleccione la opcion deseada: ");
        Console.WriteLine("1. Menú de Creación");
        Console.WriteLine("2. Menú de Modificación");
        Console.WriteLine("3. Menú de Eliminación");
        Console.WriteLine("4. Menú de BUsqueda");
        Console.WriteLine("5. Salir");


        var option = Int32.Parse(Console.ReadLine());
        var sqlOperation = new SQLOperation();

        switch (option)
        {
            case 1:
                menuCreacion();
                break;
            case 2:
                //menuModifcacion();
                break;
            case 3:
                //menuEliminacion();
                break;
            case 4:
                //menuBusqueda();
                break;
            case 5:
                System.Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Opcion no valida");
                break;
        }
    }

    public static void menuCreacion()
    {
        Console.WriteLine("Seleccione la opcion deseada: ");
        Console.WriteLine("1. Crear Admin");
        Console.WriteLine("2. Crear Cliente");
        Console.WriteLine("3. Crear Comercio");
        Console.WriteLine("4. Crear Cuenta de Comercio");
        Console.WriteLine("5. Crear Institucion Bancaria");
        Console.WriteLine("6. Salir");


        var option = Int32.Parse(Console.ReadLine());
        var sqlOperation = new SQLOperation();

        switch (option)
        {
            case 1:
                CRE_ADMIN_PR();
                break;
            case 2:
                CRE_CLIENTE_PR();
                break;
            case 3:
                //menuEliminacion();
                break;
            case 4:
                //menuBusqueda();
                break;
            case 5:
                System.Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Opcion no valida");
                break;
        }
    }

    public static void CRE_ADMIN_PR()
    {

        Console.WriteLine("Digite el nombre de usuario: ");
        var usuarioAdmin = Console.ReadLine();

        Console.WriteLine("Digite la contraseña: ");
        var contrasenaAdmin = Console.ReadLine();

        var admin = new Admin()
        {
            nombreUsuario = usuarioAdmin,
            contrasena = contrasenaAdmin
        };

        var am = new AdminManager();
        var aCrud = new AdminCrudFactory();

        aCrud.Create(admin);

        var sqlOperation = new SQLOperation();
        sqlOperation.ProcedureName = "CRE_ADMIN_PR";

        sqlOperation.AddStringParameter("P_NombreUsuario", usuarioAdmin);
        sqlOperation.AddStringParameter("P_Contrasena", contrasenaAdmin);

        var sqlDao = SQL_DAO.GetInstance();

        sqlDao.ExecuteProcedure(sqlOperation);

        Console.WriteLine("Admin creado exitosamente.");

    }

    public static void CRE_CLIENTE_PR()
    {
        Console.WriteLine("Ingrese la cedula: ");
        var cedula = Console.ReadLine();
        Console.WriteLine("Ingrese el nombre: ");
        var nombre = Console.ReadLine();
        Console.WriteLine("Ingrese el apellido: ");
        var apellido = Console.ReadLine();
        Console.WriteLine("Ingrese el telefono: ");
        var telefono = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese el correo: ");
        var correo = Console.ReadLine();
        Console.WriteLine("Ingrese la direccion: ");
        var direccion = Console.ReadLine();
        Console.WriteLine("Ingrese la foto de la cedula: ");
        var fotoCedula = Console.ReadLine();
        Console.WriteLine("Ingrese la fecha de nacimiento (YYYY-MM-DD): ");
        var fechaNacimiento = DateOnly.Parse(Console.ReadLine());
        Console.WriteLine("Ingrese la foto de perfil: ");
        var fotoPerfil = Console.ReadLine();
        Console.WriteLine("Ingrese la contraseña: ");
        var contrasena = Console.ReadLine();
        Console.WriteLine("Ingrese el numero de cuenta IBAN (Dejar en blanco si no tiene)");
        var IBAN = Console.ReadLine();

        var cliente = new Cliente()
        {
            cedula = cedula,
            nombre = nombre,
            apellido = apellido,
            telefono = telefono,
            correo = correo,
            direccion = direccion,
            fotoCedula = fotoCedula,
            fechaNacimiento = fechaNacimiento,
            fotoPerfil = fotoPerfil,
            contrasena = contrasena,
            IBAN = IBAN
        };

        var cm = new ClienteManager();
        var cCrud = new ClienteCrudFactory();

        cCrud.Create(cliente);

        var sqlOperation = new SQLOperation();
        sqlOperation.ProcedureName = "CRE_CLIENTE_PR";

        sqlOperation.AddStringParameter("P_Cedula", cedula);
        sqlOperation.AddStringParameter("P_Nombre", nombre);
        sqlOperation.AddStringParameter("P_Apellidos", apellido);
        sqlOperation.AddIntParam("P_Telefono", telefono);
        sqlOperation.AddStringParameter("P_correoElectronico", correo);
        sqlOperation.AddStringParameter("P_Direccion", direccion);
        sqlOperation.AddStringParameter("P_FotoCedula", fotoCedula);
        sqlOperation.AddDateTimeParam("P_FechaNacimiento", fechaNacimiento.ToDateTime(TimeOnly.MinValue));
        sqlOperation.AddStringParameter("P_FotoPerfil", fotoPerfil);
        sqlOperation.AddStringParameter("P_Contrasena", contrasena);
        sqlOperation.AddStringParameter("P_IBAN", IBAN);

        var sqlDao = SQL_DAO.GetInstance();

        sqlDao.ExecuteProcedure(sqlOperation);

        Console.WriteLine("Cliente creado exitosamente.");

    }

}