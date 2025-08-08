using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CuentaComercio : BaseDTO
    {
        public int Id { get; set; }
        public int? IdComercio { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string CedulaJuridica { get; set; }
        public int Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Direccion { get; set; }

        public void RegistrarComercio()
        {
        }

        public void IniciarSesion()
        {
        }
    }
}
