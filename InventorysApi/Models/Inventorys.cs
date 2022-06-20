using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class Inventorys
    {
        public int Id { get; set; }
        public string InventoryCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Amount { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public Nullable<int> IdType { get; set; }
        [ForeignKey("IdType")]
        public TypeOfInventory TypeOfInventory { get; set; }
        [JsonIgnore]
        public Nullable<int> IdManufacture { get; set; }
        [ForeignKey("IdManufacture")]
        public Manufacturers Manufacturers { get; set; }
        [JsonIgnore]
        public Nullable<int> IdWorkplace { get; set; }
        [ForeignKey("IdWorkplace")]
        public Workplaces Workplaces { get; set; }
        [JsonIgnore]
        public Nullable<int> IdProvider { get; set; }
        [ForeignKey("IdProvider")]
        public Providers Providers { get; set; }
        [JsonIgnore]
        public Nullable<int> IdLocations { get; set; }
        [ForeignKey("IdLocations")]
        public Locations Locations { get; set; }
    }
}
