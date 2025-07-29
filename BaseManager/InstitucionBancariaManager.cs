using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class InstitucionBancariaManager : BaseManager
    {

        public async Task Create(InstitucionBancaria institucionBancaria)
        {
            try
            {
                var iCrud = new InstitucionBancariaCrudFactory();

                var iExist = iCrud.RetrieveById<InstitucionBancaria>(institucionBancaria.Id);

                if (iExist == null)
                {
                    iExist = iCrud.RetrieveByEmail<InstitucionBancaria>(institucionBancaria.correoElectronico);

                    if (iExist == null)
                    {
                        iExist = iCrud.RetrieveByTelefono<InstitucionBancaria>(institucionBancaria.telefono);

                        if(iExist == null)
                        {
                            iCrud.Create(institucionBancaria);
                            return;
                        }

                        throw new Exception("Ese telefono no esta disponible");
                    }

                    throw new Exception("Ese correo electronico no esta disponible.");
                }

                throw new Exception("Ese codigo de identidad no esta disponible");
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

        public List<InstitucionBancaria> RetrieveAll()
        {
            var iCrud = new InstitucionBancariaCrudFactory();
            return iCrud.RetrieveAll<InstitucionBancaria>();
        }

        public InstitucionBancaria RetrieveById(int Id)
        {
            var iCrud = new InstitucionBancariaCrudFactory();
            return iCrud.RetrieveById<InstitucionBancaria>(Id);
        }

        public InstitucionBancaria RetrieveByEmail(string correoElectronico)
        {
            var iCrud = new InstitucionBancariaCrudFactory();
            return iCrud.RetrieveByEmail<InstitucionBancaria>(correoElectronico);
        }

        public InstitucionBancaria RetrieveByCodigoIdentidad(string institucionBancaria)
        {
            var iCrud = new InstitucionBancariaCrudFactory();
            return iCrud.RetrieveByCodigoIdentidad<InstitucionBancaria>(institucionBancaria);
        }
        public InstitucionBancaria RetrieveByTelefono(int telefono)
        {
            var iCrud = new InstitucionBancariaCrudFactory();
            return iCrud.RetrieveByTelefono<InstitucionBancaria>(telefono);
        }

        public void Update(InstitucionBancaria institucionBancaria)
        {
            try
            {
                var iCrud = new InstitucionBancariaCrudFactory();
                var iExist = iCrud.RetrieveById<InstitucionBancaria>(institucionBancaria.Id);
                if (iExist != null)
                {
                    iCrud.Update(institucionBancaria);
                }
                else
                {
                    throw new Exception("No existe una institucion bancaria con ese ID");
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
                var iCrud = new InstitucionBancariaCrudFactory();
                var institucionBancaria = new InstitucionBancaria { Id = id };
                var iExist = iCrud.RetrieveById<InstitucionBancaria>(institucionBancaria.Id);
                if (iExist != null)
                {
                    iCrud.Delete(institucionBancaria);
                }
                else
                {
                    throw new Exception("No existe una institucion bancaria con ese ID");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }
    }
}
