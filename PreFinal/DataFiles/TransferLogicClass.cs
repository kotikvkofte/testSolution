using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PreFinal.DataFiles
{
    public static class TransferLogicClass
    {
        public static void CreateTransfer(Inventorys oldInv, Inventorys newInv)
        {
            try
            {

                Transfers transfers = new Transfers()
                {
                    StartDate = DateTime.Now,
                    StartPoint = oldInv.Workplaces.Id,

                    EndPoint = newInv.Workplaces.Id,
                    IdInventory = newInv.Id
                };

                if (oldInv.Workplaces.Locations.Users != null)
                {
                    transfers.StartPerson = oldInv.Workplaces.Locations.Users.Id;
                }
                if (newInv.Workplaces.Locations.Users != null)
                {
                    transfers.EndPerson = newInv.Workplaces.Locations.Users.Id;
                }

                //OdbConnectHelper.entObj.Transfers.Add(transfers);
                //OdbConnectHelper.entObj.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void CheckDeliver(Inventorys checkInv, Users user)
        {
            if (DbActions.GetLocations().FirstOrDefault(x => x.Users.Id == user.Id) != null)
            {
                try
                {
                    Transfers transfers = DbActions.GetTransfers().FirstOrDefault(x => x.Inventorys.InventoryCode == checkInv.InventoryCode);

                    if (transfers != null && transfers.Workplaces1.Locations.Users.Id == user.Id)
                    {
                        transfers.EndDate = DateTime.Now;
                        MessageBox.Show("Инвентарь добрался до вас!!!");
                        DbActions.PutTransfer(transfers);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
