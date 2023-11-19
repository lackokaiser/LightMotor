using LightMotorViewModel.ViewModel;

namespace LightMotorViewModel;

public sealed class Navigation
{
    private static Navigation? _instance;
    
    private Navigation()
    { }

    public static Navigation Get()
    {
        _instance ??= new Navigation();

        return _instance;
    }
    
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewChanged();
        }
    }

    public event Action? CurrentViewChanged;

    private void OnCurrentViewChanged()
    {
        CurrentViewChanged?.Invoke();
    }
}