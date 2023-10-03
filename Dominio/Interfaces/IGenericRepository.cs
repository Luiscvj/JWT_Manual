using System.Linq.Expressions;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Dominio.Interfaces;

    public interface  IGenericRepository<T>  
    {
        

        void Add(T Entity);
        

        void AddRange(IEnumerable<T> entities);
       

        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
       

        Task<IEnumerable<T>> GetAll();
       

        Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search);
       

        Task<T> GetById(int Id);
      

        void Remove(T Entity);
        

        void  RemoveRange(IEnumerable<T> entities);
       

        void Update(T Entity);
        
    }
