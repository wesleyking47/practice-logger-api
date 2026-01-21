using AutoFixture;
using AutoFixture.Xunit3;

namespace PracticeLogger.Tests.Unit.Infrastructure;

public sealed class AutoFixtureDataAttribute : AutoDataAttribute
{
    public AutoFixtureDataAttribute()
        : base(CreateFixture) { }

    private static IFixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Register(() => DateOnly.FromDateTime(DateTime.UtcNow.Date));

        return fixture;
    }
}
