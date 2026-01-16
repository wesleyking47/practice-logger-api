using MediatR;
using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Domain.Models;

namespace PracticeLogger.Application.Sessions.Commands;

public class CreateSessionCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateSessionCommand, int>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<int> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = new Session(
            0,
            request.Date,
            request.Activity,
            request.Minutes,
            request.Notes
        );
        _context.PracticeSessions.Add(session);

        await _context.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
