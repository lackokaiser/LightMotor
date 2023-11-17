using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LightMotor.Root;
using LightMotorViewModel;
using LightMotorViewModel.ViewModel;

namespace WPFLightMotor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Game _model = new ();

        protected override void OnStartup(StartupEventArgs e)
        {
            Navigation.Get().CurrentViewModel = new MenuViewModel(ref _model);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(ref _model)
            };

            MainWindow.Show();
            
            base.OnStartup(e);
        }
    }
}