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
    public class CompanyRepository : GenericRepository<CMPContext, Company>, ICompanyRepository
    {
        private IQueryable<CompanyView> SelectCompanyView(Expression<Func<CompanyView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                            .Include(c=>c.Customer)
                            let tblCount = (from t in Context.Set<Experience>() where tbl.ID == t.CompanyID select t).Count()
                        select new CompanyView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            CustomerID=tbl.CustomerID,
                            CustomerName = tbl.Customer.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = tblCount==0
                        };


            if ((Filtering ?? "") != "")
                Expression = Expression.And(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          tbl.CustomerName.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
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
        public IEnumerable<CompanyView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<CompanyView> ListCompany = null;
            CountRecords = 0;
            try
            {
                var query = SelectCompanyView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListCompany = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListCompany;

        }

        public CompanyView GetField(int CompanyID)
        {
            return SelectCompanyView(c => c.ID == CompanyID).FirstOrDefault();
        }

    }
}
