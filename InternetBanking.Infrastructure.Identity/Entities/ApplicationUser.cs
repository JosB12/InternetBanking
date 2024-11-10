﻿using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public string UserType { get; set; }
        public string Identification { get; set; } //Cedula


        //public ICollection<ProductosFinancieros> Products { get; set; } = new List<ProductosFinancieros>(); 

    }
}