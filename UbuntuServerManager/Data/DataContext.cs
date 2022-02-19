using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UbuntuServerManager.Models;

namespace UbuntuServerManager.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }

        public DataContext()
        {
            var created = Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Context.db");
        }


    }
}
