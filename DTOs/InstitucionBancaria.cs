using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class InstitucionBancaria : BaseDTO
    {
        public string codigoIdentidad { get; set; }
        public string codigoIBAN { get; set; }
        public string cedulaJuridica { get; set; }
        public string direccionSedePrincipal { get; set; }
        public int telefono { get; set; }
        public string correoElectronico { get; set; }
        public string estadoSolicitud { get; set; }
        public string contrasena { get; set; }


        public void iniciarSesion()
        {
        }

        public void ofrecerDescuentos()
        { 
        }

        public void RegistrarInstitucionBancaria()
        {
        }

        public void verTransacciones()
        {
        }



    }
}
