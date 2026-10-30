using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class TestTypeRepository(IApplicationDbContext context) : ITestTypeRepository
    {
        public async Task<List<TestTypeDto>> GetAllAsync()
        {
            return await context.TestTypes
                .AsNoTracking()
                .OrderBy(t => t.TestTypeID)
                .Select(t => new TestTypeDto
                {
                    TestTypeID = t.TestTypeID,
                    TestTypeTitle = t.TestTypeTitle,
                    TestTypeDescription = t.TestTypeDescription,
                    TestTypeFees = t.TestTypeFees
                })
                .ToListAsync();
        }

        public async Task<TestTypeDto?> GetByIdAsync(int id)
        {
            return await context.TestTypes
                .AsNoTracking()
                .Where(t => t.TestTypeID == id)
                .Select(t => new TestTypeDto
                {
                    TestTypeID = t.TestTypeID,
                    TestTypeTitle = t.TestTypeTitle,
                    TestTypeDescription = t.TestTypeDescription,
                    TestTypeFees = t.TestTypeFees
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddAsync(TestTypeDto type)
        {
            var entity = new TestType
            {
                TestTypeTitle = type.TestTypeTitle,
                TestTypeDescription = type.TestTypeDescription,
                TestTypeFees = type.TestTypeFees
            };

            context.TestTypes.Add(entity);
            await context.SaveChangesAsync();
            return entity.TestTypeID;
        }

        public async Task<bool> UpdateAsync(int id, TestTypeDto type)
        {
            var rowsAffected = await context.TestTypes
                .Where(t => t.TestTypeID == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.TestTypeTitle, type.TestTypeTitle)
                    .SetProperty(t => t.TestTypeDescription, type.TestTypeDescription)
                    .SetProperty(t => t.TestTypeFees, type.TestTypeFees));

            return rowsAffected > 0;
        }
    }
}
