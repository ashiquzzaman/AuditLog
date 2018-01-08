using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzR.AuditLog.DataAccess.Repositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        #region LINQ

        IQueryable<T> GetAll { get; }
        bool Any(Expression<Func<T, bool>> predicate);
        IQueryable<string> Select(Expression<Func<T, string>> predicate);
        T Find(params object[] keys);
        T Find(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        bool IsExist(Expression<Func<T, bool>> predicate);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        int Count { get; }
        bool ShareContext { get; set; }
        int Counting(Expression<Func<T, bool>> predicate);
        string MaxValue(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where);
        string Max(Expression<Func<T, string>> predicate);
        T Create(T t);
        int Update(T t);

        int Delete(T t);

        int SaveChanges();
        int Delete(Expression<Func<T, bool>> predicate);
        IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters);
        int ExecuteCommand(string sqlCommand, params object[] parameters);
        #endregion

        #region LINQ ASYNC

        Task<ICollection<T>> GetAllAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> CreateAsync(T entity);
        Task<int> UpdateAsync(T item);
        Task<int> UpdateAsync(Expression<Func<T, bool>> predicate);
        Task<int> DeleteAsync(T t);
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<long> LongCountAsync();
        Task<int> CountFuncAsync(Expression<Func<T, bool>> predicate);
        Task<long> LongCountFuncAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<string> MaxAsync(Expression<Func<T, string>> predicate);
        Task<string> MaxFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where);
        Task<string> MinAsync(Expression<Func<T, string>> predicate);
        Task<string> MinFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
        #endregion

        string Create(Dictionary<string, object> model, string tableName);
        int Update(Dictionary<string, object> model, string tableName, string id);
    }
}
