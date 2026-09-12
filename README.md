# 🚗 AutoHub.API - Automotive Management RESTful API

**AutoHub.API** is a scalable, highly secure backend Web API developed using **ASP.NET Core (.NET 8)** to power modern automotive dealership platforms. It provides full control over user authentication, role management, vehicle inventory, dynamic search & filtering, and media upload management.

---

## 🔥 Key Technical Highlights

* **Authentication & Authorization**:
  * Implements **ASP.NET Core Identity** with custom **JWT (JSON Web Tokens)** for stateless authentication.
  * **Role-Based Access Control (RBAC)** securing endpoints specifically for `Admin`, `Dealer`, and `Customer` roles.
  
* **Architecture & Design Patterns**:
  * **Unit of Work & Repository Pattern** for decoupling data access logic and optimizing database transactions.
  * **AutoMapper** integration for seamless object-to-object mapping between Entities and DTOs.
  
* **Advanced Querying & Performance**:
  * **Deferred Execution Querying** (`IQueryable`) for dynamic filtering, text search, multi-column sorting, and server-side pagination.
  * Optimized image handling supporting modern formats (`.webp`, `.png`, `.jpg`) with file system validation, size limits (5MB), and cascading storage management.

---

## 🛠️ Tech Stack

* **Framework**: .NET 8 (ASP.NET Core Web API)
* **Database & ORM**: Entity Framework Core, SQL Server
* **Identity & Security**: ASP.NET Core Identity, JWT Bearer Authentication
* **Mapping**: AutoMapper
* **Documentation**: Swagger / OpenAPI

---

## 📌 API Endpoints Overview

### 🔐 Authentication & User Management (`/api/Account`)
* `POST /api/Account/register` — Register new users and automatically assign roles.
* `POST /api/Account/login` — Authenticate and issue JWT tokens with embedded claims.
* `GET /api/Account/users` — Retrieve all registered users with their assigned roles `[Admin]`.
* `PUT /api/Account/users/{id}` — Update user profile and role allocations `[Admin]`.
* `DELETE /api/Account/users/{id}` — Remove user accounts `[Admin]`.

### 🚘 Vehicle Management (`/api/Cars`)
* `GET /api/Cars` — Fetch paginated list of cars with dynamic search, filtering, and sorting parameters `[Public]`.
* `GET /api/Cars/{id}` — Retrieve detailed car information including image gallery `[Public]`.
* `POST /api/Cars` — Add new vehicle listing `[Admin, Dealer]`.
* `PUT /api/Cars/{id}` — Update existing vehicle details `[Admin, Dealer]`.
* `DELETE /api/Cars/{id}` — Delete a vehicle record `[Admin]`.

### 🖼️ Image Management (`/api/CarImages`)
* `POST /api/CarImages/upload/{carId}` — Upload single or multiple images for a car `[Admin, Dealer]`.
* `PUT /api/CarImages/{imageId}` — Replace an existing car image on disk and database `[Admin, Dealer]`.
* `DELETE /api/CarImages/{imageId}` — Remove image record and purge file from local storage `[Admin, Dealer]`.

---

## 🚀 Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/)

### Configuration

1. Clone the repository:

Update appsettings.json with your Database Connection String and JWT settings:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AutoHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_HERE_MIN_32_CHARS",
    "Issuer": "AutoHub.API",
    "Audience": "AutoHub.Clients"
  }
}
Run EF Core Migrations & Start the API:

Bash
dotnet ef database update
dotnet run
👨‍💻 Author
Developed by Omar Mahmoud Hassani

Software Engineer & Backend Specialist


<ElicitationsGroup message="كيف تفضل متابعة إعداد مشروع AutoHub.API؟">
  <Elicitation label="حماية المفاتيح الحساسة عبر User Secrets" query="كيف أستخدم .NET User Secrets لإخفاء مفتاح JWT ConnectionString أثناء التطوير؟"/>
  <Elicitation label="إضافة Swagger Authentication Button" query="كيف أضبط SwaggerUI في Program.cs ليدعم إدخال JWT Bearer Token مباشرة أثناء التجربة؟"/>
</ElicitationsGroup>

   ```bash
   git clone [https://github.com/YOUR_USERNAME/AutoHub.API.git](https://github.com/YOUR_USERNAME/AutoHub.API.git)
   cd AutoHub.API
