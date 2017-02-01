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
    public class ProjectTechnologyRepository : GenericRepository<CMPContext, ProjectTechnology>, IProjectTechnologyRepository
    {
        private IQueryable<ProjectTechnologyView> SelectProjectTechnologyView(Expression<Func<ProjectTechnologyView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = DbSet
                .Include(a=>a.Project)
                .Include(a=>a.Technology)
                           .Select(tbl => new ProjectTechnologyView()
                           {
                               ID = tbl.ID,
                               ProjectID = tbl.ProjectID,
                               ProjectName=tbl.Project.Name,
                               TechnologyID = tbl.TechnologyID,
                               TechnologyName=tbl.Technology.Name,
                               IsActive = tbl.IsActive,
                               IsAllowDelete=true
                           });


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          //tbl.Name.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                //Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<ProjectTechnologyView> GetByFiltering(int ProjectID,int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<ProjectTechnologyView> ListProjectTechnology = null;
            CountRecords = 0;
            try
            {
                var query = SelectProjectTechnologyView(p=>p.ProjectID==ProjectID, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListProjectTechnology = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListProjectTechnology;

        }

        public ProjectTechnologyView GetField(int ProjectTechnologyID)
        {
            return SelectProjectTechnologyView(c => c.ID == ProjectTechnologyID).FirstOrDefault();
        }

    }
}
