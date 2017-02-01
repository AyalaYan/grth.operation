using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface IFocalPointRepository : IGenericRepository<FocalPoint>
    {
        IEnumerable<FocalPointView> GetByFiltering(int CustomerID, int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        FocalPointView GetField(int FocalPointID);

    }
}