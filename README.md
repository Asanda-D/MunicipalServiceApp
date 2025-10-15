# PROG7312-POE-PART-1

# Municipal Services App

## Overview
The **Municipal Services App** is a web-based application built with **ASP.NET Core MVC** that allows citizens to easily report community issues such as sanitation, roads, and utilities. The app empowers municipalities to manage reports efficiently while maintaining clear communication with citizens.  

The application emphasizes **dignity, respect, and efficiency**—ensuring that municipal workers are supported while encouraging citizens to engage responsibly and courteously.

---

## Features

- **Report Issues**  
  Users can submit issues with details such as location, category, description, and optional attachments.

- **View Submissions**  
  Users can see a list of previously submitted issues in a structured format.

- **Category Management**  
  Predefined categories help users classify their reports correctly:  
  `Sanitation`, `Roads`, `Utilities`, `Water Supply`, `Electricity`, `Public Safety`, `Traffic`, `Community Services`, `Parks & Recreation`, `Other`.

- **Attachment Support**  
  Optional image or document uploads for more detailed reporting.

- **Responsive Layout**  
  Modern, Bootstrap-based interface that works across devices.

- **In-Memory Storage**  
  Issues are temporarily stored in memory for quick testing and demonstration purposes.

---

## Technologies Used

- **ASP.NET Core MVC** – Web application framework for building the front-end and back-end.
- **C#** – Server-side logic and controllers.
- **Bootstrap 5** – Responsive UI components.
- **HTML, CSS** – Layout and styling, including custom `site.css`.
- **Google Fonts** – Typography with `Poppins`.
- **IWebHostEnvironment** – File handling for attachments.

---

## Setup Instructions

1. **Clone the repository**
`` git clone https://github.com/YourUsername/MunicipalServicesApp.git
`` 
2. **Open in Visual Studio 2022**
- Open the solution file .sln from the cloned folder.

3. **Restore NuGet Packages**
- Visual Studio usually restores automatically. Otherwise, right-click the solution → Restore NuGet Packages.

4. **Run the Application**
- Press F5 or Ctrl+F5 to launch in your browser.

5. **Optional: Add attachments folder**
- Create a folder wwwroot/Uploads to store uploaded files.

---

## Folder Structure

MunicipalServicesApp/
├─ Controllers/
│   └─ ReportIssuesController.cs
├─ Models/
│   └─ Issue.cs
├─ Views/
│   ├─ ReportIssues/
│   │   ├─ Create.cshtml
│   │   └─ List.cshtml
│   └─ Shared/
│       └─ _Layout.cshtml
├─ wwwroot/
│   ├─ css/
│   │   └─ site.css
│   ├─ js/
│   │   └─ site.js
│   └─ Uploads/
├─ Program.cs
└─ README.md

---

## Author
- **Asanda Dimba**
- **Web & Application Developer**
- **Varsity College Westville Student**
