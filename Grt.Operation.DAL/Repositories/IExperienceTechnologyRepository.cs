using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface IExperienceTechnologyRepository : IGenericRepository<ExperienceTechnology>
    {
        IEnumerable<ExperienceTechnologyView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        ExperienceTechnologyView GetField(int ExperienceTechnologyID);

    }
}