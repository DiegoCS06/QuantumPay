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
        private readonly CuentaComercioCrudFactory _crudFactory;

        public CuentaComercioManager()
        {
            _crudFactory = new CuentaComercioCrudFactory();
        }

        public async Task Create(CuentaComercio cuentaComercio)
        {
            try
            {
                var cExist = _crudFactory.RetrieveByUserName<CuentaComercio>(cuentaComercio.NombreUsuario);

                if (cExist == null)
                {
                    cExist = _crudFactory.RetrieveByEmail<CuentaComercio>(cuentaComercio.CorreoElectronico);

                    if (cExist == null)
                    {
                        cExist = _crudFactory.RetrieveByTelefono<CuentaComercio>(cuentaComercio.Telefono);

                        if (cExist == null)
                        {
                            await Task.Run(() => _crudFactory.Create(cuentaComercio));
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
                throw;
            }
        }

        public List<CuentaComercio> RetrieveAll()
        {
            return _crudFactory.RetrieveAll<CuentaComercio>();
        }

        public CuentaComercio RetrieveById(int Id)
        {
            return _crudFactory.RetrieveById<CuentaComercio>(Id);
        }

        public CuentaComercio RetrieveByEmail(string CorreoElectronico)
        {
            return _crudFactory.RetrieveByEmail<CuentaComercio>(CorreoElectronico);
        }

        public CuentaComercio RetrieveByUserName(string NombreUsuario)
        {
            return _crudFactory.RetrieveByUserName<CuentaComercio>(NombreUsuario);
        }

        public CuentaComercio RetrieveByTelefono(int Telefono)
        {
            return _crudFactory.RetrieveByTelefono<CuentaComercio>(Telefono);
        }

        public void Update(CuentaComercio cuentaComercio)
        {
            try
            {
                var cExist = _crudFactory.RetrieveById<CuentaComercio>(cuentaComercio.Id);
                if (cExist != null)
                {
                    _crudFactory.Update(cuentaComercio);
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
                var cuentaComercio = new CuentaComercio { Id = id };
                var cExist = _crudFactory.RetrieveById<CuentaComercio>(cuentaComercio.Id);
                if (cExist != null)
                {
                    _crudFactory.Delete(cuentaComercio);
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

        public async Task AsociarComercioAsync(int cuentaId, int comercioId)
        {
            await Task.Run(() => AsociarComercio(cuentaId, comercioId));
        }

        public async Task<CuentaComercio> GetByIdAsync(int id)
        {
            return await Task.Run(() => _crudFactory.RetrieveById<CuentaComercio>(id));
        }

        public void AsociarComercio(int cuentaId, int comercioId)
        {
            // Aquí deberías actualizar la cuenta para asociar el comercio
            var crud = new CuentaComercioCrudFactory();
            crud.AsociarComercio(cuentaId, comercioId);
        }
    }
}