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
    public class RoleRepository : GenericRepository<CMPContext, Role>,IRoleRepository
    {
        private IQueryable<RoleView> SelectRoleView(Expression<Func<RoleView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                           

                        select new RoleView()
                        {
                            ID = tbl.ID,
                            RoleName = tbl.RoleName,
                            IsActive = tbl.IsActive
                        };

            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.RoleName.ToString().Contains(Filtering) ||
                         (tbl.IsActive == true ? "true" : "false").Contains(Filtering));

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                //Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<RoleView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<RoleView> ListRole = null;
            CountRecords = 0;
            try
            {
                var query = SelectRoleView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListRole = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListRole;

        }

        public RoleView GetField(int RoleID)
        {
            return SelectRoleView(r => r.ID == RoleID).FirstOrDefault();
        }

    }
}
