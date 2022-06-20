using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIModels.Models
{
    public class Workplaces
    {
        public int Id { get; set; }

        public string Place { get; set; }
        public Nullable<int> IdLocation { get; set; }
        [ForeignKey("IdLocation")]
        public Locations Locations { get; set; }
    }
}
