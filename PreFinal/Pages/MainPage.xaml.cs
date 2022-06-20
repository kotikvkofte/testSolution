using APIModels.DataFiles;
using APIModels.Models;
using PreFinal.DataFiles;
using PreFinal.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace PreFinal.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        bool ContinueInventarization = false;
        Inventorys inventory;
        bool StartInventory = false;
        int LacationSelectedValue;
        int? WorkplacesSelectedValue;
        bool BoolScan = false;
        bool IsBtnEditPressed = false;
        List<Inventorys> AllInventorys = DbActions.GetInventorys();
        List<Workplaces> AllWorkplaces;
        List<Locations> AllLocations;

        public MainPage()
        {
            InitializeComponent();
            Keyboard.Focus(BarcodeTxb);
            LocationCmb.DisplayMemberPath = "Location";
            LocationCmb.SelectedValuePath = "Id";
            //LocationCmb.ItemsSource = DbActions.GetLocations();
            WorkplaceCmb.DisplayMemberPath = "Place";
            WorkplaceCmb.SelectedValuePath = "Id";
            WorkplaceCmb.ItemsSource = DbActions.GetWorkplaces();
            LocationCmb.ItemsSource = DbActions.GetLocations();

            //RespPerson.ItemsSource = DbActions.GetUsers();
            BarcodeTxb.Focus();
            if (UserInfo.user.Roles.Id == 1)
            {

                AdminBtn.Visibility = Visibility.Visible;
            }
            else
            {
                AdminBtn.Visibility = Visibility.Collapsed;
            }
            UpdateCurrentInventory();
        }
        private void UpdateCurrentInventory()
        {
            if (StartInventory == true || ContinueInventarization == true)
            {
                InventoryCurrentTxt.Text = InventorysList.GetCurrentInventoyNumb().ToString();
            }
        }
        private void ScanMode()
        {
            if (SelectChkb.IsChecked == true)
            {
                if (BoolScan)
                {
                    BarcodeTxb.Clear();
                    //BoolScan = false;
                }
            }
        }
        private void ClearFields()
        {
            NameTxt.Text = "";
            PriceTxt.Text = "";
            AmountTxt.Text = "";
            LocationCmb.SelectedItem = null;
            //ScanCodeImg.Source = null;

            MainPanel.Visibility = Visibility.Hidden;
            InputPanel.Visibility = Visibility.Visible;

            if (BarcodeTxb.Text != "")
            {
                InputMsg.Text = "Код не найден";
            }
            else
            {
                InputMsg.Text = "Введите инвентарный номер";
            }
        }

        private void OutputFromDB()
        {
            if (BarcodeTxb.Text != "")
            {
                inventory = AllInventorys.FirstOrDefault(x => x.InventoryCode == BarcodeTxb.Text);
                if (inventory != null)
                {
                    //LocationCmb.SelectedItem = "";
                    TransferLogicClass.CheckDeliver(inventory, UserInfo.user);


                    NameTxt.Text = inventory.Name;
                    PriceTxt.Text = inventory.Price.ToString();
                    AmountTxt.Text = inventory.Amount;

                    var k = inventory.Workplaces;
                    if (inventory.Workplaces == null)
                    {
                        WorkplaceCmb.SelectedValue = null;
                        WorkplaceTxt.Text = null;
                    }
                    else
                    {
                        WorkplaceCmb.SelectedValue = inventory.Workplaces.Id;
                        WorkplaceTxt.Text = inventory.Workplaces.Place;
                    }
                    if (inventory.Locations == null)
                    {
                        LocationCmb.SelectedValue = null;
                        LocationTxt.Text = null;
                        RespPerson.Text = null;
                        RespPersonTxt.Text = null;
                    }
                    else
                    {
                        LocationCmb.SelectedValue = inventory.Locations.Id;
                        LocationTxt.Text = inventory.Locations.Location;
                        RespPerson.Text = $"{inventory.Locations.Users.Surname} {inventory.Locations.Users.FirstName} {inventory.Locations.Users.Patronymic}";
                        RespPersonTxt.Text = inventory.Locations.Users != null ? $"{inventory.Locations.Users.Surname} {inventory.Locations.Users.FirstName} {inventory.Locations.Users.Patronymic}" : " ";
                    }
                    
                    InputPanel.Visibility = Visibility.Hidden;
                    MainPanel.Visibility = Visibility.Visible;


                    //Уведомление о списании
                   /* if (DbActions.GetInventory(inventory.InventoryCode) == null)
                    {
                        AddToSpisatBtn.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AddToSpisatBtn.Visibility = Visibility.Hidden;
                        MessageBox.Show("Данный предмет списан", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }*/
                }
                else
                {
                    ClearFields();
                }
            }
            
        }
        private void BarcodeTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            OutputFromDB();
            if (inventory != null)
            {
                InventorysList.AddToCurrentInventoy(inventory);
                UpdateCurrentInventory();
            }
        }
        private void AddLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLocationWindow locationWindow = new AddLocationWindow();

            locationWindow.ShowDialog();
            LocationCmb.ItemsSource = DbActions.GetLocations();
            OutputFromDB();
            Keyboard.Focus(BarcodeTxb);
        }

        private void AddWorkplaceBtn_Click(object sender, RoutedEventArgs e)
        {
            AddWorkplaceWindow addWorkplace = new AddWorkplaceWindow();

            addWorkplace.ShowDialog();
            LocationCmb.ItemsSource = DbActions.GetWorkplaces();
            OutputFromDB();
            Keyboard.Focus(BarcodeTxb);
        }

        private void LocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BarcodeTxb.Text == "")
            {
                return;
            }
            if (inventory != null)
            {
                try
                {
                    if (LocationCmb.SelectedItem != null && IsBtnEditPressed)
                    {
                        WorkplaceCmb.SelectedItem = null;
                        LacationSelectedValue = Convert.ToInt32(LocationCmb.SelectedValue);
                        Users user = AllLocations.FirstOrDefault(x => x.Id == LacationSelectedValue).Users;
                        RespPerson.Text = $"{user.Surname} {user.FirstName} {user.Patronymic}";
                        var WorkplaceList = AllWorkplaces.Where(x => x.Locations.Id == LacationSelectedValue).ToList();
                        WorkplaceCmb.ItemsSource = WorkplaceList;
                        WorkplaceCmb.IsEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void WorkplaceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (WorkplaceCmb.SelectedItem != null)
                {
                    WorkplacesSelectedValue = Convert.ToInt32(WorkplaceCmb.SelectedValue);
                }
                else
                {
                    WorkplacesSelectedValue = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void BarcodeTxb_GotFocus(object sender, RoutedEventArgs e)
        {
            //ScanMode();
        }
        private void SelectChkb_Checked(object sender, RoutedEventArgs e)
        {
            BarcodeTxb.SelectionStart = 0;
            BarcodeTxb.SelectionLength = BarcodeTxb.Text.Length;
            BarcodeTxb.Focus();
        }

        private void InventoryzationButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartInventory == false)
            {
                InventoryzationButton.Content = " Остановить ";
                InventoryzationContinueButton.Visibility = Visibility.Hidden;
                StartInventory = true;
                ChkbsInventoryPanel.Visibility = Visibility.Visible;
                InventorysList.ClearCurrentInventoy();
                UpdateCurrentInventory();
                InventoryTotalTxt.Text = DbActions.GetInventorys().Count().ToString();
                if (UserInfo.user.IdRole != 1 && UserInfo.user != null)
                {
                    InventoryTotalTxt.Text = DbActions.GetInventorys().Where(x => x.Workplaces.Locations.Users.Id == UserInfo.user.Id).Count().ToString();

                }
            }
            else
            {
                InventoryzationButton.Content = " Начать ";
                InventoryzationContinueButton.Visibility = Visibility.Visible;
                StartInventory = false;
                ChkbsInventoryPanel.Visibility = Visibility.Hidden;
                ResultWindow window = new ResultWindow();
                window.ShowDialog();
            }
            Keyboard.Focus(BarcodeTxb);
        }

        private void SelectChkb_Unchecked(object sender, RoutedEventArgs e)
        {
            BarcodeTxb.SelectionStart = BarcodeTxb.Text.Length;
            BarcodeTxb.SelectionLength = BarcodeTxb.Text.Length;
            BarcodeTxb.Focus();
        }

        private void UpdateDBBtn_Click_1(object sender, RoutedEventArgs e)
        {
            //ExcelHelper.ReadExcelFile();
            LocationCmb.ItemsSource = DbActions.GetLocations();
        }

        private void InventoryzationContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ContinueInv();
        }

        private void AddInventory_Click(object sender, RoutedEventArgs e)
        {
            if (inventory != null)
            {
                MessageBox.Show("Такой предмет уже имеется в базе данных", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AddInventoryWindow addInventoryWindow = new AddInventoryWindow(BarcodeTxb.Text);
            addInventoryWindow.ShowDialog();
            if (addInventoryWindow.IsUpdate)
            {
                AllInventorys = DbActions.GetInventorys();
                OutputFromDB();
            }
            Keyboard.Focus(BarcodeTxb);
        }

        private void OpenProvlemInvListBtn_Click(object sender, RoutedEventArgs e)
        {
            ProblemInvList prpblmList = new ProblemInvList();
            prpblmList.ShowDialog();
            Keyboard.Focus(BarcodeTxb);
        }

        private void BarcodeTxb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //ScanMode();
        }
        private void BarcodeTxb_KeyDown(object sender, KeyEventArgs e)
        {
            if (SelectChkb.IsChecked == true)
            {
                if (BoolScan)
                {
                    BarcodeTxb.Clear();
                    BoolScan = false;
                }
                if (e.Key == Key.Enter)
                {
                    BoolScan = true;
                    //MessageBox.Show(BoolScan.ToString());
                }
                else
                {
                    BoolScan = false;
                }
            }
        }

        private void SpisatBtn_Click(object sender, RoutedEventArgs e)
        {
            SpisatWindow spisatWindow = new SpisatWindow();
            spisatWindow.ShowDialog();
        }

        private void AddToSpisatBtn_Click(object sender, RoutedEventArgs e)
        {
            ReasonWindow reasonWindow = new ReasonWindow(inventory);
            reasonWindow.ShowDialog();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewPanel.Visibility = Visibility.Visible;
            ViewBtnsPanel.Visibility = Visibility.Visible;
            EditPanel.Visibility = Visibility.Collapsed;
            EditBtnsPanel.Visibility = Visibility.Collapsed;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Inventorys oldInv = new Inventorys()
                {
                    Name = inventory.Name,
                    InventoryCode = inventory.InventoryCode,
                    Price = inventory.Price,
                    Amount = inventory.Amount,
                    Providers = inventory.Providers,
                    Workplaces = inventory.Workplaces,
                    Description = inventory.Description,
                    Manufacturers = inventory.Manufacturers,
                    TypeOfInventory = inventory.TypeOfInventory,
                    Photo = inventory.Photo,
                    Locations = inventory.Locations
                };
                if (WorkplacesSelectedValue!=null)
                {
                    inventory.Workplaces = DbActions.GetWorkplace((int)WorkplacesSelectedValue);
                }
                else
                {
                    inventory.Workplaces = null;
                }
                inventory.Locations = DbActions.GetLocation((int)LacationSelectedValue);
                //TransferLogicClass.CreateTransfer(oldInv, inventory);
                
                //OdbConnectHelper.entObj.SaveChanges();
                //MessageBox.Show(DbActions.PutInventory(inventory).ToString());
                if (DbActions.PutInventory(inventory))
                {
                    MessageBox.Show("Изменения успешно сохранены");
                    List<Inventorys> AllInventorys = DbActions.GetInventorys();
                }
                OutputFromDB();
                ViewPanel.Visibility = Visibility.Visible;
                ViewBtnsPanel.Visibility = Visibility.Visible;
                EditPanel.Visibility = Visibility.Collapsed;
                EditBtnsPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            AllLocations = DbActions.GetLocations();
            AllWorkplaces = DbActions.GetWorkplaces();
            ViewPanel.Visibility = Visibility.Collapsed;
            ViewBtnsPanel.Visibility = Visibility.Collapsed;
            EditPanel.Visibility = Visibility.Visible;
            EditBtnsPanel.Visibility = Visibility.Visible;
            IsBtnEditPressed = true;
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
            OutputFromDB();
            Keyboard.Focus(BarcodeTxb);
        }

        private void OpenListBtn_Click(object sender, RoutedEventArgs e)
        {
            ListWindow listWindow = new ListWindow();

            listWindow.ShowDialog();

            if (listWindow.Code != "")
            {
                BarcodeTxb.Text = listWindow.Code;
            }
            BarcodeTxb.SelectionStart = BarcodeTxb.Text.Length;
            Keyboard.Focus(BarcodeTxb);
        }

        private void ContinueInv()
        {
            if (ContinueInventarization == false)
            {
                var lst = ExcelHelper.ExcelToList();
                if (lst == null)
                {
                    return;
                }
                for (int i = 0; i < lst.Count; i++)
                {
                    string ic = lst[i].InventoryCode;
                    lst[i] = DbActions.GetInventory(ic);
                }
                InventorysList.SetCurrentList(lst);
                InventoryzationContinueButton.Content = " Остановить ";
                InventoryzationButton.Visibility = Visibility.Hidden;
                ContinueInventarization = true;
                ChkbsInventoryPanel.Visibility = Visibility.Visible;
                InventoryTotalTxt.Text = DbActions.InventorysCount.ToString();
                InventoryCurrentTxt.Text = InventorysList.GetCurrentInventoyNumb().ToString();
            }
            else
            {
                InventoryzationButton.Content = " Начать инвентаризацию ";
                InventoryzationContinueButton.Content = " Продолжить инвентаризацию ";
                InventoryzationButton.Visibility = Visibility.Visible;
                ContinueInventarization = false;
                ChkbsInventoryPanel.Visibility = Visibility.Hidden;

                ResultWindow window = new ResultWindow();
                window.ShowDialog();
            }
            Keyboard.Focus(BarcodeTxb);
        }
    }
}
