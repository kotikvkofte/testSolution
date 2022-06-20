using APIModels.DataFiles;
using APIModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : UserControl
    {
        public AddUser()
        {
            InitializeComponent();
            RoleCmb.DataContext = StaticHtppClass.HttpData.MainRolesList;
        }
        private Roles role;
        private List<Roles> roles;
        public static string sas(string ka)
        {
            var str = ka.ToLower();
            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToUpper();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Users users;
                if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
                {
                    users = StaticHtppClass.HttpData.MainUsersList.FirstOrDefault(x => x.Login == TxbLogin.Text && sas(x.FirstName) == sas(TxbFirstName.Text) && sas(x.Surname) == sas(TxbSurname.Text) && sas(x.Patronymic) == sas(PatronymicTxb.Text));
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
                            Roles = role
                        };
                        if (DbActions.PostUsers(users))
                        {
                            MessageBox.Show("Пользователь зарегистрирован",
                                 "Уведомление",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Information);
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
            if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
            {
                BtnReg.IsEnabled = true;
            }
            else
            {
                BtnReg.IsEnabled = false;
            }
        }

        private void TxbPassword2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (TxbFirstName.Text != "" && TxbSurname.Text != "" && TxbLogin.Text != "" && TxbPassword1.Password != "" && TxbPassword2.Password != "" && TxbPassword1.Password == TxbPassword2.Password)
            {
                BtnReg.IsEnabled = true;
            }
            else
            {
                BtnReg.IsEnabled = false;
            }
        }

        private void RoleCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedRoleId = Convert.ToInt32(RoleCmb.SelectedValue);
            role = roles.FirstOrDefault(x => x.Id == selectedRoleId);
        }

        private void TxbFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[а-яА-Я]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
