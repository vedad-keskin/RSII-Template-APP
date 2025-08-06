using ManiFest.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManiFest.Services.Database
{
    public static class DataSeeder
    {
        private const string DefaultPhoneNumber = "+387 00 000 000";
        
        private const string TestMailSender = "test.sender@gmail.com";
        private const string TestMailReceiver = "test.receiver@gmail.com";

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Use a fixed date for all timestamps
            var fixedDate = new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role 
                { 
                    Id = 1, 
                    Name = "Administrator", 
                    Description = "System administrator with full access", 
                    CreatedAt = fixedDate, 
                    IsActive = true 
                },
                new Role 
                { 
                    Id = 2, 
                    Name = "User", 
                    Description = "Regular user role", 
                    CreatedAt = fixedDate, 
                    IsActive = true 
                }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User 
                {
                    Id = 1,
                    FirstName = "Denis",
                    LastName = "Mušić",
                    Email = TestMailReceiver,
                    Username = "admin",
                    PasswordHash = "3KbrBi5n9zdQnceWWOK5zaeAwfEjsluyhRQUbNkcgLQ=",
                    PasswordSalt = "6raKZCuEsvnBBxPKHGpRtA==",
                    IsActive = true,
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 1, // Male
                    CityId = 5, // Sarajevo
                    Picture = ImageConversion.ConvertImageToByteArray("Assets", "denis.png")
                },
                new User 
                { 
                    Id = 2, 
                    FirstName = "Amel", 
                    LastName = "Musić",
                    Email = "example1@gmail.com",
                    Username = "user", 
                    PasswordHash = "kDPVcZaikiII7vXJbMEw6B0xZ245I29ocaxBjLaoAC0=", 
                    PasswordSalt = "O5R9WmM6IPCCMci/BCG/eg==", 
                    IsActive = true, 
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 1, // Male
                    CityId = 5, // Mostar
                    Picture = ImageConversion.ConvertImageToByteArray("Assets", "amel.png")
                },
                new User 
                { 
                    Id = 3, 
                    FirstName = "Adil", 
                    LastName = "Joldić",
                    Email = "example2@gmail.com",
                    Username = "admin2", 
                    PasswordHash = "BiWDuil9svAKOYzii5wopQW3YqjVfQrzGE2iwH/ylY4=", 
                    PasswordSalt = "pfNS+OLBaQeGqBIzXXcWuA==", 
                    IsActive = true, 
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 1, // Male
                    CityId = 3, // Tuzla
                    Picture = ImageConversion.ConvertImageToByteArray("Assets", "adil.png")
                },
                new User 
                { 
                    Id = 4, 
                    FirstName = "Test", 
                    LastName = "Test", 
                    Email = TestMailSender, 
                    Username = "user2", 
                    PasswordHash = "KUF0Jsocq9AqdwR9JnT2OrAqm5gDj7ecQvNwh6fW/Bs=", 
                    PasswordSalt = "c3ZKo0va3tYfnYuNKkHDbQ==", 
                    IsActive = true, 
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 2, // Female
                    CityId = 1, // Sarajevo
                    //Picture = ImageConversion.ConvertImageToByteArray("Assets", "test.png")
                }
            );

            // Seed UserRoles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = 1, RoleId = 1, DateAssigned = fixedDate }, 
                new UserRole { Id = 2, UserId = 2, RoleId = 1, DateAssigned = fixedDate }, 
                new UserRole { Id = 3, UserId = 3, RoleId = 2, DateAssigned = fixedDate }, 
                new UserRole { Id = 4, UserId = 4, RoleId = 2, DateAssigned = fixedDate }  
            );

            // Seed Genders
            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Name = "Male" },
                new Gender { Id = 2, Name = "Female" }
            );

            // Seed Cities
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Sarajevo" },
                new City { Id = 2, Name = "Banja Luka" },
                new City { Id = 3, Name = "Tuzla" },
                new City { Id = 4, Name = "Zenica" },
                new City { Id = 5, Name = "Mostar" },
                new City { Id = 6, Name = "Bijeljina" },
                new City { Id = 7, Name = "Prijedor" },
                new City { Id = 8, Name = "Brčko" },
                new City { Id = 9, Name = "Doboj" },
                new City { Id = 10, Name = "Zvornik" }
            );
        }
    }
} 