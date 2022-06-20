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
    /// Логика взаимодействия для AddInventoryWindow.xaml
    /// </summary>
    public partial class AddInventoryWindow : Window
    {
        public bool IsUpdate { get; private set; } = false;
        private string Code;
        private List<Workplaces> WorkplacesList;
        public AddInventoryWindow(string InventoryCode)
        {
            InitializeComponent();

            Code = InventoryCode;
            if (Code == null)
            {
                Code = "";
            }
            CodeTxb.Text = Code;
            ProblemInvCodeTxb.Text = Code;
            LocationCmb.DisplayMemberPath = "Location";
            LocationCmb.SelectedValuePath = "Id";
            var LocList = DbActions.GetLocations();
            LocationCmb.ItemsSource = LocList;
            ProblemLocationCmb.DisplayMemberPath = "Location";
            ProblemLocationCmb.SelectedValuePath = "Id";
            ProblemLocationCmb.ItemsSource = LocList;
            WorkplaceCmb.DisplayMemberPath = "Place";
            WorkplaceCmb.SelectedValuePath = "Id";
            ProvidersCmb.DisplayMemberPath = "Name";
            ProvidersCmb.SelectedValuePath = "Id";
            ProvidersCmb.ItemsSource = DbActions.GetProviders();

            WorkplacesList = DbActions.GetWorkplaces();
        }

        private void AddInventoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AmountTxb.Text == "" || CodeTxb.Text == "" || NameTxb.Text == "" || PriceTxb.Text == "")
            {
                MessageBox.Show("Заполните все поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (DbActions.GetInventory(CodeTxb.Text) != null)
                {
                    MessageBox.Show("Такой инвентарный номер уже имеется в базе данных", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    int SelectWorkplaceId = Convert.ToInt32(WorkplaceCmb.SelectedValue);
                    int SelectProviderId = Convert.ToInt32(ProvidersCmb.SelectedValue);
                    int SelectedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                    Workplaces workplaces = DbActions.GetWorkplace(SelectWorkplaceId);
                    Providers providers = DbActions.GetProvider(SelectProviderId);
                    Locations location = DbActions.GetLocation(SelectedLocId);

                    Inventorys inventory = new Inventorys()
                    {
                        Name = NameTxb.Text.FirstCharToUpper(),
                        InventoryCode = CodeTxb.Text,
                        Price = Convert.ToDouble(PriceTxb.Text),
                        Amount = AmountTxb.Text,
                        Providers = providers,
                        Workplaces = workplaces,
                        Description = null,
                        Manufacturers = null,
                        TypeOfInventory = null,
                        Photo = null,
                        Locations = location
                    };

                    try
                    {
                        DbActions.PostInventory(inventory);
                        MessageBox.Show("Добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        IsUpdate = true;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }

        private void LocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationCmb.SelectedItem != null)
            {
                int SelectedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                WorkplaceCmb.IsEnabled = true;
                WorkplaceCmb.ItemsSource = WorkplacesList.Where(x => x.Locations.Id == SelectedLocId);
            }
        }

        private void ProblemLocationCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAddProblemInventory_Click(object sender, RoutedEventArgs e)
        {
            if (ProblemNameTxb.Text == "")
            {
                MessageBox.Show("Введите наименование инвентаря", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                int SelectProblemLocId = Convert.ToInt32(ProblemLocationCmb.SelectedValue);
                Locations PorblemLoc = DbActions.GetLocation(SelectProblemLocId);
                string inv_code = null;
                /*if (ProblemInvCodeTxb.Text != "")
                    inv_code = ProblemInvCodeTxb.Text;
                Problem_inventory problem = new Problem_inventory
                {
                    Locations = PorblemLoc,
                    inventory_code = inv_code,
                    Name = ProblemNameTxb.Text
                };
                if (ProblemAmountTxb.Text != "")
                    problem.Amount = Convert.ToInt32(ProblemAmountTxb.Text);

                try
                {
                    OdbConnectHelper.entObj.Problem_inventory.Add(problem);
                    OdbConnectHelper.entObj.SaveChanges();
                    MessageBox.Show("Добавлено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Критическая ошибка приложения" + ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }*/


            }
        }

        private void ProvidersCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AmountTxb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //e.Handled = !Char.IsDigit(e.Text, 0);
        }
    }
}
