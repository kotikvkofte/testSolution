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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreFinal.Components
{
    /// <summary>
    /// Логика взаимодействия для AddWorkplace.xaml
    /// </summary>
    public partial class AddWorkplace : UserControl
    {
        List<Workplaces> WrkplsList = StaticHtppClass.HttpData.MainWorkplacesList;
        List<Locations> LocList = StaticHtppClass.HttpData.MainLocationsList;
        public AddWorkplace()
        {
            InitializeComponent();
            LocationCmb.DisplayMemberPath = "Location";
            LocationCmb.SelectedValuePath = "Id";
            WorkplaceList.DataContext = StaticHtppClass.HttpData;
            LocationCmb.DataContext = StaticHtppClass.HttpData;
        }

        async void updateWorkplaces()
        {
            await StaticHtppClass.HttpData.GetMainWorkplacesListAsync();
            List<Workplaces> WrkplsList = StaticHtppClass.HttpData.MainWorkplacesList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SaveBtnChecker == true)
            {
                Workplaces workplaces = WrkplsList.FirstOrDefault(x => x.Id == SelectedId);
                int selctedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                workplaces.Locations = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Id == selctedLocId);
                workplaces.Place = WorkplaceTxb.Text;
                if (DbActions.PutWorkplaces(workplaces))
                {
                    LocationCmb.SelectedItem = null;
                    WorkplaceTxb.Text = "";
                    AddBtn.Content = "Добавить";
                    SaveBtnChecker = false;
                    SelectedId = 0;
                    updateWorkplaces();
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
                string workplc = WorkplaceTxb.Text;
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
                        updateWorkplaces();
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
            if (wp.Locations is not null)
            {
                LocationCmb.SelectedValue = wp.Locations.Id;
            }
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
                var list = DbActions.GetInventorys().Where(x => x.Workplaces != null && x.Workplaces.Id == wp.Id).ToList();

                foreach (var item in list)
                {
                    Inventorys inv = item;
                    inv.Workplaces = null;
                    DbActions.PutInventory(inv);
                }
                if (DbActions.DeleteWorkplaces(wp))
                {
                    updateWorkplaces();
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
