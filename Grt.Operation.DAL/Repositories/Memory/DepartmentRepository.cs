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
    public class DepartmentRepository : GenericRepository<CMPContext, Department>, IDepartmentRepository
    {
        private IQueryable<DepartmentView> SelectDepartmentView(Expression<Func<DepartmentView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        join project in (Context.Set<Project>()
                                            .GroupBy(c => c.DepartmentID)
                                            .Select(g => new { DepartmentID = g.Key, ProjectsCount = g.Count() })
                                            ) on tbl.ID equals project.DepartmentID into GroupProject
                        from projectCount in GroupProject.DefaultIfEmpty()
                        select new DepartmentView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            CustomerID = tbl.CustomerID,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = (projectCount==null?0: projectCount.ProjectsCount)==0
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
                //Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<DepartmentView> GetByFiltering(int CustomerID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<DepartmentView> ListDepartment = null;
            CountRecords = 0;
            try
            {
                var query = SelectDepartmentView(c => c.CustomerID == CustomerID, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListDepartment = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListDepartment;

        }

        public DepartmentView GetField(int DepartmentID)
        {
            return SelectDepartmentView(c => c.ID == DepartmentID).FirstOrDefault();
        }

    }
}
