using DVLD.Api.Constants;
using DVLD.Api.Data.Entities;
using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
using DVLD.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Repositories
{
    internal sealed class PersonRepository(IApplicationDbContext context) : IPersonRepository
    {
        public async Task<PagedResult<PersonDto>> GetPagedAsync(
            PersonQuery query,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Person> peopleQuery = context.People.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.FilterOn) &&
                !string.IsNullOrWhiteSpace(query.FilterQuery))
            {
                var value = query.FilterQuery;

                if (string.Equals(query.FilterOn, PersonFilters.NationalNo, StringComparison.OrdinalIgnoreCase))
                {
                    peopleQuery = peopleQuery.Where(p => p.NationalNo.Contains(value));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.FirstName, StringComparison.OrdinalIgnoreCase))
                {
                    peopleQuery = peopleQuery.Where(p => p.FirstName.Contains(value));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.LastName, StringComparison.OrdinalIgnoreCase))
                {
                    peopleQuery = peopleQuery.Where(p => p.LastName.Contains(value));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.CountryId, StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(value, out var countryId))
                    {
                        throw new ApplicationValidationException(ApiErrorMessages.InvalidCountryFilter);
                    }

                    peopleQuery = peopleQuery.Where(p => p.NationalityCountryID == countryId);
                }
                else
                {
                    throw new ApplicationValidationException(ApiErrorMessages.UnknownPersonFilter);
                }
            }

            var totalCount = await peopleQuery.CountAsync(cancellationToken);
            var skip = (int)Math.Min(((long)(query.PageNumber - 1)) * query.PageSize, int.MaxValue);

            var items = await peopleQuery
                .OrderBy(p => p.PersonID)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(p => new PersonDto
                {
                    PersonID = p.PersonID,
                    FirstName = p.FirstName,
                    SecondName = p.SecondName,
                    ThirdName = p.ThirdName ?? string.Empty,
                    LastName = p.LastName,
                    NationalNo = p.NationalNo,
                    DateOfBirth = p.DateOfBirth,
                    Gendor = p.Gendor,
                    Address = p.Address,
                    Phone = p.Phone,
                    Email = p.Email ?? string.Empty,
                    NationalityCountryID = p.NationalityCountryID,
                    ImagePath = p.ImagePath ?? string.Empty
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<PersonDto>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var person = await context.People
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PersonID == id, cancellationToken);

            return person is null ? null : ToDto(person);
        }

        public async Task<bool> NationalNoExistsAsync(
            string nationalNo,
            int? excludePersonId = null,
            CancellationToken cancellationToken = default)
        {
            return await context.People
                .AsNoTracking()
                .AnyAsync(
                    p => p.NationalNo == nationalNo &&
                         (!excludePersonId.HasValue || p.PersonID != excludePersonId.Value),
                    cancellationToken);
        }

        public async Task<int> AddAsync(PersonDto person, CancellationToken cancellationToken = default)
        {
            var entity = ToEntity(person);
            context.People.Add(entity);
            await SaveChangesTranslatingConflictsAsync(cancellationToken, isDelete: false);
            return entity.PersonID;
        }

        public async Task<bool> UpdateAsync(int id, PersonDto person, CancellationToken cancellationToken = default)
        {
            var entity = await context.People
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

            await SaveChangesTranslatingConflictsAsync(cancellationToken, isDelete: false);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await context.People
                .FirstOrDefaultAsync(p => p.PersonID == id, cancellationToken);

            if (entity is null)
            {
                return false;
            }

            context.People.Remove(entity);
            await SaveChangesTranslatingConflictsAsync(cancellationToken, isDelete: true);
            return true;
        }

        private async Task SaveChangesTranslatingConflictsAsync(
            CancellationToken cancellationToken,
            bool isDelete)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException exception) when (exception.InnerException is SqlException sqlException)
            {
                var conflict = DatabaseConflictTranslator.Translate(sqlException.Number, isDelete);

                if (conflict is not null)
                {
                    throw conflict;
                }

                throw;
            }
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
