using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzR.AuditLog.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext Context;
        private bool _shareContext;
        private bool disposed = false;
        public bool ShareContext
        {
            get { return _shareContext; }
            set { _shareContext = value; }
        }

        public Repository(DbContext context)
        {
            Context = context;
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected DbSet<T> DbSet
        {
            get
            {
                return Context.Set<T>();
            }
        }

        #region IDisposable Members

        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (ShareContext || Context == null) return;
            if (!disposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                        Context = null;
                    }
                }
            }
            disposed = true;
        }


        #endregion

        #region LINQ
        public IQueryable<T> GetAll
        {
            get
            {
                return DbSet.AsNoTracking().AsQueryable();
            }

        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }
        public T First(Expression<Func<T, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public int Count
        {
            get { return DbSet.Count(); }
        }

        public int Counting(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }

        public string MaxValue(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return DbSet.Where(where).Max(predicate);
        }

        public string Max(Expression<Func<T, string>> predicate)
        {
            return DbSet.Max(predicate);
        }

        public T Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }


        public IQueryable<string> Select(Expression<Func<T, string>> predicate)
        {
            return DbSet.Select(predicate);
        }


        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        public T Create(T t)
        {
            //Context.Entry(t).State = EntityState.Added;
            DbSet.Add(t);

            if (!ShareContext)
            {
                Context.SaveChanges();
            }

            return t;
        }

        public int Update(T t)
        {
            var entry = Context.Entry(t);

            DbSet.Attach(t);

            entry.State = EntityState.Modified;

            return !ShareContext ? Context.SaveChanges() : 0;
        }

        public int Delete(T t)
        {
            //  Context.Entry(t).State = EntityState.Deleted;
            DbSet.Remove(t);

            return !ShareContext ? Context.SaveChanges() : 0;
        }

        public bool IsExist(Expression<Func<T, bool>> predicate)
        {
            var count = DbSet.Count(predicate);
            return count > 0;
        }
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var records = FindAll(predicate);

            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return !ShareContext ? Context.SaveChanges() : 0;
        }
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return Context.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        #endregion

        #region LINQ ASYNC

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }
        public async Task<T> CreateAsync(T entity)
        {
            DbSet.Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<int> UpdateAsync(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var entry = Context.Entry(item);
            DbSet.Attach(item);
            entry.State = EntityState.Modified;
            return await Context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(Expression<Func<T, bool>> predicate)
        {
            var records = FindAll(predicate);
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                var entry = Context.Entry(record);

                DbSet.Attach(record);

                entry.State = EntityState.Modified;
            }
            return await Context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(T t)
        {
            DbSet.Remove(t);
            return await Context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var records = await DbSet.Where(predicate).ToListAsync();
            if (!records.Any())
            {
                throw new ObjectNotFoundException();
            }
            foreach (var record in records)
            {
                DbSet.Remove(record);
            }
            return await Context.SaveChangesAsync();
        }
        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }
        public async Task<long> LongCountAsync()
        {
            return await DbSet.LongCountAsync();
        }
        public async Task<int> CountFuncAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.CountAsync(predicate);
        }
        public async Task<long> LongCountFuncAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.LongCountAsync(predicate);
        }
        public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstAsync(predicate);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }
        public async Task<string> MaxAsync(Expression<Func<T, string>> predicate)
        {
            return await DbSet.MaxAsync(predicate);
        }
        public async Task<string> MaxFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).MaxAsync(predicate);
        }
        public async Task<string> MinAsync(Expression<Func<T, string>> predicate)
        {
            return await DbSet.MinAsync(predicate);
        }
        public async Task<string> MinFuncAsync(Expression<Func<T, string>> predicate, Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).MinAsync(predicate);
        }
        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            var count = await DbSet.CountAsync(predicate);
            return count > 0;
        }
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Dictionary

        public string Create(Dictionary<string, object> model, string tableName)
        {
            var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
            var dictonaryField = string.Join(",", model.Keys.ToArray());
            var dictonaryValue = model.Aggregate(string.Empty, (current, o) => current + ("@" + o.Key + ","));
            if (dictonaryValue != "")
                dictonaryValue = dictonaryValue.Remove(dictonaryValue.LastIndexOf(",", StringComparison.Ordinal));
            var dictonaryQuery =
                string.Format(
                    "INSERT INTO {0} ({1}) VALUES ( {2}); SELECT CAST(SCOPE_IDENTITY() AS VARCHAR(50)) AS LAST_IDENTITY;",
                    tableName, dictonaryField, dictonaryValue);
            var result = ExecuteQuery<string>(dictonaryQuery, dictonaryParams.ToArray()).FirstOrDefault();
            return result;
        }

        public int Update(Dictionary<string, object> model, string tableName, string id)
        {
            var dictonaryParams = model.Select(o => new SqlParameter("@" + o.Key, o.Value)).ToList();
            var dictonary = model.Aggregate(string.Empty, (current, o) => current + o.Key + "=" + "@" + o.Key + ",");
            var dictonaryQuery = string.Format("UPDATE {2} SET {0} WHERE Id={1}",
                dictonary.Remove(dictonary.LastIndexOf(",", StringComparison.Ordinal)), id, tableName);
            var result = ExecuteCommand(dictonaryQuery, dictonaryParams.ToArray());
            return result;
        }

        #endregion

    }
}