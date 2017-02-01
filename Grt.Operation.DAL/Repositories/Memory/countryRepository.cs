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
    public class CountryRepository : GenericRepository<CMPContext, Country>, ICountryRepository
    {
        private IQueryable<CountryView> SelectCountryView(Expression<Func<CountryView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        let sCount = (from s in Context.Set<State>() where tbl.ID == s.CountryID select s).Count()
                        select new CountryView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            ShortName = tbl.ShortName,
                            IsSystem = tbl.IsSystem,
                            IsAllowDelete = sCount == 0 && !tbl.IsSystem
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          tbl.ShortName.ToString().Contains(Filtering) ||
                         (tbl.IsSystem == true ? "true" : "false").Contains(Filtering) ||
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
        public IEnumerable<CountryView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<CountryView> ListCountry = null;
            CountRecords = 0;
            try
            {
                var query = SelectCountryView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListCountry = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListCountry;

        }

        public CountryView GetField(int CountryID)
        {
            return SelectCountryView(c => c.ID == CountryID).FirstOrDefault();
        }

    }
}
