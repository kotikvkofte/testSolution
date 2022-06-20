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
    /// Логика взаимодействия для AddWorkplaceWindow.xaml
    /// </summary>
    public partial class AddWorkplaceWindow : Window
    {
        List<Workplaces> WrkplsList = DbActions.GetWorkplaces();
        List<Locations> LocList = DbActions.GetLocations();
        public AddWorkplaceWindow()
        {
            InitializeComponent();
            WorkplaceList.ItemsSource = WrkplsList;
            LocationCmb.DisplayMemberPath = "Location";
            LocationCmb.SelectedValuePath = "Id";
            LocationCmb.ItemsSource = LocList;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SaveBtnChecker == true)
            {
                Workplaces workplaces = WrkplsList.FirstOrDefault(x => x.Id == SelectedId);
                workplaces.IdLocation = Convert.ToInt32(LocationCmb.SelectedValue);
                workplaces.Place = WorkplaceTxb.Text.FirstCharToUpper();
                if (DbActions.PutWorkplaces(workplaces))
                {
                    LocationCmb.SelectedItem = null;
                    WorkplaceTxb.Text = "";
                    AddBtn.Content = "Добавить";
                    SaveBtnChecker = false;
                    SelectedId = 0;
                    WrkplsList = DbActions.GetWorkplaces();
                    WorkplaceList.ItemsSource = WrkplsList;
                    MessageBox.Show("Изменения сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("При изменении данных возникла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            if (LocationCmb.SelectedItem != null && WorkplaceTxb.Text != "" && WorkplaceTxb.Text != null && SaveBtnChecker == false)
            {
                string workplc = WorkplaceTxb.Text.FirstCharToUpper();
                if (WrkplsList.FirstOrDefault(x => x.Place == workplc) == null)
                {
                    int SelectedValue = Convert.ToInt32(LocationCmb.SelectedValue);
                    Locations location = LocList.FirstOrDefault(x => x.Id == SelectedValue);
                    Workplaces workplaces = new Workplaces()
                    {
                        Place = workplc,
                        Locations = location
                    };
                    if (DbActions.PostWorkplaces(workplaces))
                    {
                        LocationCmb.SelectedItem = null;
                        WorkplaceTxb.Text = "";
                        WrkplsList = DbActions.GetWorkplaces();
                        WorkplaceList.ItemsSource = WrkplsList;
                        MessageBox.Show("Данные успешно добавлены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("При добавлении данных возникла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Такое рабочее место уже есть", "Внимание", MessageBoxButton.OK, MessageBoxImage.Question);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Question);
            }
        }

        private void ViewToTxbCmb()
        {
           /* Workplaces wp = WorkplaceList.SelectedItem as Workplaces;
            LocationCmb.SelectedItem = wp.Locations;
            WorkplaceTxb.Text = wp.Place;*/
        }
        bool SaveBtnChecker = false;
        int SelectedId;
        private void ContextMenuBtn1_Click(object sender, RoutedEventArgs e)
        {
            Workplaces wp = WorkplaceList.SelectedItem as Workplaces;
            LocationCmb.SelectedValue = wp.Locations.Id;
            WorkplaceTxb.Text = wp.Place;
            AddBtn.Content = "Сохранить";
            SelectedId = wp.Id;
            SaveBtnChecker = true;
            //WorkplaceList.ItemsSource = DataBaseActions.GetWorkplacesList();
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            Workplaces wp = WorkplaceList.SelectedItem as Workplaces;
            var Result = MessageBox.Show("Вы действительно хотите удалить выбранное рабочее место?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                if (DbActions.DeleteWorkplaces(wp))
                {
                    WrkplsList = DbActions.GetWorkplaces();
                    WorkplaceList.ItemsSource = WrkplsList;
                    MessageBox.Show("Рабочее место удалено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
