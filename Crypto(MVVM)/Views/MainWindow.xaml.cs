using Crypto_MVVM_.Models;
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

namespace Crypto_MVVM_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow window;

        public MainWindow()
        {
             System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.languageCode);
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            window = this;
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
                  if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.window.DragMove();
            }
        }
    }
}
