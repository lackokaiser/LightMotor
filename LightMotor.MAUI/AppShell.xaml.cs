using System.Text;
using CommunityToolkit.Maui.Alerts;
using LightMotor.Event;
using LightMotor.MAUI.View;
using LightMotor.Root;
using LightMotor.ViewModel;
using LightMotor.ViewModel.EventArgs;
using GameViewModel = LightMotor.ViewModel.ViewModel.GameViewModel;

namespace LightMotor.MAUI;

public partial class AppShell
{
    private Game _game;
    private const string SaveFile = "save.txt";

    public AppShell(ref Game game)
    {
        _game = game;
        InitializeComponent();
        
        ViewCallback.Get().OpenFileMessenger += OnOpenFileMessenger;
        ViewCallback.Get().SaveFileMessenger += OnSaveFileMessenger;
        ViewCallback.Get().ShowMessageMessenger += OnShowMessageMessenger;
        
        game.OnGameInitialized += OnGameInitialized;
    }
    private async void OnGameInitialized(object obj, GameInitializedEventArgs e)
    {
        var gameVm = new GameViewModel(ref _game, e);
        GamePage page = new GamePage()
        {
            BindingContext = gameVm
        };

        await Navigation.PushAsync(page);
        
        if (e.GameStatus is PlayStatus)
            await gameVm.Start();
    }
    
    private async void OnShowMessageMessenger(object obj, ShowMessageEventArgs args)
    {
        await OnShowMessageMessenger(args);
    }

    public async Task OnShowMessageMessenger(ShowMessageEventArgs args)
    {
        await Toast.Make(args.Text).Show();
    }

    private async void OnOpenFileMessenger()
    {
        if (File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, SaveFile)))
        {
            ViewCallback.Get().OpenedFile = Path.Combine(FileSystem.Current.AppDataDirectory, SaveFile);
            await OnShowMessageMessenger(new ShowMessageEventArgs("Loading Game...", "Info"));    
        }
        else
        {
            ViewCallback.Get().OpenedFile = string.Empty;
            await OnShowMessageMessenger(new ShowMessageEventArgs("No Save Available", "Info"));    
        }
        
    }
    private async void OnSaveFileMessenger()
    {
        ViewCallback.Get().OpenedFile = Path.Combine(FileSystem.Current.AppDataDirectory, SaveFile);
        await OnShowMessageMessenger(new ShowMessageEventArgs("Saving Game...", "Info"));
    }
}