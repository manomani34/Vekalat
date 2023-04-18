using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vekalat.Application.Common
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll(bool tracked = false, string includes = "");
        Task<T> GetById(int id, bool tracked = false, string includes = "");
        Task<T> InsertNew(T newEntity);
        Task Update(T updateEntity);
        Task<bool> IsExist(int id);
        Task Delete(int id);
        Task Delete(T deleteEntity);
        Task DeleteRang(List<T> deleteEntities);

        Task SoftDelete(int id);
        Task SoftDelete(T deleteEntity);
        Task SoftDeleteRang(List<T> deleteEntities);

        void SaveChanges();
        Task SaveChangesAsync();

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }

}
