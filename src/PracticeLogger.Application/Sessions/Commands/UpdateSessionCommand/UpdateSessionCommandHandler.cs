using MediatR;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Application.Sessions.Commands;

public class UpdateSessionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<UpdateSessionCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        // Check if exists and owned by user
        var existingSession = await _context.PracticeSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == _currentUserService.UserId, cancellationToken);

        if (existingSession == null)
        {
            return;
        }

        var updatedSession = new Session(
            request.Id,
            request.Date,
            request.Activity,
            request.Minutes,
            request.Notes
        )
        {
            UserId = existingSession.UserId // Preserve/Ensure UserId
        };

        _context.PracticeSessions.Update(updatedSession);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
