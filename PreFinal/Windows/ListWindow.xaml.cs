using APIModels.DataFiles;
using APIModels.Models;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace PreFinal.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        public string Code { get; private set; }
        int SelectLoc, SelectWorkplace, SelectRespPerson;
        List<Workplaces> WorkplacesList;
        List<Locations> LocationsList;
        List<Users> UsersList;
        List<Inventorys> InventorysList;
        public ListWindow()
        {
            InitializeComponent();
            Code = "";

            WorkplacesList = DbActions.GetWorkplaces();
            LocationsList = DbActions.GetLocations();
            UsersList = DbActions.GetUsers();
            InventorysList = DbActions.GetInventorys();


            LocationCmb.DisplayMemberPath = "Location";
            LocationCmb.SelectedValuePath = "Id";
            LocationCmb.ItemsSource = LocationsList;
            WorkplaceCmb.DisplayMemberPath = "Place";
            WorkplaceCmb.SelectedValuePath = "Id";
            WorkplaceCmb.ItemsSource = WorkplacesList;
            //RespPersonCmb.DisplayMemberPath = "Name";
            RespPersonCmb.SelectedValuePath = "Id";
            RespPersonCmb.ItemsSource = UsersList;
            MainList.ItemsSource = InventorysList;
        }

        private void LocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectLoc = Convert.ToInt32(LocationCmb.SelectedValue);
            WorkplaceCmb.ItemsSource = WorkplacesList.Where(x => x.Locations.Id == SelectLoc);
            SearchInDB();
        }

        private void WorkplaceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectWorkplace = Convert.ToInt32(WorkplaceCmb.SelectedValue);
            SearchInDB();
        }

        private void RespPersonCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectRespPerson = Convert.ToInt32(RespPersonCmb.SelectedValue);
            SearchInDB();
        }

        private void NameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTxb.Text != "")
            {
                //NameTxb.Text = NameTxb.Text.FirstCharToUpper();
                NameTxb.Focus();
                NameTxb.SelectionStart = NameTxb.Text.Length;
            }
            SearchInDB();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            WorkplaceCmb.SelectedItem = null;
            LocationCmb.SelectedItem = null;
            RespPersonCmb.SelectedItem = null;
            NameTxb.Text = "";
            InvCodeTxb.Text = "";
            LocationCmb.ItemsSource = LocationsList;
            WorkplaceCmb.ItemsSource = WorkplacesList;
            MainList.ItemsSource = InventorysList;
            RespPersonCmb.ItemsSource = UsersList;
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Inventorys> inventories = (List<Inventorys>)MainList.ItemsSource;
            //PrintClassHelper.Print(inventories, "Отфильтрованный список ");
        }

        private void PrintAgaBtn_Click(object sender, RoutedEventArgs e)
        {
            /*PrintClassHelper.PrintAgaRes((List<Inventory>)MainList.ItemsSource);*/
        }

        private void InvCodeTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InvCodeTxb.Text != "")
            {
                //InvCodeTxb.Text = InvCodeTxb.Text.FirstCharToUpper();
                InvCodeTxb.Focus();
                InvCodeTxb.SelectionStart = InvCodeTxb.Text.Length;
            }
            SearchInDB();
        }

        private void ShowBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventorys inventory = MainList.SelectedItem as Inventorys;
            Code = inventory.InventoryCode.Replace(" ", "");
            this.Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventorys inventory = MainList.SelectedItem as Inventorys;
            var Result = MessageBox.Show("Вы действительно хотите удалить выбранный инвентарь?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                if (DbActions.DeleteInventory(inventory))
                {
                    SearchInDB();
                    MessageBox.Show("Инвентарь удален");
                }
                else
                {
                    MessageBox.Show("Возникла ошибка при удалении инвентаря");
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }

        private void SpisatBtn_Click(object sender, RoutedEventArgs e)
        {
            Inventorys inventory = MainList.SelectedItem as Inventorys;
            ReasonWindow reasonWindow = new ReasonWindow(inventory);
            reasonWindow.ShowDialog();
        }

        private void SearchInDB()
        {
            //MainList.ItemsSource = DbActions.GetFiltredInventorys(SelectLocation: SelectLoc, SelectWorkPlace: SelectWorkplace, SearchName: NameTxb.Text, SelectRespPerson: SelectRespPerson);
            /*if (LocationCmb.SelectedItem == null && WorkplaceCmb.SelectedItem == null && NameTxb.Text == "" && RespPersonCmb.SelectedItem == null)
            {
                MainList.ItemsSource = InventorysList;
            }*/

            List<Inventorys> CurrentList = InventorysList;

            //var test = DbActions.GetInventorys().Where(x => x.Locations.Id == loc.Id).ToList().Count;
            //MessageBox.Show(test.ToString());

            if (LocationCmb.SelectedItem != null)
                CurrentList = CurrentList.Where(z => z.Locations != null).Where(x => x.Locations.Id == SelectLoc).ToList();
            if (WorkplaceCmb.SelectedItem != null)
                CurrentList = CurrentList.Where(x =>x.Workplaces != null).Where(c => c.Workplaces.Id == SelectWorkplace).ToList();
            if (RespPersonCmb.SelectedItem != null)
                CurrentList = CurrentList.Where(z => z.Locations.Users != null).Where(x => x.Locations.Users.Id == SelectRespPerson).ToList();
            if (NameTxb.Text != "")
                CurrentList = CurrentList.Where(x => x.Name.ToLower().Contains(NameTxb.Text.ToLower())).ToList();
            if (InvCodeTxb.Text != "")
                CurrentList = CurrentList.Where(x => x.InventoryCode.ToLower().Contains(InvCodeTxb.Text.ToLower())).ToList();

            MainList.ItemsSource = CurrentList;

        }

        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            Inventorys inventory = MainList.SelectedItem as Inventorys;
            /*if (inventory.Name == null)
                inventory.Name = "-";
            if (inventory.Workplaces.Locations == null)
                inventory.Workplaces.Locations = OdbConnectHelper.entObj.Locations.FirstOrDefault(x => x.Location == "-");
            if (inventory.Workplaces == null)
                inventory.Workplaces = OdbConnectHelper.entObj.Workplaces.FirstOrDefault(x => x.Place == "-" && x.Locations.Location == inventory.Workplaces.Locations.Location);
            if (inventory.inventory_code == null)
                inventory.inventory_code = "-";
            if (inventory.Responsible_Persons == null)
                inventory.Responsible_Persons = OdbConnectHelper.entObj.Responsible_Persons.FirstOrDefault(x => x.Name == "-");*/
            //InventoryCard inventoryCard = new InventoryCard(inventory.Amount.ToString(), inventory.Name, inventory.inventory_code, inventory.Workplaces.Locations.Location, inventory.Workplaces.Place, inventory.Responsible_Persons.Name, inventory.Price.ToString());
            InventoryCard inventoryCard = new InventoryCard(inventory);

            inventoryCard.Show();
        }
    }
}
