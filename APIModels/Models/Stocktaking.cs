using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIModels.Models
{
    public class Stocktaking
    {
        public int Id { get; set; }
        public Nullable<int> IdUser { get; set; }
        [ForeignKey("IdUser")]
        public Users Users { get; set; }
        public DateTime Date { get; set; }
    }
}
