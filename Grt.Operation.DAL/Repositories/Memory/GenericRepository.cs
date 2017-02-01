using CMP.Operation.DAL.Functions;
using CMP.Operation.DAL.Functions.Extensions;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    /// <summary>
    /// Generic class that contain all the functions to One Entry Type of Entity Context 
    /// </summary>
    /// <param name="C">
    /// Context
    /// </param>
    /// <param name="T">
    /// Type of Table 
    /// </param>
    public abstract class GenericRepository<C, T> : IGenericRepository<T> where T : class where C : DbContext, new()
    {
        #region Properties

        private C _entities = new C();
        protected C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }
        protected DbSet<T> DbSet { get; set; }
        #endregion


        #region Constructors 
        public GenericRepository()
        {
            try
            {
                DbSet = _entities.Set<T>();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        #endregion

        #region Private Methods


        /// <summary>
        /// gets primary keys
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private int GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)_entities).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return (int)objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        private int GetEntityID(DbEntityEntry entry)
        {
            //var x= entry.CurrentValues.PropertyNames.First();
            var x = entry.Entity.GetType().GetProperties().FirstOrDefault(
                    p => p.CustomAttributes.Count() > 0 && p.CustomAttributes.Any(
                        attr => attr.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute)
                    && attr.ConstructorArguments.Count() > 0 && attr.ConstructorArguments.Any(ca => ca.Value.ToString() == "1"))
                    );
            if (x != null)
                return (int)entry.Property(x.Name.ToString()).CurrentValue;
            else return GetPrimaryKeyValue(entry);
            // entry.CurrentValues.
            //  var objectStateEntry = ((IObjectContextAdapter)_entities).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            //  return objectStateEntry.    
            // return x;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public  virtual int Save(WindowsPrincipal NTUser, bool isSystem = false, IDictionary<string, string> Fields = null)
        {
            int count = 0;
            try
            {
                //var EditedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                //var AddedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                //var DeletedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

                count += SaveChanges();

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                count = -1;
            }
            return count;
        }


        /// <summary>
        /// add entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual T Add(T entity)//changed from void to T
        {
            try
            {
                return DbSet.Add(entity);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return null;
        }
        /// <summary>
        /// AddRange entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void AddRange(IEnumerable<T> entities)
        {
            try
            {
                DbSet.AddRange(entities);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        public virtual void AddOrUpdate(Expression<Func<T, bool>> predicate, IEnumerable<T> entities)
        {
            try
            {
                DbSet.AddOrUpdate(entities.ToArray());
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }


        public virtual void UpdateChildCollection<T, Tid, Tchild>(T dbItem, T newItem, Func<T, IEnumerable<Tchild>> selector, Func<Tchild, Tid> idSelector) where Tchild : class
        {
            var dbItems = selector(dbItem).ToList();
            var newItems = selector(newItem).ToList();

            if (dbItems == null && newItems == null)
                return;

            var original = dbItems?.ToDictionary(idSelector) ?? new Dictionary<Tid, Tchild>();
            var updated = newItems?.ToDictionary(idSelector) ?? new Dictionary<Tid, Tchild>();

            var toRemove = original.Where(i => !updated.ContainsKey(i.Key)).ToArray();
            var removed = toRemove.Select(i => Context.Entry(i.Value).State = EntityState.Deleted).ToArray();

            //modify without ID field that is identity field
            var toUpdate = original.Where(i => updated.ContainsKey(i.Key)).ToList();
            toUpdate.ForEach(i =>
            {
                var excluded = new [] { "ID" };
                var entry = Context.Entry(i.Value);
                foreach (var name in entry.CurrentValues.PropertyNames.Except(excluded))
                {
                    entry.Property(name).IsModified = true;
                }
                // Context.Entry(i.Value).CurrentValues..SetValues(updated[i.Key])
            });

            var toAdd = updated.Where(i => !original.ContainsKey(i.Key)).ToList();
            toAdd.ForEach(i => Context.Set<Tchild>().Add(i.Value));
        }


        /// <summary>
        /// Attach entity to Db for edit
        /// </summary>
        /// <param name="entity"></param>
        public void Edit(T entity)
        {
            try
            {
                DbSet.Attach(entity);
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// delete entity to the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void Delete(T entity)
        {
            try
            {
                DbSet.Remove(entity);

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// delete range of entities from the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                DbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// delete by predicate from the table
        /// </summary>
        /// <param name="predicate"></param>        
        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            try
            {
                DbSet.RemoveRange(DbSet.Where(predicate));
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }

        /// <summary>
        /// delete all of entities from the table
        /// </summary>
        /// <param name="entity"></param>        
        public virtual void DeleteAll()
        {
            try
            {
                DbSet.RemoveRange(DbSet);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        /// <summary>
        /// Save all changes on DB
        /// </summary>
        public virtual void Save()
        {
            try
            {

                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        public virtual int Count()
        {
            int Cnt = 0;
            try
            {
                Cnt = DbSet.Count();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return Cnt;
        }
        public void Dispose()
        {
            try
            {
                Context.Dispose();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
        }
        /// <summary>
        /// Find one instance By key/s of the table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find(params object[] KeyValues)
        {
            T item = null;
            try
            {
                item = DbSet.Find(KeyValues);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }

        /// <summary>
        /// First Or Default conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FindSingle(Expression<Func<T, bool>> predicate)
        {
            T item = null;
            try
            {
                item = DbSet.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }
        /// <summary>
        /// Find by conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = null;
            try
            {
                items = DbSet.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        /// <summary>
        /// get all list of table
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetAll()
        {
            List<T> items = null;
            try
            {
                items = DbSet.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        /// <summary>
        /// get list of table according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public List<T> Get(int StartIndex, int Count, string Sorting, out int CountRecords)
        {
            CountRecords = this.Count();
            var query = Sorting != null ? DbSet.GetOrderByQuery(Sorting) : DbSet;
            return Count > 0 && CountRecords > 0
                       ? query.Skip(StartIndex).Take(Count).ToList() //Paging
                       : query.ToList(); //No paging
        }

        // Summary:
        //     Creates a System.Collections.Generic.Dictionary`2 from an System.Collections.Generic.IEnumerable`1
        //     according to specified key selector and element selector functions.
        //
        // Parameters:
        //   keySelector:
        //     A function to extract a key from each element.
        //
        //   elementSelector:
        //     A transform function to produce a result element value from each element.
        //
        // Type parameters:
        //   TKey:
        //     The type of the key returned by keySelector.
        //
        //   TElement:
        //     The type of the value returned by elementSelector.
        //
        // Returns:
        //     A System.Collections.Generic.Dictionary`2 that contains values of type TElement
        //     selected from the input sequence.
        //
        public IDictionary<TKey, TElement> ToDictionary<TKey, TElement>(Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Expression<Func<T, bool>> where = null)
        {
            IQueryable<T> query = DbSet;
            if (where != null)
                query = query.Where(where);
            return query.ToDictionary(keySelector, elementSelector);
        }


        /// <summary>
        /// return list displaytext and value to select html async
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public async Task<List<Options>> GetOptions(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null, int count = 0, Expression<Func<T, string>> orderby = null, Expression<Func<T, int>> orderbyint = null)
        {
            IQueryable<T> query = DbSet;
            if (where != null)
                query = query.Where(where);
            if (count > 0 && orderby != null) query = query.OrderBy(orderby).Take(count);
            if (count > 0 && orderbyint != null) query = query.OrderBy(orderbyint).Take(count);
            return await query.Select(keySelector).ToListAsync();
        }
        /// <summary>
        /// return list displaytext and value to select html
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public List<Options> GetOptionsNoAsync(Expression<Func<T, Options>> keySelector, Expression<Func<T, bool>> where = null)
        {
            if (where != null)
                return DbSet.Where(where).Select(keySelector).ToList();
            return DbSet.Select(keySelector).ToList();
        }

        public virtual IEnumerable<TResult> GetSelect<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select)
        {
            IEnumerable<TResult> items = null;
            try
            {
                items = DbSet.Where(predicate).Select(select).ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        public virtual bool Any(Expression<Func<T, bool>> func)
        {
            bool result = false;
            try
            {
                result = DbSet.Any(func);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return result;
        }
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> func)
        {
            IQueryable<T> items = null;
            try
            {
                items = DbSet.Where(func);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        /// <summary>
        /// get list of table include relation-ship according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual IQueryable<T> WhereAndInclude<TProperty>(Expression<Func<T, bool>> func, Expression<Func<T, TProperty>> path)
        {
            IQueryable<T> items = null;
            try
            {
                items = DbSet.Where(func).Include(path);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }

        public T CopyEntity(T entity, bool copyKeys = false)
        {
            T clone = DbSet.Create();
            PropertyInfo[] pis = entity.GetType().GetProperties();

            foreach (PropertyInfo pi in pis)
            {
                EdmScalarPropertyAttribute[] attrs = (EdmScalarPropertyAttribute[])pi.GetCustomAttributes(typeof(EdmScalarPropertyAttribute), false);

                foreach (EdmScalarPropertyAttribute attr in attrs)
                {
                    if (!copyKeys && attr.EntityKeyProperty)
                        continue;

                    pi.SetValue(clone, pi.GetValue(entity, null), null);
                }
            }

            return clone;
        }

        #endregion

        #region Public Async Methods
        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public async virtual Task<int> SaveAsync(WindowsPrincipal NTUser, bool isSystem = false, IDictionary<string, string> Fields = null)
        {
            int count = 0;
            try
            {
                //var EditedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                //var AddedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                //var DeletedEntries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

                count += await SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Errors.Write(ex);
                count = -1;
            }
            return count;
        }

        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            int count = 0;
            try
            {
                count = await _entities.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Errors.Write(ex);
                count = _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                count = -1;
                Errors.Write(ex);
            }
            return count;
        }
        /// <summary>
        /// Save Db changes asynchronous
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            int count = 0;
            try
            {
                count =  _entities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Errors.Write(ex);
                count = _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                count = -1;
                Errors.Write(ex);
            }
            return count;
        }
        /// <summary>
        /// Find one instance By key/s of the table Async
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(params object[] KeyValues)
        {
            T item = null;
            try
            {
                item = await DbSet.FindAsync(KeyValues);
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return item;
        }
        /// <summary>
        /// Find by conditional expression (predicate) on table 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        /// <summary>
        /// get all list of table
        /// </summary>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }

        public async virtual Task<IEnumerable<TResult>> GetSelectAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> select)
        {
            IEnumerable<TResult> items = null;
            try
            {
                items = await DbSet.Where(predicate).Select(select).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }

        /// <summary>
        /// get list of table according by the func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> func)
        {
            IEnumerable<T> items = null;
            try
            {
                items = await DbSet.Where(func).ToListAsync();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);
            }
            return items;
        }
        #endregion
    }

}
