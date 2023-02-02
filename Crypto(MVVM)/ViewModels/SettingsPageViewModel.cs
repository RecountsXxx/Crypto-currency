using Crypto_MVVM_.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Crypto_MVVM_.ViewModels
{
     class SettingsPageViewModel : ViewModelBase
     {
        public ICommand SelectionChangedTheme { get; set; }
        public int SelectedIndexTheme { get; set; }

        public ICommand SelectionChangedLanguage { get; set; }
        public int SelectedIndexLanguage { get; set; }


        public SettingsPageViewModel()
        {
            SelectionChangedTheme = new RelayCommand(OnSelectionChangedTheme);
            if (Application.Current.Resources.MergedDictionaries[0].Source == new Uri("Themes/WhiteTheme.xaml", UriKind.Relative))
                SelectedIndexTheme = 1;
            if (Application.Current.Resources.MergedDictionaries[0].Source == new Uri("Themes/DarkTheme.xaml", UriKind.Relative))
                SelectedIndexTheme = 0;

            if (Properties.Settings.Default.languageCode == "en-US")
                SelectedIndexLanguage = 0;
            if (Properties.Settings.Default.languageCode == "uk-UK")
                SelectedIndexLanguage = 1;
            SelectionChangedLanguage = new RelayCommand(OnSelectionChangedLanguage);
        }

        private void OnSelectionChangedTheme(object obj)
        {
            string str = obj.ToString();
            if (str == "System.Windows.Controls.ComboBoxItem: Dark theme" || str == "System.Windows.Controls.ComboBoxItem: Темна тема")
            {
                var uri = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries[0].Source = uri;
                ResourceDictionary resurce = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resurce);
            }
            if (str == "System.Windows.Controls.ComboBoxItem: White theme" || str == "System.Windows.Controls.ComboBoxItem: Світла тема")
            {
                var uri = new Uri("Themes/WhiteTheme.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries[0].Source = uri;
                ResourceDictionary resurce = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resurce);
            }
            using(StreamWriter writer = new StreamWriter("../../Themes/ThemeSettings.txt", false))
            {
                writer.WriteLine(str);
            }
        }

        private void OnSelectionChangedLanguage(object obj)
        {
            if (SelectedIndexLanguage == 0)
            {
                Properties.Settings.Default.languageCode = "en-US";
            }
            if (SelectedIndexLanguage == 1)
            {
                Properties.Settings.Default.languageCode = "uk-UK";
            }
            Properties.Settings.Default.Save();
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
