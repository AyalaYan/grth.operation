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
    public class EmployeeRepository : GenericRepository<CMPContext, Employee>, IEmployeeRepository
    {
        private IQueryable<EmployeeView> SelectEmployeeView(Expression<Func<EmployeeView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet.Include(c => c.Job)
                        let pCount = (from p in Context.Set<Project>() where tbl.ID == p.CompanyFocalPointID select p).Count()
                        let exCount = (from ex in Context.Set<Experience>() where tbl.ID == ex.EmployeeID select ex).Count()
                        select new EmployeeView()
                        {
                            ID = tbl.ID,
                            FirstName = tbl.FirstName,
                            LastName = tbl.LastName,
                            FullName = tbl.FirstName + " " + tbl.LastName,
                            FirstFamilyName = tbl.FirstFamilyName,
                            StartDate=tbl.StartDate,
                            Address = tbl.Address,
                            PhoneNumber = tbl.PhoneNumber,
                            Email = tbl.Email,
                            JobName = tbl.Job.Name,
                            JobID = tbl.Job.ID,
                            CountryName = tbl.Country.Name,
                            CountryID = tbl.Country.ID,
                            StateName = tbl.State.Name,
                            StateID = tbl.State.ID,
                            CityName = tbl.City.Name,
                            CityID = tbl.City.ID,
                            IsActive = tbl.IsActive,
                            Remarks = tbl.Remarks,
                            IsAllowDelete = pCount == 0 && exCount == 0,

                        };

            if (Expression != null)
                Query = Query.Where(Expression);

            else if ((Filtering ?? "") != "")
                Query = Query.Where(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.FullName.ToString().Contains(Filtering) ||
                          tbl.JobName.ToString().Contains(Filtering) ||
                          tbl.CountryName.ToString().Contains(Filtering) ||
                          tbl.StateName.ToString().Contains(Filtering) ||
                           tbl.CityName.ToString().Contains(Filtering) ||
                         (tbl.IsActive == true ? "true" : "false").Contains(Filtering) ||
                          tbl.Remarks.ToString().Contains(Filtering)
                          );

            if (Sorting != null)
            {
                Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<EmployeeView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<EmployeeView> ListEmployee = null;
            CountRecords = 0;
            try
            {
                var query = SelectEmployeeView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListEmployee = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListEmployee;

        }

        public EmployeeView GetField(int EmployeeID)
        {
            return SelectEmployeeView(c => c.ID == EmployeeID).FirstOrDefault();
        }
    }
}
