using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    public interface IPersonService
    {
        Task<List<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto?> GetPersonByIdAsync(int id);
        Task<PersonDto> AddNewPersonAsync(PersonDto person);
        Task<bool> UpdatePersonAsync(int id, PersonDto person);
        Task<bool> DeletePersonAsync(int id);
    }
}