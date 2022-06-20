using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Microsoft.Win32;

namespace PreFinal
{
    class ExcelHelperClass
    {
        private static Application application;
        private static Excel.Workbook workBook;
        private static Excel.Worksheet worksheet;

        public static void ListToExcel(List<Inventorys> ScanList, Stocktaking stocktaking, List<Inventorys> NotScanList)
        {
            double totalSum = 0, shortage = 0;


            // Открываем приложение
            application = new Application
            {
                DisplayAlerts = false
            };
            // Добавляем книгу
            workBook = application.Workbooks.Add(1);
            worksheet = (Excel.Worksheet)workBook.Sheets[1];
            worksheet.Name = "Отсканировано";
            //Заполняем таблицу значениями свойств из List'а
            int i = 2;
            worksheet.Range[$"A{1}"].Value = "Название";
            worksheet.Range[$"B{1}"].Value = "Инвентарный номер";
            worksheet.Range[$"C{1}"].Value = "Цена";
            worksheet.Range[$"D{1}"].Value = "Количество";
            worksheet.Range[$"E{1}"].Value = "Описание";
            worksheet.Range[$"F{1}"].Value = "Кабинет";
            worksheet.Range[$"G{1}"].Value = "Рабочее место";
            worksheet.Range[$"H{1}"].Value = "Производитель";
            worksheet.Range[$"I{1}"].Value = "Тип";
            worksheet.Range[$"J{1}"].Value = "Поставщик";
            foreach (Inventorys item in ScanList)
            {
                worksheet.Range[$"A{i}"].Value = item.Name;
                worksheet.Range[$"B{i}"].Value = item.InventoryCode;
                worksheet.Range[$"C{i}"].Value = item.Price;
                worksheet.Range[$"D{i}"].Value = item.Amount;
                worksheet.Range[$"E{i}"].Value = item.Description;
                if (item.Locations != null)
                    worksheet.Range[$"F{i}"].Value = item.Locations.Location;
                if(item.Workplaces != null)
                    worksheet.Range[$"G{i}"].Value = item.Workplaces.Place;
                if (item.Manufacturers != null)
                    worksheet.Range[$"H{i}"].Value = item.Manufacturers.Name;
                if (item.TypeOfInventory != null)
                    worksheet.Range[$"I{i}"].Value = item.TypeOfInventory.Type;
                if (item.Providers != null)
                    worksheet.Range[$"J{i}"].Value = item.Providers.Name;
                i++;
                totalSum += item.Price;
            }
            var newWS = (Worksheet)workBook.Sheets.Add(After: workBook.ActiveSheet);
            newWS.Name = "Неотсканировано";
            i = 2;
            newWS.Range[$"A{1}"].Value = "Название";
            newWS.Range[$"B{1}"].Value = "Инвентарный номер";
            newWS.Range[$"C{1}"].Value = "Цена";
            newWS.Range[$"D{1}"].Value = "Количество";
            newWS.Range[$"E{1}"].Value = "Описание";
            newWS.Range[$"F{1}"].Value = "Кабинет";
            newWS.Range[$"G{1}"].Value = "Рабочее место";
            newWS.Range[$"H{1}"].Value = "Производитель";
            newWS.Range[$"I{1}"].Value = "Тип";
            newWS.Range[$"J{1}"].Value = "Поставщик";
            foreach (Inventorys item in NotScanList)
            {
                newWS.Range[$"A{i}"].Value = item.Name;
                newWS.Range[$"B{i}"].Value = item.InventoryCode;
                newWS.Range[$"C{i}"].Value = item.Price;
                newWS.Range[$"D{i}"].Value = item.Amount;
                newWS.Range[$"E{i}"].Value = item.Description;
                if (item.Locations != null)
                    newWS.Range[$"F{i}"].Value = item.Locations.Location;
                if (item.Workplaces != null)
                    newWS.Range[$"G{i}"].Value = item.Workplaces.Place;
                if (item.Manufacturers != null)
                    newWS.Range[$"H{i}"].Value = item.Manufacturers.Name;
                if (item.TypeOfInventory != null)
                    newWS.Range[$"I{i}"].Value = item.TypeOfInventory.Type;
                if (item.Providers != null)
                    newWS.Range[$"J{i}"].Value = item.Providers.Name;
                i++;
                totalSum += item.Price;
                shortage += item.Price;
            }

            worksheet.Range["N1"].Value = "Недосдача в размере:";
            worksheet.Range["O1"].Value = $"{shortage}";
            worksheet.Range["N2"].Value = "Общая стоимость:";
            worksheet.Range["O2"].Value = $"{totalSum}";

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Reports");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string filename = $"Инвентаризация от {stocktaking.Date.ToString("dd-MM-yyyy_hh-mm")}";
            string totalPath = dirInfo.FullName + "\\" + filename;
            workBook.SaveAs(totalPath);
            CloseExcel();
        }
        public static void CloseExcel()
        {
            if (application != null)
            {
                int excelProcessId = -1;
                GetWindowThreadProcessId(application.Hwnd, ref excelProcessId);
                Marshal.ReleaseComObject(worksheet);
                workBook.Close();
                Marshal.ReleaseComObject(workBook);
                application.Quit();
                Marshal.ReleaseComObject(application);

                application = null;
                // Прибиваем висящий процесс
                try
                {
                    Process process = Process.GetProcessById(excelProcessId);
                    process.Kill();
                }
                finally { }
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(int hWnd, ref int lpdwProcessId);
    }
}
