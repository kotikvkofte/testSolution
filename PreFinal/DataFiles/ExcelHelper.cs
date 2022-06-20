using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Application = Microsoft.Office.Interop.Excel.Application;
//using Excel = Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows;
using APIModels.Models;
using APIModels.DataFiles;
using ExcelDataReader;
using System.Data;

namespace PreFinal.DataFiles
{
    public partial class ExcelHelper
    {
        private static Application application;
        private static Excel.Workbook workBook;
        private static Excel.Worksheet worksheet;

        public static void ListToExcel(List<Inventorys> list)
        {
            // Открываем приложение
            application = new Application
            {
                DisplayAlerts = false
            };

            // Файл шаблона
            const string template = "template.xlsx";

            // Открываем книгу
            workBook = application.Workbooks.Open(Path.Combine(Environment.CurrentDirectory, template));

            // Получаем активную таблицу
            worksheet = workBook.ActiveSheet as Excel.Worksheet;

            //Заполняем таблицу значениями свойств из List'а
            int i = 2;
            foreach (Inventorys item in list)
            {
                worksheet.Range[$"A{i}"].Value = item.Name;
                worksheet.Range[$"B{i}"].Value = item.InventoryCode;
                worksheet.Range[$"C{i}"].Value = item.Price;
                worksheet.Range[$"D{i}"].Value = item.Amount;
                worksheet.Range[$"E{i}"].Value = item.Description;

                if (item.Workplaces.Locations != null)
                    worksheet.Range[$"F{i}"].Value = item.Workplaces.Locations.Location;
                else
                    worksheet.Range[$"F{i}"].Value = "-";

                if (item.Workplaces != null)
                    worksheet.Range[$"G{i}"].Value = item.Workplaces.Place;
                else
                    worksheet.Range[$"G{i}"].Value = "-";
                //User
                if (item.Workplaces.Locations.Users != null) { 
                    string aaa = $"{item.Workplaces.Locations.Users.Surname} {item.Workplaces.Locations.Users.FirstName} {item.Workplaces.Locations.Users.Patronymic}";
                    worksheet.Range[$"H{i}"].Value = aaa;
                }
                else
                    worksheet.Range[$"H{i}"].Value = "-";
                if (item.Providers != null)
                    worksheet.Range[$"I{i}"].Value = item.Providers.Name;
                else
                    worksheet.Range[$"I{i}"].Value = "-";
                if (item.Manufacturers != null)
                    worksheet.Range[$"J{i}"].Value = item.Manufacturers.Name;
                else
                    worksheet.Range[$"J{i}"].Value = "-";
                if (item.TypeOfInventory != null)
                    worksheet.Range[$"K{i}"].Value = item.TypeOfInventory.Type;
                else
                    worksheet.Range[$"K{i}"].Value = "-";
                i++;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "xlsx files (*.xlsx)|*.txt|All files (*.*)|*.*";
            saveFile.FilterIndex = 2;
            saveFile.DefaultExt = ".xlsx";
            if (saveFile.ShowDialog() == true)
            {
                workBook.SaveAs(saveFile.FileName);
                CloseExcel();
            }
        }
        private static void CloseExcel()
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

        /// <summary>
        /// Метод перевода эл-в массива к общей форме и удаление дубликатов
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static string[] StringArrayToDistinctStringArray(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                string s = arr[i];
                if (arr[i] == null)
                {
                    s = "-";
                }
                if (arr[i] != "-")
                {
                    s = arr[i].FirstCharToUpper();
                }
                arr[i] = s;
            }
            return arr.Distinct().ToArray();
        }

        private static string[] StringArrayToDistinctStringArray(string[] arr, bool respName)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                string s = arr[i];
                if (arr[i] == null)
                {
                    s = "-";
                }
                if (arr[i] != "-")
                {
                    s = arr[i];
                }
                arr[i] = s;
            }
            return arr.Distinct().ToArray();
        }

        /// <summary>
        /// Возвращает строковый массив с большой буквы, остальной текст с маленькой
        /// </summary>
        private static string[] StringArrayFirstCharToUpper(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].FirstCharToUpper();
            }
            return arr;
        }

        public static List<Inventorys> ExcelToList()
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            string filePath = "";

            if (dlg.ShowDialog() == true)
            {
                // Открываем выбранную книгу
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                filePath = dlg.FileName;
            }
            else
            {
                return null;
            }

            List<Inventorys> invList = new List<Inventorys>();


            try
            {
                DataTable dataTable;
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet();
                        dataTable = result.Tables[0];
                    }
                }

            
                //Переделываем в стандартны типовой массив
                string[] NamesArray = dataTable.AsEnumerable().Select(r => r.Field<string>("Column0")).ToArray();
                string[] NumArray = dataTable.AsEnumerable().Select(r => r.Field<double>("Column1").ToString()).ToArray();
                string[] PriceArray = dataTable.AsEnumerable().Select(r => r.Field<double>("Column2").ToString()).ToArray();
                string[] AmountArray = dataTable.AsEnumerable().Select(r => r.Field<double>("Column3").ToString()).ToArray();
                string[] DescriptionArray = dataTable.AsEnumerable().Select(r => r.Field<string>("Column4")).ToArray();
                string[] LocationArray = dataTable.AsEnumerable().Select(r => r.Field<string>("Column5")).ToArray();
                string[] WorkplaceArray = dataTable.AsEnumerable().Select(r => r.Field<string>("Column6")).ToArray();
                string[] RespPersonArray = dataTable.AsEnumerable().Select(r => r.Field<string>("Column7")).ToArray();
                string[] ProvidersArr = dataTable.AsEnumerable().Select(r => r.Field<string>("Column8")).ToArray();
                string[] ManufacturersArr = dataTable.AsEnumerable().Select(r => r.Field<string>("Column9")).ToArray();
                string[] TypeOfInventoryArr = dataTable.AsEnumerable().Select(r => r.Field<string>("Column10")).ToArray();

            
                try
                {
                    List<Users> intiUsers = DbActions.GetUsers();
                    List<Workplaces> initWorkplaces = DbActions.GetWorkplaces();
                    List<Providers> initProviders = DbActions.GetProviders();
                    List<Manufacturers> initManufacturers = DbActions.GetManufacturers();
                    List<TypeOfInventory> initTypeOfInventory = DbActions.GetTypeOfInventory();
                    List<Inventorys> initInventorys = DbActions.GetInventorys();

                    for (int i = 0; i < NamesArray.Length; i++)
                    {
                        string plc = WorkplaceArray[i];
                        var k = initWorkplaces.FirstOrDefault(x => x.Place == plc);
                        if (k == null)
                            k = null;
                        
                        string provs = ProvidersArr[i];
                        var pr = initProviders.FirstOrDefault(x => x.Name == provs);
                        if (pr == null)
                            pr = null;
            
                        string manuf = ManufacturersArr[i];
                        var ma = initManufacturers.FirstOrDefault(x => x.Name == manuf);
                        if (ma == null)
                            ma = null;
            
                        string type = TypeOfInventoryArr[i];
                        var ty = initTypeOfInventory.FirstOrDefault(x => x.Type == type);
                        if (ty == null)
                            ty = null;


                        double price = Convert.ToDouble(PriceArray[i]);
                        Inventorys inv = new Inventorys
                        {
                            Name = NamesArray[i],
                            InventoryCode = NumArray[i],
                            Price = price,
                            Amount = AmountArray[i],
                            Description = DescriptionArray[i],
                            Workplaces = k,
                            Providers = pr,
                            Manufacturers = ma,
                            TypeOfInventory = ty
                        };
                        invList.Add(inv);
                    }
                    return invList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        
    }
}
