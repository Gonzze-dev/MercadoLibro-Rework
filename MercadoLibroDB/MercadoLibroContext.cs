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
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartLine> CartLine { get; set; }

        protected override void OnModelCreating(ModelBuilder mBuilder)
        {
            //Rename tables
            mBuilder
                .Entity<User>()
                .ToTable("User");

            mBuilder
                .Entity<UserAuth>()
                .ToTable("UserAuth");

            mBuilder
                .Entity<RefreshToken>()
                .ToTable("RefreshToken");

            mBuilder
                .Entity<Cart>()
                .ToTable("Cart");

            mBuilder
                .Entity<CartLine>()
                .ToTable("CartLine");

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

            //RefreshToken
            mBuilder
                .Entity<RefreshToken>()
                .Property(rToken => rToken.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            mBuilder
                .Entity<RefreshToken>()
                .HasOne(rToken => rToken.User)
                .WithMany()
                .HasForeignKey(rToken => rToken.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            //Cart
            mBuilder
                .Entity<Cart>()
                .HasOne(cart => cart.User)
                .WithMany()
                .HasForeignKey(cart => cart.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            //CarLine
            mBuilder
                .Entity<CartLine>()
                .HasOne(cLine => cLine.User)
                .WithMany()
                .HasForeignKey(cLine => cLine.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
