using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class ComercioManager : BaseManager
    {
        private readonly ComercioCrudFactory _crud = new();
        public async Task Create(Comercio comercio)
        {
            try
            {
                var uCrud = new ComercioCrudFactory();
                
                var uExist = uCrud.RetrieveByComercioName<Comercio>(comercio.Nombre);
                
                if (uExist == null)
                {
                    uCrud.Create(comercio);
                }
                else
                {
                    throw new Exception("El codigo de usuario no esta disponible");
                }
            }

            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public List<Comercio> RetrieveAll() => _crud.RetrieveAll<Comercio>();

        public Comercio RetrieveById(int Id)
        {
            var uCrud = new ComercioCrudFactory();
            return uCrud.RetrieveById<Comercio>(Id);
        }

        public Comercio Update(Comercio comercio)
        {
            var cCrud = new ComercioCrudFactory();
            cCrud.Update(comercio);
            return RetrieveById(comercio.Id);
        }

        public Comercio RetrieveByComercioName(string nombre)
        {
            try
            {
                var uCrud = new ComercioCrudFactory();
                return uCrud.RetrieveByComercioName<Comercio>(nombre);
            }
            catch (Exception ex)
            {
                ManageException(ex);
                return null; // In case of exception, return null
            }
        }
        public void Delete(int id)
        {
            try
            {
                var uCrud = new ComercioCrudFactory();
                var comercio = new Comercio { Id = id };
                var uExist = uCrud.RetrieveById<Comercio>(comercio.Id);
                if (uExist != null)
                {
                    uCrud.Delete(comercio);
                }
                else
                {
                    throw new Exception("No existe un usuario con ese ID");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}
