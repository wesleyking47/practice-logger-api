using Microsoft.EntityFrameworkCore;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Session> PracticeSessions { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
