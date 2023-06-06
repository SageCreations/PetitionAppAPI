using Microsoft.EntityFrameworkCore;
using PetitionAPI.Models;

namespace PetitionAPI;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Petition> Petitions { get; set; }
    
    //public string DbPath { get; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlite("Data Source=Data/mydatabase.db");
}