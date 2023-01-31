using Crypto_MVVM_.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crypto_MVVM_.ViewModels
{
    class ExchangePageViewModel : ViewModelBase
    {
        public ICommand AmountTextChanged { get; set; }

        public ICommand SelectionChangedOne { get; set; }
        public ICommand SelectionChangedTwo { get; set; }

        public List<Coin> coinsExchange { get; set; }
        public List<string> coins { get; set; }

        public string selectedItems { get; set; }

        private string _amount;
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(); }
        }
        private string _amountResult;
        public string AmountResult
        {
            get { return _amountResult; }
            set { _amountResult = value;OnPropertyChanged(); }
        }

        private string _selectedItemOne;
        public string selectedItemOne
        {
            get { return _selectedItemOne; }
            set { _selectedItemOne = value; }
        }

        private string _selectedItemTwo;
        public string selectedItemTwo
        {
            get { return _selectedItemTwo; }
            set { _selectedItemTwo = value; }
        }

        public ExchangePageViewModel()
        {
            AmountTextChanged = new RelayCommand(OnAmountTextChanged);

            SelectionChangedOne = new RelayCommand(OnSelectionChangedOne);
            SelectionChangedTwo = new RelayCommand(OnSelectionChangedTwo);

            selectedItems = "Bitcoin";
            getDataFromAPI();

        }
        private void OnSelectionChangedOne(object obj)
        {
            AmountResult = "";
            OnAmountTextChanged(Amount);
        }
        private void OnSelectionChangedTwo(object obj)
        {
            AmountResult = "";
            OnAmountTextChanged(Amount);
        }
        private void OnAmountTextChanged(object obj)
        {
            if(obj is string str){


                double amount, result;

                if (str == "")
                    AmountResult = "";

                    Coin coinOne = new Coin();
                    Coin coinTwo = new Coin();
                    foreach (var item in coinsExchange)
                    {
                        if (item.name == selectedItemOne)
                        {
                            coinOne = item;
                        }
                        if (item.name == selectedItemTwo)
                        {
                            coinTwo = item;
                        }
                    }
                    if (coinOne.priceUsd != null && coinTwo.priceUsd != null)
                    {
                        try
                        {
                            coinOne.priceUsd = coinOne.priceUsd.Replace(".", ",");
                            coinOne.priceUsd = coinOne.priceUsd.Replace("$", "");
                            amount = Convert.ToDouble(coinOne.priceUsd) * Convert.ToDouble(str);

                            coinTwo.priceUsd = coinTwo.priceUsd.Replace(".", ",");
                            coinTwo.priceUsd = coinTwo.priceUsd.Replace("$", "");
                            result = amount / Convert.ToDouble(coinTwo.priceUsd);

                            string resultat = result.ToString();
                            AmountResult = result.ToString();
                        }
                        catch
                        {

                        }
                    }
            }
        }

        private void getDataFromAPI()
        {
            coins = new List<string>();
            List<Coin> tempList = new List<Coin>();
            HttpClient Client = new HttpClient();
            string URL = "https://api.coincap.io/v2/";
            string response = Client.GetStringAsync(URL + "assets").Result;
            tempList = JObject.Parse(response)["data"].Select(d => d.ToObject<Coin>()).ToList();
            coinsExchange = tempList;
            foreach (var item in tempList)
                coins.Add(item.name);
        }
    }
}
