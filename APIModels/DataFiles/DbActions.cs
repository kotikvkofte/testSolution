using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIModels.Models;
using Newtonsoft.Json;

namespace APIModels.DataFiles
{
    public static class DbActions
    {
        private static HttpClient httpClient = new HttpClient() 
        {
            BaseAddress = new Uri("https://u1689651.plsk.regruhosting.ru/api/")
        };
        //Inventory
        public static async Task<List<Inventorys>> GetInventorysAsync()
        {
            try
            {
                var res = await httpClient.GetAsync("Inventorys");
                var json = JsonConvert.DeserializeObject<List<Inventorys>>(await res.Content.ReadAsStringAsync());
                InventorysCount = json.Count;
                return json;
            }
            catch (Exception)
            {
                return null;
            }
            
        }


        public static List<Inventorys> GetInventorys()
        {
            var res = httpClient.GetAsync("Inventorys").Result;
            var json = JsonConvert.DeserializeObject<List<Inventorys>>(res.Content.ReadAsStringAsync().Result);
            InventorysCount = json.Count;
            return json;
        }
        public static Inventorys GetInventory(string code)
        {
            var res = httpClient.GetAsync($"Inventorys/{code}").Result;
            var json = JsonConvert.DeserializeObject<Inventorys>(res.Content.ReadAsStringAsync().Result);
            GetInventorys();
            return json;
        }
        public static bool PostInventory(Inventorys inventory)
        {
            var json = JsonConvert.SerializeObject(inventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Inventorys", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool PutInventory(Inventorys inventory)
        {

            var json = JsonConvert.SerializeObject(inventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            var response = httpClient.PutAsync("Inventorys", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteInventory(Inventorys inventory)
        {
            var response = httpClient.DeleteAsync($"Inventorys/{inventory.Id}").Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> DeleteInventorysAsync(List<Inventorys> inventorysList)
        {
            HttpResponseMessage response = null;
            foreach (var inv in inventorysList)
            {
                response = await httpClient.DeleteAsync($"Inventorys/{inv.Id}");
            }
            return response.IsSuccessStatusCode;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
        }
        //Roles
        public static int InventorysCount { get; private set; }
        public static List<Roles> GetRoles()
        {
            var res = httpClient.GetAsync("roles").Result;
            var json = JsonConvert.DeserializeObject<List<Roles>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PostRoles(Roles role)
        {
            var json = JsonConvert.SerializeObject(role);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("roles", data).Result;
            return response.IsSuccessStatusCode;
        }
        //Users
        public static List<Users> GetUsers()
        {
            var res = httpClient.GetAsync("users").Result;
            var json = JsonConvert.DeserializeObject<List<Users>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<Users>> GetUsersAsync()
        {
            var res = await httpClient.GetAsync("users");
            var json = JsonConvert.DeserializeObject<List<Users>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static Users GetUser(int id)
        {
            var res = httpClient.GetAsync($"users/{id}").Result;
            var json = JsonConvert.DeserializeObject<Users>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PostUsers(Users users)
        {
            var json = JsonConvert.SerializeObject(users);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("users", data).Result;
            return response.IsSuccessStatusCode;
        }

        //Locations
        public static List<Locations> GetLocations()
        {
            var res = httpClient.GetAsync("Locations").Result;
            var json = JsonConvert.DeserializeObject<List<Locations>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }

        public async static Task<List<Locations>> GetLocationsAsync()
        {
            var res = await httpClient.GetAsync("Locations");
            var json = JsonConvert.DeserializeObject<List<Locations>>(await res.Content.ReadAsStringAsync());
            return json;
        }

        public static Locations GetLocation(int id)
        {
            var res = httpClient.GetAsync($"Locations/{id}").Result;
            var json = JsonConvert.DeserializeObject<Locations>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PostLocations(Locations locations)
        {
            var json = JsonConvert.SerializeObject(locations);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Locations", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool PutLocation(Locations location)
        {

            var json = JsonConvert.SerializeObject(location);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync("Locations", data).Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteLocation(Locations location)
        {
            var response = httpClient.DeleteAsync($"Locations/{location.Id}").Result;
            return response.IsSuccessStatusCode;
        }
        //TypeOfInventory
        public static List<TypeOfInventory> GetTypeOfInventory()
        {
            var res = httpClient.GetAsync("TypeOfInventory").Result;
            var json = JsonConvert.DeserializeObject<List<TypeOfInventory>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<TypeOfInventory>> GetTypeOfInventoryAsync()
        {
            var res = await httpClient.GetAsync("TypeOfInventory");
            var json = JsonConvert.DeserializeObject<List<TypeOfInventory>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static bool PostTypeOfInventory(TypeOfInventory typeOfInventory)
        {
            var json = JsonConvert.SerializeObject(typeOfInventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("TypeOfInventory", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> PostTypeOfInventoryAsync(TypeOfInventory typeOfInventory)
        {

            var json = JsonConvert.SerializeObject(typeOfInventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("TypeOfInventory", data);
            return response.IsSuccessStatusCode;
        }
        public static bool PutTypeOfInventory(TypeOfInventory type)
        {
            var json = JsonConvert.SerializeObject(type);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync("TypeOfInventory", data).Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteTypeOfInventory(TypeOfInventory type)
        {
            var response = httpClient.DeleteAsync($"TypeOfInventory/{type.Id}").Result;
            return response.IsSuccessStatusCode;
        }
        //Manufacturers
        public static List<Manufacturers> GetManufacturers()
        {
            var res = httpClient.GetAsync("Manufacturers").Result;
            var json = JsonConvert.DeserializeObject<List<Manufacturers>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }

        public static async Task<List<Manufacturers>> GetManufacturersAsync()
        {
            var res = await httpClient.GetAsync("Manufacturers");
            var json = JsonConvert.DeserializeObject<List<Manufacturers>>(await res.Content.ReadAsStringAsync());
            return json;
        }

        public static bool PostManufacturers(Manufacturers manufacturer)
        {
            var json = JsonConvert.SerializeObject(manufacturer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Manufacturers", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool PutManufacturers(Manufacturers manufacturer)
        {
            var json = JsonConvert.SerializeObject(manufacturer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync("Manufacturers", data).Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteManufacturers(Manufacturers manufacturer)
        {
            var response = httpClient.DeleteAsync($"Manufacturers/{manufacturer.Id}").Result;
            return response.IsSuccessStatusCode;
        }
        //Providers
        public static List<Providers> GetProviders()
        {
            var res = httpClient.GetAsync("Providers").Result;
            var json = JsonConvert.DeserializeObject<List<Providers>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<Providers>> GetProvidersAsync()
        {
            var res = await httpClient.GetAsync("Providers");
            var json = JsonConvert.DeserializeObject<List<Providers>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static Providers GetProvider(int id)
        {
            var res = httpClient.GetAsync($"Providers/{id}").Result;
            var json = JsonConvert.DeserializeObject<Providers>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PostProviders(Providers providers)
        {
            var json = JsonConvert.SerializeObject(providers);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Providers", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool PutProviders(Providers providers)
        {
            var json = JsonConvert.SerializeObject(providers);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync("Providers", data).Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteProviders(Providers providers)
        {
            var response = httpClient.DeleteAsync($"Providers/{providers.Id}").Result;
            return response.IsSuccessStatusCode;
        }
        //Workplaces
        public static List<Workplaces> GetWorkplaces()
        {
            var res = httpClient.GetAsync("Workplaces").Result;
            var json = JsonConvert.DeserializeObject<List<Workplaces>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<Workplaces>> GetWorkplacesAsync()
        {
            var res = await httpClient.GetAsync("Workplaces");
            var json = JsonConvert.DeserializeObject<List<Workplaces>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static Workplaces GetWorkplace(int id)
        {
            var res = httpClient.GetAsync($"Workplaces/{id}").Result;
            var json = JsonConvert.DeserializeObject<Workplaces>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PostWorkplaces(Workplaces workplaces)
        {
            var json = JsonConvert.SerializeObject(workplaces);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Workplaces", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static bool DeleteWorkplaces(Workplaces workplaces)
        {
            var response = httpClient.DeleteAsync($"Workplaces/{workplaces.Id}").Result;
            return response.IsSuccessStatusCode;
        }
        public static bool PutWorkplaces(Workplaces workplaces)
        {

            var json = JsonConvert.SerializeObject(workplaces);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PutAsync("Workplaces", data).Result;
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        //Transfers
        public static List<Transfers> GetTransfers()
        {
            var res = httpClient.GetAsync("Transfers").Result;
            var json = JsonConvert.DeserializeObject<List<Transfers>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static bool PutTransfer(Transfers transfer)
        {
            var json = JsonConvert.SerializeObject(transfer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var res = httpClient.PutAsync("Transfers", data).Result;
            return res.IsSuccessStatusCode;
        }
        //Stocktaking
        public static List<Stocktaking> GetStocktaking()
        {
            var res = httpClient.GetAsync("Stocktaking").Result;
            var json = JsonConvert.DeserializeObject<List<Stocktaking>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<Stocktaking>> GetStocktakingAsync()
        {
            var res = await httpClient.GetAsync("Stocktaking");
            var json = JsonConvert.DeserializeObject<List<Stocktaking>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static bool PostStocktaking(Stocktaking stocktaking)
        {
            var json = JsonConvert.SerializeObject(stocktaking);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("Stocktaking", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> PostStocktakingAsync(Stocktaking stocktaking)
        {
            var json = JsonConvert.SerializeObject(stocktaking);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Stocktaking", data);
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> DeleteStocktakingAsync(Stocktaking stocktaking)
        {
            var response = await httpClient.DeleteAsync($"Stocktaking/{stocktaking.Id}");
            //File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            return response.IsSuccessStatusCode;
        }
        //StocktakingInventory
        public static List<StocktakingInventory> GetStocktakingInventory()
        {
            var res = httpClient.GetAsync("StocktakingInventory").Result;
            var json = JsonConvert.DeserializeObject<List<StocktakingInventory>>(res.Content.ReadAsStringAsync().Result);
            return json;
        }
        public static async Task<List<StocktakingInventory>> GetLiteStocktakingInventoryAsync()
        {
            var res = await httpClient.GetAsync("LiteStocktakingInventory");
            var json = JsonConvert.DeserializeObject<List<StocktakingInventory>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static async Task<List<StocktakingInventory>> GetStocktakingInventoryAsync()
        {
            var res = await httpClient.GetAsync("StocktakingInventory");
            var json = JsonConvert.DeserializeObject<List<StocktakingInventory>>(await res.Content.ReadAsStringAsync());
            return json;
        }
        public static bool PostStocktakingInventory(List<StocktakingInventory> stocktakingInventory)
        {
            var json = JsonConvert.SerializeObject(stocktakingInventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            var response = httpClient.PostAsync("StocktakingInventory", data).Result;
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> PostStocktakingInventoryAsync(List<StocktakingInventory> stocktakingInventory)
        {
            var json = JsonConvert.SerializeObject(stocktakingInventory);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            File.WriteAllText("C:\\Users\\ex9\\Desktop\\json.txt", json);
            var response =await httpClient.PostAsync("StocktakingInventory", data);
            return response.IsSuccessStatusCode;
        }
        public static async Task<bool> DeleteStockTakingInventoryAsync(StocktakingInventory stocktakingInventory)
        {
            var response = await httpClient.DeleteAsync($"StocktakingInventory/{stocktakingInventory.Id}");
            return response.IsSuccessStatusCode;
        }
    }
}
