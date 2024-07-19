# MakeupStore

## Overview

This project is about managing and purchasing makeup products online. The IDE used is Visual Studio 2022, and the technology used is ASP.NET Core Web API in .NET 7.0. The programming languages used to write the project are C# (backend), SQL (database context), HTML/CSS, and JavaScript (frontend). This repository contains the source code and resources required to deploy and run the application.

## Directory Structure

The project is structured as follows:

```
MakeupStore/
├── Makeup.API/
│ ├── Connected Services/
│ ├── Dependencies/
│ ├── Properties/
│ ├── Controllers/
│ ├── appsettings.json
│ ├── Program.cs
├── Makeup.Data/
│ ├── Dependencies/
│ ├── Contexts/
│ ├── Migrations/
├── Makeup.Entities/
│ ├── Dependencies/
│ ├── ApiUser.cs
│ ├── Brand.cs
│ ├── Category.cs
│ ├── Customer.cs
│ ├── Order.cs
│ ├── OrderDetail.cs
│ ├── Product.cs
├── Makeup.Services/
│ ├── Dependencies/
│ ├── DTO/
│ ├── External/
│ ├── Mapping/
│ ├── ApiAuthService.cs
│ ├── OrdersService.cs
│ ├── ProductsService.cs
├── Makeup.Web/
│ ├── Connected Services/
│ ├── Dependencies/
│ ├── Properties/
│ ├── wwwroot/
│ ├── Pages/
│ ├── appsettings.json
│ ├── Program.cs
└── MakeupStore.sln
```

### Key Directories and Files

- **Makeup.API/Controllers**: Contains the MVC controllers responsible for handling HTTP requests.
- **Makeup.Entities**: Includes the data models used throughout the application.
- **Makeup.Data/Contexts**: Contains the database context and migration files.
- **Makeup.Services**: Contains service classes for handling business logic.
- **Makeup.Web/wwwroot**: Static files such as JavaScript, CSS, and images.
- **Makeup.Web/Pages**: Razor pages for rendering HTML.
- **appsettings.json**: Configuration settings for the application.
- **Program.cs**: The main entry point for the application.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 7.0 or later)
- [Node.js](https://nodejs.org/) (for managing frontend dependencies)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) (for database management)

## Setup and Installation

1. **Clone the repository:**
    ```sh
    git clone https://github.com/siyana-m/Makeupstore.git
    cd Makeupstore
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

3. **Build the project:**
    ```sh
    dotnet build
    ```

4. **Run the application:**
    ```sh
    dotnet run --project Makeup.Web
    ```

## Database Setup

1. **Create a new database in SQL Server Management Studio (SSMS):**
    - Open SSMS and connect to your SQL Server instance.
    - Right-click on the `Databases` folder and select `New Database`.
    - Name the database `MakeupstoreDB` and click `OK`.

2. **Run the provided SQL script:**
    - Open a new query window in SSMS.
    - Copy the contents of the `Solution Items/makeup_db.txt` file into the query window.
    - Execute the script to create the necessary tables, triggers, and seed data.

3. **Update the connection string:**
    - Open the `appsettings.json` file in the `Makeup.Web` project.
    - Update the `ConnectionStrings` section with your SQL Server connection details:

    If you log into SSMS with a username and password:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MakeupstoreDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;"
    }
    ```

    If you log into SSMS directly with the server (Windows Authentication):
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MakeupstoreDB;Trusted_Connection=True;"
    }
    ```

## Running Tests

To run the tests for the application, use the following command:

```sh
dotnet test
```

## Deployment

For deployment, you can publish the application using:

```sh
dotnet publish --configuration Release
```

Follow your hosting provider's instructions to deploy the published files.

## Contributing

Contributions are welcome! Please create an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For any questions or feedback, please contact [siskataam32@gmail.com](mailto:siskataam32@gmail.com).

## Notes

The project is still under development. There may be issues. In case of finding one, please contact [siskataam32@gmail.com](mailto:siskataam32@gmail.com).
