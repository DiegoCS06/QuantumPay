using DTOs;
using DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class AdminManager : BaseManager
    {

        public async Task  Create(Admin admin)
        {
            try
            {
                var aCrud = new AdminCrudFactory();

                var aExist = aCrud.RetrieveByUserName<Admin>(admin);

                if (aExist == null)
                {
                    aCrud.Create(admin);
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

        public List<Admin> RetrieveAll()
        {
            var aCrud = new AdminCrudFactory();
            return aCrud.RetrieveAll<Admin>();

        }

        public Admin RetrieveById(int Id)
        {
            var aCrud = new AdminCrudFactory();
            return aCrud.RetrieveById<Admin>(Id);
        }



        public Admin Retrieve(string nombreUsuario)
        {
            var aCrud = new AdminCrudFactory();
            var admin = new Admin { nombreUsuario = nombreUsuario };
            return aCrud.RetrieveByUserName<Admin>(admin);
        }

        public Admin Update(Admin admin)
        {
            var aCrud = new AdminCrudFactory();
            aCrud.Update(admin);
            return RetrieveById(admin.Id);
        }

        public void Delete(int id)
        {
            try
            {
                var aCrud = new AdminCrudFactory();
                var admin = new Admin { Id = id };
                var aExist = aCrud.RetrieveById<Admin>(admin.Id);
                if (aExist != null)
                {
                    aCrud.Delete(admin);
                }
                else
                {
                    throw new Exception("No existe un admin con ese ID");
                }
            }
            catch (Exception ex)
            {
                ManageException(ex);
            }
        }

    }
}
