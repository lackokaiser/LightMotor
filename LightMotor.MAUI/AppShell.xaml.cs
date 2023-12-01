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

    public AppShell(ref Game game)
    {
        _game = game;
        InitializeComponent();
        
        ViewCallback.Get().OpenFileMessenger += OnOpenFileMessenger;
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
        await DisplayAlert(args.Caption, args.Text, "Ok");
    }

    private void OnOpenFileMessenger()
    {
        
    }
}