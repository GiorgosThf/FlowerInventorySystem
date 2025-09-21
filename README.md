
# FlowerInventorySystem

## Description of Implementation

The FlowerInventorySystem is implemented as a modular ASP.NET Core 9 Razor Pages application with clean separation of concerns,
persistence through SQL Server, object storage through MinIO, and unit testing via xUnit.

## Web Application (FlowerInventory.Web)

Razor Pages are used as the UI layer, simplifying page-based development. Pages interact directly with services for CRUD operations.

- GlobalExceptionFilter provides centralized error handling:

  - Logs unhandled exceptions.

  - Returns user-friendly error messages (different between Development/Production).

  - Uses TempData to surface error titles and IDs for debugging.
  
- PageModelExtensions.RunWithToastAsync wraps any async action:

    - Executes the business logic.

    - On success: sets TempData["SuccessTitle"] and TempData["SuccessMessage"], which the frontend renders as a toast popup.

    - On failure: sets TempData["ErrorTitle"], TempData["ErrorMessage"], and a correlation ErrorId (from HttpContext.TraceIdentifier).

   - Always performs a redirect after post (Post-Redirect-Get), preventing duplicate submissions and ensuring toast messages are shown once.

   - This makes user interaction consistent:
     - Every operation yields immediate visual feedback (toast success/error).
     - Users are redirected back to the correct page (/Index by default). 
     - Errors are logged and traced without exposing raw exceptions to the user.

- Static Files (CSS/JS/images) are served from the wwwroot/ directory using app.UseStaticFiles().


## Database Integration (AppDbContext)

Built with Entity Framework Core, configured for SQL Server via UseSqlServer.

### Domain models:

- Category: represents flower categories, with validation, indexing, and default values.

- Flower: represents individual flowers, with validation, unique SKU, foreign key to Category, and auto-included navigation.

Migrations ensure schema is applied (db.Database.Migrate()).

DbInitializer (optional in dev mode) seeds data if the init.sql approach is not used.

## Repository & Service Layer

Implements a classic Repository + Service pattern for separation of concerns:

- BaseRepository<T>: generic EF Core repository with async CRUD (GetAllAsync, FindAsync, CreateAsync, etc.).

- BaseService<T>: wraps repositories and adds logging, providing a consistent service layer for controllers/pages.

- FlowerRepository / CategoryRepository and FlowerService / CategoryService extend the base abstractions to manage domain-specific logic.

## Object Storage with MinIO

Configured via MinioOptions bound from appsettings.json.

- MinioStorageService implements IFileStorage:

  - Uploads, deletes, and lists files in MinIO buckets.

  - Ensures buckets exist before use (EnsureBucketAsync).

  - Generates public URLs for gallery display.

#### createbuckets init service in docker-compose automatically creates a bucket (images) and seeds it with demo content from seed_images/.

## Infrastructure & Docker Support

 docker-compose.yml provisions:

### SQL Server 2025 Developer Edition:

  - Mounted volume for persistence.

   - Mounts init.sql on startup to prepare schema/data. (It can be later executed)

   - Health checks to ensure readiness.

### MinIO (S3-compatible object storage):

   - Accessible on ports 9000 (API) and 9001 (console).

   - Health checks to ensure availability.

### MinIO Client (mc): 
  - creates default buckets (images) and uploads seed files.

### Volumes: 
- persistent volumes for SQL Server (mssql_data) and MinIO (minio_data).

## Logging & Monitoring

BaseComponent<T> provides a static logger per class using LoggerFactory.

### Logs:

  - Repository/service operations (CRUD calls).

  - Exception filter captures and logs errors with correlation IDs (Activity.Current.Id).

## Testing (FlowerInventory.Tests)

- xUnit framework with Microsoft.NET.Test.Sdk and xunit.runner.visualstudio.
#### Running the Tests
````bash
dotnet test
````

## Project Structure
````
FlowerInventorySystem/
├── FlowerInventory.Web/           # ASP.NET Core web project
│   ├── src/                       # Contains the sourced of the app
│   │   ├── Configuration/         # Contains the configuration classes
│   │   ├── Dto/                   # Contains the dto classes
│   │   ├── Model/                 # Contains the model classes
│   │   ├── Repository/            # Contains the repository classes
│   │   ├── Service/               # Contains the service classes
│   │   └── Utils/                 # Contains the utils classes
│   ├── Pages/                     # Contains the Razor pages
│   │   ├── Categories/            # Contains the categories crud pages
│   │   ├── Flowers/               # Contains the flowers crud pages
│   │   ├── Media/                 # Contains the media crud pages
│   │   ├── Shared/                # Contains the shared components
│   │   └── Utils/                 # Contains the util components  
│   ├── docker-compose.yaml        # The docker file containing the sql server and minio
│   ├── init.sql                   # The sql file to create and seed the db
│   ├── Program.cs                 # The main program class.
│   └── seed_images/               # The folder containing the default images.
│
├── FlowerInventory.Tests/         # xUnit test project.
│   └── src/                       # Contains the tests.
│       └── Service/               # Contains the services tests.
│
├── FlowerInventorySystem.sln      # Solution file
└── README.md                      # Documentation

````
## Prerequisites

- .NET 9 SDK

- Docker Desktop

- SQL Server (local or container)

- MinIO (contairized)

#### docker-compose.yaml --> handles the initiation of both sql server and MinIO

## Application Setup

### 1) Starting the SqlServer and Minio containers

````bash
docker compose up -d
````

### 2) Database Setup

Option 1: Run SQL script inside the docker container 

```` bash
docker exec -it mssql bash -lc "
>>   set +H                                                                                                                                                      
>>   /opt/mssql-tools18/bin/sqlcmd -S 127.0.0.1,1433 -U sa -P 'YourStrong!Passw0rd' -C -l 5 -b -i /sql/init.sql                                                  
>> "                                     
````

Option 2: EF Core migrations

````bash
dotnet ef database update --project FlowerInventory.Web
````

#### Also enable the DbInitializer from the Program.cs if you want to seed the database with some test data.
````csharp
if (app.Environment.IsDevelopment())
{
    /* Uncomment this to use seeding trhough the initilizer */
    //await DbInitializer.InitializeAsync(db);
}
````

### 3) Running the Application

```bash
# from solution root
dotnet run --project FlowerInventory.Web
```

### Application will start at:
👉 https://localhost:5012


## Technologies Used

- .NET 9 / ASP.NET Core — main framework

- Entity Framework Core — database access

- SQL Server 2025 — relational database

- MinIO - image storage

- Docker / Docker Compose — containerized DB setup

- xUnit — unit testing framework


## Challenges 

- Project structure confusion: Initially mixed solution/project folders; resolved by separating FlowerInventory.Web and FlowerInventory.Tests inside one solution.

- Test discovery issues: xUnit tests were not found until proper packages (Microsoft.NET.Test.Sdk, xunit.runner.visualstudio) were restored.

- Static files not loading: Fixed by enabling app.UseStaticFiles() middleware and ensuring CSS/JS were inside wwwroot/.

- Database initialization: Balancing between using a SQL script (fast for Docker) and EF Core migrations (more maintainable).




