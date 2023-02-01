using Crypto_MVVM_.Pages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Crypto_MVVM_.Models
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Coin> coins = new ObservableCollection<Coin>();

        public string langCode = Crypto_MVVM_.Properties.Settings.Default.languageCode;

        private WindowState _windowState;
        public WindowState WindowState
        {
            get { return _windowState; }
            set { _windowState = value; OnPropertyChanged(); }
        }

        private string _currentPageLabel;
        public string CurrentPageLabel
        {
            get { return _currentPageLabel; }
            set { _currentPageLabel = value;OnPropertyChanged(); }
        }

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(); }
        }

        private double _frameOpacity;
        public double FrameOpacity
        {
            get { return _frameOpacity; }
            set { _frameOpacity = value; OnPropertyChanged(); }
        }

        public string languageString { get; set; }

        #region Commands window
        public ICommand btnMinimize { get; set; }
        public ICommand btnRestore { get; set; }
        public ICommand btnClose { get; set; }
        #endregion

        #region Left menu buttons
        public ICommand DashboardClick { get; set; }
        public ICommand SettingsClick { get; set; }
        public ICommand ExchangeClick { get; set; }
        #endregion


        public MainWindowViewModel()
        {
            refreshDataFromAPI();

            DashboardClick = new RelayCommand(OnDashboardClick);
            SettingsClick = new RelayCommand(OnSettingsClick);
            ExchangeClick = new RelayCommand(OnExchangeClick);
            CurrentPage = new DashboardPage(coins);
            FrameOpacity = 1;

            if (langCode == "uk-UK")
                CurrentPageLabel = "Панель";
            if (langCode == "en-US")
                    CurrentPageLabel = "Dashboard";

            btnMinimize = new RelayCommand(OnBtnMinimize);
            btnRestore = new RelayCommand(OnBtnRestore);
            btnClose = new RelayCommand(OnBtnClose);
            WindowState = WindowState.Normal;
        }

        private void OnBtnMinimize(object obj) => WindowState = WindowState.Minimized;
        private void OnBtnRestore(object obj)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        private void OnBtnClose(object obj) => Environment.Exit(0);

        private  void OnDashboardClick(object obj) => SlowOpacity(new DashboardPage(coins));
        private  void OnSettingsClick(object obj) => SlowOpacity(new SettingsPage());
        private  void OnExchangeClick(object obj) => SlowOpacity(new ExchangePage());
        private async void SlowOpacity(Page page)
        {
            if (page.ToString() == "Crypto_MVVM_.Pages.DashboardPage")
            {
                if (langCode == "uk-UK")
                    CurrentPageLabel = "Панель";
                if (langCode == "en-US")
                    CurrentPageLabel = "Dashboard";
            }
            if (page.ToString() == "Crypto_MVVM_.Pages.SettingsPage")
            {
                if (langCode == "uk-UK")
                    CurrentPageLabel = "Ладнати";
                if (langCode == "en-US")
                    CurrentPageLabel = "Settings";
            }
            if (page.ToString() == "Crypto_MVVM_.Pages.ExchangePage")
            {
                if (langCode == "uk-UK")
                    CurrentPageLabel = "Обмін";
                if (langCode == "en-US")
                    CurrentPageLabel = "Exchange";
            }
            await Task.Factory.StartNew(() =>
            {

                for (double i = 1.0; i > 0.0; i -= 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(4);
                }
                CurrentPage = page;
                for (double i = 0.0; i < 1.1; i += 0.1)
                {
                    FrameOpacity = i;
                    Thread.Sleep(4);

                }
            });
        }

        private void getDataFromAPI()
        {
            List<Coin> tempList = new List<Coin>();
            HttpClient Client = new HttpClient();
            string URL = "https://api.coincap.io/v2/";
            string response = Client.GetStringAsync(URL + "assets").Result;
            tempList = JObject.Parse(response)["data"].Select(d => d.ToObject<Coin>()).ToList();
            Random rd = new Random();
            coins = new ObservableCollection<Coin>(tempList.ToList());
            foreach (var item in coins)
                item.colorCoin = new SolidColorBrush(Color.FromRgb((byte)rd.Next(0, 256), (byte)rd.Next(0, 256), (byte)rd.Next(0, 256)));

            for (int i = 0; i < coins.Count; i++)
            {
                coins[i].priceUsd = coins[i].priceUsd.Substring(0, coins[i].priceUsd.IndexOf(".") + 6);
                coins[i].volumeUsd24Hr = coins[i].volumeUsd24Hr.Substring(0, coins[i].volumeUsd24Hr.IndexOf(".") + 6);
                coins[i].changePercent24Hr = coins[i].changePercent24Hr.Substring(0, coins[i].changePercent24Hr.IndexOf(".") + 6);
                coins[i].volumeUsd24Hr += " $";
                coins[i].changePercent24Hr += " %";
                coins[i].priceUsd += " $";
            }
        }
        private void refreshDataFromAPI()
        {
            getDataFromAPI();
            foreach (var item in coins)
            {
                if (item.changePercent24Hr.Contains("-"))
                {
                    item.changePercentColor = Brushes.OrangeRed;
                }
                else
                {
                    item.changePercentColor = Brushes.LimeGreen;
                }
            }
        }
    }
}
