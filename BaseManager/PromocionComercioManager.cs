using DataAccess.CRUD;
using DTOs;

namespace CoreApp
{
    public class PromocionComercioManager : BaseManager
    {
        private readonly PromocionComercioCrudFactory crud = new();

        public async Task Crear(PromocionComercio p) => crud.Create(p);
        public List<PromocionComercio> RetrieveAll() => crud.RetrieveAll<PromocionComercio>();
        public PromocionComercio OrdenarPorId(int id) => crud.RetrieveById<PromocionComercio>(id);
        public void Actualizar(PromocionComercio p) => crud.Update(p);
        public void Eliminar(int id)
        {
            var promocion = new PromocionComercio { Id = id };
            crud.Delete(promocion);
        }
    }
}
