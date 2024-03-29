﻿using Microsoft.EntityFrameworkCore;
using ThunderHerd.Core.Entities;
using ThunderHerd.Core.Enums;
using ThunderHerd.DataAccess.Interfaces;

namespace ThunderHerd.DataAccess.Repositories
{
    public class TestResultRepository : GeneralRepository<TestResult>, ITestResultRepository
    {
        public TestResultRepository(ThunderHerdContext context) : base(context)
        { }

        public IAsyncEnumerable<TestResult> GetTestResultsByTestId(Guid testId)
        {
            return _context.TestResult
                .Where(p => p.TestId == testId)
                .AsAsyncEnumerable();
        }
    }
}
