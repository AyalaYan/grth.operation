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
    public class CityRepository : GenericRepository<CMPContext, City>, ICityRepository
    {
        private IQueryable<CityView> SelectCityView(Expression<Func<CityView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                           .Include(c => c.State.Country)

                        select new CityView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            IsSystem = tbl.IsSystem,
                            IsAllowDelete = !tbl.IsSystem,
                            StateID = tbl.StateID,
                            StateName = tbl.State.Name,
                            CountryID=tbl.State.Country.ID,
                            CountryName=tbl.State.Country.Name  
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          tbl.StateName.ToString().Contains(Filtering) ||
                          tbl.CountryName.ToString().Contains(Filtering) ||
                         ((tbl.IsSystem == true ? "true" : "false").Contains(Filtering) ||
                         (tbl.IsActive == true ? "true" : "false").Contains(Filtering)));


            if (Expression != null)
                Query = Query.Where(Expression);

            if (Sorting != null)
            {
                //Sorting = Sorting.Replace("JobID", "JobName");
                Query = Query.GetOrderByQuery(Sorting);
            }
            return Query;
        }
        public IEnumerable<CityView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<CityView> ListCity = null;
            CountRecords = 0;
            try
            {
                var query = SelectCityView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListCity = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListCity;

        }

        public CityView GetField(int CityID)
        {
            return SelectCityView(c => c.ID == CityID).FirstOrDefault();
        }

    }
}
