using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<List<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> AddAsync(PersonDto person, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, PersonDto person, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
