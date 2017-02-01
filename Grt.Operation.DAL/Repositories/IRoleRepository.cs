using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        IEnumerable<RoleView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        RoleView GetField(int RoleID);

    }
}