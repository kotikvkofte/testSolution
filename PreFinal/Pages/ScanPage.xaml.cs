using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ScanPage.xaml
    /// </summary>
    public partial class ScanPage : Page, INotifyPropertyChanged
    {

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


        List<Inventorys> _initInventorysList;
        public List<Inventorys> initInventorysList
        {
            get
            {
                return _initInventorysList;
            }
            set
            {
                _initInventorysList = value;
                OnPropertyChanged();
            }
        }


        dynamic _allList;
        public dynamic allList
        {
            get
            {
                return _allList;
            }
            set
            {
                _allList = value;
                OnPropertyChanged();
            }
        }

        private List<Inventorys> newInventorys = new List<Inventorys>();

        private Stocktaking CurrentStocktaking;

        private bool _isAdded = true;

        private Inventorys _scanedInv;
        public Inventorys ScanedInv
        {
            get
            {
                return _scanedInv;
            }
            set
            {
                _scanedInv = value;
                if (value == null)
                {
                    IsAdded = true;
                }
                OnPropertyChanged("ScanedInv");
            }
        }

       public bool IsAdded
        {
            get
            {
                return _isAdded;
            }
            set
            {
                _isAdded = value;
                OnPropertyChanged();
            }
        }


        private void AddToAllList()
        {
            allList = initInventorysList.Select(x => new 
            {
                Id = x.Id,
                Amount = x.Amount,
                Description = x.Description,
                Locations = x.Locations,
                Workplaces = x.Workplaces,
                Price = x.Price,
                Photo = x.Photo,
                Name = x.Name,
                InventoryCode = x.InventoryCode,
                Providers = x.Providers,
                TypeOfInventory = x.TypeOfInventory,
                Manufacturers = x.Manufacturers,
                IsScanned = ScanList.FirstOrDefault(c => c.Id == x.Id) != null ? true : false
            }).ToList();
        }

        public ObservableCollection<Inventorys> ScanList { get; set; }

        public ScanPage()
        {
            InitializeComponent();
            DataContext = this;
            CurrentStocktaking = null;
            ScanList = new ObservableCollection<Inventorys>();
            FrameApp.OnIsScanPageOpen(); 

            getInitInvAsync();
        }
        public ScanPage(int stocktakingId, List<Inventorys> oldInventory)
        {
            InitializeComponent();
            DataContext = this;
            CurrentStocktaking = new Stocktaking() { Id = stocktakingId };
            ScanList = new ObservableCollection<Inventorys>(oldInventory);
            FrameApp.OnIsScanPageOpen(); 
            getInitInvAsync();
        }

        private void BarcodeTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ScanedInv = initInventorysList.FirstOrDefault(x => x.InventoryCode == BarcodeTxb.Text);
            if (ScanedInv != null)
            {
                if (ScanList.FirstOrDefault(x => x.InventoryCode == ScanedInv.InventoryCode) != null)
                {
                    IsAdded = true;
                }
                else
                {
                    IsAdded = false;
                }
            }
        }

        private void AddInventoryBtn_Click(object sender, RoutedEventArgs e)
        {
            ScanList.Add(ScanedInv);
            newInventorys.Add(ScanedInv);
            AddToAllList();
            BarcodeTxb.Clear();
            SaveBtn.IsEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.OffIsScanPageOpen();
            FrameApp.NavigateInventorizationPage();
        }

        private void postInventoriesToStocktaking(List<Inventorys> inventorys)
        {
            List<StocktakingInventory> stocktakingInventory = new List<StocktakingInventory>();
            var stocktaking = new Stocktaking() { Users = UserInfo.user, Date = DateTime.UtcNow.AddHours(9) };
            if (DbActions.PostStocktaking(stocktaking))
            {
                updateStocktaking();
                List<Stocktaking> bufStock = StaticHtppClass.HttpData.MainStocktakingList;
                CurrentStocktaking = bufStock.FirstOrDefault(x => x.Date == stocktaking.Date);
                stocktakingInventory.AddRange(inventorys.Select(x => new StocktakingInventory() { Inventorys = x, Stocktaking = CurrentStocktaking }).ToList());

                if (DbActions.PostStocktakingInventory(stocktakingInventory))
                {
                    MessageBox.Show("Сохранено");
                }
            }
        }

        private async void getInitInvAsync()
        {
            //await StaticHtppClass.HttpData.GetMainInventoryListAsync();
            initInventorysList = StaticHtppClass.HttpData.MainInventoryList;
            BarcodeTxb.IsEnabled = true;
            AddToAllList();
        }

        private async Task<Stocktaking> addStockTaking()
        {
            var stocktaking = new Stocktaking() { Users = UserInfo.user, Date = DateTime.UtcNow.AddHours(9) };
            DbActions.PostStocktaking(stocktaking);
            await StaticHtppClass.HttpData.getInventoryzationItems();
            var stock = StaticHtppClass.HttpData.MainStocktakingList.FirstOrDefault(x => x.Date == stocktaking.Date);
            return stock;
        }

        private async void updateStocktaking()
        {
            await StaticHtppClass.HttpData.getInventoryzationItems();

        }

        private async void postInventoriesToStocktakingAsync(List<Inventorys> inventorys)
        {
            List<StocktakingInventory> stocktakingInventory = new List<StocktakingInventory>();
            stocktakingInventory.AddRange(inventorys.Select(x => new StocktakingInventory() { Inventorys = x, Stocktaking = CurrentStocktaking }).ToList());

            if (await DbActions.PostStocktakingInventoryAsync(stocktakingInventory))
            {
                await StaticHtppClass.HttpData.getInventoryzationItems();
                MessageBox.Show("Сохранено");
            }
            
            
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStocktaking == null)
            {
                CurrentStocktaking = await addStockTaking();
                //FrameApp.UpdateInventorizationPage();
                //postInventoriesToStocktakingAsync(newInventorys);
            }
            if (newInventorys != null)
            {
                postInventoriesToStocktakingAsync(newInventorys);
                //FrameApp.UpdateInventorizationPage();
            }
            SaveBtn.IsEnabled = true;
        }

    }
}
