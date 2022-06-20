using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreFinal.DataFiles
{
    public static class InventorysList
    {
        private static List<Inventorys> CurrentInventory = new List<Inventorys>();

        public static void SetCurrentList(List<Inventorys> ik)
        {
            CurrentInventory = ik;
        }

        public static void ClearCurrentInventoy() => CurrentInventory.Clear();

        /// <summary>
        /// возвращает количество текущих элементов листа
        /// (для счетчика)
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentInventoyNumb()
        {

            if (CurrentInventory == null)
            {
                return 0;
            }
            else
            {
                return CurrentInventory.Count();
            }
        }
        /// <summary>
        /// Добавляет объект в лист CurrentInventory
        /// </summary>
        /// <param name="inventory">Объект инвентаря</param>
        public static void AddToCurrentInventoy(Inventorys inventory)
        {
            if (CurrentInventory == null)
            {
                CurrentInventory.Add(inventory);
            }
            if (CurrentInventory.FirstOrDefault(x => x == inventory) == null)
            {
                CurrentInventory.Add(inventory);
            }
        }
        /// <summary>
        /// Возвращает неотсканированные элементы CurrentInventory
        /// </summary>
        /// <returns></returns>
        public static List<Inventorys> GetResultInventoryList()
        {
            var res = DbActions.GetInventorys().Except(CurrentInventory).ToList();
            if (UserInfo.user.IdRole != 1 && UserInfo.user != null)
            {
                res = DbActions.GetInventorys().Where(x => x.Workplaces.Locations.Users.Id == UserInfo.user.Id).ToList();

            }
            return res;
        }
        /// <summary>
        /// возвращает CurrentInventory
        /// </summary>
        /// <returns></returns>
        public static List<Inventorys> GetCurrentInventoryList()
        {
            return CurrentInventory.ToList();
        }
    }
}
