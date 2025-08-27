# Todo App REST API

A complete Todo application REST API built with .NET Core and SQL Server, featuring JWT authentication, CRUD operations, and best practices implementation.

## Features

### Authentication
- **User Registration** (`POST /api/auth/signup`) - Create new user accounts
- **User Login** (`POST /api/auth/login`) - Authenticate users and return JWT tokens

### Protected Todo Operations (JWT Required)
- **List Todos** (`GET /api/todos`) - Retrieve user's todos with pagination
- **Get Todo** (`GET /api/todos/{id}`) - Retrieve specific todo by ID
- **Create Todo** (`POST /api/todos`) - Create a new todo item
- **Update Todo** (`PUT /api/todos/{id}`) - Modify existing todo item
- **Delete Todo** (`DELETE /api/todos/{id}`) - Remove todo item

## Technical Implementation

### Architecture & Patterns
- **JWT Authentication** - Secure token-based authentication
- **Repository Pattern** - Generic repository implementation for data access
- **Entity Framework Core** - Code-first approach with SQL Server
- **AutoMapper** - Object mapping between DTOs and entities
- **Global Exception Handling** - Custom middleware for consistent error management

### Database
- **SQL Server** - Primary database system
- **Code-First Migrations** - Automatic database schema updates on application startup
- **User Isolation** - Data separation between users

### Validation & Security
- **Input Validation** - Comprehensive validation of all API request models
- **Pagination** - Efficient data retrieval for todo listings
- **Error Handling** - Consistent error response format
- **Protected Endpoints** - JWT-secured API endpoints

## Prerequisites

Before running this application, ensure you have the following installed:
- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or full version)
- Git

## Setup Instructions

### 1. Clone the Repository
```bash
git clone <repository-url>
cd TodoApp

2. Database Configuration
Update the connection string in appsettings.json:
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TodoAppDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
3. Apply Database Migrations
The application uses automatic migrations. On first run, the database will be created and seeded with necessary schema.
4. Run the Application
bash
dotnet run
The API will be available at https://localhost:7000 (or http://localhost:5000 depending on your configuration).
API Usage
1. Register a New User
Endpoint: POST /api/auth/signup
Request Body:
json
{
  "email": "user@example.com",
  "password": "YourPassword123!",
  "confirmPassword": "YourPassword123!"
}
2. Login
Endpoint: POST /api/auth/login
Request Body:
json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
Response:
json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires": "2023-12-31T23:59:59Z"
}
3. Access Protected Endpoints
Include the JWT token in the Authorization header:
text
Authorization: Bearer <your-token>
4. Todo Operations Examples
Create a Todo:
bash
POST /api/todos
Authorization: Bearer <your-token>
Content-Type: application/json

{
  "title": "Buy groceries",
  "description": "Milk, Eggs, Bread",
  "dueDate": "2023-12-25"
}
Get Todos with Pagination:
bash
GET /api/todos?page=1&pageSize=10
Authorization: Bearer <your-token>
Project Structure
text
TodoApp/
├── Controllers/          # API controllers
├── Data/                # Database context and entities
├── DTOs/               # Data transfer objects
├── Interfaces/          # Repository interfaces
├── Middleware/          # Custom middleware
├── Models/             # Domain models
├── Repository/          # Repository implementations
├── Services/           # Business logic services
└── Utilities/          # Helpers and extensions
Postman Collection
A Postman collection is included in the repository at /postman/TodoApp.postman_collection.json. Import this collection to quickly test all API endpoints.

