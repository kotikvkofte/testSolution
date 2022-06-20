using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
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

namespace PreFinal.Components
{
    /// <summary>
    /// Логика взаимодействия для AddLocation.xaml
    /// </summary>
    public partial class AddLocation : UserControl, INotifyPropertyChanged
    {
        List<Locations> _locationsList;
        public List<Locations> LocationsList 
        {
            get
            {
                return _locationsList;
            }
            set
            {
                _locationsList = value.Select(x => new Locations() 
                {
                    Id = x.Id,
                    Location = x.Location,
                    Users = new Users() 
                    { 
                        Id = x.Users.Id, 
                        FirstName = x.Users.FirstName[0].ToString(),
                        Patronymic = x.Users.Patronymic[0].ToString(),
                        Surname = x.Users.Surname
                    }
                }).ToList();
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void getLocations() => LocationsList = StaticHtppClass.HttpData.MainLocationsList;
        public AddLocation()
        {
            InitializeComponent();
            LocList.DataContext = this;
            getLocations();
            Keyboard.Focus(LocationTxb);
            UsersCmb.DataContext = StaticHtppClass.HttpData;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (SaveBtnChecker)
                {
                    Locations location = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Id == SelectedId);
                    location.Location = LocationTxb.Text;
                    int SelectedUserId = Convert.ToInt32(UsersCmb.SelectedValue);
                    Users user = StaticHtppClass.HttpData.MainUsersList.FirstOrDefault(x => x.Id == SelectedUserId);
                    location.Users = user;
                    if (DbActions.PutLocation(location))
                    {
                        LocationTxb.Text = "";
                        UsersCmb.SelectedItem = null;
                        AddLocBtn.Content = "Добавить";
                        SaveBtnChecker = false;
                        SelectedId = 0;
                        updateLocations();
                        MessageBox.Show("Изменения сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при изменении данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }
                if (LocationTxb.Text != null && SaveBtnChecker == false)
                {
                    if (LocationTxb.Text == "")
                    {
                        MessageBox.Show("Введите место", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    var CheckLoc = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Location.ToLower() == LocationTxb.Text.ToLower());
                    if (CheckLoc != null)
                    {
                        MessageBox.Show("Такое место уже имеется в списке", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    int SelectedUserId = Convert.ToInt32(UsersCmb.SelectedValue);
                    Users user = StaticHtppClass.HttpData.MainUsersList.FirstOrDefault(x => x.Id == SelectedUserId);


                    Locations location = new Locations()
                    {
                        Location = LocationTxb.Text,
                        Users = user
                    };
                    if (DbActions.PostLocations(location))
                    {
                        MessageBox.Show("Новое место успешно добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        updateLocations();
                        LocationTxb.Text = "";
                        UsersCmb.SelectedItem = null;
                        Keyboard.Focus(LocationTxb);
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при добавлении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        async void updateLocations()
        {
            await StaticHtppClass.HttpData.GetMainLocationsListAsync();
            getLocations();
        }

        bool SaveBtnChecker = false;
        int SelectedId;
        private void ContextMenuEditBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = Convert.ToInt32(LocList.SelectedValue);
            Locations locs = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Id == selectedId);
            LocationTxb.Text = locs.Location;
            AddLocBtn.Content = "Сохранить";
            SelectedId = locs.Id;
            UsersCmb.SelectedValue = locs.Users.Id;
            SaveBtnChecker = true;
            //LocList.ItemsSource = DataBaseActions.GetLocationsList();
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedId = Convert.ToInt32(LocList.SelectedValue);
            Locations locs = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Id == selectedId);
            var Result = MessageBox.Show("Вместе с выбранным местом удалятся и рабочие места, привязанные к этому месту. Вы действительно хотите удалить выбранное место?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                List<Inventorys> Invlist =StaticHtppClass.HttpData.MainInventoryList.Where(x => x.Locations != null && x.Locations.Id == locs.Id).ToList();
                List<Workplaces> Wplist = StaticHtppClass.HttpData.MainWorkplacesList.Where(x => x.Locations != null && x.Locations.Id == locs.Id).ToList();

                foreach (var item in Invlist)
                {
                    Inventorys inv = item;
                    inv.Locations = null;
                    DbActions.PutInventory(inv);
                }
                foreach (var item in Wplist)
                {
                    Workplaces wp = item;
                    wp.Locations = null;
                    DbActions.DeleteWorkplaces(wp);
                }
                if (DbActions.DeleteLocation(locs))
                {
                    MessageBox.Show("Место успешно удалено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    updateLocations();
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
