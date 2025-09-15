# 🏗️ Sasiad-Ma Application Scaffold

This document outlines the complete scaffold for the Sasiad-Ma neighborhood sharing application based on the requirements in the README.md file.

## 🎯 Project Overview
- **Purpose**: Community-driven application for borrowing and lending items between neighbors
- **Architecture**: Monolithic application with .NET backend and React frontend
- **Database**: PostgreSQL
- **Hosting**: Azure or similar cloud platform

## 📁 Project Structure

```
sasiad-ma/
├── backend/
│   ├── src/
│   │   ├── SasiadMa.Api/
│   │   │   ├── Program.cs
│   │   │   ├── SasiadMa.Api.csproj
│   │   │   ├── appsettings.json
│   │   │   ├── appsettings.Development.json
│   │   │   ├── Extensions/
│   │   │   │   ├── ServiceCollectionExtensions.cs
│   │   │   │   └── WebApplicationExtensions.cs
│   │   │   └── Endpoints/
│   │   │       ├── AuthEndpoints.cs
│   │   │       ├── CommunityEndpoints.cs
│   │   │       ├── ItemEndpoints.cs
│   │   │       ├── UserEndpoints.cs
│   │   │       └── NotificationEndpoints.cs
│   │   ├── SasiadMa.Core/
│   │   │   ├── SasiadMa.Core.csproj
│   │   │   ├── Common/
│   │   │   │   ├── Result.cs
│   │   │   │   ├── Error.cs
│   │   │   │   ├── IEntity.cs
│   │   │   │   └── BaseEntity.cs
│   │   │   ├── Entities/
│   │   │   │   ├── User.cs
│   │   │   │   ├── Community.cs
│   │   │   │   ├── Item.cs
│   │   │   │   ├── BorrowRequest.cs
│   │   │   │   ├── ItemRequest.cs
│   │   │   │   └── Notification.cs
│   │   │   ├── ValueObjects/
│   │   │   │   ├── Email.cs
│   │   │   │   ├── ItemCategory.cs
│   │   │   │   ├── ItemCondition.cs
│   │   │   │   └── ReputationScore.cs
│   │   │   ├── Enums/
│   │   │   │   ├── ItemStatus.cs
│   │   │   │   ├── BorrowStatus.cs
│   │   │   │   ├── NotificationType.cs
│   │   │   │   └── UserRole.cs
│   │   │   └── Interfaces/
│   │   │       ├── IUserRepository.cs
│   │   │       ├── ICommunityRepository.cs
│   │   │       ├── IItemRepository.cs
│   │   │       ├── IBorrowRequestRepository.cs
│   │   │       ├── IItemRequestRepository.cs
│   │   │       └── INotificationRepository.cs
│   │   ├── SasiadMa.Application/
│   │   │   ├── SasiadMa.Application.csproj
│   │   │   ├── Services/
│   │   │   │   ├── AuthService.cs
│   │   │   │   ├── UserService.cs
│   │   │   │   ├── CommunityService.cs
│   │   │   │   ├── ItemService.cs
│   │   │   │   ├── BorrowService.cs
│   │   │   │   ├── NotificationService.cs
│   │   │   │   └── EmailService.cs
│   │   │   ├── DTOs/
│   │   │   │   ├── Auth/
│   │   │   │   │   ├── LoginRequest.cs
│   │   │   │   │   ├── RegisterRequest.cs
│   │   │   │   │   ├── LoginResponse.cs
│   │   │   │   │   └── GoogleLoginRequest.cs
│   │   │   │   ├── User/
│   │   │   │   │   ├── UserDto.cs
│   │   │   │   │   ├── CreateUserRequest.cs
│   │   │   │   │   └── UpdateUserRequest.cs
│   │   │   │   ├── Community/
│   │   │   │   │   ├── CommunityDto.cs
│   │   │   │   │   ├── CreateCommunityRequest.cs
│   │   │   │   │   ├── UpdateCommunityRequest.cs
│   │   │   │   │   └── JoinCommunityRequest.cs
│   │   │   │   ├── Item/
│   │   │   │   │   ├── ItemDto.cs
│   │   │   │   │   ├── CreateItemRequest.cs
│   │   │   │   │   ├── UpdateItemRequest.cs
│   │   │   │   │   └── ItemSearchRequest.cs
│   │   │   │   ├── Borrow/
│   │   │   │   │   ├── BorrowRequestDto.cs
│   │   │   │   │   ├── CreateBorrowRequest.cs
│   │   │   │   │   └── UpdateBorrowRequest.cs
│   │   │   │   └── Notification/
│   │   │   │       ├── NotificationDto.cs
│   │   │   │       └── CreateNotificationRequest.cs
│   │   │   └── Interfaces/
│   │   │       ├── IAuthService.cs
│   │   │       ├── IUserService.cs
│   │   │       ├── ICommunityService.cs
│   │   │       ├── IItemService.cs
│   │   │       ├── IBorrowService.cs
│   │   │       ├── INotificationService.cs
│   │   │       └── IEmailService.cs
│   │   └── SasiadMa.Infrastructure/
│   │       ├── SasiadMa.Infrastructure.csproj
│   │       ├── Data/
│   │       │   ├── ApplicationDbContext.cs
│   │       │   ├── DbInitializer.cs
│   │       │   └── Configurations/
│   │       │       ├── UserConfiguration.cs
│   │       │       ├── CommunityConfiguration.cs
│   │       │       ├── ItemConfiguration.cs
│   │       │       ├── BorrowRequestConfiguration.cs
│   │       │       ├── ItemRequestConfiguration.cs
│   │       │       └── NotificationConfiguration.cs
│   │       ├── Repositories/
│   │       │   ├── BaseRepository.cs
│   │       │   ├── UserRepository.cs
│   │       │   ├── CommunityRepository.cs
│   │       │   ├── ItemRepository.cs
│   │       │   ├── BorrowRequestRepository.cs
│   │       │   ├── ItemRequestRepository.cs
│   │       │   └── NotificationRepository.cs
│   │       ├── Services/
│   │       │   ├── EmailService.cs
│   │       │   ├── FileUploadService.cs
│   │       │   └── GoogleAuthService.cs
│   │       └── Migrations/
│   └── tests/
│       ├── SasiadMa.UnitTests/
│       │   ├── SasiadMa.UnitTests.csproj
│       │   ├── Services/
│       │   ├── Entities/
│       │   └── ValueObjects/
│       └── SasiadMa.IntegrationTests/
│           ├── SasiadMa.IntegrationTests.csproj
│           ├── Endpoints/
│           └── TestFixtures/
├── frontend/
│   ├── package.json
│   ├── package-lock.json
│   ├── tsconfig.json
│   ├── tailwind.config.js
│   ├── postcss.config.js
│   ├── vite.config.ts
│   ├── index.html
│   ├── public/
│   │   ├── favicon.ico
│   │   └── manifest.json
│   └── src/
│       ├── main.tsx
│       ├── App.tsx
│       ├── App.css
│       ├── index.css
│       ├── vite-env.d.ts
│       ├── components/
│       │   ├── common/
│       │   │   ├── Header.tsx
│       │   │   ├── Footer.tsx
│       │   │   ├── LoadingSpinner.tsx
│       │   │   ├── ErrorBoundary.tsx
│       │   │   └── ProtectedRoute.tsx
│       │   ├── auth/
│       │   │   ├── LoginForm.tsx
│       │   │   ├── RegisterForm.tsx
│       │   │   ├── GoogleLoginButton.tsx
│       │   │   └── EmailConfirmation.tsx
│       │   ├── community/
│       │   │   ├── CommunityList.tsx
│       │   │   ├── CommunityCard.tsx
│       │   │   ├── CreateCommunityForm.tsx
│       │   │   ├── CommunityDetail.tsx
│       │   │   ├── JoinCommunityForm.tsx
│       │   │   └── CommunityChat.tsx
│       │   ├── item/
│       │   │   ├── ItemList.tsx
│       │   │   ├── ItemCard.tsx
│       │   │   ├── ItemDetail.tsx
│       │   │   ├── CreateItemForm.tsx
│       │   │   ├── EditItemForm.tsx
│       │   │   ├── ItemSearch.tsx
│       │   │   └── ItemImageUpload.tsx
│       │   ├── borrow/
│       │   │   ├── BorrowRequestList.tsx
│       │   │   ├── BorrowRequestCard.tsx
│       │   │   ├── CreateBorrowRequest.tsx
│       │   │   ├── BorrowHistory.tsx
│       │   │   └── ReturnItemForm.tsx
│       │   ├── user/
│       │   │   ├── UserProfile.tsx
│       │   │   ├── EditProfile.tsx
│       │   │   ├── UserSettings.tsx
│       │   │   └── ReputationDisplay.tsx
│       │   └── notifications/
│       │       ├── NotificationList.tsx
│       │       ├── NotificationCard.tsx
│       │       └── NotificationSettings.tsx
│       ├── pages/
│       │   ├── HomePage.tsx
│       │   ├── LoginPage.tsx
│       │   ├── RegisterPage.tsx
│       │   ├── DashboardPage.tsx
│       │   ├── CommunityPage.tsx
│       │   ├── ItemsPage.tsx
│       │   ├── ProfilePage.tsx
│       │   ├── NotificationsPage.tsx
│       │   └── NotFoundPage.tsx
│       ├── hooks/
│       │   ├── useAuth.ts
│       │   ├── useApi.ts
│       │   ├── useLocalStorage.ts
│       │   ├── useDebounce.ts
│       │   └── useNotifications.ts
│       ├── services/
│       │   ├── api.ts
│       │   ├── authService.ts
│       │   ├── userService.ts
│       │   ├── communityService.ts
│       │   ├── itemService.ts
│       │   ├── borrowService.ts
│       │   └── notificationService.ts
│       ├── context/
│       │   ├── AuthContext.tsx
│       │   ├── CommunityContext.tsx
│       │   └── NotificationContext.tsx
│       ├── utils/
│       │   ├── constants.ts
│       │   ├── helpers.ts
│       │   ├── validators.ts
│       │   └── formatters.ts
│       └── types/
│           ├── auth.ts
│           ├── user.ts
│           ├── community.ts
│           ├── item.ts
│           ├── borrow.ts
│           ├── notification.ts
│           └── common.ts
└── docker-compose.yml
```

## 🔧 Backend Technology Stack

### Core Technologies
- **.NET 8**: Latest LTS version for backend API
- **PostgreSQL**: Database for data persistence
- **Entity Framework Core**: ORM for database operations
- **JWT**: Authentication and authorization
- **Google OAuth**: Alternative authentication method

### Key Packages
```xml
<!-- API Project -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.Google" />
<PackageReference Include="Swashbuckle.AspNetCore" />

<!-- Core Project -->
<!-- No external dependencies - pure domain logic -->

<!-- Application Project -->
<PackageReference Include="BCrypt.Net-Next" />
<PackageReference Include="System.IdentityModel.Tokens.Jwt" />

<!-- Infrastructure Project -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" />
<PackageReference Include="CloudinaryDotNet" /> <!-- For image uploads -->

<!-- Test Projects -->
<PackageReference Include="Microsoft.NET.Test.Sdk" />
<PackageReference Include="xunit" />
<PackageReference Include="xunit.runner.visualstudio" />
<PackageReference Include="SpecFlow.xUnit" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" />
```

## ⚛️ Frontend Technology Stack

### Core Technologies
- **React 18**: Modern React with hooks
- **TypeScript**: Type safety and better developer experience
- **Vite**: Fast build tool and dev server
- **React Router**: Client-side routing
- **TailwindCSS**: Utility-first CSS framework

### Key Dependencies
```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.8.0",
    "axios": "^1.3.0",
    "react-query": "^3.39.0",
    "react-hook-form": "^7.43.0",
    "react-hot-toast": "^2.4.0",
    "lucide-react": "^0.150.0"
  },
  "devDependencies": {
    "@types/react": "^18.0.27",
    "@types/react-dom": "^18.0.10",
    "@vitejs/plugin-react": "^3.1.0",
    "typescript": "^4.9.4",
    "vite": "^4.1.0",
    "tailwindcss": "^3.2.0",
    "postcss": "^8.4.21",
    "autoprefixer": "^10.4.13",
    "@types/node": "^18.14.0"
  }
}
```

## 🏗️ Backend Architecture Details

### 1. Domain Layer (SasiadMa.Core)
**Key Entities:**
- `User`: Represents application users with authentication and profile data
- `Community`: Groups of neighbors who can share items
- `Item`: Items available for lending/borrowing
- `BorrowRequest`: Tracks borrowing transactions
- `ItemRequest`: Requests for items not yet available
- `Notification`: System notifications and alerts

**Value Objects:**
- `Email`: Validates and encapsulates email addresses
- `ItemCategory`: Categorizes items (tools, appliances, sports, etc.)
- `ReputationScore`: Tracks user trustworthiness
- `ItemCondition`: Describes item condition (new, good, fair, poor)

**Result Pattern Implementation:**
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error Error { get; }
    
    // Implementation details...
}
```

### 2. Application Layer (SasiadMa.Application)
**Services:**
- `AuthService`: Handle login, registration, JWT tokens
- `UserService`: User management operations
- `CommunityService`: Community CRUD and membership
- `ItemService`: Item management and search
- `BorrowService`: Borrowing workflows
- `NotificationService`: Notification management
- `EmailService`: Email sending capabilities

### 3. Infrastructure Layer (SasiadMa.Infrastructure)
**Repositories:** Implement domain interfaces using Entity Framework
**External Services:** Email, file upload, Google OAuth integration
**Data Configurations:** Entity Framework configurations

### 4. API Layer (SasiadMa.Api)
**Minimal API Endpoints:**
- `AuthEndpoints`: /api/auth/* (login, register, refresh)
- `UserEndpoints`: /api/users/* (profile, settings)
- `CommunityEndpoints`: /api/communities/* (CRUD, join, members)
- `ItemEndpoints`: /api/items/* (CRUD, search, upload images)
- `BorrowEndpoints`: /api/borrows/* (request, approve, return)
- `NotificationEndpoints`: /api/notifications/* (list, mark as read)

## 🎨 Frontend Architecture Details

### 1. Component Structure
**Common Components:**
- Reusable UI components (buttons, forms, modals)
- Layout components (header, footer, navigation)
- Protected routes and authentication guards

**Feature Components:**
- Organized by domain (auth, community, items, borrow, notifications)
- Each feature has list, detail, and form components

### 2. State Management
**React Context:** For global state (authentication, current community)
**React Query:** For server state management and caching
**Local State:** useState and useReducer for component-specific state

### 3. Routing Structure
```typescript
/                     -> HomePage
/login               -> LoginPage
/register            -> RegisterPage
/dashboard           -> DashboardPage (protected)
/communities         -> CommunityPage (protected)
/communities/:id     -> CommunityDetail (protected)
/items               -> ItemsPage (protected)
/items/:id           -> ItemDetail (protected)
/profile             -> ProfilePage (protected)
/notifications       -> NotificationsPage (protected)
```

## 🔐 Authentication & Authorization

### Backend JWT Implementation
- JWT tokens for stateless authentication
- Refresh token mechanism for security
- Role-based access control (User, CommunityAdmin)
- Google OAuth integration

### Frontend Auth Flow
- Login/Register forms with validation
- Automatic token refresh
- Protected routes with authentication guards
- Context-based auth state management

## 📊 Database Schema

### Core Tables
- `Users`: User accounts and profiles
- `Communities`: Neighborhood groups
- `CommunityMembers`: Many-to-many relationship
- `Items`: Shareable items
- `BorrowRequests`: Borrowing transactions
- `ItemRequests`: Requests for missing items
- `Notifications`: System alerts
- `Images`: File storage references

### Key Relationships
- User -> Community (many-to-many)
- User -> Item (one-to-many, as owner)
- User -> BorrowRequest (one-to-many, as borrower)
- Community -> Item (one-to-many)
- Item -> BorrowRequest (one-to-many)

## 🚀 API Endpoints Overview

### Authentication Endpoints
```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/google-login
POST /api/auth/refresh
POST /api/auth/logout
POST /api/auth/confirm-email
```

### Community Endpoints
```
GET /api/communities
POST /api/communities
GET /api/communities/{id}
PUT /api/communities/{id}
DELETE /api/communities/{id}
POST /api/communities/{id}/join
POST /api/communities/{id}/leave
GET /api/communities/{id}/members
```

### Item Endpoints
```
GET /api/items
POST /api/items
GET /api/items/{id}
PUT /api/items/{id}
DELETE /api/items/{id}
POST /api/items/{id}/images
GET /api/items/search
GET /api/communities/{id}/items
```

### Borrow Endpoints
```
GET /api/borrows
POST /api/borrows
GET /api/borrows/{id}
PUT /api/borrows/{id}/approve
PUT /api/borrows/{id}/return
PUT /api/borrows/{id}/cancel
GET /api/users/{id}/borrow-history
```

## 🎯 Key Features Implementation

### 1. Community Management
- Create communities with unique invitation codes
- Join communities via invitation links
- Community-scoped item sharing
- Community chat/message board

### 2. Item Sharing
- Add items with photos, descriptions, categories
- Track item availability and condition
- Search and filter items within community
- Request missing items

### 3. Borrowing System
- Request to borrow items
- Owner approval workflow
- Return tracking and confirmations
- Borrowing history and statistics

### 4. Reputation System
- Track successful borrows/lends
- Display user reputation scores
- Build trust within communities

### 5. Notifications
- Real-time updates for borrow requests
- Email notifications for important events
- In-app notification center
- Customizable notification preferences

## 🧪 Testing Strategy

### Backend Testing
- **Unit Tests**: Domain entities, value objects, services
- **Integration Tests**: API endpoints, database operations
- **BDD Tests**: User story scenarios with SpecFlow

### Frontend Testing
- **Component Tests**: React Testing Library
- **E2E Tests**: Cypress for user workflows
- **Type Safety**: Full TypeScript coverage

## 🔧 Development Setup

### Backend Setup
```bash
# Create solution and projects
dotnet new sln -n SasiadMa
dotnet new webapi -n SasiadMa.Api
dotnet new classlib -n SasiadMa.Core
dotnet new classlib -n SasiadMa.Application
dotnet new classlib -n SasiadMa.Infrastructure

# Add to solution
dotnet sln add src/SasiadMa.Api/SasiadMa.Api.csproj
dotnet sln add src/SasiadMa.Core/SasiadMa.Core.csproj
dotnet sln add src/SasiadMa.Application/SasiadMa.Application.csproj
dotnet sln add src/SasiadMa.Infrastructure/SasiadMa.Infrastructure.csproj
```

### Frontend Setup
```bash
# Create React app with Vite
npm create vite@latest frontend -- --template react-ts
cd frontend
npm install

# Add dependencies
npm install react-router-dom axios react-query react-hook-form react-hot-toast lucide-react
npm install -D tailwindcss postcss autoprefixer @types/node
npx tailwindcss init -p
```

### Database Setup
```bash
# Install EF Tools
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate --project src/SasiadMa.Infrastructure --startup-project src/SasiadMa.Api

# Update database
dotnet ef database update --project src/SasiadMa.Infrastructure --startup-project src/SasiadMa.Api
```

## 🚀 Deployment Considerations

### Docker Configuration
- Multi-stage Docker builds for .NET API
- Nginx for serving React frontend
- PostgreSQL container for development
- Docker Compose for local development

### Azure Deployment
- Azure App Service for API hosting
- Azure Static Web Apps for frontend
- Azure Database for PostgreSQL
- Azure Blob Storage for images

## 📋 Development Checklist

### Phase 1: Foundation
- [ ] Setup project structure
- [ ] Implement domain entities and value objects
- [ ] Create database schema and migrations
- [ ] Setup authentication with JWT
- [ ] Basic API endpoints (auth, users)
- [ ] React app structure and routing
- [ ] Authentication components and context

### Phase 2: Core Features
- [ ] Community management (create, join, manage)
- [ ] Item management (CRUD, image upload)
- [ ] Basic UI for communities and items
- [ ] Search and filtering functionality

### Phase 3: Borrowing System
- [ ] Borrow request workflow
- [ ] Approval and return processes
- [ ] Notification system
- [ ] Email notifications
- [ ] Borrowing history

### Phase 4: Enhancement
- [ ] Reputation system
- [ ] Community chat/messaging
- [ ] Advanced search and filters
- [ ] Mobile responsiveness
- [ ] Performance optimization

### Phase 5: Testing & Polish
- [ ] Comprehensive test coverage
- [ ] Error handling and validation
- [ ] Security hardening
- [ ] Documentation
- [ ] Deployment scripts

## 🎯 Success Metrics

### Technical Metrics
- 90%+ test coverage
- <200ms average API response time
- Mobile-first responsive design
- Accessibility compliance (WCAG 2.1)

### Business Metrics
- Community creation and growth
- Item sharing frequency
- User retention and engagement
- Successful borrow/return transactions

## 📝 Notes

This scaffold follows Domain-Driven Design principles with a focus on:
- Clean architecture separation
- Result pattern for error handling
- Manual mapping instead of AutoMapper
- Minimal API for lightweight endpoints
- React with TypeScript for type safety
- Open-source solutions only

The architecture is designed to be simple initially but scalable for future enhancements like CQRS and microservices migration.
