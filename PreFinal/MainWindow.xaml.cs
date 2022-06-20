using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APIModels.DataFiles;
using APIModels.Models;
using PreFinal.Pages;
using PreFinal.Windows;
using Squirrel;

namespace PreFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            PrintClass.PrintInventorysList = new System.Collections.ObjectModel.ObservableCollection<Inventorys>();
            FrameApp.FrameObject = FrmMain;
            VisApp.visiblePanel = MenuPanel;
            FrameApp.FrameObject.Navigate(new AutorisationPage());
            selectedButtonIndex = 1;
            //FrameApp.FrameObject.Navigate(new StartPage());
        }

        bool _isAdmin;
        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        bool _IsAuth = false;
        public bool IsAuth
        {
            get
            {
                return _IsAuth;
            }
            set
            {
                _IsAuth = value;
                OnPropertyChanged();
            }
        }

        int _selectedButtonIndex;
        public int selectedButtonIndex
        {
            get
            {
                return _selectedButtonIndex;
            }
            set
            {
                _selectedButtonIndex = value;
                OnPropertyChanged();
            }
        }


        bool isAnimate = true;

        private void Animate()
        {
            if (!isAnimate)
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = MenuPanel.Width;
                buttonAnimation.To = 40;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.3);
                MenuPanel.BeginAnimation(StackPanel.WidthProperty, buttonAnimation);
                LogoutPanel.BeginAnimation(StackPanel.WidthProperty, buttonAnimation);
            }
            else
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = MenuPanel.Width;
                buttonAnimation.To = 300;
                buttonAnimation.Duration = TimeSpan.FromSeconds(0.3);
                MenuPanel.BeginAnimation(StackPanel.WidthProperty, buttonAnimation);
                LogoutPanel.BeginAnimation(StackPanel.WidthProperty, buttonAnimation);
            }
            isAnimate = !isAnimate;
        }

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            Animate();
        }

        private void ViewInvBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.NavigateViewInventoryPage();
            selectedButtonIndex = 1;
            if (!isAnimate)
            {
                Animate();
            }
            
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.ClearAllPages();
            FrameApp.FrameObject.Navigate(new AutorisationPage());
            selectedButtonIndex = 1;
            if (!isAnimate)
            {
                Animate();
            }
        }

        private void InventoryzationBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.NavigateInventorizationPage();
            selectedButtonIndex = 2;
            if (!isAnimate)
            {
                Animate();
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.NavigateAddPage();
            selectedButtonIndex = 3;
            if (!isAnimate)
            {
                Animate();
            }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.NavigateNewPrintPage();
            selectedButtonIndex = 4;
            if (!isAnimate)
            {
                Animate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void ReportsBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Reports");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            Process.Start("explorer.exe", path);
        }

        private void WriteOffBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameApp.FrameObject.Navigate(new WriteOffPage());
            selectedButtonIndex = 6;
            if (!isAnimate)
            {
                Animate();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var updateManager = await UpdateManager.GitHubUpdateManager("github"))
                {
                    var release = await updateManager.UpdateApp();
                }
            }
            catch (Exception)
            {

                throw;
            }
            //using (var updateManager = new UpdateManager(@"C:\SquirrelReleases"))
            //{
            //    MessageBox.Show($"Current version: {updateManager.CurrentlyInstalledVersion()}");
            //    var releaseEntry = await updateManager.UpdateApp();
            //    MessageBox.Show($"Update Version: {releaseEntry?.Version.ToString() ?? "No update"}");
            //}

        }
    }
}
