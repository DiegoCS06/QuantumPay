using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PromocionBancoManager : BaseManager
    {
        private readonly PromocionBancoCrudFactory crud = new();

        public async Task Crear(PromocionBanco p) => crud.Create(p);
        public List<PromocionBanco> RetrieveAll() => crud.RetrieveAll<PromocionBanco>();
        public PromocionBanco OrdenarPorId(int id) => crud.RetrieveById<PromocionBanco>(id);
        public void Actualizar(PromocionBanco p) => crud.Update(p);
        public void Eliminar(int id)
        {
            var promocion = new PromocionBanco { Id = id };
            crud.Delete(promocion);
        }
    }
}
