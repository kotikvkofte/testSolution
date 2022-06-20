using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InventorysApi.Models
{
    public class inventorysIFormFiles
    {
        public int Id { get; set; }
        public string InventoryCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Amount { get; set; }
        public IFormFile Photo { get; set; }
        public string Description { get; set; }
        public Nullable<int> IdType { get; set; }
        public TypeOfInventory TypeOfInventory { get; set; }
        public Nullable<int> IdManufacture { get; set; }
        public Manufacturers Manufacturers { get; set; }
        public Nullable<int> IdWorkplace { get; set; }
        public Workplaces Workplaces { get; set; }
        public Nullable<int> IdProvider { get; set; }
        public Providers Providers { get; set; }
        public Nullable<int> IdLocations { get; set; }
        public Locations Locations { get; set; }
    }
}
