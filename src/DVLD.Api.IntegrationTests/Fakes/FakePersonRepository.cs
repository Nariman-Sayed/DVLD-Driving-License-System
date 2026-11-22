using DVLD.Api.Constants;
using DVLD.Api.Data.Interfaces;
using DVLD.Api.Exceptions;
using DVLD.Shared;

namespace DVLD.Api.IntegrationTests.Fakes
{
    public sealed class FakePersonRepository : IPersonRepository
    {
        /// <summary>
        /// File name of a seeded image that tests can place in the temporary storage folder.
        /// </summary>
        public const string SeededImageFileName = "00000000000000000000000000000001.png";

        /// <summary>
        /// Absolute legacy path used to verify that old rows are treated as missing.
        /// </summary>
        public static readonly string LegacyAbsolutePath =
            Path.Combine(Path.GetTempPath(), "legacy-person-photo.jpg");

        private readonly Dictionary<int, string> _imagePaths = new()
        {
            [2] = SeededImageFileName,
            [5] = LegacyAbsolutePath
        };

        private readonly List<PersonDto> _people = new()
        {
            CreatePerson(1, "T1", "Test", "User", "One", 1),
            CreatePerson(2, "T2", "Alpha", "Beta", "Gamma", 1),
            CreatePerson(3, "T3", "Bravo", "Charlie", "Delta", 2),
            CreatePerson(4, "T4", "Charlie", "Delta", "Echo", 2),
            CreatePerson(5, "T5", "Delta", "Echo", "Foxtrot", 3)
        };

        public Task<PagedResult<PersonDto>> GetPagedAsync(
            PersonQuery query,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<PersonDto> matching = _people;

            if (!string.IsNullOrWhiteSpace(query.FilterOn) &&
                !string.IsNullOrWhiteSpace(query.FilterQuery))
            {
                var value = query.FilterQuery;

                if (string.Equals(query.FilterOn, PersonFilters.NationalNo, StringComparison.OrdinalIgnoreCase))
                {
                    matching = matching.Where(p => p.NationalNo.Contains(value, StringComparison.OrdinalIgnoreCase));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.FirstName, StringComparison.OrdinalIgnoreCase))
                {
                    matching = matching.Where(p => p.FirstName.Contains(value, StringComparison.OrdinalIgnoreCase));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.LastName, StringComparison.OrdinalIgnoreCase))
                {
                    matching = matching.Where(p => p.LastName.Contains(value, StringComparison.OrdinalIgnoreCase));
                }
                else if (string.Equals(query.FilterOn, PersonFilters.CountryId, StringComparison.OrdinalIgnoreCase))
                {
                    if (!int.TryParse(value, out var countryId))
                    {
                        throw new ApplicationValidationException(ApiErrorMessages.InvalidCountryFilter);
                    }

                    matching = matching.Where(p => p.NationalityCountryID == countryId);
                }
                else
                {
                    throw new ApplicationValidationException(ApiErrorMessages.UnknownPersonFilter);
                }
            }

            var ordered = matching.OrderBy(p => p.PersonID).ToList();
            var skip = (int)Math.Min(((long)(query.PageNumber - 1)) * query.PageSize, int.MaxValue);

            var page = new PagedResult<PersonDto>
            {
                Items = ordered.Skip(skip).Take(query.PageSize).ToList(),
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = ordered.Count
            };

            return Task.FromResult(page);
        }

        public Task<PersonDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var person = _people.FirstOrDefault(p => p.PersonID == id);
            return Task.FromResult(person);
        }

        public Task<bool> NationalNoExistsAsync(
            string nationalNo,
            int? excludePersonId = null,
            CancellationToken cancellationToken = default)
        {
            var exists = _people.Any(p =>
                string.Equals(p.NationalNo, nationalNo, StringComparison.OrdinalIgnoreCase) &&
                (!excludePersonId.HasValue || p.PersonID != excludePersonId.Value));

            return Task.FromResult(exists);
        }

        public Task<int> AddAsync(PersonDto person, CancellationToken cancellationToken = default)
        {
            person.PersonID = _people.Count > 0 ? _people.Max(p => p.PersonID) + 1 : 1;
            _people.Add(person);
            return Task.FromResult(person.PersonID);
        }

        public Task<bool> UpdateAsync(int id, PersonDto person, CancellationToken cancellationToken = default)
        {
            var existing = _people.FirstOrDefault(p => p.PersonID == id);
            if (existing is null) return Task.FromResult(false);

            _people.Remove(existing);
            person.PersonID = id;
            _people.Add(person);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Person 1 is reserved to simulate the SQL 547 foreign key block in tests.
            if (id == 1)
            {
                throw new ConflictException(ApiErrorMessages.PersonDeleteBlocked);
            }

            var existing = _people.FirstOrDefault(p => p.PersonID == id);
            if (existing is null) return Task.FromResult(false);

            _people.Remove(existing);
            return Task.FromResult(true);
        }

        public Task<string?> GetImagePathAsync(int id, CancellationToken cancellationToken = default)
        {
            if (_imagePaths.TryGetValue(id, out var fileName))
            {
                return Task.FromResult<string?>(fileName);
            }

            var person = _people.FirstOrDefault(p => p.PersonID == id);
            return Task.FromResult(person?.ImagePath);
        }

        public Task<bool> SetImagePathAsync(
            int id,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            if (!_people.Any(p => p.PersonID == id))
            {
                return Task.FromResult(false);
            }

            _imagePaths[id] = fileName;
            return Task.FromResult(true);
        }

        private static PersonDto CreatePerson(
            int id,
            string nationalNo,
            string firstName,
            string secondName,
            string lastName,
            int countryId)
        {
            return new PersonDto
            {
                PersonID = id,
                FirstName = firstName,
                SecondName = secondName,
                ThirdName = string.Empty,
                LastName = lastName,
                NationalNo = nationalNo,
                DateOfBirth = new DateTime(1990, 1, 1),
                Gendor = 0,
                Address = "Test Address",
                Phone = "0000000",
                Email = "test@test.com",
                NationalityCountryID = countryId,
                ImagePath = string.Empty
            };
        }
    }
}
