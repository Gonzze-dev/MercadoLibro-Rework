using MercadoLibroDB.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoLibroDB
{
    public class MercadoLibroContext(
        DbContextOptions<MercadoLibroContext> options
    ) : DbContext(options)
    {
        public DbSet<User> User { get; set; }
        public DbSet<UserAuth> UserAuth { get; set; }

        protected override void OnModelCreating(ModelBuilder mBuilder)
        {
            //Rename tables
            mBuilder
                .Entity<User>()
                .ToTable("User");

            mBuilder
                .Entity<UserAuth>()
                .ToTable("UserAuth");

            //User
            mBuilder
                .Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            //UserAuth
            mBuilder
                .Entity<UserAuth>()
                .Property(uAuth => uAuth.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            mBuilder
                .Entity<UserAuth>()
                .HasOne(uAuth => uAuth.User)
                .WithMany()
                .HasForeignKey(uAuth => uAuth.UserID)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
