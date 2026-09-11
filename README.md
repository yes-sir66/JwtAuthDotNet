# JwtAuthDotNet
# JWT Authentication API - ASP.NET Core
A backend authentication API built with ASP.NET Core to learn and implement secure authentication and authorization using JSON Web Tokens (JWT).

## What I Learned

Through this project, I learned how to build an authentication system from the ground up using ASP.NET Core.

### Authentication & Authorization
- Implemented user registration and login
- Learned the difference between authentication and authorization
- Implemented JWT-based authentication
- Generated and validated JWT access tokens
- Protected API endpoints using `[Authorize]`
- Learned how claims are stored inside JWT tokens
- Implemented role-based authorization

### Password Security
- Learned why passwords should never be stored as plain text
- Implemented password hashing using BCrypt
- Learned how password hashes are verified during login

### Refresh Tokens
- Implemented refresh token functionality
- Learned why access tokens should have a limited lifetime
- Used refresh tokens to generate new access tokens
- Learned how refresh tokens can be stored and managed securely

### ASP.NET Core
- Learned how authentication middleware works
- Configured JWT authentication in `Program.cs`
- Worked with Dependency Injection
- Applied the Service/Repository-style separation of responsibilities
- Created DTOs to control the data exposed by API endpoints
- Built REST API endpoints for authentication
- Tested API endpoints using Swagger

### Database & Entity Framework Core
- Connected the API to PostgreSQL
- Used Entity Framework Core for database operations
- Created database migrations
- Designed relationships between users and refresh tokens
- Learned how authentication data is persisted in a database

### Security & Configuration
- Learned how to keep sensitive information such as database passwords and JWT signing keys out of source control
- Used ASP.NET Core User Secrets for local development
- Used configuration files for non-sensitive application settings
- Learned why secrets should never be committed to GitHub

## Technologies Used

- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT
- BCrypt
- Swagger / OpenAPI
- Git & GitHub

## Main Authentication Flow

1. User registers with a username, email, and password.
2. Password is hashed using BCrypt before being stored.
3. User logs in with their credentials.
4. The API verifies the password hash.
5. A JWT access token and refresh token are generated.
6. The client sends the JWT with protected API requests.
7. ASP.NET Core validates the JWT and its claims.
8. When the access token expires, the refresh token can be used to obtain a new access token.

## Project Goal

The main goal of this project was to gain practical experience with authentication, authorization, JWT, password security, refresh tokens, Entity Framework Core, PostgreSQL, and building secure ASP.NET Core Web APIs.