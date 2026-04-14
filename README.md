# Student Information System — C# Backend + JSON Database

## Project Structure

```
SIS_Backend/
│
├── SIS.Api/                        ← C# ASP.NET Core Web API
│   ├── SIS.Api.csproj
│   ├── Program.cs                  ← App entry point, CORS, DI
│   ├── appsettings.json            ← Port config (5000)
│   ├── Models/
│   │   └── Models.cs               ← Student, User, LoginRequest/Response
│   ├── Services/
│   │   └── JsonDatabase.cs         ← Flat-file JSON database service
│   └── Controllers/
│       ├── StudentsController.cs   ← GET/POST/PUT/DELETE /api/students
│       └── AuthController.cs       ← POST /api/auth/login
│
├── frontend/                       ← Original HTML/CSS/JS UI (unchanged)
│   ├── index.html                  ← Login page
│   ├── dashboard.html
│   ├── add.html
│   ├── view.html
│   ├── index.css
│   └── index.js                    ← Connects to C# API via fetch()
│
├── start-api.bat                   ← Windows: double-click to start
└── README.md
```

## JSON Database Files

The data files are created automatically the **first time** the API runs,
stored right next to the compiled binary:

```
SIS.Api/bin/Debug/net8.0/
├── students.json     ← all student records
└── users.json        ← admin user accounts (seeded with default admin)
```

They are plain, human-readable JSON — you can open and edit them directly.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## How to Run

### Option 1 — Double-click (Windows)
Run `start-api.bat`

### Option 2 — Command line
```bash
cd SIS.Api
dotnet run
```

The API starts on **http://localhost:5000**

## Open the Frontend

Open `frontend/index.html` in your browser.
The API must be running for the frontend to work.

## Default Login

| Username | Password  |
|----------|-----------|
| admin    | admin123  |

## API Endpoints

| Method | Endpoint               | Description            |
|--------|------------------------|------------------------|
| POST   | /api/auth/login        | Login, returns token   |
| GET    | /api/students          | Get all students       |
| GET    | /api/students/{id}     | Get student by ID      |
| POST   | /api/students          | Add new student        |
| PUT    | /api/students/{id}     | Update student         |
| DELETE | /api/students/{id}     | Delete student         |

## Student JSON Schema

```json
{
  "id": 1,
  "name": "Juan Dela Cruz",
  "course": "BSIT",
  "year": "2nd Year",
  "record": "2024-00123",
  "age": 19,
  "parentName": "Maria Dela Cruz",
  "parentContact": "09171234567",
  "createdAt": "2026-04-13T10:00:00Z"
}
```
