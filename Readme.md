# ATS System – Mini Applicant Tracking System

Ett mini-ATS (Applicant Tracking System) byggt med React + TypeScript i frontend och C# .NET i backend, med Supabase (PostgreSQL) som databas.

---

## Tech Stack

- **Frontend:** React + TypeScript (Vite)
- **Backend:** C# .NET 8
- **Databas:** Supabase (PostgreSQL)
- **Auth:** JWT via C# backend
- **Extern API:** Arbetsförmedlingen (Platsbanken API)

---

## Arkitektur

```
React (TypeScript) ──→ C# API (.NET 8) ──→ Supabase (PostgreSQL)
                                       ──→ Arbetsförmedlingen API
```

Frontend pratar enbart med C# API:et. C# API:et pratar med Supabase och Arbetsförmedlingens API. Ingen direktkommunikation mellan React och externa tjänster.

---

## Projektstruktur

```bash
ats-system/
├── backend/
│   └── ATS.Api/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── JobsController.cs
│       │   ├── CandidatesController.cs
│       │   └── ArbetsformedlingenController.cs    # Söka & importera jobbannonser
│       ├── Models/
│       │   ├── User.cs
│       │   ├── Job.cs
│       │   └── Candidate.cs
│       ├── DTOs/
│       │   ├── Auth/
│       │   │   ├── LoginDto.cs
│       │   │   ├── LoginResponseDto.cs
│       │   │   └── CreateUserDto.cs
│       │   ├── Jobs/
│       │   │   ├── CreateJobDto.cs
│       │   │   └── JobResponseDto.cs
│       │   ├── Candidates/
│       │   │   ├── CreateCandidateDto.cs
│       │   │   ├── UpdateCandidateStatusDto.cs
│       │   │   └── CandidateResponseDto.cs
│       │   └── Arbetsformedlingen/
│       │       ├── AfJobSearchResultDto.cs         # Sökresultat från AF
│       │       └── AfImportJobDto.cs               # Importera annons till ATS
│       ├── Services/
│       │   ├── Interfaces/
│       │   │   ├── IAuthService.cs
│       │   │   ├── IJobService.cs
│       │   │   ├── ICandidateService.cs
│       │   │   └── IArbetsformedlingenService.cs   # Interface för AF-integration
│       │   ├── AuthService.cs
│       │   ├── JobService.cs
│       │   ├── CandidateService.cs
│       │   └── ArbetsformedlingenService.cs        # Anrop mot AF API
│       ├── Data/
│       │   ├── SupabaseClient.cs
│       │   └── Repositories/
│       │       ├── Interfaces/
│       │       │   ├── IUserRepository.cs
│       │       │   ├── IJobRepository.cs
│       │       │   └── ICandidateRepository.cs
│       │       ├── UserRepository.cs
│       │       ├── JobRepository.cs
│       │       └── CandidateRepository.cs
│       ├── Middleware/
│       │   ├── JwtMiddleware.cs
│       │   └── ErrorHandlingMiddleware.cs
│       ├── Helpers/
│       │   └── JwtHelper.cs
│       ├── Exceptions/
│       │   ├── AppException.cs
│       │   ├── NotFoundException.cs
│       │   └── UnauthorizedException.cs
│       ├── Program.cs
│       ├── appsettings.json
│       └── ATS.Api.csproj
│
└── frontend/
    └── ats-client/
        ├── src/
        │   ├── api/
        │   │   ├── apiClient.ts
        │   │   ├── auth.ts
        │   │   ├── jobs.ts
        │   │   ├── candidates.ts
        │   │   └── arbetsformedlingen.ts           # API-anrop för AF-sökning
        │   ├── components/
        │   │   ├── Layout/
        │   │   │   ├── Navbar.tsx
        │   │   │   └── Sidebar.tsx
        │   │   ├── Kanban/
        │   │   │   ├── KanbanBoard.tsx
        │   │   │   ├── KanbanColumn.tsx
        │   │   │   └── CandidateCard.tsx
        │   │   ├── Jobs/
        │   │   │   ├── JobList.tsx
        │   │   │   ├── JobForm.tsx
        │   │   │   └── AfJobSearch.tsx             # Sök & importera från AF
        │   │   ├── Candidates/
        │   │   │   ├── CandidateForm.tsx
        │   │   │   └── CandidateFilter.tsx
        │   │   └── Auth/
        │   │       └── LoginForm.tsx
        │   ├── pages/
        │   │   ├── LoginPage.tsx
        │   │   ├── DashboardPage.tsx
        │   │   ├── JobsPage.tsx
        │   │   └── KanbanPage.tsx
        │   ├── hooks/
        │   │   ├── useAuth.ts
        │   │   ├── useJobs.ts
        │   │   ├── useCandidates.ts
        │   │   └── useAfSearch.ts                  # Hook för AF-sökning
        │   ├── context/
        │   │   └── AuthContext.tsx
        │   ├── types/
        │   │   └── index.ts
        │   ├── utils/
        │   │   └── helpers.ts
        │   ├── constants/
        │   │   └── kanbanColumns.ts
        │   ├── App.tsx
        │   └── main.tsx
        ├── index.html
        ├── package.json
        ├── tsconfig.json
        └── vite.config.ts
```

---

## Varför denna struktur?

### Repository Pattern
Vi separerar databaslogiken helt från affärslogiken. Services vet inte hur data hämtas – det är Repositoryns ansvar. Om vi byter databas imorgon ändrar vi bara i Repository-filerna, ingenting annat.

### DTOs uppdelade per domän
Auth, Jobs, Candidates och Arbetsförmedlingen har egna DTO-mappar. Ju fler endpoints vi lägger till, desto viktigare är detta. Separata Response-DTOs säkerställer att vi aldrig läcker känslig data (t.ex. lösenordshash) till fronten.

### Interfaces på allt
`IAuthService`, `IJobRepository`, `IArbetsformedlingenService` osv följer SOLID-principerna och gör koden enkel att testa. Du kan mocka ett interface i tester utan att röra databasen eller externa API:er.

### Exceptions-mappen
`NotFoundException` och `UnauthorizedException` gör att `ErrorHandlingMiddleware` automatiskt returnerar rätt HTTP-statuskod (404, 401 osv) utan att varje controller behöver hantera det själv.

### JwtHelper separerad
AuthService hanterar affärslogik. JwtHelper hanterar token-generering. En fil – ett ansvar.

### Custom Hooks (Frontend)
`useJobs`, `useCandidates`, `useAfSearch` osv håller komponenterna rena. Logiken lever i hooken, inte i komponenten.

### Central apiClient
En enda Axios-instans med auth-headers. Annars skriver man samma header-logik på 20 ställen.

### kanbanColumns.ts
Kanban-statusar definieras på ett ställe. Enkelt att ändra eller lägga till nya kolumner utan att leta i hela kodbasen.

---

## Roller

| Roll  | Behörighet |
|-------|-----------|
| Admin | Kan skapa admin- och kundkonton, samt göra allt kunder kan göra |
| Kund  | Kan skapa jobb, lägga till kandidater, se och filtrera kanban-vy |

---

## Kanban-statusar

| Status      | Beskrivning                  |
|-------------|------------------------------|
| Ny          | Kandidat precis tillagd      |
| Intervju    | Kallas till intervju         |
| Erbjudande  | Jobberbjudande skickat       |
| Avvisad     | Kandidat avvisad             |

---

## Arbetsförmedlingen API Integration

Kunder kan söka efter jobbannonser direkt från Arbetsförmedlingens öppna API (Platsbanken) och importera dem in i ATS:et med ett klick.

### Flöde
```
Kund söker jobbannons →
Resultat från AF API visas →
Kund klickar "Importera" →
Jobbet sparas i databasen →
Kund börjar lägga till kandidater
```

### Varför detta?
- Arbetsförmedlingens API är öppet och gratis – ingen API-nyckel krävs
- Kunden slipper fylla i jobbinformation manuellt
- Snabbare onboarding för nya kunder
- C# proxar anropen vilket skyddar mot CORS-problem i browsern

---

## Kom igång

### Backend
```bash
cd backend/ATS.Api
dotnet restore
dotnet run
```

### Frontend
```bash
cd frontend/ats-client
npm install
npm run dev
```

### Miljövariabler (backend – appsettings.json)
```json
{
  "Supabase": {
    "ConnectionString": "YOUR_SUPABASE_CONNECTION_STRING"
  },
  "Jwt": {
    "Secret": "YOUR_JWT_SECRET",
    "Issuer": "ats-api",
    "ExpiryMinutes": 60
  },
  "Arbetsformedlingen": {
    "BaseUrl": "https://jobsearch.api.jobtechdev.se"
  }
}
```

### Miljövariabler (frontend – .env)
```env
VITE_API_URL=http://localhost:5000
```