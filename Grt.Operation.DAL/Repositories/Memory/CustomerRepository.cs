using CMP.Operation.DAL;
using CMP.Operation.DAL.Functions;
using CMP.Operation.DAL.Functions.Extensions;
using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public class CustomerRepository : GenericRepository<CMPContext, Customer>, ICustomerRepository
    {
        private IQueryable<CustomerView> SelectCustomerView(Expression<Func<CustomerView, bool>> Expression, string Sorting = null, string Filtering = null)
        {
            var Query = from tbl in DbSet
                        let pCount = (from p in Context.Set<Project>() where tbl.ID == p.CustomerID select p).Count()
                        let dCount = (from d in Context.Set<Department>() where tbl.ID == d.CustomerID select d).Count()
                        let fpCount = (from fp in Context.Set<Project>() where tbl.ID == fp.CustomerID select fp).Count()
                        let cCount = (from c in Context.Set<Company>() where tbl.ID == c.CustomerID select c).Count()
                        select new CustomerView()
                        {
                            ID = tbl.ID,
                            Name = tbl.Name,
                            IsActive = tbl.IsActive,
                            IsAllowDelete = pCount == 0 && dCount == 0 && fpCount == 0 && cCount == 0
                        };

            if (Expression != null)
                Query = Query.Where(Expression);

            else if ((Filtering ?? "") != "")
                Query = Query.Where(tbl =>
                          tbl.ID.ToString().Contains(Filtering) ||
                          tbl.Name.ToString().Contains(Filtering) ||
                          (tbl.IsActive == true ? "true" : "false").Contains(Filtering)
                          );

            if (Sorting != null)
                Query = Query.GetOrderByQuery(Sorting);
            return Query;
        }
        public IEnumerable<CustomerView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords)
        {
            IEnumerable<CustomerView> ListCustomer = null;
            CountRecords = 0;
            try
            {
                var query = SelectCustomerView(null, Sorting, Filtering);
                query = query.Get(StartIndex, Count, out CountRecords);
                ListCustomer = query.ToList();
            }
            catch (Exception ex)
            {
                Errors.Write(ex);

            }
            return ListCustomer;

        }
        public CustomerView GetField(int CustomerID)
        {
            return SelectCustomerView(c => c.ID == CustomerID).FirstOrDefault();
        }
    }
}
