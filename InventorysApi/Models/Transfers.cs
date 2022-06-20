using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class Transfers
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Nullable<int> StartPoint { get; set; }
        [ForeignKey("StartPoint")]
        public Workplaces Workplaces { get; set; }
        [JsonIgnore]
        public Nullable<int> EndPoint { get; set; }
        [ForeignKey("EndPoint")]
        public Workplaces Workplaces1 { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        [JsonIgnore]
        public Nullable<int> StartPerson { get; set; }
        [ForeignKey("StartPerson")]
        public Users Users { get; set; }
        [JsonIgnore]
        public Nullable<int> EndPerson { get; set; }
        [ForeignKey("EndPerson")]
        public Users Users1 { get; set; }
        [JsonIgnore]
        public int IdInventory { get; set; }
        [ForeignKey("IdInventory")]
        public Inventorys Inventorys { get; set; }
    }
}
