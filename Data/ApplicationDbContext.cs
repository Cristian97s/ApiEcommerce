using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    //tabla para la Bb por convencion siempre en plural
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}