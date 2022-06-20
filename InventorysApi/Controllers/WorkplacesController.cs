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
    public class WorkplacesController : Controller
    {
        DataContext db;
        public WorkplacesController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workplaces>>> Get()
        {
            return await db.Workplaces.Include(x => x.Locations.Users.Roles).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Workplaces>> Get(int id)
        {
            return await db.Workplaces.Include(x => x.Locations.Users.Roles).FirstOrDefaultAsync(x => x.Id == id);
        }
        [HttpPost]
        public async Task<ActionResult<Workplaces>> Post(Workplaces workplace)
        {
            if (workplace == null || workplace.Place == null || workplace.Place.Replace(" ", "") == "")
            {
                BadRequest();
            }

            if (workplace.Locations != null)
            {
                var addLocation = db.Locations.FirstOrDefault(x=> x.Id == workplace.Locations.Id);
                if (addLocation!= null)
                {
                    workplace.Locations = addLocation;
                }
            }

            db.Workplaces.Add(workplace);
            await db.SaveChangesAsync();
            return Ok(workplace);
        }
        [HttpPut]
        public async Task<ActionResult<Workplaces>> Put(Workplaces workplace)
        {
            if (workplace == null)
            {
                BadRequest();
            }

            var workplaceNew = db.Workplaces.FirstOrDefault(x => x.Id == workplace.Id);

            if (workplaceNew == null)
            {
                return NotFound();
            }

            if (workplace.Place != null)
            {
                workplaceNew.Place = workplace.Place;
            }

            if (workplace.Locations != null)
            {
                var location = db.Locations.FirstOrDefault(x => x.Id == workplace.Locations.Id);

                if (location != null)
                {
                    workplaceNew.Locations = location;
                }
                else
                {
                    workplaceNew.Locations = workplace.Locations;
                }
            }

            await db.SaveChangesAsync();

            return Ok(workplaceNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Workplaces>> Delete(int id)
        {
            Workplaces workplace = db.Workplaces.FirstOrDefault(x => x.Id == id);
            if (workplace == null)
            {
                return NotFound();
            }
            db.Workplaces.Remove(workplace);
            await db.SaveChangesAsync();
            return Ok(workplace);
        }
    }
}
