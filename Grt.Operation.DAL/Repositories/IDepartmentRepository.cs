using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IEnumerable<DepartmentView> GetByFiltering(int CustomerID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        DepartmentView GetField(int DepartmentID);

    }
}