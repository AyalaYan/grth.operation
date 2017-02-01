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
    public class FocalPointRepository : GenericRepository<CMPContext, FocalPoint>, IFocalPointRepository
    {
        private IQueryable<FocalPointView> SelectFocalPointView(Expression<Func<FocalPointView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        let pCount = (from p in Context.Set<Project>() where tbl.ID == p.CustomerID select p).Count()
                        select new FocalPointView()
                        {
                            ID = tbl.ID,
                            CustomerID = tbl.CustomerID,
                            FirstName = tbl.FirstName,
                            LastName = tbl.LastName,
                            PhoneNumber = tbl.PhoneNumber,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = pCount == 0,
                            CountryName = tbl.Country.Name,
                            CountryID = tbl.Country.ID,
                            StateName = tbl.State.Name,
                            StateID = tbl.State.ID,
                            CityName = tbl.City.Name,
                            CityID = tbl.City.ID,
                            Email = tbl.Email,
                            Remarks = tbl.Remarks
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.FirstName.ToString().Contains(Filtering) ||
                          tbl.LastName.ToString().Contains(Filtering) ||
                          tbl.PhoneNumber.ToString().Contains(Filtering) ||
                          tbl.CountryName.ToString().Contains(Filtering) ||
                          tbl.StateName.ToString().Contains(Filtering) ||
                          tbl.CityName.ToString().Contains(Filtering) ||
                          tbl.Email.ToString().Contains(Filtering) ||
                          tbl.Remarks.ToString().Contains(Filtering) ||
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
        public IEnumerable<FocalPointView> GetByFiltering(int CustomerID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<FocalPointView> ListFocalPoint = null;
            CountRecords = 0;
            try
            {
                var query = SelectFocalPointView(c => c.CustomerID == CustomerID, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListFocalPoint = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListFocalPoint;

        }

        public FocalPointView GetField(int FocalPointID)
        {
            return SelectFocalPointView(c => c.ID == FocalPointID).FirstOrDefault();
        }

    }
}
