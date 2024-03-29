﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProSoLoPortal.Models;

namespace ProSoLoPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<ProSoLoPortal.Models.Bids> Bids { get; set; }
        
        public DbSet<ProSoLoPortal.Models.Case> Case { get; set; }
        
        public DbSet<ProSoLoPortal.Models.Imagebank> Imagebank { get; set; }
        
        public DbSet<ProSoLoPortal.Models.Profile> Profile { get; set; }
        
        public DbSet<ProSoLoPortal.Models.Rating> Rating { get; set; }
    }
}