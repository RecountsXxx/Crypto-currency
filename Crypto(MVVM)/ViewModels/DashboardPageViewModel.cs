using Crypto_MVVM_.Models;
using Crypto_MVVM_.Pages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Crypto_MVVM_.ViewModels
{
    class DashboardPageViewModel : ViewModelBase
    {
        public ObservableCollection<Coin> coinsList = new ObservableCollection<Coin>();
        private ObservableCollection<Coin> _coins;
        public ObservableCollection<Coin> coins
        {
            get { return _coins; }
            set { _coins = value;OnPropertyChanged(); }
        }

        public DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);

        public ICommand ItemSelectionChanged { get; set; }
        public ICommand SearchTextChanged { get; set; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(); }
        }
        
        public Page page = new Page();

        public DashboardPageViewModel(Page page,ObservableCollection<Coin> coinsq)
        {
            this.page = page;

            this.coins = coinsq;
            coinsList = coins;

            ItemSelectionChanged = new RelayCommand(OnItemSelectionChanged);

            SearchTextChanged = new RelayCommand(OnSearchTextChanged);

            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) => refreshDataFromAPI();
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
                item.colorRankCoin = Brushes.White;
                item.textColorRankCoin = Brushes.LightGray;
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

        private void OnItemSelectionChanged(object obj)
        {
            if (obj is Coin coin)
            {
                page.NavigationService.Navigate(new InfoCoinPage(coin));
            }
        }
        private void OnSearchTextChanged(object obj)
        {
            string str = SearchText;
            List<Coin> filderCoinList = new List<Coin>();
            if (str.Equals(""))
            {
                filderCoinList.AddRange(coinsList);
            }
            else
            {
                foreach (var item in coins)
                {
                    if (item.name.Contains(str) || item.rank.Contains(str) || item.symbol.Contains(str) || item.id.Contains(str))
                    {
                        filderCoinList.Add(item);
                    }
                }
            }
            coins = new ObservableCollection<Coin>(filderCoinList);
        }
    }
}
