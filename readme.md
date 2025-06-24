# Bootstrap Project

This is a .NET Aspire bootstrap project that provides a modern distributed application setup.

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Node.js (for the SPA frontend)
- Docker (for containerized services)

### Running the Application

To start the entire application with all its services, run:

```bash
dotnet run --project src/AspireHost
```

This will launch the Aspire dashboard where you can monitor and manage all services in your application.

## Project Structure

- `src/` - Contains all source code
  - `AspireHost/` - The Aspire host application that orchestrates all services
  - `Api/` - Backend API service
  - `spa/` - Frontend SPA application
  - `Todos/` - Todo-related functionality
  - `ServiceDefaults/` - Shared service configurations
  - `BuildingBlocks/` - Reusable components

- `docs/` - Documentation
  - `adr/` - Architecture Decision Records (ADRs)
  - `slides/` - Presentation slides

- `infra/` - Infrastructure as code

- `local/` - Local development tools and utilities

- `tests/` - Test projects

## Architecture Decision Records (ADRs)

We maintain Architecture Decision Records (ADRs) to document important architectural decisions in this project. These can be found in the `docs/adr/` directory.

ADRs capture the context and consequences of significant architectural decisions, making it easier for team members to understand why certain choices were made.

