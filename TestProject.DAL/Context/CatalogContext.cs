using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.DAL.Entities;

namespace TestProject.DAL.Context
{
    public class CatalogContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<Category>().HasData(
                new Category[]
                {
                new Category { Id = 1,Name="Еда"},
                new Category { Id = 2,Name="Вкусности"},
                new Category { Id = 3,Name="Вода"},
                });
            modelBuilder.Entity<Product>().HasData(
                new Product[]
                {
                new Product { Id=1, Name="Селёдка",CategoryId=1,Discription="Селёдка солёная",Price=10.000f,GeneralNote="Акция",SpecialNote="Пересоленая"},
                new Product { Id=2, Name="Тушёнка",CategoryId=1,Discription="Тушёнка говяжая",Price=20.000f,GeneralNote="Вкусная",SpecialNote="Жилы"},
                new Product { Id=3, Name="Сгущёнка",CategoryId=2,Discription="В банках",Price=30.000f,GeneralNote="С ключом",SpecialNote="Вкусная"},
                new Product { Id=4, Name="Квас",CategoryId=3,Discription="В бутылках",Price=15.000f,GeneralNote="Вятский",SpecialNote="Тёплый"},
                });
            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                new Role { Id = 1,Name="Administrator"},
                new Role { Id = 2,Name="SimpleUser"},
                new Role { Id = 3,Name="AdvanceUser"},
                });

            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                new User { Id = 1,Login="Admin",Password="Admin123",RoleId=1},
                new User { Id = 2,Login="Simple",Password="S321",RoleId=2},
                new User { Id = 3,Login="Advance",Password="A123",RoleId=3},
                });
        }
    }
}
