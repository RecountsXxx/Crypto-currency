using Crypto_MVVM_.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Crypto_MVVM_.ViewModels
{
    class InfoCoinPageViewModel : ViewModelBase
    {
        public Coin coin = new Coin();
        public List<CoinHistory> listCoins { get; set; }
        public string coinName { get; set; }
        public string coinRank { get; set; }
        public string coinSymbol { get; set; }
        public string coinOffers { get; set; }
        public string coinVolume { get; set; }
        public string coinPrice { get; set; }
        public string coinChange { get; set; }
        public string coinMedium { get; set; }

    public InfoCoinPageViewModel(Coin coin)
        {
            this.coin = coin;

            var langCode = Crypto_MVVM_.Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(langCode);

            if(langCode == "uk-UK")
            {
                coinName = "Монета: " + coin.name;
                coinRank = "Ранг монети: " + coin.rank;
                coinSymbol = "Символ монети: " + coin.symbol;
                coinOffers = "Пропозицій для продажу: " + coin.supply.Substring(0, coin.supply.IndexOf(".") + 6);

                coinVolume = "Обірт за(24г): " + coin.volumeUsd24Hr;
                coinPrice = "Ціна в USD " + coin.priceUsd + " $";
                coinChange = "Зміна за(24г): " + coin.changePercent24Hr + " %";
                coinMedium = "Середня ціна за день: " + coin.vwap24Hr.Substring(0, coin.vwap24Hr.IndexOf(".") + 6) + " $";
            }
            if(langCode == "en-US")
            {
                coinName = "Coin name: " + coin.name;
                coinRank = "Coin rank: " + coin.rank;
                coinSymbol = "Coin symbol: " + coin.symbol;
                coinOffers = "Offers for trading: " + coin.supply.Substring(0, coin.supply.IndexOf(".") + 6);

                coinVolume = "Volume per(24h): " + coin.volumeUsd24Hr;
                coinPrice = "Price for USD: " + coin.priceUsd + " $";
                coinChange = "Change per(24h): " + coin.changePercent24Hr + " %";
                coinMedium = "Medium price for day: " + coin.vwap24Hr.Substring(0, coin.vwap24Hr.IndexOf(".") + 6) + " $";

            }

            HttpClient Client = new HttpClient();
            string URL = "https://api.coincap.io/v2/";
            string response = Client.GetStringAsync(URL + "assets/" + coin.id + "/history?interval=d1").Result;
            listCoins = JObject.Parse(response)["data"].Select(d => d.ToObject<CoinHistory>()).ToList();
            foreach (var item in listCoins)
            {
                item.priceUsd = item.priceUsd.Substring(0, item.priceUsd.IndexOf(".") + 6);
                item.date = item.date.Substring(item.date.IndexOf("00:") - item.date.IndexOf("00:"), 10);
            }
        }
    }
}
