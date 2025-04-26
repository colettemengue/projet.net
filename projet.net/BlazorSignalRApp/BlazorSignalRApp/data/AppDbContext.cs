using Microsoft.EntityFrameworkCore;
using BlazorSignalRApp.Models;
using System.Collections.Generic;
using NLog;
namespace BlazorSignalRApp.data
{

    public class AppDbContext : DbContext
    {
       
        public string DbPath { get; }

        
        public DbSet<Message> Messages { get; set; } = default!;

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
        
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "messages.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

    }
}