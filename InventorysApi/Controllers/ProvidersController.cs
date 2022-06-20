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
    public class ProvidersController : Controller
    {
        DataContext db;
        public ProvidersController(DataContext dataContext)
        {
            db = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Providers>>> Get()
        {
            return await db.Providers.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Providers>> Get(int id)
        {
            return await db.Providers.FirstOrDefaultAsync(x => x.Id == id);

        }
        [HttpPost]
        public async Task<ActionResult<Providers>> Post(Providers provider)
        {

            if (provider == null || provider.Name == null || provider.Name.Replace(" ", "") == "")
            {
                BadRequest();
            }

            db.Providers.Add(provider);

            await db.SaveChangesAsync();

            return Ok(provider);
        }
        [HttpPut]
        public async Task<ActionResult<Providers>> Put(Providers provider)
        {
            if (provider == null || provider.Name == null || provider.Name.Replace(" ", "") == "")
            {
                return BadRequest();
            }

            Providers providerNew = db.Providers.FirstOrDefault(x => x.Id == provider.Id);

            if (providerNew == null)
            {
                return NotFound();
            }

            providerNew.Name = provider.Name;

            await db.SaveChangesAsync();

            return Ok(providerNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Providers>> Delete(int id)
        {
            Providers provider = db.Providers.FirstOrDefault(x => x.Id == id);
            if (provider == null)
            {
                NotFound();
            }

            db.Providers.Remove(provider);
            await db.SaveChangesAsync();
            return Ok(provider);
        }
    }
}
