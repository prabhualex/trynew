Sure, let's generate the complete code with steps:

### Step 1: Create a New Worker Service Project
Open Visual Studio 2022 and create a new C# Worker Service project.

### Step 2: Add Oracle.DataAccess.dll
Copy `Oracle.DataAccess.dll` Version: 4.122.19.1 from `C:\app\client\product\19.0.0\client_1\ODP.NET\bin\4\Oracle.DataAccess.dll` into your project folder.

### Step 3: Add Reference
In your Visual Studio project, right-click on "References" in the Solution Explorer, and select "Add Reference...". Browse to the location where you copied `Oracle.DataAccess.dll` and add it as a reference to your project.

### Step 4: Create DALC Layer
Add a new class file to your project named `DataAccessLayer.cs`. This class will contain methods to handle database operations.

```csharp
using Oracle.DataAccess.Client;

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

### Step 5: Create Worker Class
Modify the `Worker.cs` file to include database insertion logic.

```csharp
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceProject
{
    public class Worker : BackgroundService
    {
        private readonly DataAccessLayer _dataAccess;

        public Worker(DataAccessLayer dataAccess)
        {
            _dataAccess = dataAccess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Insert values into DB
                string query = "INSERT INTO TESTTABLE (BRIM, EXPMGRID) VALUES ('01', 'NYKCM24947235')";
                _dataAccess.UpdateDB(query);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
```

### Step 6: Configure Connection String
Replace `"your_connection_string_here"` in the `DataAccessLayer.cs` file with your actual Oracle connection string.

### Step 7: Setup Dependency Injection
In the `Program.cs` file, configure dependency injection to inject the `DataAccessLayer` into the `Worker` class.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    services.AddHostedService

```csharp
(typeof(Worker));
                });
    }
}
}
```

### Step 8: Replace `WorkerServiceProject` with Your Project Name
Replace `WorkerServiceProject` with the actual name of your project throughout the code.

### Step 9: Build and Run
Build your project and run the worker service. It will continuously insert values into the database based on the specified query.

This setup should provide you with a basic worker service program with a separate DALC layer for handling Oracle database operations. Make sure to replace placeholder values with actual values specific to your database setup.