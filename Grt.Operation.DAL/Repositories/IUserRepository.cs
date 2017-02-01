using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IEnumerable<UserView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        UserView GetField(int UserID);

    }
}