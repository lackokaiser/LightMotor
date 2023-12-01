using LightMotor.Event;
using LightMotor.Root;

namespace LightMotor.ViewModel.ViewModel;
public class MainViewModel : ViewModelBase
{
    private Game _model;
    public ViewModelBase? CurrentViewModel => Navigation.Get().CurrentViewModel;

    public MainViewModel(ref Game game)
    {
        _model = game;
        
        game.OnGameInitialized += OnGameInitialized;
        Navigation.Get().CurrentViewChanged += OnCurrentViewChanged;
    }
    private void OnCurrentViewChanged()
    {
        PropertyChangedInvoke(nameof(CurrentViewModel));
    }
    
    /// <summary>
    /// Game must start in this function
    /// </summary>
    /// <param name="obj">The sender</param>
    /// <param name="e">The event parameter></param>
    private async void OnGameInitialized(object obj, GameInitializedEventArgs e)
    {
        var vm = new GameViewModel(ref _model, e);
        Navigation.Get().CurrentViewModel = vm;

        if(e.GameStatus is PlayStatus)
            await vm.Start();
    }
}