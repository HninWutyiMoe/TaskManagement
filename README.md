# TaskManagement

# Features
- User Login & User management
- Task Header & Task Detail management
- File Upload & Download API
- RESTful API design
- Validation (Invalid Id , Invalid File , Empty Fields, File Size and Types, Wrong Credentials)
- Entity Framework Core integration
- Controller - Service - Repository Pattern
- Response HTTP status codes & validation

- # Tech Stack
- .NET Web API
- Entity Framework Core
- SQL Server
- Swagger (OpenAPI)

- # Project Structure
TaskManagementAPI_folder/
│── TaskManagement/
│── Controllers/
│── BAL Services/
│── Repositories/
│── Models/
    │── DTOs/ (AutoMapperDTOs for Entities) 
    │── Entities/ 
│── wwwroot/ (for uploaded files)
│── appsettings.json
# How to Run the Project
# Restore dependencies
dotnet restore
# Build the project
dotnet build  
dotnet run
API will run http://localhost:5000/swagger/index.html

# For Entity FrameWork Usage
# Install required packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Update Connection String
`appsettings.json`:
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TaskManagement;Trusted_Connection=True;TrustServerCertificate=True"
  },
# Create DbContext
# Create Entity Models
# Register DbContext (Program.cs)

# Database Schema
 Tables Overview
# 1. Department

| Column Name   | Data Type | Description |
|--------------|----------|------------|
| DepartmentId | UNIQUEIDENTIFIER (PK) | Primary Key |
| Name         | NVARCHAR | Department name |
| CreatedBy    | NVARCHAR | Created by user |
| CreatedAt    | DATETIME | Created date |
| UpdatedAt    | DATETIME | Updated date |

---

# 2. Role

| Column Name   | Data Type | Description |
|--------------|----------|------------|
| RoleId       | UNIQUEIDENTIFIER (PK) | Primary Key |
| DepartmentId | UNIQUEIDENTIFIER (FK) | Reference to Department |
| RoleName     | NVARCHAR | Role name |
| CreatedAt    | DATETIME | Created date |
| UpdatedAt    | DATETIME | Updated date |

---

# 3. User

| Column Name   | Data Type | Description |
|--------------|----------|------------|
| UserId       | UNIQUEIDENTIFIER (PK) | Primary Key |
| RoleId       | UNIQUEIDENTIFIER (FK) | Reference to Role |
| UserName     | NVARCHAR | Username |
| Email        | NVARCHAR | User email |
| PasswordHash | NVARCHAR | Hashed password |
| CreatedAt    | DATETIME | Created date |
| UpdatedAt    | DATETIME | Updated date |

---

# 4. TaskHeader

| Column Name            | Data Type | Description |
|----------------------|----------|------------|
| TaskId               | UNIQUEIDENTIFIER (PK) | Primary Key |
| AssignToDepartmentId | UNIQUEIDENTIFIER (FK) | Assigned department |
| TaskCode             | NVARCHAR | Task code |
| Title                | NVARCHAR | Task title |
| Description          | NVARCHAR | Task description |
| Priority             | INT      | Task priority |
| Status               | INT      | Task status |
| DueDate              | DATETIME | Due date |
| CreatedBy            | NVARCHAR | Created by |
| CreatedAt            | DATETIME | Created date |
| UpdatedAt            | DATETIME | Updated date |

---

# 5. TaskDetail

| Column Name   | Data Type | Description |
|--------------|----------|------------|
| TaskDetailId | UNIQUEIDENTIFIER (PK) | Primary Key |
| TaskId       | UNIQUEIDENTIFIER (FK) | Reference to TaskHeader |
| UserId       | UNIQUEIDENTIFIER (FK) | Assigned user |
| LineNumber   | NVARCHAR | Line number |
| ItemTitle    | NVARCHAR | Item title |
| ItemDescription | NVARCHAR | Item description |
| IsCompleted  | BIT      | Completion status |
| Remark       | NVARCHAR | Remarks |
| CreatedAt    | DATETIME | Created date |
| UpdatedAt    | DATETIME | Updated date |

---

# 6. UploadedFile

| Column Name   | Data Type | Description |
|--------------|----------|------------|
| FileId       | UNIQUEIDENTIFIER (PK) | Primary Key |
| TaskDetailId | UNIQUEIDENTIFIER (FK) | Reference to TaskDetail |
| FileUrl      | NVARCHAR | File path |
| FileName     | NVARCHAR | File name |
| ContentType  | NVARCHAR | MIME type |
| CreatedBy    | NVARCHAR | Uploaded by |
| CreatedAt    | DATETIME | Created date |
| UpdatedAt    | DATETIME | Updated date |

# Status Explanation

    public enum TaskStatus 
    {
        NotStarted = 1,
        InProgress = 2,
        Finished1 = 3,
    }
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
    }
---

# Relationships

- Department (1) → (Many) Role  
- Role (1) → (Many) User  
- Department (1) → (Many) TaskHeader  
- TaskHeader (1) → (Many) TaskDetail  
- User (1) → (Many) TaskDetail  
- TaskDetail (1) → (Many) UploadedFile  
# Implementation for File upload and download (Please check under Downloads\Documents)
📌 Overview
File management functionality including:
Upload file
Download file
Update file
Delete file
DeleteMultiple Files 
Retrieve file metadata

- #  File Restrictions
private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

- #  Upload File Steps
Create uploads folder if not exists
Generate unique filename using Guid
Save file to wwwroot/uploads
Store file metadata in database
- #  Download File
Get file record from database
Build full file path:
wwwroot/uploads/{filename}
Check if file exists
Return file path + metadata

- #  User APIs GetAll |GetByUserId| Create | Update | Dalete |

# Login (Token Generation)

| Method | Endpoint |
|--------|---------|
| POST | /api/auth/login |

# Request Body:
```json
{
  "email": "su@gmail.com",
  "password": "su@123"
}
## Response
{
  "message": "Login successful",
  "status": 0,
  "data": {
    "username": "Su Su",
    "email": "su@gmail.com",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI0MDFiOTE4YS1hNjE5LTRiNmEtOWU0My01YjYwMDcxNGMyYzMiLCJ1c2VyTmFtZSI6IlN1IFN1IiwiZW1haWwiOiJzdUBnbWFpbC5jb20iLCJyb2xlSWQiOiIwYTJjODViYy01NGQzLTRjOGEtODU3Mi01MDhiZGY3N2YzOTkiLCJuYmYiOjE3NzU5OTEzNTIsImV4cCI6MTc3NjAyMDE1MiwiaWF0IjoxNzc1OTkxMzUyfQ.0PPURqFBqAMBP-eSayBSAhpD-f8PBKx5DoswKNx-PUQ",
    "roleName": "Senior Developer"
  },
  "statusCode": 200
}
- 
  | Method | Endpoint       | Description    |
| ------ | -------------- | -------------- |
| GET    | /api/task      | Get all tasks  |
| GET    | /api/task/{id} | Get task by ID |
| POST   | /api/task      | Create task    |
| PUT    | /api/task/{id} | Update task    |
| DELETE | /api/task/{id} | Delete task    |
|DELETEMULTIPLE| /api/File/DeleteFilesByMultipleId | DELETE Multiple IDs|

- # API Endpoints

http://localhost:5000

/api/Authentication/login

/api/Department/GetAllDepartments
/api/Department/GetDepartmentById/{teamId}
/api/Department/CreateDepartment
/api/Department/UpdateDepartment/{id}
/api/Department/UpdateDepartment/{id}

/api/File/GetAllFiles
/api/File/GetFileById/{fileId}
/api/File/FileUpload
/api/File/DeleteFile
/api/File/DeleteFilesByMultipleId
/api/File/DownloadFile/{fileId}

/api/Role/GetAllRoles
/api/Role/GetAllRoles
/api/Role/CreateRole
/api/Role/DeleteRole/{roleId}

/api/TaskDetail/GetAllTaskDetails
/api/TaskDetail/GetTaskDetailById/{taskDetailId}
/api/TaskDetail/CreateTaskDetail
/api/TaskDetail/UpdateTaskDetail/{taskDetailId}
/api/TaskDetail/DeleteTaskDetail/{taskDetailId}

/api/TaskHeader/GetAllTaskHeaders
/api/TaskHeader/GetTaskHeaderById/{taskId}
/api/TaskHeader/CreateTaskHeader
/api/TaskHeader/UpdateTaskHeader/{taskId}
/api/TaskHeader/DeleteTaskHeader/{taskId}

/api/User/GetAllUsers
/api/User/GetUserById/{id}
/api/User/CreateUser
/api/User/UpdateUser/{userId}
/api/User/UpdateUser/{userId}
