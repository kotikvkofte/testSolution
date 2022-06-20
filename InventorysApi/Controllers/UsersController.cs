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
    public class UsersController : Controller
    {
        DataContext db;
        public UsersController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Get()
        {
            var result = db.Users.Include(x => x.Roles).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return await result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get(int id)
        {
            Users user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<Users>> Post(Users user)
        {
            if (user == null || user.FirstName == null || user.FirstName == "" || user.Surname == null || user.Surname =="")
            {
                return BadRequest();
            }

            if (user.Roles != null)
            {
                var addRole = db.Roles.FirstOrDefault(x => x.Id == user.Roles.Id);

                if (addRole != null)
                {
                    user.Roles = addRole;
                }
            }
            db.Users.Add(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPut]
        public async Task<ActionResult<Users>> Put(Users user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var userNew = db.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userNew == null)
            {
                return NotFound();
            }

            if (user.Login != null)
            {
                userNew.Login = user.Login;
            }

            if (user.Password != null)
            {
                userNew.Password = user.Password;
            }

            if (user.FirstName != null)
            {
                userNew.FirstName = user.FirstName;
            }

            if (user.Surname != null)
            {
                userNew.Surname = user.Surname;
            }

            if (user.Patronymic != null)
            {
                userNew.Patronymic = user.Patronymic;
            }

            if (user.Roles != null)
            {
                var role = db.Roles.FirstOrDefault(x => x.Id == user.Roles.Id);
                if (role != null)
                {
                    userNew.Roles = role;
                }
                else
                {
                    userNew.Roles = user.Roles;
                }

            }

            await db.SaveChangesAsync();

            return Ok(userNew);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> Delete(int id)
        {
            Users user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }
    }
}
