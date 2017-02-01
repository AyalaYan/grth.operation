using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.Repositories;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        IEnumerable<JobView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        JobView GetField(int JobID);
    }
}