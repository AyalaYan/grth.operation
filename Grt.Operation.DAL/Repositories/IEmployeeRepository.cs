using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<EmployeeView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        EmployeeView GetField(int EmployeeID);
    }
}