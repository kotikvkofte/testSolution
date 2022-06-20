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
    public class TypeOfInventoryController : Controller
    {
        DataContext db;
        public TypeOfInventoryController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOfInventory>>> Get()
        {
            var result = db.TypeOfInventory.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return await result;
        }
        [HttpPost]
        public async Task<ActionResult<TypeOfInventory>> Post(TypeOfInventory type)
        {
            if (type == null || type.Type == null || type.Type.Replace(" ","") == "")
            {
                BadRequest();
            }

            db.TypeOfInventory.Add(type);

            await db.SaveChangesAsync();

            return Ok(type);
        }
        [HttpPut]
        public async Task<ActionResult<TypeOfInventory>> Put(TypeOfInventory type)
        {
            if (type == null || type.Type == null || type.Type.Replace(" ", "") == "")
            {
                return BadRequest();
            }
            TypeOfInventory typeNew = db.TypeOfInventory.FirstOrDefault(x => x.Id == type.Id);

            if (typeNew == null)
            {
                return NotFound();
            }

            typeNew.Type = type.Type;

            await db.SaveChangesAsync();

            return Ok(typeNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TypeOfInventory>> Delete(int id)
        {
            TypeOfInventory type = db.TypeOfInventory.FirstOrDefault(x => x.Id == id);

            if (type == null)
            {
                NotFound();
            }

            db.TypeOfInventory.Remove(type);
            await db.SaveChangesAsync();
            return Ok(type);
        }
    }
}
