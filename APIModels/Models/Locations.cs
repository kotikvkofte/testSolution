using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIModels.Models
{
    public class Locations
    {
        public int Id { get; set; }
        
        public string Location { get; set; }
        public Nullable<int> Responsible_Person { get; set; }
        [ForeignKey("Responsible_Person")]
        public Users Users { get; set; }
    }
}
