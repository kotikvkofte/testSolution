using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class Workplaces
    {
        public int Id { get; set; }

        public string Place { get; set; }
        [JsonIgnore]
        public Nullable<int> IdLocation { get; set; }
        [ForeignKey("IdLocation")]
        public Locations Locations { get; set; }
    }
}
