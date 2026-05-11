# AI Task Tracker API

AI Task Tracker API is a backend project developed with ASP.NET Core Web API.  
The project allows users to manage tasks, daily learning logs, and learning topics. It also includes AI-powered features such as text summarization and quiz generation using Google's Generative AI API.

This project was developed as a learning and portfolio project to practice backend development, layered architecture, authentication, authorization, database operations, and AI API integration.

---

## Features

- User registration and login
- JWT-based authentication
- BCrypt password hashing
- Protected API endpoints
- Task management
- Daily learning log management
- Learning topic management
- Dashboard summary endpoint
- User-based data isolation
- AI-powered text summarization
- AI-powered quiz generation
- Standard API response format
- Global exception handling
- SQL Server database integration
- Entity Framework Core migrations
- Swagger API documentation

---

## Tech Stack

- ASP.NET Core Web API
- C#
- .NET 9
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt.Net
- Swagger / Swashbuckle
- Google Generative AI API
- Gemini / Gemma model integration

---

## Architecture

The project follows a layered architecture approach.

```text
Controllers
    ↓
Services
    ↓
Repositories
    ↓
AppDbContext
    ↓
SQL Server
```

### Controller Layer

Controllers handle HTTP requests and responses.  
They validate incoming requests, read the logged-in user information from JWT claims, and call the related service methods.

### Service Layer

Services contain the business logic of the application.  
They handle operations such as creating, updating, deleting, mapping entities to DTOs, and applying user-based rules.

### Repository Layer

Repositories are responsible for database operations.  
They use Entity Framework Core to communicate with SQL Server.

### DTO Layer

DTOs are used to separate internal database entities from external API request and response models.

---

## Main Modules

### Authentication

The authentication module allows users to register and login.  
Passwords are hashed using BCrypt before being stored in the database.

After a successful login, the API returns a JWT token. This token is required to access protected endpoints.

### Tasks

Users can create, list, update, and delete their own tasks.

Each task belongs to a specific user.  
A user cannot access another user's task even if they know the task id.

### Daily Logs

Users can create daily learning logs to track what they studied or practiced each day.

Daily logs are also user-specific.

### Learning Topics

Users can save technical topics they have learned, such as:

- JWT Authentication
- Entity Framework Core
- Repository Pattern
- Gemini API Integration

Learning topics can be grouped by category.

### Dashboard

The dashboard endpoint returns a summary of the logged-in user's progress.

It includes:

- Total tasks
- Completed tasks
- Pending tasks
- Total daily logs
- Total learning topics
- Learning topics grouped by category

### AI Features

The project includes AI-powered endpoints.

#### Text Summarization

Users can send a learning log or any text, and the API returns a short AI-generated summary.

#### Quiz Generation

Users can send a topic, and the API generates a multiple-choice quiz using AI.

The AI response is parsed into structured DTOs and returned as JSON.

---

## User-Based Data Isolation

The API uses JWT claims to identify the logged-in user.

For user-owned resources, the system filters records by `UserId`.

Example:

```text
GET /api/Tasks
```

This endpoint only returns the tasks of the currently logged-in user.

For single-record operations, both `Id` and `UserId` are checked.

Example:

```text
GET /api/Tasks/5
```

The API checks:

```text
Id = 5
UserId = Logged-in user's id
```

If the record belongs to another user, the API returns `404 Not Found`.

---

## API Endpoints

### Auth

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/register` | Register a new user |
| POST | `/api/Auth/login` | Login and receive JWT token |

### Tasks

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Tasks` | Get logged-in user's tasks |
| GET | `/api/Tasks/{id}` | Get a task by id |
| POST | `/api/Tasks` | Create a task |
| PUT | `/api/Tasks/{id}` | Update a task |
| DELETE | `/api/Tasks/{id}` | Delete a task |

### Daily Logs

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/DailyLogs` | Get logged-in user's daily logs |
| GET | `/api/DailyLogs/{id}` | Get a daily log by id |
| POST | `/api/DailyLogs` | Create a daily log |
| PUT | `/api/DailyLogs/{id}` | Update a daily log |
| DELETE | `/api/DailyLogs/{id}` | Delete a daily log |

### Learning Topics

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/LearningTopics` | Get logged-in user's learning topics |
| GET | `/api/LearningTopics/{id}` | Get a learning topic by id |
| POST | `/api/LearningTopics` | Create a learning topic |
| PUT | `/api/LearningTopics/{id}` | Update a learning topic |
| DELETE | `/api/LearningTopics/{id}` | Delete a learning topic |

### Dashboard

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Dashboard/summary` | Get logged-in user's dashboard summary |

### AI

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Ai/summarize` | Summarize text using AI |
| POST | `/api/Ai/generate-quiz` | Generate quiz using AI |

---

## Example Requests

### Register

```json
{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "123456"
}
```

### Login

```json
{
  "email": "test@example.com",
  "password": "123456"
}
```

Example response:

```json
{
  "success": true,
  "message": "Login successful.",
  "data": {
    "userId": 1,
    "fullName": "Test User",
    "email": "test@example.com",
    "role": "User",
    "token": "jwt-token-value"
  }
}
```

### Create Task

```json
{
  "title": "Learn JWT Authentication",
  "description": "Practice token generation and protected endpoints."
}
```

### Create Daily Log

```json
{
  "content": "Today I practiced user-based data isolation in ASP.NET Core Web API.",
  "logDate": "2026-05-11T10:00:00Z"
}
```

### Create Learning Topic

```json
{
  "name": "Repository Pattern",
  "category": "Architecture",
  "notes": "I learned how to separate database operations from business logic."
}
```

### Summarize Text

```json
{
  "text": "Today I learned JWT authentication, repository pattern, service layer and user-based data filtering."
}
```

### Generate Quiz

```json
{
  "topic": "ASP.NET Core Web API Controllers"
}
```

---

## Standard API Response Format

All successful and failed responses follow a standard response format.

### Success Response

```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": {}
}
```

### Error Response

```json
{
  "success": false,
  "message": "Error message.",
  "data": null
}
```

---

## Configuration

The project uses `appsettings.json` for configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AITaskTrackerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your-development-secret-key",
    "Issuer": "AITaskTrackerAPI",
    "Audience": "AITaskTrackerClient",
    "ExpirationMinutes": 60
  },
  "GeminiSettings": {
    "ApiKey": "",
    "Model": "gemma-3-1b-it"
  }
}
```

For security reasons, API keys should not be committed to GitHub.

Use environment variables for sensitive values.

Example:

```bash
export GeminiSettings__ApiKey="your-api-key"
```

---

## Database Setup

The project uses SQL Server with Entity Framework Core.

Run migrations:

```bash
dotnet ef database update
```

If you need to create a new migration:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

---

## How to Run

Clone the repository:

```bash
git clone https://github.com/berataltinsuyu/AI-TaskTracker-API.git
```

Navigate to the project folder:

```bash
cd AI-TaskTracker-API/AITaskTracker.API
```

Restore packages:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the project:

```bash
dotnet run
```

Open Swagger in your browser:

```text
https://localhost:{port}/swagger
```

or

```text
http://localhost:{port}/swagger
```

---

## Authentication Flow

1. Register a user.
2. Login with email and password.
3. Copy the JWT token from the login response.
4. Click the Authorize button in Swagger.
5. Paste the token.
6. Use protected endpoints.

---

## Project Purpose

This project was built to practice and demonstrate:

- ASP.NET Core Web API development
- Clean and layered backend architecture
- Entity Framework Core usage
- SQL Server integration
- JWT authentication
- Secure password hashing
- Repository and service patterns
- User-based authorization logic
- AI API integration
- API documentation with Swagger

---

## Future Improvements

Possible future improvements:

- Refresh token support
- Role-based authorization
- FluentValidation integration
- Unit tests
- Integration tests
- Docker support
- CI/CD pipeline
- Frontend application
- More advanced AI learning assistant features
