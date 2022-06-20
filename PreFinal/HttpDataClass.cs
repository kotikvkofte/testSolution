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

namespace PreFinal
{
    class HttpDataClass : INotifyPropertyChanged
    {
        public HttpDataClass()
        {
            IsLoaded = false;
            InventorizationLoaded = false;
            getAllItems();
        }
        public HttpDataClass(Users user)
        {
            IsLoaded = false;
            InventorizationLoaded = false;
            getItemsWhenUserIsNotAdmin(user);
        }
        public async void getAllItems()
        {
            IsLoaded = false;
            await GetMainInventoryListAsync();
            await GetMainLocationsListAsync();
            await GetMainWorkplacesListAsync();
            await GetMainManufacturersListAsync();
            await GetMainTypeOfInventoryListAsync();
            await GetMainProvidersListAsync();
            await GetMainUsersListAsync();
            await GetMainRolesListAsync();
            IsLoaded = true;
        }
        public async void getItemsWhenUserIsNotAdmin(Users user)
        {
            IsLoaded = false;
            await GetMainInventoryListAsync();
            MainInventoryList = MainInventoryList.Where(x => x.Locations.Users.Id == user.Id).ToList();
            await GetMainLocationsListAsync();
            CmbLocationsList = MainLocationsList.Where(x => x.Users.Id == user.Id).Select(x => (object)new
            {
                Id = x.Id,
                Location = x.Location
            }).ToList();
            await GetMainWorkplacesListAsync();
            MainWorkplacesList = MainWorkplacesList.Where(x => x.Locations.Users.Id == user.Id).ToList();
            await GetMainManufacturersListAsync();
            await GetMainTypeOfInventoryListAsync();
            await GetMainProvidersListAsync();
            await GetMainUsersListAsync();
            await GetMainRolesListAsync();
            IsLoaded = true;
        }
        public async Task GetMainInventoryListAsync()
        {
            MainInventoryList = await DbActions.GetInventorysAsync();
        }
        List<Inventorys> _mainInventoryList;
        public List<Inventorys> MainInventoryList
        {
            get
            {
                return _mainInventoryList;
            }
            set
            {
                _mainInventoryList = value;
                OnPropertyChanged(nameof(MainInventoryList));
            }
        }
        bool _inventorizationLoaded;
        public bool InventorizationLoaded
        {
            get
            {
                return _inventorizationLoaded;
            }
            set
            {
                _inventorizationLoaded = value;
                OnPropertyChanged();
            }
        }
        bool _isLoaded;
        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
            set
            {
                if (FrameApp.viewInventory != null && value == true)
                {
                    FrameApp.viewInventory.getOthers();
                }
                _isLoaded = value;
                OnPropertyChanged();
            }
        }
        List<Locations> _mainLocationsList;
        public List<Locations> MainLocationsList
        {
            get
            {
                return _mainLocationsList;
            }
            set
            {
                _mainLocationsList = value;
                OnPropertyChanged();
            }
        }
        List<object> _cmbLocationsList;
        public List<object> CmbLocationsList
        {
            get
            {
                return _cmbLocationsList;
            }
            set
            {
                _cmbLocationsList = value;
                OnPropertyChanged();
            }
        }
        List<Workplaces> _mainWorkplacesList;
        public List<Workplaces> MainWorkplacesList
        {
            get
            {
                return _mainWorkplacesList;
            }
            set
            {
                _mainWorkplacesList = value;
                OnPropertyChanged();
            }
        }
        List<Manufacturers> _mainManufacturersList;
        public List<Manufacturers> MainManufacturersList
        {
            get
            {
                return _mainManufacturersList;
            }
            set
            {
                _mainManufacturersList = value;
                OnPropertyChanged();
            }
        }
        List<object> _cmbManufacturersList;
        public List<object> CmbManufacturersList
        {
            get
            {
                return _cmbManufacturersList;
            }
            set
            {
                _cmbManufacturersList = value;
                OnPropertyChanged();
            }
        }
        List<TypeOfInventory> _mainTypeOfInventoryList;
        public List<TypeOfInventory> MainTypeOfInventoryList
        {
            get
            {
                return _mainTypeOfInventoryList;
            }
            set
            {
                _mainTypeOfInventoryList = value;
                OnPropertyChanged();
            }
        }
        List<object> _cmbTypeOfInventoryList;
        public List<object> CmbTypeOfInventoryList
        {
            get
            {
                return _cmbTypeOfInventoryList;
            }
            set
            {
                _cmbTypeOfInventoryList = value;
                OnPropertyChanged();
            }
        }
        List<Providers> _mainProvidersList;
        public List<Providers> MainProvidersList
        {
            get
            {
                return _mainProvidersList;
            }
            set
            {
                _mainProvidersList = value;
                OnPropertyChanged();
            }
        }
        List<object> _cmbProvidersList;
        public List<object> CmbProvidersList
        {
            get
            {
                return _cmbProvidersList;
            }
            set
            {
                _cmbProvidersList = value;
                OnPropertyChanged();
            }
        }
        List<Users> _mainUsersList;
        public List<Users> MainUsersList
        {
            get
            {
                return _mainUsersList;
            }
            set
            {
                _mainUsersList = value;
                OnPropertyChanged();
            }
        }
        List<Roles> _mainRolesList;
        public List<Roles> MainRolesList
        {
            get
            {
                return _mainRolesList;
            }
            set
            {
                _mainRolesList = value;
                OnPropertyChanged();
            }
        }
        List<Stocktaking> _mainStocktakingList;
        public List<Stocktaking> MainStocktakingList
        {
            get
            {
                return _mainStocktakingList;
            }
            set
            {
                _mainStocktakingList = value;
                OnPropertyChanged();
            }
        }
        List<StocktakingInventory> _mainStocktakingInventoryList;
        public List<StocktakingInventory> MainStocktakingInventoryList
        {
            get
            {
                return _mainStocktakingInventoryList;
            }
            set
            {
                _mainStocktakingInventoryList = value;
                OnPropertyChanged();
            }
        }




        public async Task getInventoryzationItems()
        {
            await GetMainStocktakingListAsync();
            await GetMainStocktakingInventoryListAsync();
            InventorizationLoaded = true;
        }
        public async Task GetMainStocktakingInventoryListAsync()
        {
            MainStocktakingInventoryList = await DbActions.GetLiteStocktakingInventoryAsync();
            //MainStocktakingInventoryList = await DbActions.GetStocktakingInventoryAsync();
        }
        public async Task GetMainStocktakingListAsync()
        {
            MainStocktakingList = await DbActions.GetStocktakingAsync();
        }
        public async Task GetMainRolesListAsync()
        {
            MainUsersList = await DbActions.GetUsersAsync();
        }
        public async Task GetMainUsersListAsync()
        {
            MainUsersList = await DbActions.GetUsersAsync();
        }
        public async Task GetMainLocationsListAsync()
        {
            MainLocationsList = await DbActions.GetLocationsAsync();
            CmbLocationsList = MainLocationsList.Select(x => (object) new 
            {
                Id = x.Id,
                Location = x.Location
            }).ToList();
        }
        public async Task GetMainWorkplacesListAsync()
        {
            MainWorkplacesList = await DbActions.GetWorkplacesAsync();
        }
        public async Task GetMainManufacturersListAsync()
        {
            MainManufacturersList = await DbActions.GetManufacturersAsync();
            CmbManufacturersList = MainManufacturersList.Select(x => (object) new
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
        public async Task GetMainTypeOfInventoryListAsync()
        {
            MainTypeOfInventoryList = await DbActions.GetTypeOfInventoryAsync();
            CmbTypeOfInventoryList = MainTypeOfInventoryList.Select(x => (object)new
            {
                Id = x.Id,
                Type = x.Type
            }).ToList();
        }
        public async Task GetMainProvidersListAsync()
        {
            MainProvidersList = await DbActions.GetProvidersAsync();
            CmbProvidersList = MainProvidersList.Select(x => (object)new
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
