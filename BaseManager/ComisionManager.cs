using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using DataAccess.CRUD;

namespace BaseManager
{
    public class ComisionManager
    {
        private readonly ComisionCrudFactory crud = new();

        public async Task Create(Comision c) => crud.Create(c);
        public void Actualizar(Comision c) => crud.Update(c);
        public void Eliminar(Comision c) => crud.Delete(c);
        public Comision Obtener(int id) => crud.RetrieveById<Comision>(new Comision { Id = id });
        public List<Comision> Listar() => crud.RetrieveAll<Comision>();
    }
}