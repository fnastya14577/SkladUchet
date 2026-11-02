using Microsoft.EntityFrameworkCore;
using SkladUchet.Models;

namespace SkladUchet.Data;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<WriteOff> WriteOffs => Set<WriteOff>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=sklad.db");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Batch>().Property(b => b.PurchasePrice).HasColumnType("decimal(10,2)");
        mb.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(10,2)");
    }

    public static void Init()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        if (!db.Roles.Any())
        {
            var admin = new Role { Name = "Администратор", AccessLevel = 2 };
            var store = new Role { Name = "Кладовщик", AccessLevel = 1 };
            db.Roles.AddRange(admin, store);
            db.Users.AddRange(
                new User
                {
                    FullName = "Админ",
                    Login = "admin",
                    PasswordHash = "admin",
                    Role = admin
                },
                new User
                {
                    FullName = "Кладовщик",
                    Login = "user",
                    PasswordHash = "user",
                    Role = store
                });
        }

        if (!db.Products.Any())
        {
            var meds = new Category { Name = "Лекарства" };
            var food = new Category { Name = "Продукты" };
            db.Categories.AddRange(meds, food);

            var p1 = new Product { Name = "Аспирин", Category = meds, Unit = "уп", Price = 150, MinShelfLifeDays = 30 };
            var p2 = new Product { Name = "Молоко", Category = food, Unit = "л", Price = 80, MinShelfLifeDays = 5 };
            db.Products.AddRange(p1, p2);

            db.Batches.AddRange(
                new Batch
                {
                    Product = p1,
                    BatchNumber = "A-001",
                    Quantity = 100,
                    Remaining = 100,
                    ReceiptDate = DateTime.Today.AddDays(-10),
                    ExpiryDate = DateTime.Today.AddDays(3)
                },
                new Batch
                {
                    Product = p2,
                    BatchNumber = "M-777",
                    Quantity = 50,
                    Remaining = 50,
                    ReceiptDate = DateTime.Today.AddDays(-2),
                    ExpiryDate = DateTime.Today.AddDays(-1)
                },
                new Batch
                {
                    Product = p1,
                    BatchNumber = "A-002",
                    Quantity = 200,
                    Remaining = 200,
                    ReceiptDate = DateTime.Today,
                    ExpiryDate = DateTime.Today.AddDays(90)
                });
        }

        foreach (var b in db.Batches)
            b.CalculateStatus();
        db.SaveChanges();
    }
}