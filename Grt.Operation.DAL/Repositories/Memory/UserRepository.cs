using CMP.Operation.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CMP.Operation.DAL.Functions.Extensions;
using CMP.Operation.DAL.Functions;
using CMP.Operation.DAL.TableViews;


namespace CMP.Operation.DAL.Repositories.Memory
{
    public class UserRepository : GenericRepository<CMPContext, User>, IUserRepository
    {
        private IQueryable<UserView> SelectUserView(Expression<Func<UserView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                            //let tblCount = (from t in Context.Set<TableUserName>() where tbl.ID == t.UserID select t).Count()
                        select new UserView()
                        {
                            ID = tbl.ID,
                            UserName = tbl.UserName,
                            Email = tbl.Email,
                            RoleId = tbl.Role.ID,  
                            RoleName = tbl.Role.RoleName,                
                            IsActive = tbl.IsActive,
                            IsAllowDelete = true
                            //IsAllowDelete = tblCount==0
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.UserName.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                //Sorting = Sorting.Replace("JobID", "JobUserName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<UserView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<UserView> ListUser = null;
            CountRecords = 0;
            try
            {
                var query = SelectUserView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListUser = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListUser;

        }

        public UserView GetField(int UserID)
        {
            return SelectUserView(c => c.ID == UserID).FirstOrDefault();
        }

    }
}
