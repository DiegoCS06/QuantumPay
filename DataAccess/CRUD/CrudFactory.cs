using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{

    //Clase madre abstracta de los CRUDs
    //Define como se hacen CRUDs en la arquitectura.


    public abstract class CrudFactory
    {

        protected SQL_DAO _sqlDao;

        //Definir los metodos que forman parte del contrato
        //C = Create
        //R = Read
        //U = Update
        //D = Delete

        public abstract void Create(BaseDTO baseDTO);

        public abstract void Update(BaseDTO baseDTO);

        public abstract void Delete(BaseDTO baseDTO);

        public abstract T Retrieve<T>();

        public abstract T RetrieveById<T>(int Id);

        public abstract List<T> RetrieveAll<T>();

    }
}
