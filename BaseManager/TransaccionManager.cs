using System;
using System.Collections.Generic;
using DTOs;
using DataAccess.CRUD;

namespace BaseManager
{
    public class TransaccionManager
    {
        private readonly TransaccionCrudFactory crud = new();

        public void Registrar(Transaccion t) => crud.Create(t);

        public List<Transaccion> ObtenerPorBanco(string iban) => crud.RetrieveByBanco(iban);

        public List<Transaccion> ObtenerPorComercio(int idComercio)
            => crud.RetrieveByComercio(idComercio);

        public List<Transaccion> RetrieveAll()
        {
            return crud.RetrieveAll<Transaccion>();
        }
        public void Actualizar(Transaccion t) => crud.Update(t);


    }
}
