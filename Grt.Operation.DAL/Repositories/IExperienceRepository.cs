using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System.Collections.Generic;


namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface IExperienceRepository : IGenericRepository<Experience>
    {
        IEnumerable<ExperienceView> GetByFiltering(int EmployeeID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        ExperienceView GetField(int EmployeeTechnologyID);

    }
}