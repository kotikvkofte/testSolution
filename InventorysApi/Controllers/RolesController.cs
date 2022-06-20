using InventorysApi.DataFiles;
using InventorysApi.Models;
using Microsoft.AspNetCore.Http;
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
    public class RolesController : ControllerBase
    {
        DataContext db;
        public RolesController(DataContext dataContext)
        {
            db = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> Get()
        {
            return await db.Roles.ToListAsync();
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Roles>>> Get(string name)
        {
            List<Roles> roles = await db.Roles.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }
        [HttpPost]
        public async Task<ActionResult<Roles>> Post(Roles role)
        {
            if (role == null || role.Name == null || role.Name.Replace(" ","") == "")
            {
                return BadRequest();
            }

            db.Roles.Add(role);

            await db.SaveChangesAsync();

            return Ok(role);
        }
        [HttpPut]
        public async Task<ActionResult<Roles>> Put(Roles role)
        {
            if (role == null || role.Name == null || role.Name.Replace(" ", "") == "")
            {
                return BadRequest();
            }
            Roles roleNew = db.Roles.FirstOrDefault(x => x.Id == role.Id);

            if (roleNew == null)
            {
                return NotFound();
            }

            roleNew.Name = role.Name;

            await db.SaveChangesAsync();

            return Ok(roleNew);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Roles>> Delete(int id)
        {
            Roles role = db.Roles.FirstOrDefault(x => x.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            db.Roles.Remove(role);

            await db.SaveChangesAsync();

            return Ok(role);
        }
    }
}
