
# FlowerInventorySystem

FlowerInventorySystem is an ASP.NET Core 9 Web Application with unit tests, SQL Server integration, and Docker support.
It demonstrates a typical layered web application setup with a testing framework, database initialization strategies, and modern development practices.


## Project Structure
````
FlowerInventorySystem/
├── FlowerInventory.Web/           # ASP.NET Core web project
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   └── FlowerInventory.Web.csproj
│
├── FlowerInventory.Tests/         # xUnit test project
│   └── FlowerInventory.Tests.csproj
│
├── FlowerInventorySystem.sln      # Solution file
└── README.md                      # Documentation

````
## Prerequisites

- .NET 9 SDK

- Docker Desktop

- SQL Server (local or container)

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



### Running the Tests
````bash
dotnet test
````

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


