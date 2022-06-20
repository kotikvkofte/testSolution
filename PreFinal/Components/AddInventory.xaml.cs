using APIModels.DataFiles;
using APIModels.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddInventory.xaml
    /// </summary>
    public partial class AddInventory : UserControl
    {
        public AddInventory()
        {
            InitializeComponent();

            LocationCmb.DataContext = StaticHtppClass.HttpData;
            ProvidersCmb.DataContext = StaticHtppClass.HttpData;
            TypeCmb.DataContext = StaticHtppClass.HttpData;
            ManufacturersCmb.DataContext = StaticHtppClass.HttpData;
        }
        public bool IsUpdate { get; private set; } = false;

        private byte[] _photo = null;

        private void AddInventoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AmountTxb.Text == "" || CodeTxb.Text == "" || NameTxb.Text == "" || PriceTxb.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (StaticHtppClass.HttpData.MainInventoryList.FirstOrDefault(x => x.InventoryCode == CodeTxb.Text) != null)
                {
                    MessageBox.Show("Такой инвентарный номер уже имеется в базе данных", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Workplaces workplaces = null;
                    Providers providers = null;
                    Locations location = null;
                    TypeOfInventory type = null;
                    Manufacturers manufacture = null;

                    if (WorkplaceCmb.SelectedItem!= null)
                    {
                        int SelectWorkplaceId = Convert.ToInt32(WorkplaceCmb.SelectedValue);
                        workplaces = StaticHtppClass.HttpData.MainWorkplacesList.FirstOrDefault(x => x.Id == SelectWorkplaceId);

                    }
                    if (ProvidersCmb.SelectedItem != null)
                    {
                        int SelectProviderId = Convert.ToInt32(ProvidersCmb.SelectedValue);
                        providers = StaticHtppClass.HttpData.MainProvidersList.FirstOrDefault(x => x.Id == SelectProviderId);

                    }
                    if (LocationCmb.SelectedItem != null)
                    {
                        int SelectedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                        location = StaticHtppClass.HttpData.MainLocationsList.FirstOrDefault(x => x.Id == SelectedLocId);

                    }
                    if (TypeCmb.SelectedItem != null)
                    {
                        int SelectedTypeId = Convert.ToInt32(TypeCmb.SelectedValue);
                        type = StaticHtppClass.HttpData.MainTypeOfInventoryList.FirstOrDefault(x => x.Id == SelectedTypeId);

                    }
                    if (ManufacturersCmb.SelectedItem != null)
                    {
                        int SelectedManufactureId = Convert.ToInt32(ManufacturersCmb.SelectedValue);
                        manufacture = StaticHtppClass.HttpData.MainManufacturersList.FirstOrDefault(x => x.Id == SelectedManufactureId);

                    }

                    Inventorys inventory = new Inventorys()
                    {
                        Name = NameTxb.Text,
                        InventoryCode = CodeTxb.Text,
                        Price = Convert.ToDouble(PriceTxb.Text),
                        Amount = AmountTxb.Text,
                        Providers = providers,
                        Workplaces = workplaces,
                        Description = DescriptionTxb.Text,
                        Manufacturers = manufacture,
                        TypeOfInventory = type,
                        Photo = _photo,
                        Locations = location
                    };

                    try
                    {
                        addAndUpdate(inventory);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }

        async void addAndUpdate(Inventorys inventory)
        {
            DbActions.PostInventory(inventory);
            MessageBox.Show("Добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            await StaticHtppClass.HttpData.GetMainInventoryListAsync();
            IsUpdate = true;
        }

        private void LocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationCmb.SelectedItem != null)
            {
                int SelectedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                WorkplaceCmb.IsEnabled = true;
                WorkplaceCmb.ItemsSource = StaticHtppClass.HttpData.MainWorkplacesList.Where(x => x.Locations.Id == SelectedLocId);
            }
        }

        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                try
                {
                    _photo = File.ReadAllBytes(openFile.FileName);
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(_photo);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();

                    ImageSource imgSrc = biImg as ImageSource;
                    MainPhoto.Source = imgSrc;
                }
                catch (Exception)
                {
                    MessageBox.Show("Критическая ошибка", "Внимание!");
                }

            }
        }

    }
}
