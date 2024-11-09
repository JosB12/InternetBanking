﻿using InternetBanking.Core.Application.Enums;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity.Seeds
{
    public static class DefaultClientUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "clientuser";
            defaultUser.Email = "clientuser@email.com";
            defaultUser.FirstName = "John";
            defaultUser.LastName = "Doe";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result =  await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    if (result.Succeeded)
                    {
                        // Asignar rol Client
                        await userManager.AddToRoleAsync(defaultUser, Roles.Client.ToString());

                        // Crear cuenta bancaria con saldo inicial
                        //var account = new Account
                        //{
                        //    Balance = 1000.00M, // Saldo inicial
                        //    UserId = defaultUser.Id
                        //};

                        // Guardar cuenta bancaria
                        //await accountRepository.CreateAsync(account);
                    }
                }
            }
        }
    }
}
