using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PreFinal
{
    public class writeOffClass
    {
        public static ObservableCollection<Inventorys> writeOffList { get; set; }

        static writeOffClass()
        {
            writeOffList = new ObservableCollection<Inventorys>();
        }

        public static async void wtiteOff()
        {
            if (writeOffList.Count == 0)
            {
                MessageBox.Show("Список пуст");
                return;
            }
            const string template = "file.docx";
            var helper = new WordHelper(System.IO.Path.Combine(Environment.CurrentDirectory, template));
            List<Inventorys> newList = new List<Inventorys>((IEnumerable<Inventorys>)writeOffList);
            double TotalSum = 0;
            foreach (var item in newList)
            {
                TotalSum += item.Price;
            }
            var items = new Dictionary<string, string>
            {
                {"<CurrentDate>", DateTime.Now.Date.ToString("dd.MM.yyyy")},
                {"<TotalSum>", TotalSum.ToString()}
            };
            if (helper.Process(items, newList))
            {

                foreach (var item in newList)
                {
                    writeOffList.Remove(item);
                }
                await DbActions.DeleteInventorysAsync(newList);
                await StaticHtppClass.HttpData.GetMainInventoryListAsync();
                MessageBox.Show("Выполнено");
            }
        }

        public static void openFolder()
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Акты списания");
            Process.Start("explorer.exe", path);
        }

        public static void addToWriteOffList(Inventorys inv)
        {
            if (writeOffList.FirstOrDefault(x => x.Id == inv.Id) == null)
            {
                writeOffList.Add(inv);
                MessageBox.Show("Добавленио в список на списание", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public static void deleteFromWriteOffList(Inventorys inv)
        {
            writeOffList.Remove(inv);
        }
    }
}
