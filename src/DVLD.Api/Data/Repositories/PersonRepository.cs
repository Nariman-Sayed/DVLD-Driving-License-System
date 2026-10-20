using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using DVLD.Shared;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IApplicationDbContext _context;

        public PersonRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PersonDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var people = await _context.People
                .AsNoTracking()
                .OrderBy(p => p.FirstName)
                .ToListAsync(cancellationToken);

            return people.Select(ToDto).ToList();
        }

        public async Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var person = await _context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PersonID == id, cancellationToken);

            return person is null ? null : ToDto(person);
        }

        public async Task<int> AddAsync(PersonDto person, CancellationToken cancellationToken = default)
        {
            var entity = ToEntity(person);
            _context.People.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.PersonID;
        }

        public async Task<bool> UpdateAsync(int id, PersonDto person, CancellationToken cancellationToken = default)
        {
            var entity = await _context.People
                .FirstOrDefaultAsync(p => p.PersonID == id, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            entity.NationalNo = person.NationalNo;
            entity.FirstName = person.FirstName;
            entity.SecondName = person.SecondName;
            entity.ThirdName = string.IsNullOrEmpty(person.ThirdName) ? null : person.ThirdName;
            entity.LastName = person.LastName;
            entity.DateOfBirth = person.DateOfBirth;
            entity.Gendor = Convert.ToByte(person.Gendor);
            entity.Address = person.Address;
            entity.Phone = person.Phone;
            entity.Email = string.IsNullOrEmpty(person.Email) ? null : person.Email;
            entity.NationalityCountryID = person.NationalityCountryID;
            entity.ImagePath = string.IsNullOrEmpty(person.ImagePath) ? null : person.ImagePath;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.People
                .FirstOrDefaultAsync(p => p.PersonID == id, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            _context.People.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static PersonDto ToDto(Person person)
        {
            return new PersonDto
            {
                PersonID = person.PersonID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName ?? string.Empty,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                DateOfBirth = person.DateOfBirth,
                Gendor = person.Gendor,
                Address = person.Address,
                Phone = person.Phone,
                Email = person.Email ?? string.Empty,
                NationalityCountryID = person.NationalityCountryID,
                ImagePath = person.ImagePath ?? string.Empty
            };
        }

        private static Person ToEntity(PersonDto person)
        {
            return new Person
            {
                NationalNo = person.NationalNo,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = string.IsNullOrEmpty(person.ThirdName) ? null : person.ThirdName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                Gendor = Convert.ToByte(person.Gendor),
                Address = person.Address,
                Phone = person.Phone,
                Email = string.IsNullOrEmpty(person.Email) ? null : person.Email,
                NationalityCountryID = person.NationalityCountryID,
                ImagePath = string.IsNullOrEmpty(person.ImagePath) ? null : person.ImagePath
            };
        }
    }
}
