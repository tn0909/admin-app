# AGENTS.md

This repository contains a full-stack admin application with an ASP.NET Core API backend and an Angular frontend.

## Project overview

Full-stack admin app: create/search companies and their users.

- Backend: `AdminApp/` — ASP.NET Core Web API, entry point `AdminApp/Program.cs`
- Frontend: `AdminAppUI/` — Angular 17
- Persistence: Elasticsearch, parent/child document model (company parent, user child), via Nest client (`IElasticClient`)
- Swagger (dev): `http://localhost:5076/swagger/index.html`
- Backend runs at `http://localhost:5076`, frontend dev server at `http://localhost:4200`
- CORS in backend allows `http://localhost:4200` — keep in mind when changing local ports/origins
- All `/api/companies` and `/api/users` endpoints require a JWT bearer token; see [README.md#authentication](README.md#authentication)

## Commands

Elasticsearch must be running for backend to work:
```bash
docker run -d --name elasticsearch \
  -p 9200:9200 -p 9300:9300 \
  -e "discovery.type=single-node" \
  docker.elastic.co/elasticsearch/elasticsearch:7.17.0
```

Backend (`AdminApp/`):
```bash
dotnet restore
dotnet build
dotnet run
```

Backend tests (from repo root):
```bash
dotnet test AdminApp.Tests
```

Frontend (`AdminAppUI/`):
```bash
npm install
npm start          # ng serve, http://localhost:4200
npm run build
npm run watch       # ng build --watch --configuration development
npm test            # ng test (Karma/Jasmine)
```

## Architecture and conventions

### Backend (`AdminApp/`)

- Services registered via DI in `Program.cs`: Elasticsearch, `ICompanyService`, `IUserService`, `IAuthService`, AutoMapper.
- Controllers are thin; business logic and Elasticsearch access belong in services (see `AdminApp/Services/CompanyService.cs` for the pattern) — never put ES queries directly in controllers.
- DTOs (`AdminApp/Dtos/`) map to domain models (`AdminApp/Models/`) via AutoMapper profiles in `AdminApp/Profiles/MappingProfile.cs`.
- Elasticsearch mapping models company and user as parent/child documents joined via a `joinField` (see README for the full mapping JSON) — routing is required for child docs.
- User-supplied search terms must go through `AdminApp.Extensions.LuceneQueryEscaper` before being interpolated into a Nest `QueryString` query — raw interpolation is a Lucene query injection hole.
- Auth (`AdminApp/Auth/`): JWT bearer, single admin credential configured under the `Auth` config section (`AdminUsername`, `AdminPasswordHash`, `JwtSecret`, `JwtIssuer`, `JwtAudience`, `JwtExpiryMinutes`). Dev-only defaults live in `appsettings.Development.json`; production must override every value via environment variables, never commit real credentials.

### Frontend (`AdminAppUI/`)

- App code in `AdminAppUI/src/app/`.
- Feature components grouped by domain: `company/`, `company-detail/`, `user/`, `user-detail/`, `login/`.
- Shared contracts: `src/app/models/`.
- API clients: `src/app/services/`.
- `AuthInterceptor` attaches the bearer token to outgoing requests and redirects to `/login` on a 401; `authGuard` protects the companies/users routes.
- Match existing component/service naming and keep DTOs aligned with backend models.

## Working expectations

- Keep response/request DTOs aligned with existing API contracts — breaking them breaks the Angular client.
- When changing API behavior, update backend and Angular models together.
- Mimic the existing Company/User domain structure and naming when adding features; favor small, focused edits over new abstraction layers unless clearly needed.
- Do not rewrite the app structure unless the task requires it.
- Prefer repository conventions over introducing new patterns.

## API contract reference

- `POST /api/auth/login` — obtain a JWT (`username`, `password`)
- `POST /api/companies` — create company (`name`, `description`, `address`, `website`)
- `GET /api/companies/{id}` — get company by id
- `POST /api/companies/search` — search by name/address/description (full-text); `searchTerm`, `limit`, `includeUsers`, `usersLimit`; empty `searchTerm` returns all
- `POST /api/users` — create user (`email`, `firstName`, `lastName`, `title`, `companyId`)
- `GET /api/users/{id}` — get user by id
- `POST /api/users/search` — search by `company` and/or `email`, `limit`; empty params return all

Full request/response shapes: [README.md](README.md).
