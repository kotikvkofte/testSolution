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
    public class TransfersController : Controller
    {
        DataContext db;
        public TransfersController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transfers>>> Get()
        {
            return await db.Transfers.Include(x => x.Inventorys.Manufacturers).Include(x=> x.Inventorys.TypeOfInventory).Include(x=> x.Inventorys.Providers).Include(x=> x.Workplaces.Locations).Include(x=>x.Workplaces1.Locations).Include(x=>x.Users.Roles).Include(x => x.Users1.Roles).ToListAsync();
            //return await db.Transfers.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Transfers>> Post(Transfers transfer)
        {
            if (transfer.StartDate == null)
            {
                BadRequest();
            }
            if (transfer.Inventorys == null)
            {
                NotFound();
            }
            if (transfer.Users != null)
            {
                var addUser = db.Users.FirstOrDefault(x => x.Id == transfer.Users.Id);
                if (addUser != null)
                {
                    transfer.Users = addUser;
                }
            }
            if (transfer.Users1 != null)
            {
                var addUser = db.Users.FirstOrDefault(x => x.Id == transfer.Users1.Id);
                if (addUser != null)
                {
                    transfer.Users1 = addUser;
                }
            }
            if (transfer.Workplaces != null)
            {
                var addWorkplace = db.Workplaces.FirstOrDefault(x => x.Id == transfer.Workplaces.Id);
                if (addWorkplace != null)
                {
                    transfer.Workplaces = addWorkplace;
                }
            }
            if (transfer.Workplaces1 != null)
            {
                var addWorkplace = db.Workplaces.FirstOrDefault(x => x.Id == transfer.Workplaces1.Id);
                if (addWorkplace != null)
                {
                    transfer.Workplaces1 = addWorkplace;
                }
            }
            db.Transfers.Add(transfer);
            await db.SaveChangesAsync();
            return Ok(transfer);
        }
        [HttpPut]
        public async Task<ActionResult<Transfers>> Put(Transfers transfer)
        {
            if (transfer == null || transfer.Id == null)
            {
                BadRequest();
            }

            var tranferNew = db.Transfers.FirstOrDefault(x => x.Id == transfer.Id);

            if (tranferNew == null)
            {
                NotFound();
            }
            if (transfer.Inventorys != null)
            {
                var inventory = db.Inventorys.FirstOrDefault(x => x.Id == transfer.Inventorys.Id);
                if (inventory != null)
                {
                    tranferNew.Inventorys = inventory;
                }
                else
                {
                    tranferNew.Inventorys = transfer.Inventorys;
                }
            }
            if (transfer.Users != null)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == transfer.Users.Id);
                if (user != null)
                {
                    tranferNew.Users = user;
                }
                else
                {
                    tranferNew.Users = transfer.Users;
                }
            }
            if (transfer.Users1 != null)
            {
                var user1 = db.Users.FirstOrDefault(x => x.Id == transfer.Users1.Id);
                if (user1 != null)
                {
                    tranferNew.Users1 = user1;
                }
                else
                {
                    tranferNew.Users1 = transfer.Users1;
                }
            }
            if (transfer.Workplaces != null)
            {
                var workplace = db.Workplaces.FirstOrDefault(x => x.Id == transfer.Workplaces.Id);
                if (workplace != null)
                {
                    tranferNew.Workplaces = workplace;
                }
                else
                {
                    tranferNew.Workplaces = transfer.Workplaces;
                }
            }
            if (transfer.Workplaces1 != null)
            {
                var workplace1 = db.Workplaces.FirstOrDefault(x => x.Id == transfer.Workplaces1.Id);
                if (workplace1 != null)
                {
                    tranferNew.Workplaces1 = workplace1;
                }
                else
                {
                    tranferNew.Workplaces1 = transfer.Workplaces1;
                }
            }
            if (transfer.EndDate != null)
            {
                tranferNew.EndDate = transfer.EndDate;
            }

            await db.SaveChangesAsync();
            return Ok(tranferNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transfers>> Delete(int id)
        {
            Transfers transfer = db.Transfers.FirstOrDefault(x => x.Id == id);
            if (transfer == null)
            {
                return NotFound();
            }
            db.Transfers.Remove(transfer);
            await db.SaveChangesAsync();
            return Ok(transfer);
        }
    }
}
