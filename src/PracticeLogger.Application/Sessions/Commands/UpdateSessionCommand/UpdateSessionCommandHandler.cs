using MediatR;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Application.Sessions.Commands;

public class UpdateSessionCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateSessionCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        // Check if exists
        var exists = await _context.PracticeSessions
            .AsNoTracking()
            .AnyAsync(s => s.Id == request.Id, cancellationToken);

        if (!exists)
        {
            // Similar to before, return if not found. 
            // Ideally should throw NotFoundException.
            return;
        }

        var updatedSession = new Session(
            request.Id,
            request.Date,
            request.Activity,
            request.Minutes,
            request.Notes
        );

        _context.PracticeSessions.Update(updatedSession);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
