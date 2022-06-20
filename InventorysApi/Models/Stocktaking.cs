using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class Stocktaking
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Nullable<int> IdUser { get; set; }
        [ForeignKey("IdUser")]
        public Users Users { get; set; }
        public DateTime Date { get; set; }
    }
}
