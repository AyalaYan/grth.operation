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
    public class StateRepository : GenericRepository<CMPContext, State>, IStateRepository
    {
        private IQueryable<StateView> SelectStateView(Expression<Func<StateView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                            .Include(c => c.Country)
                        let sCount = (from s in Context.Set<City>() where tbl.ID == s.StateID select s).Count()
                        select new StateView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            CountryID = tbl.Country.ID,
                            CountryName = tbl.Country.Name,
                            IsSystem = tbl.IsSystem,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = sCount == 0 && !tbl.IsSystem
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                            tbl.CountryName.ToString().Contains(Filtering) ||
                         (tbl.IsSystem == true ? "true" : "false").Contains(Filtering) ||
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
        public IEnumerable<StateView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<StateView> ListState = null;
            CountRecords = 0;
            try
            {
                var query = SelectStateView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListState = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListState;

        }

        public StateView GetField(int StateID)
        {
            return SelectStateView(c => c.ID == StateID).FirstOrDefault();
        }

    }
}
