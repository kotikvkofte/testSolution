using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class Locations
    {
        public int Id { get; set; }
        
        public string Location { get; set; }
        [JsonIgnore]
        public Nullable<int> Responsible_Person { get; set; }
        [ForeignKey("Responsible_Person")]
        public Users Users { get; set; }
    }
}
