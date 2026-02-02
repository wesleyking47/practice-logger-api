using MediatR;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Application.Common.Interfaces;

namespace PracticeLogger.Application.Sessions.Commands;

public class DeleteSessionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<DeleteSessionCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        await _context
            .PracticeSessions
            .Where(session => session.Id == request.Id && session.UserId == _currentUserService.UserId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
