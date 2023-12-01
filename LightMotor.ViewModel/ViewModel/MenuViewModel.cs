using System.Windows.Input;
using LightMotor.Root;
using LightMotor.ViewModel.Command;

namespace LightMotor.ViewModel.ViewModel;

public class MenuViewModel : ViewModelBase
{
    private int _boardSize = 6;
    

    public int BoardSize
    {
        get => _boardSize;
        set => Set(ref _boardSize, value);
    }

    public ICommand LoadCommand { get; }

    public ICommand StartCommand { get; }
    
    public ICommand ExitCommand { get; }
    public ICommand HelpCommand { get; }

    public MenuViewModel(ref Game model)
    {
        LoadCommand = new LoadGameCommand(ref model);
        StartCommand = new StartGameCommand(ref model, this);
        ExitCommand = new ExitCommand();
        HelpCommand = new ShowHelpCommand();
    }

    
}