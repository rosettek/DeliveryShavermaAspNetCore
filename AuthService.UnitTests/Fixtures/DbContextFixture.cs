using AuthService.UnitTests.Utils;
using NUnit.Framework;

namespace AuthService.UnitTests.Fixtures;

[SetUpFixture]
public class DbContextFixture
{
    private const string ConnectionString = "Host=localhost;Database=taskManager-test;Username=postgres;Password=1";


    private static FakeDbContext? context;

    public static FakeDbContext Context
    {
        get
        {
            if (context == null)
            {
                OneTimeSetUp();
            }

            return context;
        }
    }

    [OneTimeSetUp]
    public static void OneTimeSetUp()
    {
        Console.WriteLine($"Используется строка подключения {ConnectionString}");
        context = FakeDbContext.Create(ConnectionString);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [OneTimeTearDown]
    public static async void BaseTearDown()
    {
        await context.DisposeAsync();
    }
}