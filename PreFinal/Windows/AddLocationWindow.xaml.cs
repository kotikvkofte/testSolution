using APIModels.DataFiles;
using APIModels.Models;
using PreFinal.DataFiles;
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

namespace PreFinal.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddLocationWindow.xaml
    /// </summary>
    public partial class AddLocationWindow : Window
    {
        private List<Locations> LocationsList;
        private List<Users> UsersList = DbActions.GetUsers();

        public AddLocationWindow()
        {
            InitializeComponent();
            LocationsList = DbActions.GetLocations();
            Keyboard.Focus(LocationTxb);
            UsersCmb.SelectedValuePath = "Id";
            UsersCmb.ItemsSource = UsersList;
            LocList.ItemsSource = LocationsList;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (SaveBtnChecker)
                {
                    Locations location = DbActions.GetLocation(SelectedId);
                    location.Location = LocationTxb.Text;
                    int SelectedUserId = Convert.ToInt32(UsersCmb.SelectedValue);
                    Users user = UsersList.FirstOrDefault(x => x.Id == SelectedUserId);
                    location.Users = user;
                    if (DbActions.PutLocation(location))
                    {
                        LocationTxb.Text = "";
                        UsersCmb.SelectedItem = null;
                        AddLocBtn.Content = "Добавить";
                        SaveBtnChecker = false;
                        SelectedId = 0;
                        LocationsList = DbActions.GetLocations();
                        LocList.ItemsSource = LocationsList;
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
                    var CheckLoc = LocationsList.FirstOrDefault(x => x.Location.ToLower() == LocationTxb.Text.ToLower());
                    if (CheckLoc != null)
                    {
                        MessageBox.Show("Такое место уже имеется в списке", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    int SelectedUserId = Convert.ToInt32(UsersCmb.SelectedValue);
                    Users user = UsersList.FirstOrDefault(x => x.Id == SelectedUserId);


                    Locations location = new Locations()
                    {
                        Location = LocationTxb.Text.FirstCharToUpper(),
                        Users = user
                    };
                    if (DbActions.PostLocations(location))
                    {
                        MessageBox.Show("Новое место успешно добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        LocationsList = DbActions.GetLocations();
                        LocationTxb.Text = "";
                        UsersCmb.SelectedItem = null;
                        Keyboard.Focus(LocationTxb);
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при добавлении", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    /*Workplaces wrkp = new Workplaces()
                    {
                        Locations = location,
                        Place = "-"
                    };
                    OdbConnectHelper.entObj.Workplaces.Add(wrkp);
                    OdbConnectHelper.entObj.SaveChanges();*/
                    //MessageBox.Show("Новое место успешно добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    //LocList.ItemsSource = DataBaseActions.GetLocationsList();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        bool SaveBtnChecker = false;
        int SelectedId;
        private void ContextMenuEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Locations locs = LocList.SelectedItem as Locations;
            LocationTxb.Text = locs.Location;
            AddLocBtn.Content = "Сохранить";
            SelectedId = locs.Id;
            UsersCmb.SelectedValue = locs.Users.Id;
            SaveBtnChecker = true;
            LocList.ItemsSource = LocationsList;
            //LocList.ItemsSource = DataBaseActions.GetLocationsList();
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            Locations locs = LocList.SelectedItem as Locations;
            var Result = MessageBox.Show("Вместе с выбранным местом удалятся и рабочие места, привязанные к этому месту. Вы действительно хотите удалить выбранное место?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                if (DbActions.DeleteLocation(locs))
                {
                    MessageBox.Show("Место успешно удалено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    LocationsList = DbActions.GetLocations();
                    LocList.ItemsSource = LocationsList;
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
