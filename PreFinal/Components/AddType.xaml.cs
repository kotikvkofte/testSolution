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
    /// Логика взаимодействия для AddType.xaml
    /// </summary>
    public partial class AddType : UserControl
    {
        public List<TypeOfInventory> _typesList { get; set; }
        async void getTypes()
        {
            await StaticHtppClass.HttpData.GetMainTypeOfInventoryListAsync();
            _typesList = StaticHtppClass.HttpData.MainTypeOfInventoryList;
        }
        public AddType()
        {
            InitializeComponent();
            TypeList.DataContext = StaticHtppClass.HttpData;
            _typesList = StaticHtppClass.HttpData.MainTypeOfInventoryList;
        }

        private void AddTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SaveBtnChecker)
                {
                    TypeOfInventory type = _typesList.FirstOrDefault(x => x.Id == SelectedId);
                    type.Type = TypeTxb.Text;
                    if (DbActions.PutTypeOfInventory(type))
                    {
                        TypeTxb.Text = "";
                        AddTypeBtn.Content = "Добавить";
                        SaveBtnChecker = false;
                        SelectedId = 0;
                        getTypes();
                        MessageBox.Show("Изменения сохранены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Возникла ошибка при изменении данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                }
                if (TypeTxb.Text != null && SaveBtnChecker == false)
                {
                    if (TypeTxb.Text.Replace(" ","") == "")
                    {
                        MessageBox.Show("Введите тип", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    var CheckType = _typesList.FirstOrDefault(x => x.Type.ToLower() == TypeTxb.Text.ToLower());
                    if (CheckType != null)
                    {
                        MessageBox.Show("Такое место уже имеется в списке", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                    TypeOfInventory type = new TypeOfInventory()
                    {
                        Type = TypeTxb.Text
                    };

                    if (DbActions.PostTypeOfInventory(type))
                    {
                        MessageBox.Show("Новый тип добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        getTypes();
                        TypeTxb.Text = "";
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
            TypeOfInventory type = TypeList.SelectedItem as TypeOfInventory;
            TypeTxb.Text = type.Type;
            AddTypeBtn.Content = "Сохранить";
            SelectedId = type.Id;
            SaveBtnChecker = true;
        }

        private void ContextMenuDelBtn_Click(object sender, RoutedEventArgs e)
        {
            TypeOfInventory type = TypeList.SelectedItem as TypeOfInventory;
            var Result = MessageBox.Show("Вы действительно хотите удалить выбранный тип?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                var list = StaticHtppClass.HttpData.MainInventoryList.Where(x => x.TypeOfInventory != null && x.TypeOfInventory.Id == type.Id).ToList();

                foreach (var item in list)
                {
                    Inventorys inv = item;
                    inv.TypeOfInventory = null;
                    DbActions.PutInventory(inv);
                }
                if (DbActions.DeleteTypeOfInventory(type))
                {
                    MessageBox.Show("Тип инвентаря успешно удален", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    getTypes();
                }
            }
            else if (Result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
