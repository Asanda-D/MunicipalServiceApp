# PROG7312-POE-PART-1

# Municipal Services App

## Overview

The Municipal Services App is a web-based application built using ASP.NET Core MVC. It allows users to view local municipal events and announcements, while administrators can securely manage events through an admin interface.

The system demonstrates practical use of MVC architecture, session-based authentication, file uploads, and GitHub Actions continuous integration.

## Features

### General Users

- **View Events** – Browse upcoming and past municipal events.
- **View Attachments** – Access any uploaded files such as notices or images.
- **Responsive Design** – Mobile-friendly Bootstrap interface.

### Administrators

- **Add Events** – Upload event details with optional attachments.
- **Edit & Delete Events** – Manage events directly from the dashboard.
- **Secure Access** – Admin authentication using session management.

## Technologies Used

| Category | Technology |
|-----------|-------------|
| Framework | ASP.NET Core MVC (.NET 8.0) |
| Language | C# |
| UI Framework | Bootstrap 5 |
| Storage | In-Memory Collections (via `EventsService`) |
| Version Control | Git & GitHub |
| Continuous Integration | GitHub Actions |
| IDE | Visual Studio 2022 |

---


Run
Copy code
 ## Setup Instructions

 1. **Clone the Repository**  
    ```
    git clone https://github.com/YourUsername/MunicipalServicesApp.git
    cd MunicipalServicesApp
    ```

 2. **Open the Project**  
    Open the `.sln` file in Visual Studio 2022 (or later).

 3. **Restore NuGet Packages**  
    Visual Studio usually restores these automatically. If not, right-click the solution and select Restore NuGet Packages.

 4. **Create Uploads Folder**  
    Make sure this folder exists for file attachments:  
    ```
    wwwroot/uploads/
    ```

 5. **Run the Application**  
    Press F5 (Debug) or Ctrl + F5 (Without Debugging).  
    The site should launch automatically at:  
    ```
    https://localhost:7074
    ```

 6. **Admin Login**  
    If admin access is controlled via session, log in using the configured credentials or manually set:  
    ```
    HttpContext.Session.SetString("IsAdmin", "true");
    ```

 ## Folder Structure

 ```
 MunicipalServicesApp/
 ├─ Controllers/
 │   ├─ EventsController.cs
 │   └─ AdminController.cs
 │   └─ ReportIssuesController.cs
 ├─ Models/
 │   └─ Event.cs
 │   └─ Issue.cs
 ├─ Services/
 │   └─ EventsService.cs
 ├─ Views/
 │   ├─ ReportIssues/
 │   │   ├─ Create.cshtml
 │   │   ├─ list.cshtml
 │   ├─ Events/
 │   │   ├─ Index.cshtml
 │   │   ├─ Add.cshtml
 │   │   ├─ Edit.cshtml
 │   └─ Shared/
 │       └─ _Layout.cshtml
 ├─ wwwroot/
 │   ├─ css/
 │   ├─ js/
 │   └─ uploads/
 ├─ Program.cs
 └─ README.md
 ```

 ## Author

 Asanda Dimba  
 Web & Application Developer  
 Varsity College – Westville Campus
