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
using System.Windows.Shapes;

namespace PreFinal.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            BtnReg.Foreground = Brushes.Black;
            a = TxbLogin.BorderBrush;
        }
        public static string sas(string ka)
        {
            var str = ka.ToLower();
            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToUpper();
        }

        public static Brush a;
        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Users users;
                if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
                {
                    users = DbActions.GetUsers().FirstOrDefault(x => x.Login == TxbLogin.Text && sas(x.FirstName) == sas(TxbFirstName.Text) && sas(x.Surname) == sas(TxbSurname.Text) && sas(x.Patronymic) == sas(PatronymicTxb.Text));
                    if (users != null)
                    {
                        MessageBox.Show("Такой пользователь уже зарегистрирован",
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                    }
                    else
                    {
                        

                        users = new Users
                        {
                            Login = sas(TxbLogin.Text),
                            Password = sas(TxbPassword2.Password),
                            FirstName = sas(TxbFirstName.Text),
                            Surname = sas(TxbSurname.Text),
                            Patronymic = sas(PatronymicTxb.Text),
                            Roles = new Roles() { Id = 2}
                        };
                        if (DbActions.PostUsers(users))
                        {
                            MessageBox.Show("Пользователь зарегистрирован",
                                 "Уведомление",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критический сбой в работе приложения:" + ex.Message.ToString(),
                                "Уведомление",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void TxbFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

            TxbLogin.BorderBrush = a;
            if (DbActions.GetUsers().FirstOrDefault(x => x.Login == TxbLogin.Text) == null)
            {
                if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
                {
                    BtnReg.IsEnabled = true;
                    BtnReg.Foreground = Brushes.White;
                }
                else
                {
                    BtnReg.IsEnabled = false;
                    BtnReg.Foreground = Brushes.Black;
                }
            }
            else
            {
                TxbLogin.BorderBrush = Brushes.Red;
            }

        }

        private void TxbPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
            {
                BtnReg.IsEnabled = true;
                BtnReg.Foreground = Brushes.White;
            }
            else
            {
                BtnReg.IsEnabled = false;
                BtnReg.Foreground = Brushes.Black;
            }
        }

        
    }
}
