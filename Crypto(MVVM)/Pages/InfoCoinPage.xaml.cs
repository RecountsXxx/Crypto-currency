using Crypto_MVVM_.Models;
using Crypto_MVVM_.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crypto_MVVM_.Pages
{
    /// <summary>
    /// Логика взаимодействия для InfoCoinPage.xaml
    /// </summary>
    public partial class InfoCoinPage : Page
    {
        public InfoCoinPage(Coin coin)
        {
            Coin coinHistory = coin;
            InitializeComponent();
            DataContext = new InfoCoinPageViewModel(coinHistory);
        }
    }
}
