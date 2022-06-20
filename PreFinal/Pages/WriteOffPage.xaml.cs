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
    /// Логика взаимодействия для WriteOffPage.xaml
    /// </summary>
    public partial class WriteOffPage : Page, INotifyPropertyChanged
    {
        public WriteOffPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void WriteOffBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                writeOffClass.wtiteOff();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(),"Критическая работа приложения",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void OpenWriteOffsFolder_Click(object sender, RoutedEventArgs e)
        {
            writeOffClass.openFolder();
        }

        private void DeleteItemFromList_Click(object sender, RoutedEventArgs e)
        {
            Inventorys delItem = MainList.SelectedItem as Inventorys;
            writeOffClass.deleteFromWriteOffList(delItem);
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
