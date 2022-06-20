using APIModels.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows;

namespace PreFinal
{
    public class PrintClass
    {
        public static ObservableCollection<Inventorys> PrintInventorysList { get; set; }

        public static void addToPrintList(Inventorys newInv)
        {
            var check = PrintInventorysList.FirstOrDefault(x => x.Id == newInv.Id);
            if (check == null)
            {
                PrintInventorysList.Add(newInv);
                MessageBox.Show("Добавленио в список на печать", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        public static void addListToPrintList(List<Inventorys> newInvList)
        {
            foreach (var item in newInvList)
            {
                if (PrintInventorysList.FirstOrDefault(x => x.Id == item.Id) == null)
                {
                    PrintInventorysList.Add(item);
                }
            }
        }

        public static void deleteItem(Inventorys inventorys)
        {
            if (inventorys!= null)
            {
                PrintInventorysList.Remove(inventorys);
            }
        }

        public static void openFolder()
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Штрих-коды");
            Process.Start("explorer.exe", path);
        }

        public static void addToPrintList(ObservableCollection<Inventorys> inventorys)
        {
            Document doc = new Document();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Штрих-коды");
            var dateCreate = DateTime.Now;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string path1 = System.IO.Path.Combine(dirInfo.FullName, $"{dateCreate.ToString("d")}");
            DirectoryInfo datePrint = new DirectoryInfo(path1);
            if (!datePrint.Exists)
            {
                datePrint.Create();
            }
            string fileName = System.IO.Path.Combine(datePrint.FullName, "Список штрих-кодов " + dateCreate.ToString("HH-mm")+".pdf");

            Barcode128 barcode128 = new Barcode128();
            barcode128.BarHeight = 50;
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
            doc.Open();
            PdfContentByte cb = writer.DirectContent;
            int x = 40, y = 700, i = 0;
            foreach (var inventory in inventorys)
            {
                barcode128.Code = inventory.InventoryCode;
                var s = barcode128.CreateImageWithBarcode(cb, null, null);
                s.ScalePercent(150, 150);
                s.SetAbsolutePosition(x, y);
                doc.Add(s);
                x += 170;
                i++;
                if (i % 3 == 0)
                {
                    x = 40;
                    y -= 110;
                }
            }
            doc.Close();
        }
    }
}
