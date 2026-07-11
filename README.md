# BlogApp 📝

A full-featured blog application built with ASP.NET Core MVC, Entity Framework Core, and Bootstrap. It supports role-based authentication, rich text editing, category management, and AJAX-powered comments.

---

## Features

### Public (Anyone)
- View all blog posts on the home page
- Filter posts by category
- Read full post details
- View comments on posts

### User (Logged In)
- Register and login
- Submit comments on posts via AJAX (no page reload)

### Admin
- Create, edit, and delete blog posts
- Upload feature images for posts
- Manage categories (create, edit, delete)
- Full access to all user features

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 9) |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5 (Bootswatch) |
| Rich Text Editor | TinyMCE |
| Comments | jQuery AJAX |

---

## Project Structure

```
BlogApp/
├── Controllers/
│   ├── PostController.cs        # Post CRUD + AJAX comments
│   ├── CategoryController.cs    # Category CRUD (Admin only)
│   └── AuthController.cs        # Login, Register, Logout
│
├── Models/
│   ├── Post.cs                  # Blog post model
│   ├── Category.cs              # Category model
│   └── Comment.cs               # Comment model
│
├── ViewModels/
│   ├── PostViewModel.cs         # Create post
│   ├── PostIndexViewModel.cs    # Post list + categories
│   ├── PostDetailViewModel.cs   # Post detail + comments
│   ├── PostDeleteViewModel.cs   # Delete post confirmation
│   ├── EditViewModel.cs         # Edit post
│   ├── LoginViewModel.cs        # Login form
│   └── RegisterViewModel.cs     # Register form
│
├── Services/
│   ├── PostService.cs           # Post business logic
│   ├── CategoryService.cs       # Category business logic
│   └── FileService.cs           # Image upload/delete
│
├── Interfaces/
│   ├── IPostService.cs
│   ├── ICategoryService.cs
│   └── IFileService.cs
│
├── Data/
│   └── ApplicationDbContext.cs  # EF Core DbContext
│
└── Views/
    ├── Post/                    # Index, Detail, Create, Edit, Delete
    ├── Category/                # Index, Create, Edit, Delete
    ├── Auth/                    # Login, Register, AccessDenied
    └── Shared/                  # _Layout, _Navbar
```

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

**1. Clone the repository:**
```bash
git clone https://github.com/Tahadar7/BlogApp.git
cd BlogApp
```

**2. Set up `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=BlogApp;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "AdminUser": {
    "Email": "your admin email",
    "Password": "your admin password"
  }
}
```

**3. Apply database migrations:**
```bash
dotnet ef database update
```

**4. Run the application:**
```bash
dotnet run
```

**5. Open in browser:**
```
https://localhost:7215
```

The admin account is created automatically on first run using the credentials from `appsettings.json`.

---

## Admin Setup

Admin credentials are configured in `appsettings.json` under `AdminUser`. On first launch, the app automatically:
1. Creates the `Admin` role if it doesn't exist
2. Creates the admin user with the configured credentials
3. Assigns the `Admin` role.

---

## Password Requirements

Passwords must meet the following requirements:
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character (e.g. `!@#$%`)

---

## TinyMCE Setup

The Create Post form uses TinyMCE for rich text editing. By default it runs with `no-api-key` which shows a warning popup.

To remove the warning:
1. Sign up for a free API key at [tiny.cloud](https://www.tiny.cloud/auth/signup)
2. Replace `no-api-key` in `Views/Post/Create.cshtml`:

```html
<script src="https://cdn.tiny.cloud/1/YOUR_API_KEY/tinymce/8/tinymce.min.js"></script>
```

---

## Database Schema

```
Posts
├── Id (PK)
├── Title
├── Content
├── Author
├── FeatureImagePath
├── PublishedDate
└── CategoryId (FK → Categories)

Categories
├── Id (PK)
├── Name
└── Description

Comments
├── Id (PK)
├── UserName
├── Content
├── CommentDate
└── PostId (FK → Posts)
```

---

## Key Design Decisions

- **ViewModel pattern** — every view uses a ViewModel, no direct model passing
- **Service layer** — business logic separated from controllers via interfaces
- **PRG pattern** — all POST actions redirect after success to prevent duplicate submissions
- **AJAX comments** — comments submit without page reload using jQuery AJAX
- **Role-based auth** — Admin and User roles with cookie-based authentication
- **EF Core tracking** — Edit action maps properties onto tracked entity to avoid duplicate tracking conflict

---
