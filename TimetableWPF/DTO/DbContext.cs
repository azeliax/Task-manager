using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableWPF.DTO;

namespace TimetableWPF
{
    public class TimetableDbContext : DbContext
    {
        public DbSet<MyTask> MyTasks { get; set; }
        public DbSet<MyEvent> MyEvents { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER02;Database=TimetableDb;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }
    }
}
