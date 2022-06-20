using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace PreFinal.Pages
{
    /// <summary>
    /// Логика взаимодействия для PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Page, INotifyPropertyChanged
    {
        List<Inventorys> listInv;
        private async void GetItemsAsync()
        {
            IsLoaded = false;
            listInv = await DbActions.GetInventorysAsync();
            var InitialLocations = await DbActions.GetLocationsAsync();
            var InitialWorkplaces = await DbActions.GetWorkplacesAsync();
            var InitialTypeOfInventory = await DbActions.GetTypeOfInventoryAsync();

            listInv = listInv.Where(x => x.Locations != null && x.Locations.Users.Id == UserInfo.user.Id).ToList();

            TypeCmb.ItemsSource = InitialTypeOfInventory;
            LocationCmb.ItemsSource = InitialLocations.Where(x => x.Users.Id == UserInfo.user.Id).Distinct();
            WorkplaceCmb.ItemsSource = InitialWorkplaces.Where(x => x.Locations.Users.Id == UserInfo.user.Id).ToList();
            IsLoaded = true;
        }
        List<Workplaces> InitialWorkplaces;
        private async void GetAllItemsAsync()
        {
            IsLoaded = false;
            listInv = await DbActions.GetInventorysAsync();
            var InitialLocations = await DbActions.GetLocationsAsync();
            InitialWorkplaces = await DbActions.GetWorkplacesAsync();
            var InitialTypeOfInventory = await DbActions.GetTypeOfInventoryAsync();

            TypeCmb.ItemsSource = InitialTypeOfInventory;
            LocationCmb.ItemsSource = InitialLocations;
            WorkplaceCmb.ItemsSource = InitialWorkplaces;
            IsLoaded = true;
        }

        public PrintPage()
        {
            InitializeComponent();
            DataContext = this;
            LocationCmb.DataContext = StaticHtppClass.HttpData;
            TypeCmb.DataContext = StaticHtppClass.HttpData;
            WorkplaceCmb.DataContext = StaticHtppClass.HttpData;
            IsLoaded = true;
            //if (UserInfo.user.Roles.Name == "Admin")
            //{
            //    GetAllItemsAsync();
            //}
            //else
            //{
            //    GetItemsAsync();
            //}
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
                OnPropertyChanged();
            }
        }

        private DataGridRowDetailsVisibilityMode _rowDetailsVisible;
        public DataGridRowDetailsVisibilityMode RowDetailsVisible
        {
            get { return _rowDetailsVisible; }
            set
            {
                _rowDetailsVisible = value;
                OnPropertyChanged();
            }
        }
        int index;
        private void dgCompletedJobsMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (index == MainList.SelectedIndex)
            {
                if (RowDetailsVisible == DataGridRowDetailsVisibilityMode.Collapsed)
                {
                    RowDetailsVisible = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                }
                else
                {
                    RowDetailsVisible = DataGridRowDetailsVisibilityMode.Collapsed;
                }
            }
            else
            {
                RowDetailsVisible = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            }

            index = MainList.SelectedIndex;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PrintClass.PrintInventorysList.Count > 0)
            {
                PrintClass.addToPrintList(PrintClass.PrintInventorysList);
                MessageBox.Show("Файл с штрих-кодами создан", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Список пуст", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteItemFromList_Click(object sender, RoutedEventArgs e)
        {
            Inventorys delItem = MainList.SelectedItem as Inventorys;
            PrintClass.deleteItem(delItem);
        }

        private void OpenPtintFolder_Click(object sender, RoutedEventArgs e)
        {
            PrintClass.openFolder();
        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var list = StaticHtppClass.HttpData.MainInventoryList;

            if (LocationCmb.SelectedItem != null)
            {
                int selectedloc = Convert.ToInt32(LocationCmb.SelectedValue);
                list = list.Where(x => x.Locations != null && x.Locations.Id == selectedloc).ToList();
            }
            if (WorkplaceCmb.SelectedItem != null)
            {
                int selectedworkplace = Convert.ToInt32(WorkplaceCmb.SelectedValue);
                list = list.Where(x => x.Workplaces != null && x.Workplaces.Id == selectedworkplace).ToList();
            }
            if (TypeCmb.SelectedItem != null)
            {
                int selectedtype = Convert.ToInt32(TypeCmb.SelectedValue);
                list = list.Where(x => x.TypeOfInventory != null && x.TypeOfInventory.Id == selectedtype).ToList();
            }

            PrintClass.addListToPrintList(list);
            TypeCmb.SelectedIndex = -1;
            WorkplaceCmb.SelectedIndex = -1;
            LocationCmb.SelectedIndex = -1;
        }

        private void LocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedloc = Convert.ToInt32(LocationCmb.SelectedValue);
            WorkplaceCmb.ItemsSource = StaticHtppClass.HttpData.MainWorkplacesList.Where(x => x.Id == selectedloc);
        }

        private void TypeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WorkplaceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
