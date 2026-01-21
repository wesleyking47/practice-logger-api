using MediatR;

namespace PracticeLogger.Application.Sessions.Commands;

public record UpdateSessionCommand(int Id, string Activity, int Minutes, DateOnly Date, string? Notes)
    : IRequest;
