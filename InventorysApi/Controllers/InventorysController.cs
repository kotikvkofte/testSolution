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
    public class InventorysController : Controller
    {
        DataContext db;
        public InventorysController(DataContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventorys>>> Get()
        {
            var result = db.Inventorys.Include(x => x.Locations).Include(x => x.Workplaces).Include(x => x.TypeOfInventory).Include(x => x.Manufacturers).Include(x => x.Providers).Include(x => x.Locations.Users.Roles).Include(x => x.Locations.Users).ToListAsync();
            if (result == null)
            {
                return BadRequest();
            }
            return await result;
        }
        [HttpGet("{code}")]
        public async Task<ActionResult<Inventorys>> Get(string code)
        {
            Inventorys inventory = await db.Inventorys.Include(x => x.Locations).Include(x => x.Workplaces).Include(x => x.TypeOfInventory).Include(x => x.Manufacturers).Include(x => x.Providers).Include(x => x.Locations.Users.Roles).Include(x => x.Locations.Users).FirstOrDefaultAsync(x => x.InventoryCode == code);
            if (inventory == null)
            {
                NotFound();
            }
            return Ok(inventory);
        }
        [HttpPost]
        //public async Task<ActionResult<Inventorys>> Post(inventorysIFormFiles inventory)
        //{
        //    if (inventory.InventoryCode == null || inventory.InventoryCode.Replace(" ", "") == "" || inventory.Name.Replace(" ", "") == "" || inventory.Name == null || inventory.Price == null || inventory.Amount == null)
        //    {
        //        NotFound();
        //    }

        //    if (inventory.Manufacturers != null)
        //    {
        //        var addManufacture = db.Manufacturers.FirstOrDefault(x => x.Id == inventory.Manufacturers.Id);
        //        if (addManufacture != null)
        //        {
        //            inventory.Manufacturers = addManufacture;
        //        }
        //    }
        //    if (inventory.Providers != null)
        //    {
        //        var addProvider = db.Providers.FirstOrDefault(x => x.Id == inventory.Providers.Id);
        //        if (inventory.Providers != null)
        //        {
        //            inventory.Providers = addProvider;
        //        }
        //    }
        //    if (inventory.TypeOfInventory != null)
        //    {
        //        var addType = db.TypeOfInventory.FirstOrDefault(x => x.Id == inventory.TypeOfInventory.Id);
        //        if (inventory.TypeOfInventory != null)
        //        {
        //            inventory.TypeOfInventory = addType;
        //        }
        //    }
        //    if (inventory.Locations != null)
        //    {
        //        var addLocations = db.Locations.FirstOrDefault(x => x.Id == inventory.Locations.Id);
        //        if (inventory.Locations != null)
        //        {
        //            inventory.Locations = addLocations;
        //        }
        //    }
        //    if (inventory.Workplaces != null)
        //    {
        //        var addWorkplace = db.Workplaces.FirstOrDefault(x => x.Id == inventory.Workplaces.Id);
        //        if (inventory.Locations != null)
        //        {
        //            if (addWorkplace.Locations == inventory.Locations)
        //            {
        //                if (inventory.Workplaces != null)
        //                {
        //                    inventory.Workplaces = addWorkplace;
        //                    inventory.Locations = addWorkplace.Locations;
        //                }
        //            }
        //            else
        //            {
        //                return BadRequest();
        //            }
        //        }
        //        else
        //        {
        //            if (inventory.Workplaces != null)
        //            {
        //                inventory.Locations = addWorkplace.Locations;
        //                inventory.Workplaces = addWorkplace;
        //            }
        //        }
        //    }

        //    Inventorys inv = new Inventorys()
        //    {
        //        Amount = inventory.Amount,
        //        Description = inventory.Description,
        //        Locations = inventory.Locations,
        //        Price = inventory.Price,
        //        Workplaces = inventory.Workplaces,
        //        Name = inventory.Name,
        //        TypeOfInventory = inventory.TypeOfInventory,
        //        Manufacturers = inventory.Manufacturers,
        //        Providers = inventory.Providers,
        //        InventoryCode = inventory.InventoryCode
        //    };

        //    byte[] imageData = null;
        //    // считываем переданный файл в массив байтов
        //    using (var binaryReader = new System.IO.BinaryReader(inventory.Photo.OpenReadStream()))
        //    {
        //        imageData = binaryReader.ReadBytes((int)inventory.Photo.Length);
        //    }
        //    // установка массива байтов
        //    inv.Photo = imageData;

        //    db.Inventorys.Add(inv);
        //    await db.SaveChangesAsync();
        //    return Ok(inventory);
        //}
        public async Task<ActionResult<Inventorys>> Post(Inventorys inventory)
        {
            if (inventory.InventoryCode == null || inventory.InventoryCode.Replace(" ", "") == "" || inventory.Name.Replace(" ", "") == "" || inventory.Name == null || inventory.Price == null || inventory.Amount == null)
            {
                NotFound();
            }

            if (inventory.Manufacturers != null)
            {
                var addManufacture = db.Manufacturers.FirstOrDefault(x => x.Id == inventory.Manufacturers.Id);
                if (addManufacture != null)
                {
                    inventory.Manufacturers = addManufacture;
                }
            }
            if (inventory.Providers != null)
            {
                var addProvider = db.Providers.FirstOrDefault(x => x.Id == inventory.Providers.Id);
                if (addProvider != null)
                {
                    inventory.Providers = addProvider;
                }
            }
            if (inventory.TypeOfInventory != null)
            {
                var addType = db.TypeOfInventory.FirstOrDefault(x => x.Id == inventory.TypeOfInventory.Id);
                if (addType != null)
                {
                    inventory.TypeOfInventory = addType;
                }
            }
            if (inventory.Locations != null)
            {
                var addLocations = db.Locations.FirstOrDefault(x => x.Id == inventory.Locations.Id);
                if (addLocations != null)
                {
                    inventory.Locations = addLocations;
                }
            }
            if (inventory.Workplaces != null)
            {
                var addWorkplace = db.Workplaces.FirstOrDefault(x => x.Id == inventory.Workplaces.Id);
                if (inventory.Locations != null)
                {
                    if (addWorkplace.Locations == inventory.Locations)
                    {
                        if (inventory.Workplaces != null)
                        {
                            inventory.Workplaces = addWorkplace;
                            inventory.Locations = addWorkplace.Locations;
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    if (inventory.Workplaces != null)
                    {
                        inventory.Locations = addWorkplace.Locations;
                        inventory.Workplaces = addWorkplace;
                    }
                }
            }
            db.Inventorys.Add(inventory);
            await db.SaveChangesAsync();
            return Ok(inventory);
        }
        [HttpPut]
        public async Task<ActionResult<Inventorys>> Put(Inventorys inventory)
        {
            if (inventory == null || inventory.Id == null)
            {
                BadRequest();
            }

            var inventoryNew = db.Inventorys.Include(x => x.TypeOfInventory).Include(x => x.Manufacturers).Include(x => x.Providers).Include(x => x.Workplaces.Locations.Users.Roles).FirstOrDefault(x => x.Id == inventory.Id);

            if (inventoryNew == null)
            {
                NotFound();
            }

            if (inventory.InventoryCode != null)
            {
                inventoryNew.InventoryCode = inventory.InventoryCode;
            }
            if (inventory.Name != null)
            {
                inventoryNew.Name = inventory.Name;
            }
            if (inventory.Amount != null)
            {
                inventoryNew.Amount = inventory.Amount;
            }
            if (inventory.Price != null)
            {
                inventoryNew.Price = inventory.Price;
            }

            inventoryNew.Description = inventory.Description;
            
            
            inventoryNew.Photo = inventory.Photo;
            
            if (inventory.Manufacturers != null)
            {
                var manufacture = db.Manufacturers.FirstOrDefault(x => x.Id == inventory.Manufacturers.Id);

                if(manufacture != null)
                {
                    inventoryNew.Manufacturers = manufacture;
                }
                else
                {
                    inventoryNew.Manufacturers = inventory.Manufacturers;
                }
            }
            if (inventory.Locations != null)
            {
                var location = db.Locations.FirstOrDefault(x => x.Id == inventory.Locations.Id);
                if (location != null)
                {
                    inventoryNew.Locations = location;

                    if (inventory.Workplaces == null)
                    {
                        inventoryNew.Workplaces = null;
                    }
                }
                else
                {
                    inventoryNew.Locations = inventory.Locations;
                    if (inventory.Workplaces == null)
                    {
                        inventoryNew.Workplaces = null;
                    }
                }

            }
            if (inventory.Workplaces != null)
            {
                var workplace = db.Workplaces.FirstOrDefault(x => x.Id == inventory.Workplaces.Id);
                if (inventory.Locations != null)
                {
                    if (workplace != null)
                    {
                        if (workplace.Locations.Id == inventoryNew.Locations.Id)
                        {
                            inventoryNew.Workplaces = workplace;
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        if (inventory.Workplaces.Locations.Id == inventoryNew.Locations.Id)
                        {
                            inventoryNew.Workplaces = inventory.Workplaces;
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    if (workplace != null)
                    {
                        if (inventoryNew.Locations == null)
                        {
                            inventoryNew.Locations = workplace.Locations;

                        }
                        inventoryNew.Workplaces = workplace;
                    }
                    else
                    {
                        if (inventoryNew.Locations == null)
                        {
                            inventoryNew.Locations = inventory.Workplaces.Locations;
                        }
                        inventoryNew.Workplaces = inventory.Workplaces;
                    }
                }
            }
            
            if (inventory.TypeOfInventory != null)
            {
                var type = db.TypeOfInventory.FirstOrDefault(x => x.Id == inventory.TypeOfInventory.Id);
                if (type != null)
                {
                    inventoryNew.TypeOfInventory = type;
                }
                else
                {
                    inventoryNew.TypeOfInventory = inventory.TypeOfInventory;
                }
            }
            if (inventory.Providers != null)
            {
                var provider = db.Providers.FirstOrDefault(x => x.Id == inventory.Providers.Id);
                if (provider != null)
                {
                    inventoryNew.Providers = provider;
                }
                else
                {
                    inventoryNew.Providers = inventory.Providers;
                }
            }
            db.Inventorys.Update(inventoryNew);
            await db.SaveChangesAsync();

            return Ok(inventoryNew);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inventorys>> Delete(int id)
        {
            Inventorys inventory = db.Inventorys.FirstOrDefault(x => x.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }
            db.Inventorys.Remove(inventory);
            await db.SaveChangesAsync();
            return Ok(inventory);
        }
    }
}
