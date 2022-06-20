using InventorysApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorysApi.DataFiles
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Workplaces> Workplaces { get; set; }
        public DbSet<TypeOfInventory> TypeOfInventory { get; set; }
        public DbSet<Providers> Providers { get; set; }
        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<Inventorys> Inventorys { get; set; }
        public DbSet<Transfers> Transfers { get; set; }
        public DbSet<Stocktaking> Stocktaking { get; set; }
        public DbSet<StocktakingInventory> StocktakingInventory { get; set; }
    }
}
