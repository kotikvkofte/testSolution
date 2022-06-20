using APIModels.DataFiles;
using APIModels.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace PreFinal.Pages
{
    /// <summary>
    /// Логика взаимодействия для ViewIndentoryPage.xaml
    /// </summary>
    
    public partial class ViewIndentoryPage : Page, INotifyPropertyChanged
    {
        Users user = UserInfo.user;
        List<Inventorys> _invList;
        List<Inventorys> initialInvList;
        Inventorys selectedItem;
        int SelectedLocId;
        bool _IsEditBtnPressed = false;
        List<Workplaces> _workplaces;
        bool _IsLoaded = true;
        bool _IsFiltering = false;
        bool isAnimate = false;
        byte[] _changedPhoto;

        #region properties
        public string role { get; set; } = UserInfo.user.Roles.Name;
        public int SelectedValueTable { get; set; } = -1;
        public List<Locations> InitialLocations { get; set; }/* = DbActions.GetLocations();*/
        public List<Workplaces> InitialWorkplaces { get; set; }// = DbActions.GetWorkplaces();
        public List<Manufacturers> InitialManufacturers { get; set; } //= DbActions.GetManufacturers();
        public List<TypeOfInventory> InitialTypeOfInventory { get; set; }// = DbActions.GetTypeOfInventory();
        public List<Providers> InitialProviders { get; set; }// = DbActions.GetProviders();
        public byte[] changedPhoto
        {
            get
            {
                return _changedPhoto;
            }
            set
            {
                _changedPhoto = value;
                OnPropertyChanged("changedPhoto");
            }
        }

        public List<Workplaces> changedWorkplaces
        {
            get
            {
                return _workplaces;
            }
            set
            {
                _workplaces = value;
                OnPropertyChanged("changedWorkplaces");
            }
        }

        public bool IsFiltering
        {
            get
            {
                return _IsFiltering;
            }
            set
            {
                _IsFiltering = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoaded
        {
            get
            {
                return _IsLoaded;
            }
            set
            {
                _IsLoaded = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditBtnPressed
        {
            get
            {
                return _IsEditBtnPressed;
            }
            set
            {
                _IsEditBtnPressed = value;
                OnPropertyChanged("IsEditBtnPressed");
            }
        }


        public List<Inventorys> InvList
        {
            get
            {
                return _invList;
            }
            private set
            {
                _invList = value;
                OnPropertyChanged("InvList");
                MainList.SelectedValue = SelectedValueTable;

            }
        }
        #endregion


        public void getOthers()
        {
            initialInvList = StaticHtppClass.HttpData.MainInventoryList;
            InitialLocations = StaticHtppClass.HttpData.MainLocationsList;
            InitialManufacturers = StaticHtppClass.HttpData.MainManufacturersList;
            InitialTypeOfInventory = StaticHtppClass.HttpData.MainTypeOfInventoryList;
            InitialProviders = StaticHtppClass.HttpData.MainProvidersList;
            InitialWorkplaces = StaticHtppClass.HttpData.MainWorkplacesList;
            IsLoaded = true;
        }


        #region async

        private async void GetInvAsync(int index)
        {
            await StaticHtppClass.HttpData.GetMainInventoryListAsync();
            initialInvList = StaticHtppClass.HttpData.MainInventoryList;
            //InvList = initialInvList;
            MainList.SelectedValue = index;
        }

        private async void GetAllItemsAsync()
        {
            IsLoaded = false;
            //initialInvList = await DbActions.GetInventorysAsync();
            InvList = initialInvList;
            InitialLocations = await DbActions.GetLocationsAsync();
            LocationCmb.ItemsSource = InitialLocations.Select(x => new
            {
                Id = x.Id,
                Location = x.Location
            }
            );
            InitialManufacturers = await DbActions.GetManufacturersAsync();
            Manuffacturers.ItemsSource = InitialManufacturers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            InitialTypeOfInventory = await DbActions.GetTypeOfInventoryAsync();
            TypeCmb.ItemsSource = InitialTypeOfInventory.Select(x => new
            {
                Id = x.Id,
                Type = x.Type
            });
            InitialProviders = await DbActions.GetProvidersAsync();
            ProvidersCmb.ItemsSource = InitialProviders.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });
            InitialWorkplaces = await DbActions.GetWorkplacesAsync();
            WorkplaceCmb.ItemsSource = InitialWorkplaces;
            IsLoaded = true;
        }
        private async void GetUserItemsAsync()
        {
            IsLoaded = false;
            initialInvList = await DbActions.GetInventorysAsync();
            InvList = initialInvList.Where(x => x.Locations != null && x.Locations.Users.Id == user.Id).ToList();

            InitialLocations = await DbActions.GetLocationsAsync();
            LocationCmb.ItemsSource = InitialLocations.Where(x => x.Users.Id == user.Id).Select(x => new
            {
                Id = x.Id,
                Location = x.Location
            }
            );
            
            InitialManufacturers = await DbActions.GetManufacturersAsync();
            Manuffacturers.ItemsSource = InitialManufacturers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });

            InitialTypeOfInventory = await DbActions.GetTypeOfInventoryAsync();
            TypeCmb.ItemsSource = InitialTypeOfInventory.Select(x => new
            {
                Id = x.Id,
                Type = x.Type
            });

            InitialProviders = await DbActions.GetProvidersAsync();
            var pcmb = InvList.Select(x => new
            {
                Id = x.Providers.Id,
                Name = x.Providers.Name
            }).Distinct() ?? null;
            if (pcmb != null)
            {
                ProvidersCmb.ItemsSource = pcmb;
            }
            InitialWorkplaces = await DbActions.GetWorkplacesAsync();
            WorkplaceCmb.ItemsSource = InvList.Select(x => x.Workplaces).Distinct();
            IsLoaded = true;
        }
        private void GetItemsAsync()
        {
            if (user.Roles.Name == "Admin")
            {
                //GetAllItemsAsync();
                getOthers();
            }
            else
            {
                GetUserItemsAsync();
            }
        }
        #endregion

        public ViewIndentoryPage()
        {
            InitializeComponent();
            //GetItemsAsync();
            LocationCmb.DataContext = StaticHtppClass.HttpData;
            WorkplaceCmb.DataContext = StaticHtppClass.HttpData;
            ProvidersCmb.DataContext = StaticHtppClass.HttpData;
            TypeCmb.DataContext = StaticHtppClass.HttpData;
            Manuffacturers.DataContext = StaticHtppClass.HttpData;
            MainList.DataContext = StaticHtppClass.HttpData;
            IsFiltering = false;
            this.DataContext = this;
        }

        #region TextChanged
        private void NameTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchInDB();
        }

        private void DescriptionTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (IsEditBtnPressed)
            {
                selectedItem.Description = txb.Text;
            }
        }
        private void InvCodeTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchInDB();

        }
        private void SearchInDB()
        {
            IsFiltering = true;
            InvList = null;
            List<Inventorys> CurrentList = StaticHtppClass.HttpData.MainInventoryList;
            if (LocationCmb.SelectedItem != null)
            {
                int SelectLoc = Convert.ToInt32(LocationCmb.SelectedValue);
                CurrentList = CurrentList.Where(z => z.Locations != null).Where(x => x.Locations.Id == SelectLoc).ToList();
            }
            if (WorkplaceCmb.SelectedItem != null)
            {
                int SelectWorkplace = Convert.ToInt32(WorkplaceCmb.SelectedValue);
                CurrentList = CurrentList.Where(x => x.Workplaces != null).Where(c => c.Workplaces.Id == SelectWorkplace).ToList();
            }
            if (Manuffacturers.SelectedItem != null)
            {
                int SelectManufacturer = Convert.ToInt32(Manuffacturers.SelectedValue);
                CurrentList = CurrentList.Where(x => x.Manufacturers != null).Where(c => c.Manufacturers.Id == SelectManufacturer).ToList();
            }
            if (TypeCmb.SelectedItem != null)
            {
                int SelectType = Convert.ToInt32(TypeCmb.SelectedValue);
                CurrentList = CurrentList.Where(x => x.TypeOfInventory != null).Where(c => c.TypeOfInventory.Id == SelectType).ToList();
            }
            if (ProvidersCmb.SelectedItem != null)
            {
                int SelectProvider = Convert.ToInt32(ProvidersCmb.SelectedValue);
                CurrentList = CurrentList.Where(x => x.Providers != null).Where(c => c.Providers.Id == SelectProvider).ToList();
            }
            if (NameTxb.Text != "")
            {
                if (CurrentList.Where(x => x.Name.ToLower().Contains(NameTxb.Text.ToLower())).ToList() != null)
                {
                    CurrentList = CurrentList.Where(x => x.Name.ToLower().Contains(NameTxb.Text.ToLower())).ToList();

                }
            }
            if (InvCodeTxb.Text != "")
            {
                if (CurrentList.Where(x => x.InventoryCode.ToLower().Contains(InvCodeTxb.Text.ToLower())).ToList() != null)
                {
                    CurrentList = CurrentList.Where(x => x.InventoryCode.ToLower().Contains(InvCodeTxb.Text.ToLower())).ToList();

                }
            }

            if (InvCodeTxb.Text == "" && NameTxb.Text == "" && ProvidersCmb.SelectedItem == null && TypeCmb.SelectedItem == null && Manuffacturers.SelectedItem == null && WorkplaceCmb.SelectedItem == null && LocationCmb.SelectedItem == null)
            {
                IsFiltering = false;
            }

            InvList = CurrentList;

        }

        private void InvNameInList_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (IsEditBtnPressed)
            {
                selectedItem.Name = txb.Text;
            }
            
        }

        private void CodeInList_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (IsEditBtnPressed)
            {
                selectedItem.InventoryCode = txb.Text;
            }
            
        }

        private void PriceTxb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (IsEditBtnPressed)
            {
                if (txb.Text != "")
                {
                    selectedItem.Price = Convert.ToDouble(txb.Text);

                }
            }
        }

        private void TxbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (IsEditBtnPressed)
            {
                selectedItem.Amount = txb.Text;

            }
        }
        #endregion

        #region selections changed

        private void CmbLoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationCmb.SelectedValue != null)
            {
                SelectedLocId = Convert.ToInt32(LocationCmb.SelectedValue);
                InvList = initialInvList.Where(z => z.Locations != null).Where(x => x.Locations.Id == SelectedLocId).ToList();
                WorkplaceCmb.ItemsSource = InitialWorkplaces.Where(x => x.Locations.Id == SelectedLocId);
                SearchInDB();
            }
        }

        private void WorkplaceCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInDB();
        }

        private void Cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchInDB();
        }

        private void ProviderInList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            int select = Convert.ToInt32(box.SelectedValue);
            if (IsEditBtnPressed)
            {
                Providers providers = InitialProviders.FirstOrDefault(x => x.Id == select);
                selectedItem.Providers = providers;

            }
        }

        private void LocCmbInList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            int select = Convert.ToInt32(box.SelectedValue);
            changedWorkplaces = InitialWorkplaces.Where(x => x.Locations.Id == select).ToList();
            if (IsEditBtnPressed)
            {
                Locations locations = InitialLocations.FirstOrDefault(x => x.Id == select);
                selectedItem.Locations = locations;
                selectedItem.Workplaces = null;
            }
        }

        private void TypeInList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            int select = Convert.ToInt32(box.SelectedValue);
            if (IsEditBtnPressed)
            {
                TypeOfInventory type = InitialTypeOfInventory.FirstOrDefault(x => x.Id == select);
                selectedItem.TypeOfInventory = type;
            }
        }

        private void ManufInList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            int select = Convert.ToInt32(box.SelectedValue);
            if (IsEditBtnPressed)
            {
                Manufacturers manufacturers = InitialManufacturers.FirstOrDefault(x => x.Id == select);
                selectedItem.Manufacturers = manufacturers;

            }
        }

        private void WrkplceCmd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            int select = Convert.ToInt32(box.SelectedValue);
            if (IsEditBtnPressed)
            {
                Workplaces workplaces = InitialWorkplaces.FirstOrDefault(x => x.Id == select);
                selectedItem.Workplaces = workplaces;
            }
        }

        private void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedValueTable == Convert.ToInt32(MainList.SelectedValue) && Convert.ToInt32(MainList.SelectedValue) != -1)
            {
                IsEditBtnPressed = true;
            }
            else
            {
                IsEditBtnPressed = false;
            }
        }
        #endregion

        #region Buttons clicks

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!isAnimate)
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = FilterPanel.Height;
                buttonAnimation.To = 150;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
                FilterPanel.BeginAnimation(StackPanel.HeightProperty, buttonAnimation);
                DoubleAnimation buttonAnimation1 = new DoubleAnimation();
                buttonAnimation1.From = MainList.Height;
                buttonAnimation1.To = 245;
                buttonAnimation1.Duration = TimeSpan.FromSeconds(0.5);
                MainList.BeginAnimation(DataGrid.HeightProperty, buttonAnimation1);
                DoubleAnimation da = new DoubleAnimation() { From = 360, To = 180, Duration = TimeSpan.FromSeconds(0.5) };
                RotateTransform rt = new RotateTransform() { CenterX = 10, CenterY = 10 };
                ArrowImg.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
                isAnimate = true;
            }
            else
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = FilterPanel.Height;
                buttonAnimation.To = 35;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
                FilterPanel.BeginAnimation(StackPanel.HeightProperty, buttonAnimation);
                DoubleAnimation buttonAnimation1 = new DoubleAnimation();
                buttonAnimation1.From = MainList.Height;
                buttonAnimation1.To = 370;
                buttonAnimation1.Duration = TimeSpan.FromSeconds(0.5);
                MainList.BeginAnimation(DataGrid.HeightProperty, buttonAnimation1);
                DoubleAnimation da = new DoubleAnimation() { From = 180, To = 0, Duration = TimeSpan.FromSeconds(0.5) };
                RotateTransform rt = new RotateTransform() { CenterX = 10, CenterY = 10 };
                ArrowImg.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
                isAnimate = false;
            }
        }


        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            WorkplaceCmb.SelectedItem = null;
            LocationCmb.SelectedItem = null;
            ProvidersCmb.SelectedItem = null;
            Manuffacturers.SelectedItem = null;
            TypeCmb.SelectedItem = null;

            NameTxb.Text = "";
            InvCodeTxb.Text = "";
            InvList = StaticHtppClass.HttpData.MainInventoryList;

            Binding binding = new Binding();
            binding.Source = StaticHtppClass.HttpData;
            binding.Path = new PropertyPath(nameof(StaticHtppClass.HttpData.MainWorkplacesList));
            WorkplaceCmb.SetBinding(ComboBox.ItemsSourceProperty, binding);
            IsFiltering = false;
        }

        private void EditInv_Click(object sender, RoutedEventArgs e)
        {
            SelectedValueTable = Convert.ToInt32(MainList.SelectedValue);
            selectedItem = selectItem();
            changedPhoto = selectedItem.Photo;
            IsEditBtnPressed = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsEditBtnPressed = false;
            SelectedValueTable = -1;
            
            int k = Convert.ToInt32(MainList.SelectedValue);
            //InvList = StaticHtppClass.HttpData.MainInventoryList;
            changedWorkplaces = null;
            SearchInDB();
            MainList.SelectedValue = k;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (initialInvList.FirstOrDefault(x => x.Id != selectedItem.Id && x.InventoryCode == selectedItem.InventoryCode) != null)
            {
                MessageBox.Show("Предмет с таким инвентарным номером уже имеется  базе данных", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DbActions.PutInventory(selectedItem))
            {
                IsEditBtnPressed = false;
                MessageBox.Show("Данные обновлены","Уведомление",MessageBoxButton.OK,MessageBoxImage.Information);
                SelectedValueTable = -1;
                int k = Convert.ToInt32(MainList.SelectedValue);
                GetInvAsync(k);
            }
            else
            {
                MessageBox.Show("Возникла ошибка при изменении данных","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                IsEditBtnPressed = false;
            }
            changedWorkplaces = null;
        }

        private void PhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {

                try
                {
                    for (int i = 0; i < openFile.FileNames.Length; i++)
                    {

                        byte[] fileByteArray = File.ReadAllBytes(openFile.FileNames[i]);

                        changedPhoto = fileByteArray;
                        selectedItem.Photo = fileByteArray;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("Критическая ошибка", "Внимание!");
                }

            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            //GetItemsAsync();
            getOthers();
        }

        private void AddToPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintClass.addToPrintList(selectItem());
        }

        #endregion

        #region others

        private Inventorys selectItem()
        {
            return new Inventorys()
            {
                Id = (MainList.SelectedItem as Inventorys).Id,
                Amount = (MainList.SelectedItem as Inventorys).Amount,
                Description = (MainList.SelectedItem as Inventorys).Description,
                Locations = (MainList.SelectedItem as Inventorys).Locations,
                Workplaces = (MainList.SelectedItem as Inventorys).Workplaces,
                Price = (MainList.SelectedItem as Inventorys).Price,
                Photo = (MainList.SelectedItem as Inventorys).Photo,
                Name = (MainList.SelectedItem as Inventorys).Name,
                InventoryCode = (MainList.SelectedItem as Inventorys).InventoryCode,
                Providers = (MainList.SelectedItem as Inventorys).Providers,
                TypeOfInventory = (MainList.SelectedItem as Inventorys).TypeOfInventory,
                Manufacturers = (MainList.SelectedItem as Inventorys).Manufacturers
            };
        }
        private void RowDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = (DataGridRow)sender;

            row.DetailsVisibility = row.DetailsVisibility == Visibility.Collapsed ?
                Visibility.Visible : Visibility.Collapsed;
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
        #endregion

        #region propertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        private void writeOffBtn_Click(object sender, RoutedEventArgs e)
        {
            writeOffClass.addToWriteOffList(selectItem());
        }
    }
}
