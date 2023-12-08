using LightMotor.MAUI.View;
using LightMotor.Persistence;
using LightMotor.Root;
using LightMotor.ViewModel.EventArgs;
using LightMotor.ViewModel.ViewModel;

namespace LightMotor.MAUI;

public partial class App : IDisposable
{
    private const string SuspendedGameSavePath = "susp.txt";
    private readonly Game _model;
    private readonly AppShell _appShell;
    
    public App()
    {
        InitializeComponent();
        _model = new(new MauiPersistenceProvider());

        _appShell = new AppShell(ref _model)
        {
            BindingContext = new MenuViewModel(ref _model)
        };

        MainPage = _appShell;

    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window w = base.CreateWindow(activationState);

        w.Deactivated += OnWindowDeactivating;
        w.Activated += OnWindowActivate;
        
        return w;
    }

    private async void OnWindowDeactivating(object? sender, EventArgs e)
    {
        if (_appShell.CurrentPage is not GamePage)
        {
            try
            {
                File.Delete(Path.Combine(FileSystem.Current.AppDataDirectory,
                    SuspendedGameSavePath));
            }
            catch
            {
                // ignored
            }
            return;
        }
        try
        {
            _model.Paused = true;
            await _model.SaveGame(Path.Combine(FileSystem.Current.AppDataDirectory, SuspendedGameSavePath));
            await _appShell.OnShowMessageMessenger(new ShowMessageEventArgs("Detected Game Interruption...", "Info"));
        }
        catch
        {
            // ignored
        }
    }
    
    private async void OnWindowActivate(object? sender, EventArgs e)
    {
        if (!File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, SuspendedGameSavePath)) ||
            _appShell.CurrentPage is not MenuPage)
            return;
        try
        {
#if  ANDROID
            bool answer = await _appShell.DisplayAlert("Interrupted Game", "It looks like you have an interrupted game. Want to resume?", "Yes", "No");
            if (answer)
            {
#endif
                await _model.LoadFile(Path.Combine(FileSystem.Current.AppDataDirectory,
                    SuspendedGameSavePath));
                await _appShell.OnShowMessageMessenger(new ShowMessageEventArgs("Resuming...", "Info"));
#if ANDROID
            }
#endif
            File.Delete(Path.Combine(FileSystem.Current.AppDataDirectory,
                SuspendedGameSavePath));
        }
        catch
        {
            // ignored
        }
        
    }

    public void Dispose()
    {
        _model.Dispose();
    }
}