using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace ErrorLoggerDatabaseModel
{
    public partial class DatabaseModelFromDb : DbContext
    {
        public DatabaseModelFromDb() : base("name=DatabaseModelFromDb")
        {
            // To handle the seeding
            Database.SetInitializer<DatabaseModelFromDb>(new ApplicationInitializer());
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Applications)
                .Map(m => m.ToTable("UserApplications"));
        }


    }
}
