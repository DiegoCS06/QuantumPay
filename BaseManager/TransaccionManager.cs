using DataAccess.CRUD;
using DTOs;

namespace CoreApp
{
    public class TransaccionManager : BaseManager
    {
        public async Task Create(Transaccion transaccion)
        {
            if (transaccion == null)
                throw new Exception("La transacción no puede ser nula.");

            if (transaccion.Monto <= 0)
                throw new Exception("El monto de la transacción debe ser mayor a cero.");

            // Validar existencia de Cliente
            var clienteCrud = new ClienteCrudFactory();
            var clienteExist = clienteCrud.RetrieveById<Cliente>(transaccion.IdCuentaCliente);
            if (clienteExist == null)
                throw new Exception("El cliente especificado no existe.");

            // Validar existencia de Comercio
            var comercioCrud = new ComercioCrudFactory();
            var comercioExist = comercioCrud.RetrieveById<Comercio>(transaccion.IdCuentaComercio);
            if (comercioExist == null)
                throw new Exception("El comercio especificado no existe.");

            // Validar existencia de Institución Bancaria
            var bancoCrud = new InstitucionBancariaCrudFactory();
            var bancoExist = bancoCrud.RetrieveById<InstitucionBancaria>(transaccion.IdCuentaBancaria);
            if (bancoExist == null)
                throw new Exception("La cuenta bancaria especificada no existe.");

            var tCrud = new TransaccionCrudFactory();
            tCrud.Create(transaccion);
        }

        public List<Transaccion> RetrieveAll(int clientId, string clientRole)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveAll<Transaccion>();
        }

        public Transaccion OrdenarPorId(int id)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveById<Transaccion>(id);
        }

        public Transaccion OrdenarPorBanco(int idComercio)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveByBanco<Transaccion>(idComercio);
        }


        public Transaccion OrdenarPorComercio(int idComercio)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveByComercio<Transaccion>(idComercio);
        }

        public Transaccion OrdenarPorCliente(int idCliente)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveByCliente<Transaccion>(idCliente);
        }


        public Transaccion Update(Transaccion t)
        {
            if (t == null)
                throw new Exception("La transacción no puede ser nula.");

            var cCrud = new TransaccionCrudFactory();
            var transaccionExistente = cCrud.RetrieveById<Transaccion>(t.Id);
            if (transaccionExistente == null)
                throw new Exception("No existe una transacción con ese ID.");

            // Validaciones de entidades relacionadas
            var clienteCrud = new ClienteCrudFactory();
            var clienteExist = clienteCrud.RetrieveById<Cliente>(t.IdCuentaCliente);
            if (clienteExist == null)
                throw new Exception("El cliente especificado no existe.");

            var comercioCrud = new ComercioCrudFactory();
            var comercioExist = comercioCrud.RetrieveById<Comercio>(t.IdCuentaComercio);
            if (comercioExist == null)
                throw new Exception("El comercio especificado no existe.");

            var bancoCrud = new InstitucionBancariaCrudFactory();
            var bancoExist = bancoCrud.RetrieveById<InstitucionBancaria>(t.IdCuentaBancaria);
            if (bancoExist == null)
                throw new Exception("La cuenta bancaria especificada no existe.");

            cCrud.Update(t);
            return OrdenarPorId(t.Id);
        }

        public void Delete(int id)
        {
            var cCrud = new TransaccionCrudFactory();
            var transaccion = new Transaccion { Id = id };
            var cExist = cCrud.RetrieveById<Transaccion>(transaccion.Id);
            if (cExist != null)
            {
                cCrud.Delete(transaccion);
            }
            else
            {
                throw new Exception("No existe una transacción con ese ID");
            }
        }
    }
}