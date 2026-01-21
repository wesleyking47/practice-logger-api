using MediatR;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Application.Common.Interfaces;

namespace PracticeLogger.Application.Sessions.Commands;

public class DeleteSessionCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteSessionCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        await _context
            .PracticeSessions.Where(session => session.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
