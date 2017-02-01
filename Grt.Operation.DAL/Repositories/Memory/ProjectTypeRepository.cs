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
    public class ProjectTypeRepository : GenericRepository<CMPContext, ProjectType>, IProjectTypeRepository
    {
        private IQueryable<ProjectTypeView> SelectProjectTypeView(Expression<Func<ProjectTypeView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        let pCount = (from p in Context.Set<Project>() where tbl.ID == p.ProjectTypeID select p).Count()
                        select new ProjectTypeView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete= pCount==0
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<ProjectTypeView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<ProjectTypeView> ListProjectType = null;
            CountRecords = 0;
            try
            {
                var query = SelectProjectTypeView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListProjectType = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListProjectType;

        }

        public ProjectTypeView GetField(int ProjectTypeID)
        {
            return SelectProjectTypeView(c => c.ID == ProjectTypeID).FirstOrDefault();
        }

    }
}
