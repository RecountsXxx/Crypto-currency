using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Crypto_MVVM_
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
    }
    
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var langCode = Crypto_MVVM_.Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(langCode);

            string themeTempIndex;
            if (File.Exists("ThemeSettings.txt"))
            {
                using (StreamReader reader = new StreamReader("ThemeSettings.txt"))
                {
                    themeTempIndex = reader.ReadLine();
                }
                if (themeTempIndex == "System.Windows.Controls.ComboBoxItem: Dark theme")
                {
                    var uri = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                    Application.Current.Resources.MergedDictionaries[0].Source = uri;
                    ResourceDictionary resurce = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resurce);
                }
                if (themeTempIndex == "System.Windows.Controls.ComboBoxItem: White theme")
                {
                    var uri = new Uri("Themes/WhiteTheme.xaml", UriKind.Relative);
                    Application.Current.Resources.MergedDictionaries[0].Source = uri;
                    ResourceDictionary resurce = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resurce);
                }
            }
        

            base.OnStartup(e);
        }
    }
}
