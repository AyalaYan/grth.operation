using CMP.Operation.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;
using CMP.Operation.DAL.Functions.Extensions;
using CMP.Operation.DAL.Functions;
using CMP.Operation.DAL.TableViews;
using System.Data.Entity.SqlServer;
using System.Security.Principal;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public class ExperienceRepository : GenericRepository<CMPContext, Experience>, IExperienceRepository
    {

        public async Task Update(Experience exp,WindowsPrincipal User)
        {
            var existingParent = DbSet
                .Where(p => p.ID == exp.ID)
                .Include(p => p.ExperienceTechnologys)
                .SingleOrDefault();

            var ExperienceTechnologySet = Context.Set<ExperienceTechnology>();
            if (existingParent != null)
            {
                // Update parent
                Context.Entry(existingParent).CurrentValues.SetValues(exp);

                // Delete ExperienceTechnologys
                foreach (var existingChild in existingParent.ExperienceTechnologys.ToList())
                {
                    if (!exp.ExperienceTechnologys.Any(c => c.ID == existingChild.ID))
                        ExperienceTechnologySet.Remove(existingChild);
                }

                // Update and Insert ExperienceTechnologys
                foreach (var childexp in exp.ExperienceTechnologys)
                {
                    var existingChild = existingParent.ExperienceTechnologys
                        .Where(c => c.ID == childexp.ID)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        Context.Entry(existingChild).CurrentValues.SetValues(childexp);
                    else
                    {
                        
                        existingParent.ExperienceTechnologys.Add(childexp);
                    }
                }

                await SaveAsync(User);
            }
        }

        private IQueryable<ExperienceView> SelectEmployeeCompanyView(Expression<Func<ExperienceView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                            .Include(e => e.Employee)
                            .Include(t => t.Company)
                        let et = (from et in Context.Set<ExperienceTechnology>() where tbl.ID == et.ExperienceID select et.TechnologyID)
                        select new ExperienceView()
                        {
                            ID = tbl.ID,
                            EmployeeID = tbl.EmployeeID,
                            EmployeeName = tbl.Employee.FirstName + " " + tbl.Employee.LastName,
                            CompanyID = tbl.CompanyID,
                            CompanyName = tbl.Company.Name,
                            FromDate = tbl.FromDate,
                            ToDate = tbl.ToDate,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = et.Count() == 0,
                            Technologies = et.ToList(),
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.CompanyName.ToString().Contains(Filtering) ||
                         ((SqlFunctions.DateName("mm", tbl.FromDate).Substring(0, 3).ToString() + " ," + SqlFunctions.DateName("yyyy", tbl.FromDate)).ToString()).Contains(Filtering) ||
                         ((SqlFunctions.DateName("mm", tbl.ToDate).Substring(0, 3).ToString() + " ," + SqlFunctions.DateName("yyyy", tbl.ToDate)).ToString()).Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                // Sorting = Sorting.Replace("CompanyID", "CompanyName");
                if (!Sorting.StartsWith("ID")) Sorting = Sorting.Replace("ID", "Name");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<ExperienceView> GetByFiltering(int EmployeeID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<ExperienceView> ListEmployeeCompany = null;
            CountRecords = 0;
            try
            {
                var query = SelectEmployeeCompanyView(c=>c.EmployeeID== EmployeeID, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListEmployeeCompany = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListEmployeeCompany;

        }

        public ExperienceView GetField(int EmployeeCompanyID)
        {
            return SelectEmployeeCompanyView(c => c.ID == EmployeeCompanyID).FirstOrDefault();
        }

    }
}
