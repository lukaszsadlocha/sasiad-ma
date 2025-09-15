# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
Sasiad-Ma is a neighborhood item sharing application that allows community members to borrow and lend useful items for free. The project uses a monolithic architecture with a .NET backend and React frontend.

## Development Commands

### Frontend (React + Vite + TypeScript)
```bash
cd frontend
npm run dev          # Start development server
npm run build        # Build for production (runs TypeScript compiler first)
npm run lint         # Run ESLint with TypeScript support
npm run preview      # Preview production build
```

### Backend (.NET Minimal API)
```bash
cd backend
dotnet restore       # Restore NuGet packages
dotnet build         # Build the solution
dotnet run --project src/SasiadMa.Api  # Run the API
```

## Architecture

### Backend Structure (Domain-Driven Design)
- **SasiadMa.Core**: Domain layer with entities, value objects, and domain logic
- **SasiadMa.Application**: Application layer with DTOs, services, and business logic
- **SasiadMa.Infrastructure**: Infrastructure layer with data access and external services
- **SasiadMa.Api**: Presentation layer with Minimal API endpoints

The backend follows these architectural principles:
- **Domain-Driven Design (DDD)** with clear layer separation
- **Result pattern** for error handling across all layers
- **Minimal API** instead of traditional controllers
- Manual mapping instead of AutoMapper
- No MediatR - direct service calls for simplicity

### Frontend Structure
- **React 18** with TypeScript and Vite
- **React Router** for navigation
- **React Query** (@tanstack/react-query) for server state management
- **React Hook Form** for form handling
- **Tailwind CSS** for styling
- **Axios** for HTTP requests
- **Context API** for authentication state

Key directories:
- `components/`: Reusable UI components
- `pages/`: Page-level components
- `context/`: React context providers (e.g., AuthContext)
- `hooks/`: Custom hooks
- `services/`: API service functions
- `types/`: TypeScript type definitions

## Key Domain Entities
- **User**: Community members with authentication
- **Community**: Groups of neighbors sharing items
- **Item**: Things that can be borrowed/lent
- **BorrowRequest**: Requests to borrow items
- **Notification**: System alerts for users

## Development Guidelines
- Do not use AutoMapper - map manually for clarity
- Do not use MediatR - keep request handling explicit
- Use the Result pattern consistently for error handling
- Avoid paid libraries - stick to open-source solutions
- Follow Domain-Driven Design principles
- Keep UI minimal - focus on functionality over design
- Use PostgreSQL as the primary database