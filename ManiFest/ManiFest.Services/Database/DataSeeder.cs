using eRent.Services.Helpers;
using ManiFest.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManiFest.Services.Database
{
    public static class DataSeeder
    {
        private const string DefaultPhoneNumber = "+387 61 111 111";

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Use a fixed date for all timestamps
            var fixedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                   new Role
                   {
                       Id = 1,
                       Name = "Administrator",
                       Description = "Full system access and administrative privileges",
                       CreatedAt = fixedDate,
                       IsActive = true
                   },
                   new Role
                   {
                       Id = 2,
                       Name = "User",
                       Description = "Standard user with limited system access",
                       CreatedAt = fixedDate,
                       IsActive = true
                   }
            );


            const string defaultPassword = "test";

            // Admin user (desktop)
            var desktopSalt = PasswordGenerator.GenerateDeterministicSalt("desktop");
            var desktopHash = PasswordGenerator.GenerateHash(defaultPassword, desktopSalt);

            // Regular users
            var user1Salt = PasswordGenerator.GenerateDeterministicSalt("user");
            var user1Hash = PasswordGenerator.GenerateHash(defaultPassword, user1Salt);
            var user2Salt = PasswordGenerator.GenerateDeterministicSalt("user2");
            var user2Hash = PasswordGenerator.GenerateHash(defaultPassword, user2Salt);
            var user3Salt = PasswordGenerator.GenerateDeterministicSalt("user3");
            var user3Hash = PasswordGenerator.GenerateHash(defaultPassword, user3Salt);



            // Seed Users
            modelBuilder.Entity<User>().HasData(
                // Admin user (desktop)
                new User
                {
                    Id = 1,
                    FirstName = "Vedad",
                    LastName = "Nuhić",
                    Email = "admin@erent.com",
                    Username = "desktop",
                    PasswordHash = desktopHash,
                    PasswordSalt = desktopSalt,
                    IsActive = true,
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 1, 
                    CityId = 1,
                    //Picture = ImageConversion.ConvertImageToByteArray("Assets", "pic1.png")
                },
                new User
                {
                    Id = 2,
                    FirstName = "Amel",
                    LastName = "Musić",
                    Email = "test.vedadnuhic@gmail.com",
                    Username = "user",
                    PasswordHash = user1Hash,
                    PasswordSalt = user1Salt,
                    IsActive = true,
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 1, 
                    CityId = 1,
                    Picture = ImageConversion.ConvertImageToByteArray("Assets", "amel.png")

                },
                // User 2
                new User
                {
                    Id = 3,
                    FirstName = "Nina",
                    LastName = "Bijedić",
                    Email = "user2@erent.com",
                    Username = "user2",
                    PasswordHash = user2Hash,
                    PasswordSalt = user2Salt,
                    IsActive = true,
                    CreatedAt = fixedDate,
                    PhoneNumber = DefaultPhoneNumber,
                    GenderId = 2, 
                    CityId = 5, 
                }
            );

            // Seed UserRoles
            modelBuilder.Entity<UserRole>().HasData(


                new UserRole { Id = 1, UserId = 1, RoleId = 1, DateAssigned = fixedDate },
                new UserRole { Id = 2, UserId = 2, RoleId = 2, DateAssigned = fixedDate },
                new UserRole { Id = 3, UserId = 3, RoleId = 2, DateAssigned = fixedDate }


            );

            // Seed Genders
            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Name = "Male" },
                new Gender { Id = 2, Name = "Female" }
            );

            // Seed Cities (30 Bosnia and Herzegovina cities)
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Sarajevo" },
                new City { Id = 2, Name = "Banja Luka" },
                new City { Id = 3, Name = "Tuzla" },
                new City { Id = 4, Name = "Mostar" },
                new City { Id = 5, Name = "Zenica" },
                new City { Id = 6, Name = "Bihać" },
                new City { Id = 7, Name = "Prijedor" },
                new City { Id = 8, Name = "Brčko" },
                new City { Id = 9, Name = "Doboj" },
                new City { Id = 10, Name = "Cazin" },
                new City { Id = 11, Name = "Bijeljina" },
                new City { Id = 12, Name = "Travnik" },
                new City { Id = 13, Name = "Zvornik" },
                new City { Id = 14, Name = "Velika Kladuša" },
                new City { Id = 15, Name = "Gračanica" },
                new City { Id = 16, Name = "Lukavac" },
                new City { Id = 17, Name = "Tešanj" },
                new City { Id = 18, Name = "Gradačac" },
                new City { Id = 19, Name = "Visoko" },
                new City { Id = 20, Name = "Konjic" },
                new City { Id = 21, Name = "Živinice" },
                new City { Id = 22, Name = "Sanski Most" },
                new City { Id = 23, Name = "Livno" },
                new City { Id = 24, Name = "Orašje" },
                new City { Id = 25, Name = "Srebrenik" },
                new City { Id = 26, Name = "Gradiška" },
                new City { Id = 27, Name = "Kakanj" },
                new City { Id = 28, Name = "Bugojno" },
                new City { Id = 29, Name = "Jajce" },
                new City { Id = 30, Name = "Trebinje" }
            );

      
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Music" },
                new Category { Id = 2, Name = "Art" },
                new Category { Id = 3, Name = "Film" },
                new Category { Id = 4, Name = "Theater" },
                new Category { Id = 5, Name = "Dance" },
                new Category { Id = 6, Name = "Literature" }
            );

        }
    }
}