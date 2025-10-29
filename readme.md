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

## Event Documentation

The project includes auto-generated documentation for integration events (CAP) and domain events (MediatR):

- **Integration Events**: CAP event bus topics with publishers and consumers
- **Domain Events**: MediatR events with their handlers and flow diagrams
- **AsyncAPI Specification**: Standard API documentation for async messaging

### Viewing Event Documentation

Event documentation is available in the `docs/events/` directory:

- [`docs/events/README.md`](docs/events/README.md) - Overview and architecture
- [`docs/events/integration-events.md`](docs/events/integration-events.md) - CAP integration events
- [`docs/events/domain-events.md`](docs/events/domain-events.md) - MediatR domain events
- [`docs/events/asyncapi.yaml`](docs/events/asyncapi.yaml) - AsyncAPI 3.0 specification

### Regenerating Documentation

To regenerate the event documentation after making changes to events:

```bash
./tools/generate-docs.sh
```

This will scan the source code and update the documentation files automatically.

### CI/CD Integration

The documentation is automatically generated and updated on every push to the main branch through a GitHub Actions workflow. The workflow:

1. Builds the project
2. Generates event documentation
3. Validates the AsyncAPI specification
4. Commits updated documentation
5. Deploys to GitHub Pages (if configured)

