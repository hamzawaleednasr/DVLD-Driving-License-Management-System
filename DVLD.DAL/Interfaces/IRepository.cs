using System.Collections.Generic;

namespace DVLD.DAL.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetByID(int id);
        int Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}
