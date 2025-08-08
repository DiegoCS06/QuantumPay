using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PromocionManager : BaseManager
    {
        private readonly PromocionCrudFactory crud = new();

        public async Task Create(Promocion p) => crud.Create(p);
        public List<Promocion> RetrieveAll() => crud.RetrieveAll<Promocion>();
        public Promocion RetrieveById(int id) => crud.RetrieveById<Promocion>(id);
        public void Update(Promocion p) => crud.Update(p);
        public void Delete(int id)
        {
            var promocion = new Promocion { Id = id };
            crud.Delete(promocion);
        }
    }
}
