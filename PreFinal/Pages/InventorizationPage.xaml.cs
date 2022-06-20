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

namespace PreFinal.Pages
{
    /// <summary>
    /// Логика взаимодействия для InventorizationPage.xaml
    /// </summary>
    public partial class InventorizationPage : Page, INotifyPropertyChanged
    {
        List<Stocktaking> initStocktakingList { get; set; }
        List<Stocktaking> _stocktakingList;
        public List<Stocktaking> stocktakingList
        {
            get
            {
                return _stocktakingList;
            }
            set
            {
                _stocktakingList = value;
                OnPropertyChanged("stocktakingList");
            }
        }

        bool _isDateFiltering;
        public bool IsDateFiltering
        {
            get
            {
                return _isDateFiltering;
            }
            set
            {
                _isDateFiltering = value;
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

        List<Inventorys> _stocktakingInventoryList;
        public List<Inventorys> stocktakingInventoryList
        {
            get
            {
                return _stocktakingInventoryList;
            }
            set
            {
                _stocktakingInventoryList = value;
                OnPropertyChanged();
            }
        }

        
        private async void update()
        {
            await StaticHtppClass.HttpData.getInventoryzationItems();
            stocktakingList = StaticHtppClass.HttpData.MainStocktakingList;
        }


        public InventorizationPage()
        {
            
            InitializeComponent();
            DataContext = this;
            IsDateFiltering = false;
            DateList.DataContext = StaticHtppClass.HttpData;
            update();
        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                filterDateList(StartDate.SelectedDate, EndDate.SelectedDate);
            }
            if(StartDate.SelectedDate == null && EndDate.SelectedDate == null)
            {
                IsDateFiltering = false;
                stocktakingList = StaticHtppClass.HttpData.MainStocktakingList;
            }
            if(StartDate.SelectedDate != null && EndDate.SelectedDate == null)
            {
                filterDateList(StartDate.SelectedDate);
            }
            if (StartDate.SelectedDate == null && EndDate.SelectedDate != null)
            {
                filterDateListOnlyEnd(EndDate.SelectedDate);
            }
        }

        private void DateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateList.SelectedIndex != -1)
            {
                int selectStocktaking = (DateList.SelectedItem as Stocktaking).Id;
                stocktakingInventoryList = StaticHtppClass.HttpData.MainStocktakingInventoryList.Where(x => x.IdStocktaking == selectStocktaking).Select(x => StaticHtppClass.HttpData.MainInventoryList.FirstOrDefault(c => c.Id == x.IdInventory)).ToList();
            }
            else
            {
                stocktakingInventoryList = null;
            }
        }

        private void filterDateList(DateTime? start, DateTime? end)
        {
            IsDateFiltering = true;
            stocktakingList = StaticHtppClass.HttpData.MainStocktakingList.Where(x => x.Date.Date >= start && x.Date.Date <= end).ToList();
        }

        private void filterDateList(DateTime? start)
        {
            IsDateFiltering = true;
            stocktakingList = StaticHtppClass.HttpData.MainStocktakingList.Where(x => x.Date.Date >= start && x.Date.Date <= DateTime.Now).ToList();
        }

        private void filterDateListOnlyEnd(DateTime? end)
        {
            IsDateFiltering = true;
            stocktakingList = initStocktakingList.Where(x => x.Date.Date <= end).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void ContinueInventoryzationBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectStocktaking = (DateList.SelectedItem as Stocktaking).Id;
            FrameApp.NavigateScanPage(selectStocktaking, stocktakingInventoryList);
        }

        private void AddStocktaking_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.NavigateScanPage();
        }

        private async void deleteStocktaking()
        {
            var selectStocktaking = DateList.SelectedItem as Stocktaking;
            StaticHtppClass.HttpData.MainStocktakingList.Remove(selectStocktaking);
            OnPropertyChanged(nameof(StaticHtppClass.HttpData.MainStocktakingList));
            bool res = await DbActions.DeleteStocktakingAsync(selectStocktaking);
            if (res)
            {
                update();
            }
        }

        /// <summary>
        /// Событие для выделения строки в ListView при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите удалить выбранную инвентаризацию?", "Внимание!", MessageBoxButton.YesNo,MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                deleteStocktaking();
                stocktakingInventoryList = null;
            }
        }

        private void report()
        {
            try
            {
                if (stocktakingInventoryList.Count > 0)
                {
                    List<Inventorys> inventorys = StaticHtppClass.HttpData.MainInventoryList;
                   
                    inventorys.RemoveAll(x => stocktakingInventoryList.FirstOrDefault(c => c.Id == x.Id) != null);
                    
                    ExcelHelperClass.ListToExcel(stocktakingInventoryList, DateList.SelectedItem as Stocktaking, inventorys);
                    MessageBox.Show("Excel-отчет создан", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ExcelHelperClass.CloseExcel();
                MessageBox.Show(ex.ToString(), "Критическая работа приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExcelBtn_Click(object sender, RoutedEventArgs e)
        {
            report();
        }
    }
}
