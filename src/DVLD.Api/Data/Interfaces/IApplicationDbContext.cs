using DVLD.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD.Api.Data.Interfaces
{
    internal interface IApplicationDbContext
    {
        DbSet<Person> People { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
