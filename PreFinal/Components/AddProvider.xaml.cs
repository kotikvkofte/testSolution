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
    /// Логика взаимодействия для AddProvider.xaml
    /// </summary>
    public partial class AddProvider : UserControl
    {
        public List<Providers> _providersList { get; set; }
        bool SaveBtnChecker = false;
        int SelectedId;
        async void getProviders()
        {
            await StaticHtppClass.HttpData.GetMainProvidersListAsync();
            _providersList = await DbActions.GetProvidersAsync();
        }
        public AddProvider()
        {
            InitializeComponent();
            ProvidersList.DataContext = StaticHtppClass.HttpData;
            getProviders();
        }

        private void ContextMenuEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Providers providers = ProvidersList.SelectedItem as Providers;
            ProvidersTxb.Text = providers.Name;
            AddProvidersBtn.Content = "Сохранить";
            SelectedId = providers.Id;
            SaveBtnChecker = true;
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            Providers providers = ProvidersList.SelectedItem as Providers;
            var Result = MessageBox.Show("Вы действительно хотите удалить выбранного поставщика?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                var list = StaticHtppClass.HttpData.MainInventoryList.Where(x => x.Providers != null && x.Providers.Id == providers.Id).ToList();

                foreach (var item in list)
                {
                    Inventorys inv = item;
                    inv.Providers = null;
                    DbActions.PutInventory(inv);
                }
                if (DbActions.DeleteProviders(providers))
                {
                    MessageBox.Show("Поставщик успешно удален", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    getProviders();
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }

        private void AddProvidersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveBtnChecker)
                {
                    Providers providers = _providersList.FirstOrDefault(x => x.Id == SelectedId);
                    providers.Name = ProvidersTxb.Text;
                    if (DbActions.PutProviders(providers))
                    {
                        ProvidersTxb.Text = "";
                        AddProvidersBtn.Content = "Добавить";
                        SaveBtnChecker = false;
                        SelectedId = 0;
                        getProviders();
                        MessageBox.Show("Изменения сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при изменении данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }
                if (ProvidersTxb.Text != null && SaveBtnChecker == false)
                {
                    if (ProvidersTxb.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Введите поставщика", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    var CheckManufacturer = _providersList.FirstOrDefault(x => x.Name.ToLower() == ProvidersTxb.Text.ToLower());
                    if (CheckManufacturer != null)
                    {
                        MessageBox.Show("Такой поставщик уже имеется в списке", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                    Providers providers = new Providers()
                    {
                        Name = ProvidersTxb.Text
                    };

                    if (DbActions.PostProviders(providers))
                    {
                        MessageBox.Show("Новый поставщик добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        getProviders();
                        ProvidersTxb.Text = "";
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
    }
}
