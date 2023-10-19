using System;
using BlogApp.Data;
using BlogApp.Enum;
using BlogApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services
{
    public class DataService
    {
        
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            //to create the db from the migrations if not exists
            await _dbContext.Database.MigrateAsync();

            // 1: Seeding a few roles to the system
           
            await SeedRolesAsync();
            // 2: Seed a few users into the system
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            //If there are already roles in the system do nothing
            if (_dbContext.Roles.Any()) {
                return;
            }
            //otherwise we want to create a few roles

            foreach(var role in System.Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            //if there are already users in the system do nothing
            if (_dbContext.Users.Any())
            {
                return;
            }
            //Step 1: Creates a new instance of BlogUser
            var adminUser = new BlogUser()
            {
                Email = "dkguruge@gmail.com",
                UserName = "dkguruge@gmail.com",
                FirstName = "Dulmini",
                LastName = "Guruge",
                PhoneNumber = "(306) 321 4577",
                EmailConfirmed = true


            };

            //Step 2: Use the user manager to create a new user that s defined by adminUser
            await _userManager.CreateAsync(adminUser, "Abc&123!");

            //add this user to the Administrator role
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


            //Step 1: Creates the moderator user
            var modUser = new BlogUser()
            {
                Email = "agdkg12@gmail.com",
                UserName = "agdkg12@gmail.com",
                FirstName = "Dul",
                LastName = "Kans",
                PhoneNumber = "(306) 321 4577",
                EmailConfirmed = true


            };

            //Step 2: Use the user manager to create a new user that s defined by adminUser
            await _userManager.CreateAsync(modUser, "Abc&123!");

            //add this user to the Administrator role
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());

        }



    } 
    
}

