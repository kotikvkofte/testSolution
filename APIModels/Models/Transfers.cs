using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIModels.Models
{
    public class Transfers
    {
        public int Id { get; set; }
        public Nullable<int> StartPoint { get; set; }
        [ForeignKey("StartPoint")]
        public Workplaces Workplaces { get; set; }
        public Nullable<int> EndPoint { get; set; }
        [ForeignKey("EndPoint")]
        public Workplaces Workplaces1 { get; set; }
        public DateTime StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<int> StartPerson { get; set; }
        [ForeignKey("StartPerson")]
        public Users Users { get; set; }
        public Nullable<int> EndPerson { get; set; }
        [ForeignKey("EndPerson")]
        public Users Users1 { get; set; }
        public int IdInventory { get; set; }
        [ForeignKey("IdInventory")]
        public Inventorys Inventorys { get; set; }
    }
}
