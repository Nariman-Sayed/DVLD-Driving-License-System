using DVLD.Shared;

namespace DVLD.Api.Services.Interfaces
{
    // Public because PersonController is public (MVC discovery) and its constructor
    // parameter types must be at least as accessible as the constructor (CS0051).
    public interface IPersonService
    {
        Task<List<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto?> GetPersonByIdAsync(int id);
        Task<PersonDto> AddNewPersonAsync(PersonDto person);
        Task<bool> UpdatePersonAsync(int id, PersonDto person);
        Task<bool> DeletePersonAsync(int id);
    }
}