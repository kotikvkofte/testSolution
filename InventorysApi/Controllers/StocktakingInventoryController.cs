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
    public class StocktakingInventoryController : Controller
    {
        DataContext db;
        public StocktakingInventoryController(DataContext context)
        {
            db = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StocktakingInventory>>> Get()
        {
            var result = db.StocktakingInventory.Include(x => x.Inventorys.Locations).Include(x => x.Inventorys.TypeOfInventory).Include(x => x.Inventorys.Manufacturers).Include(x => x.Inventorys.Providers).Include(x => x.Inventorys.Workplaces).Include(x => x.Stocktaking.Users.Roles).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return await result;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<StocktakingInventory>>> Post(List<StocktakingInventory> stocktakingInventorys)
        {
            if (stocktakingInventorys == null || stocktakingInventorys.Count == 0 )
            {
                return BadRequest();
            }

            foreach (StocktakingInventory stocktakingInventory in stocktakingInventorys)
            {
                if (stocktakingInventory.Inventorys != null)
                {
                    var addInventorys = db.Inventorys.FirstOrDefault(x => x.Id == stocktakingInventory.Inventorys.Id);

                    if (addInventorys != null)
                    {
                        stocktakingInventory.Inventorys = addInventorys;
                    }
                }

                if (stocktakingInventory.Stocktaking != null)
                {
                    var addStocktaking = db.Stocktaking.FirstOrDefault(x => x.Id == stocktakingInventory.Stocktaking.Id);

                    if (addStocktaking != null)
                    {
                        stocktakingInventory.Stocktaking = addStocktaking;
                    }
                }

                db.StocktakingInventory.Add(stocktakingInventory);
            }
            await db.SaveChangesAsync();

            return Ok(stocktakingInventorys);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<StocktakingInventory>>> Put(StocktakingInventory stocktakingInventory)
        {
            if (stocktakingInventory == null)
            {
                return BadRequest();
            }
            var stocktakingInventoryNew = db.StocktakingInventory.FirstOrDefault(x => x.Id == stocktakingInventory.Id);

            if (stocktakingInventoryNew == null)
            {
                return NotFound();
            }

            if (stocktakingInventory.Inventorys != null)
            {
                var inventory = db.Inventorys.FirstOrDefault(x => x.Id == stocktakingInventory.Inventorys.Id);
                if (inventory != null)
                {
                    stocktakingInventoryNew.Inventorys = inventory;
                }
                else
                {
                    stocktakingInventoryNew.Inventorys = stocktakingInventory.Inventorys;
                }
            }

            if (stocktakingInventory.Stocktaking != null)
            {
                var stocktaking = db.Stocktaking.FirstOrDefault(x => x.Id == stocktakingInventory.Stocktaking.Id);
                if (stocktaking != null)
                {
                    stocktakingInventoryNew.Stocktaking = stocktaking;
                }
                else
                {
                    stocktakingInventoryNew.Stocktaking = stocktakingInventory.Stocktaking;
                }
            }
            await db.SaveChangesAsync();

            return Ok(stocktakingInventoryNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<StocktakingInventory>> Delete(int id)
        {
            StocktakingInventory stocktakingInventory = db.StocktakingInventory.FirstOrDefault(x => x.Id == id);

            if (stocktakingInventory == null)
            {
                return NotFound();
            }

            db.StocktakingInventory.Remove(stocktakingInventory);

            await db.SaveChangesAsync();

            return Ok(stocktakingInventory);
        }
    }
}
