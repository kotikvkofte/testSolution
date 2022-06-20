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
    /// Логика взаимодействия для AddManufacture.xaml
    /// </summary>
    public partial class AddManufacture : UserControl
    {
        public List<Manufacturers> _manufacturersList { get; set; }
        async void getManufacturers()
        {
            await StaticHtppClass.HttpData.GetMainManufacturersListAsync();
            _manufacturersList = StaticHtppClass.HttpData.MainManufacturersList; 
        }
        public AddManufacture()
        {
            InitializeComponent();
            ManufacturersList.DataContext = StaticHtppClass.HttpData;
            getManufacturers();
        }

        private void AddManufacturersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveBtnChecker)
                {
                    Manufacturers manufacturers = _manufacturersList.FirstOrDefault(x => x.Id == SelectedId);
                    manufacturers.Name = ManufacturersTxb.Text;
                    if (DbActions.PutManufacturers(manufacturers))
                    {
                        ManufacturersTxb.Text = "";
                        AddManufacturersBtn.Content = "Добавить";
                        SaveBtnChecker = false;
                        SelectedId = 0;
                        getManufacturers();
                        MessageBox.Show("Изменения сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при изменении данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }
                if (ManufacturersTxb.Text != null && SaveBtnChecker == false)
                {
                    if (ManufacturersTxb.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Введите производителя", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    var CheckManufacturer = _manufacturersList.FirstOrDefault(x => x.Name.ToLower() == ManufacturersTxb.Text.ToLower());
                    if (CheckManufacturer != null)
                    {
                        MessageBox.Show("Такой производитель уже имеется в списке", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                    Manufacturers manufacturers = new Manufacturers()
                    {
                        Name = ManufacturersTxb.Text
                    };

                    if (DbActions.PostManufacturers(manufacturers))
                    {
                        MessageBox.Show("Новый производитель добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        getManufacturers();
                        ManufacturersTxb.Text = "";
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
        bool SaveBtnChecker = false;
        int SelectedId;
        private void ContextMenuEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manufacturers manufacturers = ManufacturersList.SelectedItem as Manufacturers;
            ManufacturersTxb.Text = manufacturers.Name;
            AddManufacturersBtn.Content = "Сохранить";
            SelectedId = manufacturers.Id;
            SaveBtnChecker = true;
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            Manufacturers manufacturers = ManufacturersList.SelectedItem as Manufacturers;
            var Result = MessageBox.Show("Вы действительно хотите удалить выбранного производителя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                var list = StaticHtppClass.HttpData.MainInventoryList.Where(x => x.Manufacturers != null && x.Manufacturers.Id == manufacturers.Id).ToList();

                foreach (var item in list)
                {
                    Inventorys inv = item;
                    inv.Manufacturers = null;
                    DbActions.PutInventory(inv);
                }
                if (DbActions.DeleteManufacturers(manufacturers))
                {
                    MessageBox.Show("Производитель успешно удален", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    getManufacturers();
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
