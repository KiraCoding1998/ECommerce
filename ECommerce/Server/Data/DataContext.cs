using ECommerce.Server.Helpers;
using ECommerce.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.Server.Data
{
    public class DataContext :DbContext
    {
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Models.ClientProduct> ClientProducts { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string password = "@adminPassword";
            PasswordManager encryptedPassword = PasswordManager.CreatePassowrdObject(password);

            modelBuilder.Entity<Models.Client>().HasData
                (new Models.Client{
                    Id = 1,
                    IsAdmin = true,
                    Username = "@adminUserName",
                    PasswordHash = encryptedPassword.HashedPassword,
                    PasswordSalt = encryptedPassword.PasswordSalt,
                });


            modelBuilder.Entity<ClientProduct>()
                .HasKey(k => 
                new{
                    k.ClientId,
                    k.ProductId
                });

            modelBuilder.Entity<ClientProduct>()
                .HasOne(c => c.Client)
                .WithMany(cp => cp.ClientProducts)
                .HasForeignKey(c => c.ClientId);


            modelBuilder.Entity<ClientProduct>()
                .HasOne(p => p.Product)
                .WithMany(cp => cp.ClientProducts)
                .HasForeignKey(p => p.ProductId);
        }
    }
}
