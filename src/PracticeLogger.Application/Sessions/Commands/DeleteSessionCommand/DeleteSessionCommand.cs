using MediatR;

namespace PracticeLogger.Application.Sessions.Commands;

public record DeleteSessionCommand(int Id) : IRequest;
