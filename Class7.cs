Sure, I can modify the program to use `Oracle.ManagedDataAccess.Core` instead of `Oracle.DataAccess.Client`. `Oracle.ManagedDataAccess.Core` is the managed version of the Oracle Data Provider for .NET (ODP.NET) and is compatible with .NET Core and .NET 5/6.

Here's how you can update the program:

1. **Add Reference to Oracle.ManagedDataAccess.Core**:
   Ensure that you have installed the `Oracle.ManagedDataAccess.Core` package in your project. You can do this using NuGet Package Manager or Package Manager Console.

2. **Update DataAccessLayer.cs**:
   Modify the `DataAccessLayer.cs` file to use `Oracle.ManagedDataAccess.Client` instead of `Oracle.DataAccess.Client`.

```csharp
using Oracle.ManagedDataAccess.Client;

namespace WorkerServiceProject
{
    public class DataAccessLayer
    {
        private string connectionString = "your_connection_string_here";

        public void UpdateDB(string query)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
```

3. * *Update Program.cs * *:
   Modify the `Program.cs` file to use `AddOracleManagedDataAccess()` to register the Oracle Data Provider for .NET Core.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;

namespace WorkerServiceProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                services.AddSingleton<DataAccessLayer>();
                services.AddHostedService(typeof(Worker));
                services.AddTransient

```csharp
(typeof(OracleConnection), (sp) => new OracleConnection("your_connection_string_here")));
    });
        }
    }
}
```

4. * *Replace `your_connection_string_here`**:
   Replace `"your_connection_string_here"` with your actual Oracle connection string in both `DataAccessLayer.cs` and `Program.cs`.

5. **Build and Run**:
   Build your project and run the worker service. It should now use `Oracle.ManagedDataAccess.Core` for database operations.

By making these changes, your program will use `Oracle.ManagedDataAccess.Core` instead of `Oracle.DataAccess.Client` for database connectivity, making it compatible with .NET Core and .NET 5/6 environments.