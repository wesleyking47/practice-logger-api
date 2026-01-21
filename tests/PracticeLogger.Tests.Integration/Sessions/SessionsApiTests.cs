using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;
using PracticeLogger.Tests.Integration.Infrastructure;

namespace PracticeLogger.Tests.Integration.Sessions;

public class SessionsApiTests : IClassFixture<TestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _client;

    public SessionsApiTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateSession_ReturnsCreatedAndPersists()
    {
        var command = SessionFaker.CreateCreateCommand(1);

        var response = await _client.PostAsJsonAsync(
            "/Sessions",
            command,
            TestContext.Current.CancellationToken
        );

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdId = await response.Content.ReadFromJsonAsync<int>(
            JsonOptions,
            TestContext.Current.CancellationToken
        );

        var sessions = await GetSessionsAsync();
        Assert.Contains(sessions.Sessions, s => s.Id == createdId);
    }

    [Fact]
    public async Task UpdateSession_ReturnsNoContentAndUpdates()
    {
        var command = SessionFaker.CreateCreateCommand(2);
        var id = await CreateSessionAsync(command);
        var update = new UpdateSessionCommand(id, "Technique", 45, new DateOnly(2024, 2, 1), "Updated");

        var response = await _client.PutAsJsonAsync(
            $"/Sessions/{id}",
            update,
            TestContext.Current.CancellationToken
        );

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var sessions = await GetSessionsAsync();
        var updated = Assert.Single(sessions.Sessions, s => s.Id == id);
        Assert.Equal(update.Activity, updated.Activity);
        Assert.Equal(update.Minutes, updated.Minutes);
        Assert.Equal(update.Date, updated.Date);
        Assert.Equal(update.Notes, updated.Notes);
    }

    [Fact]
    public async Task UpdateSession_WhenIdMismatch_ReturnsBadRequest()
    {
        var update = new UpdateSessionCommand(10, "Technique", 45, new DateOnly(2024, 2, 1), "Updated");

        var response = await _client.PutAsJsonAsync(
            "/Sessions/11",
            update,
            TestContext.Current.CancellationToken
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteSession_ReturnsOkAndRemoves()
    {
        var command = SessionFaker.CreateCreateCommand(3);
        var id = await CreateSessionAsync(command);

        var response = await _client.DeleteAsync(
            $"/Sessions/{id}",
            TestContext.Current.CancellationToken
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var sessions = await GetSessionsAsync();
        Assert.DoesNotContain(sessions.Sessions, s => s.Id == id);
    }

    [Fact]
    public async Task GetSessions_FiltersByQuery()
    {
        var first = new CreateSessionCommand("Scales", 10, new DateOnly(2024, 1, 10), null);
        var second = new CreateSessionCommand("Chords", 20, new DateOnly(2024, 1, 15), null);
        var third = new CreateSessionCommand("Scales", 30, new DateOnly(2024, 1, 20), null);

        await CreateSessionAsync(first);
        await CreateSessionAsync(second);
        await CreateSessionAsync(third);

        var response = await _client.GetFromJsonAsync<GetSessionsResponse>(
            "/Sessions?startDate=2024-01-12&endDate=2024-01-20&activity=Scales",
            JsonOptions,
            TestContext.Current.CancellationToken
        );

        Assert.NotNull(response);
        var session = Assert.Single(response.Sessions);
        Assert.Equal("Scales", session.Activity);
        Assert.Equal(new DateOnly(2024, 1, 20), session.Date);
    }

    private async Task<int> CreateSessionAsync(CreateSessionCommand command)
    {
        var response = await _client.PostAsJsonAsync(
            "/Sessions",
            command,
            TestContext.Current.CancellationToken
        );
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<int>(
            JsonOptions,
            TestContext.Current.CancellationToken
        ))!;
    }

    private async Task<GetSessionsResponse> GetSessionsAsync()
    {
        var response = await _client.GetFromJsonAsync<GetSessionsResponse>(
            "/Sessions",
            JsonOptions,
            TestContext.Current.CancellationToken
        );
        return response ?? new GetSessionsResponse([]);
    }
}
