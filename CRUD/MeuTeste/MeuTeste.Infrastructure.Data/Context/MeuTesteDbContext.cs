using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Enums;
using MeuTeste.Domain.Services;
using MeuTeste.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MeuTeste.Infrastructure.Data.Context
{
    public class MeuTesteDbContext : DbContext
    {
        public MeuTesteDbContext(DbContextOptions<MeuTesteDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar mappings
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new UserMap());

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Hash duplo para admin: admin123
            var adminPasswordHash = PasswordHasher.HashPassword("admin123", "admin");

            // Criar usuários de teste
            var users = new List<User>
            {
                new()
                {
                    Id = 1,
                    Username = "admin",
                    Email = "vladimirca2000@gmail.com",
                    PasswordHash = adminPasswordHash,
                    Role = Role.Admin,
                    IsActive = true,
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                }
            };

            modelBuilder.Entity<User>().HasData(users);

            // Criar categorias
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Eletrônicos", IsDelete = false, CreatedAt = DateTime.UtcNow, CreatedUser = "system" },
                new() { Id = 2, Name = "Livros", IsDelete = false, CreatedAt = DateTime.UtcNow, CreatedUser = "system" },
                new() { Id = 3, Name = "Roupas", IsDelete = false, CreatedAt = DateTime.UtcNow, CreatedUser = "system" },
                new() { Id = 4, Name = "Alimentos", IsDelete = false, CreatedAt = DateTime.UtcNow, CreatedUser = "system" },
                new() { Id = 5, Name = "Casa e Jardim", IsDelete = false, CreatedAt = DateTime.UtcNow, CreatedUser = "system" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            // Criar 50 produtos associados às categorias
            var products = new List<Product>();
            int productId = 1;

            // Eletrônicos (10 produtos)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    Id = productId++,
                    Name = $"Eletrônico {i + 1}",
                    CategoryId = 1,
                    Price = 100.00m + (i * 50),
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                });
            }

            // Livros (10 produtos)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    Id = productId++,
                    Name = $"Livro {i + 1}",
                    CategoryId = 2,
                    Price = 30.00m + (i * 10),
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                });
            }

            // Roupas (10 produtos)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    Id = productId++,
                    Name = $"Roupa {i + 1}",
                    CategoryId = 3,
                    Price = 50.00m + (i * 15),
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                });
            }

            // Alimentos (10 produtos)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    Id = productId++,
                    Name = $"Alimento {i + 1}",
                    CategoryId = 4,
                    Price = 10.00m + (i * 5),
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                });
            }

            // Casa e Jardim (10 produtos)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new Product
                {
                    Id = productId++,
                    Name = $"Item Casa e Jardim {i + 1}",
                    CategoryId = 5,
                    Price = 25.00m + (i * 20),
                    IsDelete = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedUser = "system"
                });
            }

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
