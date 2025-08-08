using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Cliente : BaseDTO
    {
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string fotoCedula { get; set; }
        public DateOnly fechaNacimiento { get; set; }
        public string fotoPerfil { get; set; }
        public string contrasena { get; set; }
        public string IBAN { get; set; }
        public int IdCliente { get; set; }

        public void Registrarse()
        {
        }

        public void IniciarSesion()
        {
        }

        public void anadirIBAN()
        {
        }

        public void verificarCorreo()
        {
        }

        public void verificarTelefono()
        {
        }

        public void verificarCedula()
        {
        }

        public void realizarPago()
        {

            /*Cuando hay compras mayores a 50 mil, se envía un mensaje a la plataforma para confirmar la cuenta con
            la que quiere pagar(ahí se despliegan las promociones) y así realizar dicho pago*/

            //Solo puede haber una promocion activa
            imprimirRecibo();
        }

        public void imprimirRecibo()
        {
            //Cuando se compra algo, llega un correo de notificación de que se compró en que comercio, con que método de pago y cuanto costó 

        }

        public void MostrarInstitucionesBancarias()
        {
        }


    }
}
