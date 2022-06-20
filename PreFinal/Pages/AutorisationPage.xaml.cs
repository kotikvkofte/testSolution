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
    /// Логика взаимодействия для AutorisationPage.xaml
    /// </summary>
    public partial class AutorisationPage : Page
    {

        public AutorisationPage()
        {
            InitializeComponent();
            Grid.SetColumn(FrameApp.FrameObject, 0);
            Grid.SetColumnSpan(FrameApp.FrameObject, 2);
            VisApp.visiblePanel.Visibility = Visibility.Collapsed;
        }
        private void TxbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxbLogin.Text != "" && TxbPassword.Password != "")
            {
                BtnLogin.IsEnabled = true;
            }
            else
            {
                BtnLogin.IsEnabled = false;
            }
        }

        private void TxbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (TxbPassword.Password.Length > 0)
            {
                TextPass.Visibility = Visibility.Collapsed;
            }
            else
            {
                TextPass.Visibility = Visibility.Visible;
            }

            if (TxbLogin.Text != "" && TxbPassword.Password != "")
            {
                BtnLogin.IsEnabled = true;
            }
            else
            {
                BtnLogin.IsEnabled = false;
            }
        }


        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = DbActions.GetUsers().FirstOrDefault(x => x.Login == TxbLogin.Text && x.Password == TxbPassword.Password);

                if (userObj == null)
                {
                    MessageBox.Show("Неверно введен логин или пароль, повторите попытку",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                }
                else
                {
                    UserInfo.user = userObj;
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (UserInfo.user.Roles.Name == "Admin")
                    {
                        mainWindow.IsAdmin = true;
                        StaticHtppClass.HttpData = new HttpDataClass();
                    }
                    else
                    {
                        mainWindow.IsAdmin = false;
                        StaticHtppClass.HttpData = new HttpDataClass(UserInfo.user);
                    }
                    FrameApp.viewInventory = new ViewIndentoryPage();
                    Grid.SetColumn(FrameApp.FrameObject, 1);
                    Grid.SetColumnSpan(FrameApp.FrameObject, 1);
                    VisApp.visiblePanel.Visibility = Visibility.Visible;
                    FrameApp.NavigateViewInventoryPage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критический сбор в работе приложения:" + ex.Message.ToString(),
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }
    }
}
