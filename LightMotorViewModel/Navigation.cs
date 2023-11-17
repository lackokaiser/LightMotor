namespace LightMotorViewModel;

public class Navigation
{
    private static Navigation? instance;
    
    private Navigation(){}

    public static Navigation Get()
    {
        instance ??= new Navigation();

        return instance;
    }
    
    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewChanged();
        }
    }

    public event Action CurrentViewChanged;

    protected virtual void OnCurrentViewChanged()
    {
        CurrentViewChanged?.Invoke();
    }
}