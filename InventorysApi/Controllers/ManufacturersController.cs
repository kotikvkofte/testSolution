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
    public class ManufacturersController : Controller
    {
        DataContext db;
        public ManufacturersController(DataContext dataContext)
        {
            db = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manufacturers>>> Get()
        {
            return await db.Manufacturers.ToListAsync();

        }
        [HttpPost]
        public async Task<ActionResult<Manufacturers>> Post(Manufacturers manufacturer)
        {
            if (manufacturer == null || manufacturer.Name == null || manufacturer.Name.Replace(" ", "") == "")
            {
                BadRequest();
            }

            db.Manufacturers.Add(manufacturer);

            await db.SaveChangesAsync();

            return Ok(manufacturer);
        }
        [HttpPut]
        public async Task<ActionResult<Manufacturers>> Put(Manufacturers manufacturer)
        {
            if (manufacturer == null || manufacturer.Name == null || manufacturer.Name.Replace(" ", "") == "")
            {
                return BadRequest();
            }

            Manufacturers manufacturerNew = db.Manufacturers.FirstOrDefault(x => x.Id == manufacturer.Id);

            if (manufacturerNew == null)
            {
                return NotFound();
            }

            manufacturerNew.Name = manufacturer.Name;

            await db.SaveChangesAsync();

            return Ok(manufacturerNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Manufacturers>> Delete(int id)
        {
            Manufacturers  manufacturers = db.Manufacturers.FirstOrDefault(x => x.Id == id);
            if (manufacturers == null)
            {
                NotFound();
            }

            db.Manufacturers.Remove(manufacturers);
            await db.SaveChangesAsync();
            return Ok(manufacturers);
        }
    }
}
