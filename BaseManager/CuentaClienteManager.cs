using System.Collections.Generic;
using DataAccess.CRUD;
using DTOs;

namespace CoreApp
{
    public class CuentaClienteManager : BaseManager
    {
        private readonly ClienteCuentaCrudFactory crud = new();

        public void Crear(ClienteCuenta c) => crud.Create(c);
        public void Actualizar(ClienteCuenta c) => crud.Update(c);
        public void Eliminar(int id) => crud.Delete(new ClienteCuenta { Id = id });
        public List<ClienteCuenta> Listar(int clienteId) => crud.RetrieveByCliente(clienteId);
    }
}
