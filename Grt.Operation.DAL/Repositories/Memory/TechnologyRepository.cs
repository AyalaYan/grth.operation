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
    public class TechnologyRepository : GenericRepository<CMPContext, Technology>, ITechnologyRepository
    {
        private IQueryable<TechnologyView> SelectTechnologyView(Expression<Func<TechnologyView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        let etCount = (from et in Context.Set<ExperienceTechnology>() where tbl.ID == et.TechnologyID select et).Count()
                        select new TechnologyView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = etCount == 0
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
            {
                //Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<TechnologyView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<TechnologyView> ListTechnology = null;
            CountRecords = 0;
            try
            {
                var query = SelectTechnologyView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListTechnology = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListTechnology;

        }

        public TechnologyView GetField(int TechnologyID)
        {
            return SelectTechnologyView(c => c.ID == TechnologyID).FirstOrDefault();
        }

    }
}
