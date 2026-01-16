using MediatR;

namespace PracticeLogger.Application.Sessions.Commands;

public record CreateSessionCommand(string Activity, int Minutes, DateOnly Date, string? Notes)
    : IRequest<int>;
