# Library Management System

A modern, feature-rich Library Management System built with ASP.NET Core Blazor. This application helps libraries manage their books, members, and borrowing operations efficiently.

## Features

- 📚 **Book Management**
  - Add, edit, and delete books
  - Track book availability
  - Manage book categories and locations
  - Search and filter books

- 👥 **Member Management**
  - Register and manage library members
  - Track member borrowing history
  - Manage membership status

- 🔄 **Borrowing System**
  - Process book rentals
  - Track due dates
  - Handle returns
  - Manage waiting lists

- 📧 **Email Notifications**
  - Rental confirmations
  - Due date reminders
  - Return confirmations

## Technology Stack

- ASP.NET Core 9.0
- Blazor Server
- Entity Framework Core
- SQL Server
- Bootstrap 5
- SendGrid (for email notifications)

## Prerequisites

- .NET 9.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
   ```bash
   git clone https://github.com/btelagathoti/LMS.git
   ```

2. Navigate to the project directory
   ```bash
   cd LibraryManagementSystem
   ```

3. Update the database connection string in `appsettings.json`

4. Run the database migrations
   ```bash
   dotnet ef database update
   ```

5. Run the application
   ```bash
   dotnet run
   ```

## Project Structure

```
LibraryManagementSystem/
├── Data/                 # Data access layer
│   ├── Models/          # Entity models
│   ├── Services/        # Business logic services
│   └── ApplicationDbContext.cs
├── Pages/               # Blazor pages
│   ├── Books/          # Book management pages
│   ├── Members/        # Member management pages
│   └── Catalog/        # Public catalog pages
└── Shared/             # Shared components
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

For any queries or support, please open an issue in the GitHub repository. 