# Recipe API

A backend API for managing recipes. This project implements a RESTful API using .NET Core 8, Entity Framework Core, and PostgreSQL, with deployment to Azure.
Developed a RESTful API for managing recipes using .NET Core 8, Entity Framework Core, and PostgreSQL.
Implemented CRUD operations for recipes and ingredients with a many-to-many relationship.
Added a custom feature to suggest random recipes based on available ingredients.
Deployed to Azure App Service with a PostgreSQL database for scalability.
Enhanced API with status codes (201, 202, 203) for improved RESTful design.


## Features
- CRUD operations for recipes and ingredients.
- Many-to-many relationship between recipes and ingredients.
- Extra functionality: Random recipe selection based on available ingredients (`GET /api/recipes/random`).
- Deployed to Azure for scalability.
- Enhanced status codes (`201`, `202`, `203`) for extra credit.

## Tech Stack
- ASP.NET Core 8
- Entity Framework Core (with PostgreSQL)
- Dependency Injection
- LINQ for querying
- Asynchronous communication

## Setup Instructions
1. Clone the repository:git clone https://github.com/hunor10/.NETproject.git
2. Create the appsettings.json file and configure the database connection
3. Run migrations: dotnet ef migrations add InitialCreate  
                   dotnet ef database update  
4. Run the application: dotnet run || cd bin/Debug/net8.0 -> dotnet first.dll
5. Access the API at https://localhost:5134/swagger/index.html (Frontend part in progress...)

Azure deployment (might not be momentarily available): https://first-api-a0hmhgenaagcahaj.germanywestcentral-01.azurewebsites.net/