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
    public class LocationsController : Controller
    {
        DataContext db;
        public LocationsController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Locations>>> Get()
        {
            return await db.Locations.Include(x=> x.Users.Roles).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Locations>> Get(int id)
        {
            return await db.Locations.Include(x => x.Users.Roles).FirstOrDefaultAsync(x => x.Id == id);
        }
        [HttpPost]
        public async Task<ActionResult<Locations>> Post(Locations location)
        {
            if (location == null || location.Location == null)
            {
                return BadRequest();
            }

            if (location.Users != null)
            {
                var addUser = db.Users.FirstOrDefault(x => x.Id == location.Users.Id);

                if (addUser != null)
                {
                    location.Users = addUser;
                }
            }
            db.Locations.Add(location);

            await db.SaveChangesAsync();

            return Ok(location);
        }
        [HttpPut]
        public async Task<ActionResult<Locations>> Put(Locations location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            var locationNew = db.Locations.FirstOrDefault(x => x.Id == location.Id);

            if (locationNew == null)
            {
                return NotFound();
            }
            
            if (location.Location != null)
            {
                locationNew.Location = location.Location;
            }

            if (location.Users != null)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == location.Users.Id);
                if (user != null)
                {
                    locationNew.Users = user;
                }
                else
                {
                    locationNew.Users = location.Users;
                }
            }

            await db.SaveChangesAsync();

            return Ok(locationNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Locations>> Delete(int id)
        {
            Locations location = db.Locations.FirstOrDefault(x => x.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);

            await db.SaveChangesAsync();

            return Ok(location);
        }
    }
}
