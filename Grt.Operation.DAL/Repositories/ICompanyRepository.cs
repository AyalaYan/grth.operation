﻿using CMP.Operation.DAL.Models;
using CMP.Operation.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMP.Operation.DAL.Repositories.Memory
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        IEnumerable<CompanyView> GetByFiltering(int StartIndex, int Count, string Sorting, string Filtering, out int CountRecords);
        CompanyView GetField(int CompanyID);

    }
}