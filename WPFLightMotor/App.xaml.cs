using System;
using System.Windows;
using LightMotor.Persistence;
using LightMotor.Root;
using LightMotorViewModel;
using LightMotorViewModel.EventArgs;
using LightMotorViewModel.ViewModel;
using Microsoft.Win32;

namespace WPFLightMotor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IDisposable
    {
        private Game _model = new (new WindowsPersistenceProvider());
        private MainViewModel _mainVm = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            _mainVm = new MainViewModel(ref _model);
            Navigation.Get().CurrentViewModel = new MenuViewModel(ref _model);
            
            ViewCallback.Get().OpenFileMessenger += OnFileOpenRequest;
            ViewCallback.Get().SaveFileMessenger += OnFileSaveRequest;
            ViewCallback.Get().ShowMessageMessenger += OnShowMessage;
            
            MainWindow = new MainWindow()
            {
                DataContext = _mainVm
            };
            
            MainWindow.Show();
            
            base.OnStartup(e);
        }

        private void OnShowMessage(object obj, ShowMessageEventArgs args)
        {
            if (MainWindow != null) MessageBox.Show(MainWindow, args.Text, args.Caption);
        }

        private void OnFileSaveRequest()
        {
            SaveFileDialog file = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*",
                InitialDirectory = "c:\\",
                CheckFileExists = false
            };

            bool? res = file.ShowDialog();

            if (res != null && (bool)res)
            {
                ViewCallback.Get().OpenedFile = file.FileName;
            }
            else
                ViewCallback.Get().OpenedFile = String.Empty;
        }

        private void OnFileOpenRequest()
        {
            OpenFileDialog file = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Text files (*.txt)|*txt|All files (*.*)|*.*",
                InitialDirectory = "c:\\"
            };

            bool? res = file.ShowDialog();

            if (res != null && (bool)res)
            {
                ViewCallback.Get().OpenedFile = file.FileName;
            }
            else
                ViewCallback.Get().OpenedFile = String.Empty;
        }

        public void Dispose()
        {
            _model.Dispose();
        }
    }
}