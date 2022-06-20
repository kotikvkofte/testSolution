using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventorysApi.Models
{
    public class StocktakingInventory
    {
        public int Id { get; set; }
        //[JsonIgnore]
        public int IdInventory { get; set; }
        [ForeignKey("IdInventory")]
        public Inventorys Inventorys { get; set; }
        //[JsonIgnore]
        public int IdStocktaking { get; set; }
        [ForeignKey("IdStocktaking")]
        public Stocktaking Stocktaking { get; set; }
    }
}
