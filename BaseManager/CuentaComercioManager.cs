using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class CuentaComercioManager : BaseManager
    {

        public async Task Create(CuentaComercio cuentaComercio)
        {
            try
            {
                var cCrud = new CuentaComercioCrudFactory();

                var cExist = cCrud.RetrieveByUserName<CuentaComercio>(cuentaComercio.NombreUsuario);

                if (cExist == null)
                {
                    cExist = cCrud.RetrieveByEmail<CuentaComercio>(cuentaComercio);

                    if (cExist == null)
                    {
                        cExist = cCrud.RetrieveByTelefono<CuentaComercio>(cuentaComercio.Telefono);

                        if (cExist == null)
                        {
                            cCrud.Create(cuentaComercio);
                        }
                        else
                        {
                            throw new Exception("Ese telefono no esta disponible.");
                        }
                    }
                    else
                    {
                        throw new Exception("El email ya esta registrado");
                    }
                }
                else
                {
                    throw new Exception("Ese nombre de usuario no esta disponible");
                }
            }

            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public List<CuentaComercio> RetrieveAll()
        {
            var cCrud = new CuentaComercioCrudFactory();
            return cCrud.RetrieveAll<CuentaComercio>();

        }

        public CuentaComercio RetrieveById(int Id)
        {
            var cCrud = new CuentaComercioCrudFactory();
            return cCrud.RetrieveById<CuentaComercio>(Id);
        }

        public CuentaComercio RetrieveByEmail(string CorreoElectronico)
        {
            var cCrud = new CuentaComercioCrudFactory();
            return cCrud.RetrieveByEmail<CuentaComercio>(CorreoElectronico);
        }

        public CuentaComercio RetrieveByUserName(string NombreUsuario)
        {
            var cCrud = new CuentaComercioCrudFactory();
            return cCrud.RetrieveByUserName<CuentaComercio>(NombreUsuario);
        }

        public CuentaComercio RetrieveByTelefono(int Telefono)
        {
            var cCrud = new CuentaComercioCrudFactory();
            return cCrud.RetrieveByTelefono<CuentaComercio>(Telefono);
        }

        public void Update(CuentaComercio cuentaComercio)
        {
            try
            {
                var cCrud = new CuentaComercioCrudFactory();
                var cExist = cCrud.RetrieveById<CuentaComercio>(cuentaComercio.Id);
                if (cExist != null)
                {
                    cCrud.Update(cuentaComercio);
                }
                else
                {
                    throw new Exception("No existe una cuenta de comercio con ese ID");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var cCrud = new CuentaComercioCrudFactory();
                var cuentaComercio = new CuentaComercio { Id = id };
                var cExist = cCrud.RetrieveById<CuentaComercio>(cuentaComercio.Id);
                if (cExist != null)
                {
                    cCrud.Delete(cuentaComercio);
                }
                else
                {
                    throw new Exception("No existe una cuenta de comercio con ese ID");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}
