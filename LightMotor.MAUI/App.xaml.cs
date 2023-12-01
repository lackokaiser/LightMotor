using LightMotor.Persistence;
using LightMotor.Root;
using LightMotor.ViewModel.ViewModel;

namespace LightMotor.MAUI;

public partial class App
{
    private readonly Game _model = new(new MauiPersistenceProvider());
    
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell(ref _model)
        {
            BindingContext = new MenuViewModel(ref _model)
        };

    }
}