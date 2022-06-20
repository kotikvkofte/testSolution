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
    public class StocktakingController : Controller
    {
        DataContext db;
        public StocktakingController(DataContext dataContext)
        {
            db = dataContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stocktaking>>> Get()
        {
            return await db.Stocktaking.Include(x => x.Users.Roles).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Stocktaking>>> Post(Stocktaking stocktaking)
        {
            if (stocktaking == null || stocktaking.Users == null || stocktaking.Date == null)
            {
                return BadRequest();
            }

            if (stocktaking.Users != null)
            {
                var addUser = db.Users.FirstOrDefault(x => x.Id == stocktaking.Users.Id);

                if (addUser != null)
                {
                    stocktaking.Users = addUser;
                }
            }
            db.Stocktaking.Add(stocktaking);

            await db.SaveChangesAsync();

            return Ok(stocktaking);
        }
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Stocktaking>>> Put(Stocktaking stocktaking)
        {
            if (stocktaking == null)
            {
                return BadRequest();
            }
            var stocktakingNew = db.Stocktaking.FirstOrDefault(x => x.Id == stocktaking.Id);

            if (stocktakingNew == null)
            {
                return NotFound();
            }

            if (stocktaking.Date != null)
            {
                stocktakingNew.Date = stocktaking.Date;
            }

            if (stocktaking.Users != null)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == stocktaking.Users.Id);
                if (user != null)
                {
                    stocktakingNew.Users = user;
                }
                else
                {
                    stocktakingNew.Users = stocktaking.Users;
                }
            }

            await db.SaveChangesAsync();

            return Ok(stocktakingNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stocktaking>> Delete(int id)
        {
            Stocktaking stocktaking = db.Stocktaking.FirstOrDefault(x => x.Id == id);

            if (stocktaking == null)
            {
                return NotFound();
            }

            db.Stocktaking.Remove(stocktaking);

            await db.SaveChangesAsync();

            return Ok(stocktaking);
        }
    }
}
