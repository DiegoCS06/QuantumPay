// BaseManager/TransaccionManager.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CRUD;
using DTOs;

namespace CoreApp
{
    public class TransaccionManager : BaseManager
    {
        private readonly TransaccionCrudFactory _crud = new();

        public async Task Create(Transaccion t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (t.Monto <= 0) throw new ArgumentException("Monto debe ser > 0");
                        _crud.Create(t);
            await Task.CompletedTask;
        }

        public List<Transaccion> OrdenarPorComercio(int idComercio)
        {
            var cCrud = new TransaccionCrudFactory();
            return cCrud.RetrieveByComercio<List<Transaccion>>(idComercio);
        }

        public Transaccion Update(Transaccion t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));

            var existing = _crud.RetrieveById<Transaccion>(t.Id);
            if (existing == null) throw new Exception("Transacción no encontrada");

            _crud.Update(t);
            return _crud.RetrieveById<Transaccion>(t.Id);
        }

        public void Delete(int id)
        {
            var existing = _crud.RetrieveById<Transaccion>(id);
            if (existing == null) throw new Exception("Transacción no encontrada");

            _crud.Delete(new Transaccion { Id = id });
        }

        public List<Transaccion> RetrieveAll() => _crud.RetrieveAll<Transaccion>();
        public List<Transaccion> RetrieveAllByUserId(int userId)
        {
            var allTransactions = new TransaccionCrudFactory().RetrieveAll<Transaccion>();
            return allTransactions.Where(t => t.IdCuentaCliente == userId).ToList();
        }
        public Transaccion RetrieveById(int id) => _crud.RetrieveById<Transaccion>(id);
        public List<Transaccion> RetrieveByCuenta(int cId) => _crud.RetrieveByBanco(cId);
        public List<Transaccion> RetrieveByComercio(int coId) => _crud.RetrieveByComercio(coId);
        public List<Transaccion> RetrieveByCliente(int cliId) => _crud.RetrieveByCliente(cliId);
    }
}
