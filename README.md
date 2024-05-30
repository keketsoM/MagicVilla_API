# Villa House API Project

Welcome to the Villa House API Project! This README file will guide you through the setup, configuration, and usage of the project.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Requirements](#requirements)
4. [Installation](#installation)
5. [Configuration](#configuration)
6. [Usage](#usage)
7. [API Endpoints](#api-endpoints)

## Introduction

The Villa House API is designed to manage and interact with a database of villa houses. It provides endpoints to create, read, update, and delete villa house records. This project is built using ASP.NET Core and Entity Framework Core.

## Features

- Create new villa house records
- Retrieve details of existing villa houses
- Update villa house information
- Delete villa house records


## Requirements

- Visual Studio 2022
- .NET 8.0 
- SQL Server or any other supported database

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/villa-house-api.git
   ```
2. **Open the solution in Visual Studio:**
   Navigate to the project folder and open the `WebApi test.sln` file.

3. **Restore dependencies:**
   In Visual Studio, go to `Tools` > `NuGet Package Manager` > `Manage NuGet Packages for Solution...` and restore the required packages.

4. **Setup the database:**
   Update the `appsettings.json` file with your database connection string. Then, open the Package Manager Console in Visual Studio and run the following commands to apply migrations and seed the database:
   ```powershell
   Update-Database
   ```

## Configuration

1. **Database Connection:**
   Update the `ConnectionStrings` section in `appsettings.json` with your database connection details.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your_server;Database=your_database;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=True"
   }
   ```

2. **App Settings:**
   Configure other settings in the `appsettings.json` file as needed.

## Usage

1. **Running the API:**
   Press `F5` or click on the `Start Debugging` button in Visual Studio to run the API. The API will be accessible at `https://localhost:7002/swagger/index.html'.

2. **Using Swagger:**
   Navigate to `https://localhost:7002/swagger/index.html` to access the Swagger UI, where you can test the API endpoints.

## API Endpoints

The following are some of the key endpoints provided by the Villa House API:

- **GET /api/villas**: Retrieve a list of all villa houses.
- **GET /api/villas/{id}**: Retrieve details of a specific villa house by ID.
- **POST /api/villas**: Create a new villa house.
- **PUT /api/villas/{id}**: Update an existing villa house by ID.
- **DELETE /api/villas/{id}**: Delete a villa house by ID.


Thank you for using the Villa House API! If you have any questions or need further assistance, please feel free to email keketsokeke03@gmail.com.
