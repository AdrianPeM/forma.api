# Forma.API

Forma is a lightweight web application built for learning and experimentation. It allows users to create customizable forms, share them, and collect responses.

This project is a hands-on exploration of modern web development and deployment practices, featuring:

* **Frontend**: React, Next.js, TypeScript
* **Backend**: ASP.NET Core (C#), Web API, Entity Framework
* **Databases**: SQL & NoSQL (Postgres, MongoDB)
* **DevOps**: Docker for containerization, with a focus on production-ready deployments

Forma is designed to help developers learn how to build and deploy scalable web applications using a modern tech stack.

<br>

## Frontend

### Get ready for development

<br>

## Backend

### Get ready for development

If you will work on VSCode then install dotnet ef tools

    dotnet tool install --global dotnet-ef

Create the network to connect allow connection between containers

    docker network create containers_shared_network
    
Create the volume to persist data

    docker volume create forma.api_volume

To start API services run the following command (_API and postgresql_)

    docker compose up -d

Open <http://localhost:8000/swagger> in your browser.

To stop the API services run the following command

    docker compose down

**To start API and database separately**

PostgreSQL, the first time run the following to create the container
    
    docker run --name postgresql_server -e POSTGRES_PASSWORD=YourDbPassword --volume forma.api_volume:/var/lib/postgresql/data -p 5432:5432 --network containers_shared_network -d postgres:17.5
    
Once the container it was correctly created it is just necessary to start it again

    docker container start -d postgresql_server

Run the app using dotnet

    dotnet run

Run the app using docker
    
    docker build -t adrianpem08/forma.api .
    docker run --name forma.api_server -e=ASPNETCORE_ENVIRONMENT=Development -e=ASPNETCORE_URLS=http://+:80 -e=ConnectionStrings__DefaultConnection=Host=postgresql_server;Username=YourDbUser;Password=YourDbPwd;Database=forma_api_db_dev; -e=JwtConfig__Issuer=https://localhost:8000 -e=JwtConfig__Audience=https://localhost:8000 -e=JwtConfig__Key=development-secret-key -p 8000:80 --network containers_shared_network -d adrianpem08/forma.api

**For HTTPS testing run the follow the next steps**

Trust the HTTPS development certificate by running the following command, and click `Yes` in the dialog to trust the certificate.
More information in [Trust the ASP.NET Core HTTPS development certificate](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-9.0&tabs=visual-studio%2Clinux-sles)

    dotnet dev-certs https --trust

Run the app

    dotnet run --launch-profile https

Open <http://localhost:5141/swagger> in your browser (port can be different, check it in the terminal output).

### Development workflow

#### Model-Migration-Controller

1. Create/Update the **Model**
    * Create/Add class **`ModelName`** and define its properties
2. Make sure the model is referenced in the **ApplicationDbContex**t file
    
    ```
    public DbSet<FieldType> FieldTypes { get; set; } = default!;
    ```

    **Create Seeds if necessary**
    * Create new class Seeding/FieldTypeSeed.cs
    * Add the necessary data
        ```
        public static void SeedFieldTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldType>().HasData(
                    new FieldType { Id = 1, Name = "Text", FieldTypeId = FieldTypeEnum.Text },
                    new FieldType { Id = 2, Name = "Paragraph", FieldTypeId = FieldTypeEnum.Paragraph }
            );
        }
        ```
    * Add the SeedFieldTypes() method to the ModelBuilderExtensions.cs file
        ```
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedFieldTypes();
        }
        ```

3. Create the **Migration**
    
    * In bash
    
    ```
    dotnet ef migrations add CreateFieldTypeEntity
    ```
    
    * In Package Manager Console
    
    ```
    add-migration CreateFieldTypeEntity
    ```
    _The migrations are executed when application starts. Stop application container, build image, and start image again._

4. Create the Controller
    * In Visual Studio
    
    `Controllers` > `Add class` > `Web` > `ASP.NET` > `API Controller with read/write actions`
    
    * In bash
    
    ```
    dotnet aspnet-codegenerator controller -name FieldTypesController -async -api -m FieldType -dc ApplicationDbContext -outDir Controllers
    ```

    1. Before creating a CRUD operation, endpoint or request, define its Request/Response models
        1. If it is a new feature then define a new folder structure as follows
            Controllers > NewFeature > FeatureController.cs, Feature.Requests.cs, Feature.Responses.cs
    2. Create the necessary CRUD operations,endpoints and requests, and use the previously request/response models defined for the method's request and response.

#### Docker image version management
1. Work locally using `latest` tag:
    ```bash
    docker build -t adrianpem08/forma.api .
    ```
2. When version is ready to be deployed, tag the image with Version (1.x.x):
    ```bash
    docker tag adrianpem08/forma.api:latest adrianpem08/forma.api:1.x.x
    ```
3. Push both tags (1.x.x and latest):
    ```bash
    docker push adrianpem08/forma.api:1.x.x
    docker push adrianpem08/forma.api:latest
    ```

**Recommendations**

* Name migrations meaningfully
    * ***CreateModelNameEntity***
    * ***RemoveModelNameEntity***
    * ***AddModelNameColNameColumn***
    * ***RemoveModelNameColNameColumn***
    * ***AddModelNameSeedData***
* Review migration file before appliying them
* Create seeds when necessary, `Seed` must be created before create the `Migration`
* Separate controllers by Feature folders
* Requests and Responses files must be defined for each controller
```
Controllers/
├── Users/
│   ├── UsersController.cs         ← Basic CRUD only
│   ├── Users.Requests.cs
│   └── Users.Responses.cs
├── UserProfile/
│   ├── UserProfileController.cs   ← Profile-specific
│   ├── UserProfile.Requests.cs
│   └── UserProfile.Responses.cs
└── Auth/                           ← Authentication separate
    ├── AuthController.cs
    ├── Auth.Requests.cs
    └── Auth.Responses.cs
```
* Controllers should not have more than 6 methods; if more are needed then split it

#### Migrations

**Rollback**

1. List migrations
    
    ```
    dotnet ef migrations list
    ```
    
    * Example output
        
        ```
        20230501000000_FirstMigration
        20230502000000_SecondMigration
        20230503000000_ThirdMigration
        20230504000000_FourthMigration ← Latest
        ```

2. Rollback to the desired Migration
    
    ```
    dotnet ef database update SecondMigration
    ```
    
    * All migrations after `SecondMigration` will be undone (rolled back)
    * The changes introduced by `SecondMigration` will remain in the database

**Remove Migrations**

1. Remove the latest Migration
    
    ```
    dotnet ef migrations remove
    ```
    
    * This will remove only not applied migrations, to remove multiple migrations it must be run multiple times

**Revert all Migrations (Reset Database)**

1. Rollback all migrations

    ```
    dotnet ef database update 0
    ```

2. Optionally, delete Migrations/ folder and recreate migrations from scratch
    
    ```
    rm -r Migrations
    dotnet ef migrations add MigrationName
    dotnet ef database update
    ```

<br>

## API Response Guidelines

### Common HTTP Status Codes

| Status               | Method(s)                                | Description                               |
|----------------------|------------------------------------------|-------------------------------------------|
| **200 OK**           | `GET`, `POST`, `PUT`, `PATCH`            | The request has succeeded and the server returns the requested data or confirms the action. |
| **201 Created**      | `POST`                                   | The request has been fulfilled and a new resource has been created. |
| **202 Accepted**     | `POST`, `PUT`, `PATCH`                   | The request has been accepted for processing, but the processing is not yet complete. |
| **204 No Content**   | `DELETE`, sometimes `PUT`                | The request was successful, but no content is returned (e.g., resource deletion). |
| **400 Bad Request**  | Any                                      | The server could not understand the request due to invalid syntax or missing parameters. |
| **401 Unauthorized** | Any                                      | Authentication is required or has failed (e.g., invalid or expired credentials). |
| **403 Forbidden**    | Any                                      | The server understood the request, but the client does not have permission to access the resource. |
| **404 Not Found**    | Any                                      | The server could not find the requested resource (e.g., incorrect URL). |
| **409 Conflict**     | `POST`, `PUT`                            | The request could not be completed due to a conflict with the current state of the resource (e.g., duplicate resource). |
| **422 Unprocessable Entity** | `POST`, `PUT`                    | The server understands the request, but it cannot process the contained instructions (e.g., semantic errors). |

### Typical Responses by Action

| Action               | Status Code       | When to Use                               | Notes                         | Method(s)            |
|----------------------|-------------------|--------------------------------------------|-------------------------------|----------------------|
| Fetch all or one     | `200 OK`          | Data retrieval was successful              | Return the data requested.    | `Ok(result)`         |
| Create               | `201 Created`     | A new resource has been successfully created | Confirm the creation with the newly created resource. | `CreatedAtAction()` |
| Update               | `200 OK` / `204`  | The resource has been updated               | If the resource exists and is updated, return `200 OK`. If no content is returned after update, use `204 No Content`. | `Ok()` or `NoContent()` |
| Delete               | `204 No Content`  | The resource has been successfully deleted | Return an empty response body to indicate deletion. | `NoContent()`       |
| Validation failure   | `400` or `422`    | The input is invalid or failed validation | Return detailed error information for the user to fix their input. | `BadRequest()`, `UnprocessableEntity()` |
| Unauthorized         | `401 Unauthorized`| No authentication or the session has expired | Provide an error indicating the need for authentication. | `Unauthorized()`   |
| Forbidden            | `403 Forbidden`   | The user does not have permission to access the resource | Return an error indicating insufficient rights. | `Forbid()`          |
| Not found            | `404 Not Found`   | The resource could not be located on the server | The resource doesn't exist or has been moved/deleted. | `NotFound()`        |
| Conflict             | `409 Conflict`    | The action could not be completed due to a conflict (e.g., resource already exists) | Handle cases like duplicate records or state conflicts. | `Conflict()`        |

## Troubleshooting

#### First time load

<font color="red">Unhandled exception. Npgsql.NpgsqlException (0x80004005): Failed to connect to 172.18.0.2:5432`</font>

1. Retry docker compose up or open Docker desktop, locate the forma.api_container and re-run container

    _Applied solution: add in docker-compose file condition: service_started_

#### Docker engine stopped

1. Make sure virtualization active in BIOS (Enable if it is not)
    * Open Task Manager > Performance > CPU > Virtualization: Enabled
2. cmd `ipconfig /flushdns`
3. cmd `netsh winsock reset`
4. delete directory in `C:\Users<username>\.docker`
5. make sure you only see docker-desktop with state stopped and version 2
    - cmd `wsl --list --verbose`
6. cmd `wsl --update`
7. cmd `wsl --shutdown`
8. Restart pc and run docker desktop