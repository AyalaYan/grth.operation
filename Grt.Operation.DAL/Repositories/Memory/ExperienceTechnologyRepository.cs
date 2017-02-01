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
    public class ExperienceTechnologyRepository : GenericRepository<CMPContext, ExperienceTechnology>, IExperienceTechnologyRepository
    {
        private IQueryable<ExperienceTechnologyView> SelectExperienceTechnologyView(Expression<Func<ExperienceTechnologyView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                            .Include(c=>c.Experience)
                            .Include(c => c.Technology)
                            //let tblCount = (from t in Context.Set<TableName>() where tbl.ID == t.ExperienceTechnologyID select t).Count()
                        select new ExperienceTechnologyView()
                        {
                            ID = tbl.ID,
                            ExperienceID = tbl.ExperienceID,
                            TechnologyID = tbl.TechnologyID,
                            TechnologyName = tbl.Technology.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = true
                            //IsAllowDelete = tblCount==0
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.TechnologyName.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                //Sorting = Sorting.Replace("JobID", "JobName");
                if (!Sorting.StartsWith("ID")) Sorting = Sorting.Replace("ID", "Name");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<ExperienceTechnologyView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<ExperienceTechnologyView> ListExperienceTechnology = null;
            CountRecords = 0;
            try
            {
                var query = SelectExperienceTechnologyView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListExperienceTechnology = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListExperienceTechnology;

        }

        public ExperienceTechnologyView GetField(int ExperienceTechnologyID)
        {
            return SelectExperienceTechnologyView(c => c.ID == ExperienceTechnologyID).FirstOrDefault();
        }

    }
}
