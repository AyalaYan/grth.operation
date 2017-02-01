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
using System.Data.Entity.SqlServer;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public class ProjectRepository : GenericRepository<CMPContext, Project>, IProjectRepository
    {
        private IQueryable<ProjectView> SelectProjectView(Expression<Func<ProjectView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        .Include(c => c.Customer)
                        .Include(c => c.Department)
                        .Include(c => c.ProjectType)
                        .Include(c => c.FocalPoint)
                        .Include(c => c.CompanyFocalPoint)
                        let ptCount = (from pt in Context.Set<ProjectTechnology>() where tbl.ID == pt.ProjectID select pt).Count()
                        select new ProjectView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            CustomerID = tbl.CustomerID,
                            CustomerName = tbl.Customer.Name,
                            DepartmentID = tbl.DepartmentID,
                            DepartmentName = tbl.Department.Name,
                            ProjectTypeID = tbl.ProjectTypeID,
                            ProjectTypeName = tbl.ProjectType.Name,
                            FocalPointID = tbl.FocalPointID,
                            FocalPointName = tbl.FocalPoint.FirstName + " " + tbl.FocalPoint.LastName,
                            CompanyFocalPointID = tbl.CompanyFocalPointID,
                            CompanyFocalPointName = tbl.CompanyFocalPoint.FirstName + " " + tbl.CompanyFocalPoint.LastName,
                            EndDate = tbl.EndDate,
                            IsActive = tbl.IsActive,
                            Risk = tbl.Risk,
                            IsAllowDelete = ptCount==0
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          tbl.CustomerName.ToString().Contains(Filtering) ||
                          tbl.DepartmentName.ToString().Contains(Filtering) ||
                          tbl.ProjectTypeName.ToString().Contains(Filtering) ||
                          tbl.FocalPointName.ToString().Contains(Filtering) ||
                         ((SqlFunctions.DateName("mm", tbl.EndDate).Substring(0, 3).ToString() + " " + SqlFunctions.DateName("dd", tbl.EndDate) + " ," + SqlFunctions.DateName("yyyy", tbl.EndDate)).ToString()).Contains(Filtering) ||
                         (tbl.IsActive == true ? "true" : "false").Contains(Filtering) ||
                          tbl.Risk.ToString().Contains(Filtering)
                      );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                if (!Sorting.StartsWith("ID")) Sorting = Sorting.Replace("ID", "Name");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<ProjectView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<ProjectView> ListProject = null;
            CountRecords = 0;
            try
            {
                var query = SelectProjectView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListProject = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListProject;

        }

        public ProjectView GetField(int ProjectID)
        {
            return SelectProjectView(c => c.ID == ProjectID).FirstOrDefault();
        }

    }
}
