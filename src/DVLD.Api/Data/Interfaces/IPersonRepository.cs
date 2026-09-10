using DVLD.Shared;

namespace DVLD.Api.Data.Interfaces
{
    public interface IPersonRepository
    {
        Task<List<PersonDto>> GetAllAsync();
        Task<PersonDto?> GetByIdAsync(int id);
        Task<int> AddAsync(PersonDto person);
        Task<bool> UpdateAsync(int id, PersonDto person);
        Task<bool> DeleteAsync(int id);

    }
}
