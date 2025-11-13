# PROG7312-POE-PART-3

# Municipal Services App

## Overview

The Municipal Services App is a web-based application built using ASP.NET Core MVC. It allows users to view local municipal events and announcements, while administrators can securely manage events through an admin interface.

The system demonstrates practical use of MVC architecture, session-based authentication, file uploads, and GitHub Actions continuous integration.

## Features

### General Users

- **View Events** – Browse upcoming and past municipal events.
- **View Attachments** – Access any uploaded files such as notices or images.
- **Track Service Requests** – Check the status of submitted municipal requests.
- **Responsive Design** – Mobile-friendly Bootstrap interface.

### Administrators

- **Add Events** – Upload event details with optional attachments.
- **Edit & Delete Events** – Manage events directly from the dashboard.
- **Secure Access** – Admin authentication using session management.
- **File Uploads** – Add attachments such as PDFs or images to events.

## Technologies Used

| Category               | Technology |
|------------------------|------------|
| Framework              | ASP.NET Core MVC (.NET 8.0) |
| Language               | C# |
| UI Framework           | Bootstrap 5 |
| Data Storage           | In-Memory Collections (`EventsService`, `RequestsService`) |
| Data Structures        | Binary Search Tree, Graph, Min Heap |
| Version Control        | Git & GitHub |
| Continuous Integration | GitHub Actions |
| IDE                    | Visual Studio 2022 |

---

 ## Setup Instructions

 1. **Clone the Repository**  
    ```
    git clone https://github.com/Asanda-D/MunicipalServiceApp.git
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
    Admin access is controlled via session, log in using the configured credentials or manually set:  
    ```
    HttpContext.Session.SetString("IsAdmin", "true");
    ```

---

## Data Structures and Their Role

The Service Request Status feature relies on custom data structures to provide efficient storage, search, and recommendation functionality.

A. **Binary Search Tree (BST)**

Stores service requests or events sorted by date or ID.

Allows O(log n) search for quick retrieval of a specific request.

**Example:**
```
// Find a request by ID
var request = requestsBST.Find(requestId);
```

**Role:** Quickly locate requests without scanning the entire list.

B. **Graph**

Represents relationships between requests or dependencies (e.g., linked issues or related events).

Useful for recommendation logic and navigation between connected entities.

**Example:**
```
var connectedRequests = requestsGraph.GetNeighbors(requestId);
```

**Role:** Efficiently traverse related requests for insights or recommendations.

C. **Min Heap / Priority Queue**

Maintains upcoming requests or events sorted by priority or date.

Supports O(1) access to the highest-priority request and O(log n) insertion.

**Example:**
```
var nextRequest = minHeap.Peek(); // returns request with earliest due date
```

**Role:** Shows urgent requests first, enhancing user experience and admin workflow.

D. **Dictionary / Hash Table**

Maps categories or request types to a list of corresponding events or requests.

Provides O(1) lookup by category.

**Example:**
```
var financeRequests = requestsByCategory["Finance"];
```

**Role:** Efficient filtering and recommendations.

E. **Queue and Stack**

**Queue:** Tracks recent searches in FIFO order.

**Stack:** Allows undoing last search (LIFO).

**Example:**
```
var lastSearch = searchStack.Pop();  // Undo last search
var recentSearch = recentSearchQueue.Peek();  // Most recent search
```

**Role:** Improves user experience and enables recommendation scoring.

---

 ## Folder Structure

 ```
 MunicipalServicesApp/
 ├─ .github/
 │   ├─ workflows/
 │       └─ ci.yml
 ├─ Controllers/
 │   ├─ HomeController.cs
 │   ├─ MainMenuController.cs
 │   ├─ EventsController.cs
 │   ├─ AdminController.cs
 │   ├─ ReportIssuesController.cs
 │   └─ ServiceRequestsController.cs
 ├─ Models/
 │   ├─ Event.cs
 │   ├─ Issue.cs
 │   └─ ServiceRequest.cs
 ├─ Services/
 │   ├─ DataStructures/
 │   │   ├─ BinarySearchTree.cs
 │   │   ├─ Graph.cs
 │   │   └─ MinHeap.cs
 │   ├─ EventsService.cs
 │   └─ RequestsService.cs
 ├─ Views/
 │   ├─ Admin/
 │   │   └─ Login.cshtml
 │   ├─ Events/
 │   │   ├─ Index.cshtml
 │   │   ├─ Add.cshtml
 │   │   ├─ Edit.cshtml
 │   │   └─ _Recommendations.cshtml
 │   ├─ Home/
 │   │   └─ Index.cshtml
 │   ├─ MainMenu/
 │   │   └─ Index.cshtml
 │   ├─ ReportIssues/
 │   │   ├─ Create.cshtml
 │   │   └─ list.cshtml
 │   ├─ ServiceRequests/
 │   │   ├─ Create.cshtml
 │   │   ├─ Delete.cshtml
 │   │   ├─ Details.cshtml
 │   │   ├─ Edit.cshtml
 │   │   ├─ Index.cshtml
 │   │   └─ Track.cshtml
 │   └─ Shared/
 │       └─ _Layout.cshtml
 ├─ wwwroot/
 │   ├─ css/
 │   ├─ js/
 │   └─ uploads/
 ├─ Program.cs
 └─ README.md
 ```
---

## Usage Examples

**Searching Service Requests**
- By keyword: Searches titles or descriptions.
- By category: Quickly filter requests using hash table.
- By date: Efficiently search using BST or sorted dictionary.

**Recommendations**
- Uses category frequency + upcoming date.
- PriorityQueue/MinHeap selects top requests or events to show first.

**Admin Actions**
- Add, edit, or delete requests/events.
- File uploads stored in wwwroot/uploads/.
- Session-based authentication ensures security.

 ## Author

 Asanda Dimba  
 Web & Application Developer  
 Varsity College – Westville Campus
