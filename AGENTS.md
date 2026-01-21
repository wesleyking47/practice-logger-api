# Repository Guidelines

## Project Structure & Module Organization
- `src/PracticeLogger.Api`: ASP.NET Core API host (controllers, startup, config).
- `src/PracticeLogger.Application`: application layer (commands/queries, handlers, interfaces).
- `src/PracticeLogger.Domain`: domain models (e.g., `Session`).
- `src/PracticeLogger.Infrastructure`: EF Core persistence, migrations, DI wiring.
- `tests/PracticeLogger.Tests.Unit`: unit tests.
- `tests/PracticeLogger.Tests.Integration`: integration tests.
- Solution entry point: `PracticeLogger.sln`.

## Build, Test, and Development Commands
- `dotnet build PracticeLogger.sln` — build all projects.
- `dotnet test PracticeLogger.sln` — run all tests.
- `dotnet test tests/PracticeLogger.Tests.Unit/PracticeLogger.Tests.Unit.csproj` — unit-only run.
- `dotnet test tests/PracticeLogger.Tests.Integration/PracticeLogger.Tests.Integration.csproj` — integration-only run.
- `dotnet run --project src/PracticeLogger.Api/PracticeLogger.Api.csproj` — run the API locally.

## Coding Style & Naming Conventions
- C# standard conventions: PascalCase for public types/members, camelCase for locals/parameters.
- Nullable reference types are enabled (`<Nullable>enable</Nullable>`); avoid null-unsafe APIs.
- Implicit usings are enabled; keep usings minimal and scoped to file needs.
- Prefer clear handler names aligned with folder structure: `CreateSessionCommandHandler`, `GetSessionsQueryHandler`.


## Testing Guidelines
- Test framework: xUnit v3 (`xunit.v3.mtp-v2`).
- Place unit tests under `tests/PracticeLogger.Tests.Unit`; integration tests under `tests/PracticeLogger.Tests.Integration`.
- Naming: mirror the target type with `Tests` suffix, e.g., `CreateSessionCommandHandlerTests`.
- Run targeted tests via `dotnet test` with the specific `.csproj`.

## Commit & Pull Request Guidelines
- Commit messages are short and imperative (e.g., “adds delete flow”); keep them concise.
- PRs should include a brief summary, the tests run (with commands), and any behavior changes.
- Link related issues or notes when available; screenshots only if UI behavior changes.

## Configuration & Data
- Runtime settings live in `src/PracticeLogger.Api/appsettings.json` and `appsettings.Development.json`.
- EF Core migrations are under `src/PracticeLogger.Infrastructure/Migrations`; avoid editing generated files manually.
