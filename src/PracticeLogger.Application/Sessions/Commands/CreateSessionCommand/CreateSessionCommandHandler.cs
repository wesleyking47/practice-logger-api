using MediatR;
using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Application.Sessions.Commands;

public class CreateSessionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<CreateSessionCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<int> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new Session(
            0,
            request.Date,
            request.Activity,
            request.Minutes,
            request.Notes
        )
        {
            UserId = _currentUserService.UserId!.Value
        };
        _context.PracticeSessions.Add(session);

        await _context.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
