using CMP.Operation.DAL;
using CMP.Operation.DAL.Functions;
using CMP.Operation.DAL.Functions.Extensions;
using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public class JobRepository : GenericRepository<CMPContext, Job>, IJobRepository
    {
        private IQueryable<JobView> SelectJobView(Expression<Func<JobView, bool>> Expression, string Sorting=null, string Filtering=null)
        {
            var Query = from tbl in DbSet
                        let eCount = (from e in Context.Set<Employee>() where tbl.ID == e.JobID select e).Count()
                        select new JobView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete= eCount==0
                        };

            if (Expression != null)
                Query = Query.Where(Expression);

            else if ((Filtering ?? "") != "")
                Query = Query.Where(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);
            return Query;
        }
        public IEnumerable<JobView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<JobView> ListJob = null;
            CountRecords = 0;
            try
            {
                var query = SelectJobView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListJob = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListJob;

        }
        public JobView GetField(int JobID)
        {
            return SelectJobView(c => c.ID == JobID).FirstOrDefault();
        }

    }
}
