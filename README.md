# verbose-disco

# TDD for a .NET 8 Web Application with PostgreSQL

This tutorial guides you through Test-Driven Development (TDD) for a .NET 8 web application. We will create three controllers: `Login`, `Register`, and `Profile`, using a PostgreSQL database for data storage.

## Prerequisites

- .NET 8 SDK installed
- PostgreSQL installed and running
- Basic understanding of .NET, C#, and TDD principles
- An IDE like Visual Studio or VS Code

## Project Setup

1. **Create a New .NET Web API Project**

    ```bash
    dotnet new webapi -n wellnessapi
    cd wellnessapi
    ```

2. **Install Necessary Packages**

    ```bash
    dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.Extensions.Hosting
    dotnet add package xunit
    dotnet add package Moq
    ```

3. **Set Up PostgreSQL Connection**

    Update `appsettings.json` with your PostgreSQL connection string:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=wellnessapi_db;Username=postgres;Password=yourpassword"
      }
    }
    ```

4. **Create Data Models**

    Create a new folder named `Models` and add the following classes:

    ```csharp
    // Models/User.cs
    namespace wellnessapi.Models{
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    }
    ```

5. **Configure Entity Framework Core**

    Add a `Data` folder and create an `ApplicationDbContext` class:

    ```csharp
    // Data/ApplicationDbContext.cs
    using Microsoft.EntityFrameworkCore;
    using wellnessapi.Models;

    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
    ```

    Register the context in `Program.cs`:

    ```csharp
        using Microsoft.EntityFrameworkCore;
        using Npgsql.EntityFrameworkCore.PostgreSQL;
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        
        app.MapControllers();
        app.Run();


    ```


## Test-Driven Development (TDD)

### Step 1: Write Tests for Login, Register, and Profile

1. **Create Test Project**

    ```bash
    cd ..
    dotnet new xunit -n wellnessapi.Tests
    cd wellnessapi.Tests
    dotnet add package xunit
    dotnet add reference ../wellnessapi/wellnessapi.csproj
    ```

2. **Write Tests**

    Create a `Controllers` folder and add test classes:

    ```csharp
    // Controllers/LoginControllerTests.cs
    using Xunit;
    using Moq;
    using wellnessapi.Controllers;
    using wellnessapi.Models;
    using wellnessapi.Data;
    using Microsoft.EntityFrameworkCore;

    public class LoginControllerTests {
        [Fact]
        public void Should_Login_Valid_User() {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LoginTestDb").Options;

            using (var context = new ApplicationDbContext(options)) {
                context.Users.Add(new User { Username = "testuser", Password = "testpassword" });
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options)) {
                var controller = new LoginController(context);

                // Act
                var result = controller.Login("testuser", "testpassword");

                // Assert
                Assert.NotNull(result);
            }
        }
    }
    ```

    Repeat this process for `RegisterControllerTests.cs` and `ProfileControllerTests.cs`.

### Step 2: Implement the Controllers

1. **Login Controller**

    ```csharp
    // Controllers/LoginController.cs
    using Microsoft.AspNetCore.Mvc;
    using wellnessapi.Data;
    using wellnessapi.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(string username, string password) {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) return Unauthorized();

            return Ok(user);
        }
    }
    ```

2. **Register Controller**

    ```csharp
    // Controllers/RegisterController.cs
    using Microsoft.AspNetCore.Mvc;
    using wellnessapi.Data;
    using wellnessapi.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register(User user) {
            if (_context.Users.Any(u => u.Username == user.Username)) return Conflict("Username already exists");

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }
    }
    ```

3. **Profile Controller**

    ```csharp
    // Controllers/ProfileController.cs
    using Microsoft.AspNetCore.Mvc;
    using wellnessapi.Data;
    using wellnessapi.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetProfile(int id) {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProfile(int id, User updatedUser) {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            _context.SaveChanges();

            return Ok(user);
        }
    }
    ```

### Step 3: Run Tests and Refactor

1. **Run Tests**

    ```bash
    dotnet test
    ```

2. **Refactor**

    Refactor the code as needed based on test results, ensuring all tests pass before moving on.

## Conclusion

By following this tutorial, you've created a .NET 8 web application using TDD with three core controllers: `Login`, `Register`, and `Profile`. You also set up a PostgreSQL database for persistent storage.

### Next Steps

- Add more features and tests
- Implement authentication and authorization
- Deploy the application

Happy coding!
