using Bogus;
using PracticeLogger.Application.Sessions.Commands;

namespace PracticeLogger.Tests.Integration.Infrastructure;

public static class SessionFaker
{
    private static readonly DateTime DateReference = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static CreateSessionCommand CreateCreateCommand(int seed)
    {
        var faker = new Faker<CreateSessionCommand>()
            .UseSeed(seed)
            .UseDateTimeReference(DateReference)
            .CustomInstantiator(f => new CreateSessionCommand(
                f.Hacker.Verb(),
                f.Random.Int(5, 120),
                DateOnly.FromDateTime(f.Date.Soon(30)),
                f.Lorem.Sentence()
            ));

        return faker.Generate();
    }
}
