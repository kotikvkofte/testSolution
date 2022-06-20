using InventorysApi.DataFiles;
using InventorysApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteStocktakingInventoryController : Controller
    {
        DataContext db;
        public LiteStocktakingInventoryController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StocktakingInventory>>> Get()
        {
            var result = db.StocktakingInventory.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return await result;
        }
    }
}
