using DVLD.Api.Data.Interfaces;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public class FakeTestTypeRepository : ITestTypeRepository
    {
        private readonly List<TestTypeDto> _types = new()
        {
            new TestTypeDto
            {
                TestTypeID = 1,
                TestTypeTitle = "Vision Test",
                TestTypeDescription = "Basic eye sight test.",
                TestTypeFees = 10
            },
            new TestTypeDto
            {
                TestTypeID = 2,
                TestTypeTitle = "Written Test",
                TestTypeDescription = "Written theory exam.",
                TestTypeFees = 10
            },
            new TestTypeDto
            {
                TestTypeID = 3,
                TestTypeTitle = "Street Test",
                TestTypeDescription = "Practical street driving exam.",
                TestTypeFees = 20
            }
        };

        public Task<List<TestTypeDto>> GetAllAsync()
        {
            return Task.FromResult(_types.OrderBy(t => t.TestTypeID).ToList());
        }

        public Task<TestTypeDto?> GetByIdAsync(int id)
        {
            var type = _types.FirstOrDefault(t => t.TestTypeID == id);
            return Task.FromResult(type);
        }

        public Task<int> AddAsync(TestTypeDto type)
        {
            type.TestTypeID = _types.Count > 0 ? _types.Max(t => t.TestTypeID) + 1 : 1;
            _types.Add(type);
            return Task.FromResult(type.TestTypeID);
        }

        public Task<bool> UpdateAsync(int id, TestTypeDto type)
        {
            var existing = _types.FirstOrDefault(t => t.TestTypeID == id);
            if (existing is null) return Task.FromResult(false);

            _types.Remove(existing);
            type.TestTypeID = id;
            _types.Add(type);
            return Task.FromResult(true);
        }
    }
}