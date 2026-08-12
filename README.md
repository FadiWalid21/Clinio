# Clinio

**Book a doctor's appointment in minutes — built for Egypt's clinics and patients.**

Clinio is a medical appointment booking platform connecting patients with doctors and clinics across Egypt. It's currently a demo-stage project — a patient-facing booking site is live, with a doctor/secretary management dashboard planned next — backed by a clean, domain-driven .NET API. The goal is to take it to production and deploy it for testing.

**Repository:** [github.com/FadiWalid21/Clinio](https://github.com/FadiWalid21/Clinio)

<!-- Badges -->
![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-v3-38B2AC?logo=tailwind-css&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-lightgrey)

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Conventions](#api-conventions)
- [Contributing](#contributing)
- [License](#license)

---

## Features

### For Patients
- Browse and search doctors by specialty, clinic, and availability
- View detailed doctor profiles with schedules and booking flow
- Book, view, and cancel appointments (within a configurable cancellation window)
- Patient registration and profile management
- Multi-language support via a custom, typed i18n system
- Homepage with hero section, "how it works" guide, specialties, featured doctors, and live stats

### For Doctors & Clinics
- Manage doctor schedules and time slots
- View and manage incoming appointments with clear status flows
- Secretary/staff access for day-to-day appointment management
- Image management for doctor and clinic profiles

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Backend** | ASP.NET Core (.NET 10), Clean Architecture, CQRS + MediatR |
| **Frontend** | Angular 21 (standalone components, signals-based state) |
| **Styling** | Tailwind CSS v3, SCSS |
| **Auth** | JWT with refresh-token rotation |
| **Database** | SQL Server |
| **Tooling** | Postman, Docker |

---

## Project Structure

Clinio is planned as a multi-app repository with two Angular frontends and a Clean Architecture .NET backend. The .NET projects sit at the repository root alongside the frontend. The patient-facing site (`client`) and the API are built; the doctor/secretary dashboard is not yet created.

```
ClinioSaaS/
├── client/                   # Patient-facing booking site (Angular) — built
├── dashboard/                 # Doctor/secretary management dashboard (Angular) — not yet created
├── Clinio.Domain/             # Entities, value objects, domain logic
├── Clinio.Application/        # CQRS commands/queries, MediatR handlers, interfaces
├── Clinio.Infrastructure/     # EF Core, external services, implementations
├── Clinio.Api/                # Controllers, middleware, API composition root
├── ClinioSaaS.sln
├── Dockerfile
└── compose.yaml
```

**Core domain model:** `DoctorSchedule → TimeSlot → Appointment`, with status flows, cancellation window enforcement, and optimistic concurrency to safely handle concurrent booking attempts.

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS) and npm
- [Angular CLI](https://angular.dev/tools/cli) — `npm install -g @angular/cli`
- SQL Server (LocalDB, Developer, or Docker container)
- [Docker](https://www.docker.com/) (optional, for containerized SQL Server/services)

### Clone the repository

```bash
git clone https://github.com/FadiWalid21/Clinio.git
cd ClinioSaaS
```

### Backend setup

```bash
dotnet restore
dotnet ef database update --project Clinio.Infrastructure --startup-project Clinio.Api
dotnet run --project Clinio.Api
```

> A `Dockerfile` and `compose.yaml` are also included at the repo root if you'd rather run the backend (and SQL Server) via Docker.

### Frontend setup

**client** (patient-facing site)
```bash
cd client
npm install
ng serve
```

**dashboard** (doctor/secretary management — not yet created)

---

## API Conventions

Clinio's backend follows a consistent, predictable pattern for handling success and failure across the API:

- **`Result<T>` pattern** — Application-layer handlers return a `Result<T>` instead of throwing exceptions for expected failures. A failed operation returns `Result<T>.Failure(new ResultError(Code, Description))`, keeping error handling explicit and typed.
- **Standardized error responses** — Controllers convert a `Result<T>` into a client response via `result.ToProblemDetails(this)`, ensuring every error follows the same [RFC 7807 Problem Details](https://datatracker.ietf.org/doc/html/rfc7807) shape.
- **Localized error messages** — Error descriptions are resolved through `ILocalizationService.Get("Key")`, so API error messages support multiple languages out of the box.
- **Current user context** — Authenticated user identity is accessed via `ICurrentUserService.UserId`, keeping handlers decoupled from `HttpContext`.

This keeps business logic free of exception-driven control flow and gives frontend consumers a single, predictable error shape to handle.

---

## Contributing

Clinio is a solo project, built and maintained entirely by [Fadi Walid](https://github.com/FadiWalid21). It is not currently open to outside contributions.

---

## License

No license has been chosen yet. Until one is added, all rights are reserved by the author.
