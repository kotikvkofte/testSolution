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
    /// Логика взаимодействия для InventoryCard.xaml
    /// </summary>
    public partial class InventoryCard : Window
    {
        public InventoryCard(Inventorys inventory)
        {
            InitializeComponent();
        }
    }
}
