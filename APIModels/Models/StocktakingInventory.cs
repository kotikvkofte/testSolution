using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIModels.Models
{
    public class StocktakingInventory
    {
        public int Id { get; set; }
        public int IdInventory { get; set; }
        [ForeignKey("IdInventory")]
        public Inventorys Inventorys { get; set; }
        public int IdStocktaking { get; set; }
        [ForeignKey("IdStocktaking")]
        public Stocktaking Stocktaking { get; set; }
    }
}
