# FormBuilder

A small proof-of-concept form builder: create a form template with dynamic fields (text, textarea, number, date, datetime, email, phone, dropdown, radio, checkbox, file) and a multi-step approval route, and save it via a REST API.

## Tech stack

- **Backend:** .NET 9 / ASP.NET Core Web API, Clean Architecture (`Api` → `Application` → `Infrastructure`), Entity Framework Core 9 + SQLite, Swagger.
- **Frontend:** Angular 21, standalone components, Reactive Forms (typed, with `FormArray` for dynamic fields/steps), strict TypeScript, plain SCSS (no UI component library).
- **Tests:** xUnit (backend, `FormBuilder.Application.Tests`), Vitest via Angular CLI (frontend).

## How to run

### 1. Backend API

```bash
cd formBuilderAPI/FormBuilder.Api
dotnet run
```

Runs at **http://localhost:5067** (applies EF Core migrations automatically against a local SQLite file, `formbuilder.db`). Swagger UI: http://localhost:5067/swagger.

### 2. Frontend

```bash
cd formBuilderUI
npm install
npx ng serve --port 4299
```

Open **http://localhost:4299** (redirects to `/forms/new`).

> The backend's CORS policy only allows origin `http://localhost:4299`, and the frontend's `environment.ts` points at `http://localhost:5067/api` — keep both servers on these exact ports, or update `Program.cs` (CORS) and `src/environments/environment.ts` (API URL) together if you change them.

### API endpoints

```
POST /api/forms
GET  /api/forms
GET  /api/forms/{id}
```

### Running tests

```bash
# backend
cd formBuilderAPI && dotnet test

# frontend
cd formBuilderUI && npx ng test --watch=false
```

## Built with Claude Code

This project's backend, frontend, and this documentation were built and iterated on with [Claude Code](https://claude.com/claude-code). See `server-build-history.md` and `client-build-history.md` for a session-by-session log of what was built and when.
