using LightMotorViewModel.EventArgs;

namespace LightMotorViewModel;

public delegate void MessageBoxOpen(object obj, ShowMessageEventArgs args);

public sealed class ViewCallback
{
    private ViewCallback(){}

    private static ViewCallback? _instance;

    public static ViewCallback Get()
    {
        _instance ??= new ViewCallback();

        return _instance;
    }
    
    public event Action? OpenFileMessenger;
    public event Action? SaveFileMessenger;
    public event MessageBoxOpen? ShowMessageMessenger;
    
    public string? OpenedFile { get; set; }
    
    /// <summary>
    /// Opens a file dialog for the user, than the opened file is stored in the <see cref="OpenedFile"/> property.
    /// If no file have been opened, the string is empty
    /// </summary>
    public void OpenFile()
    {
        OpenFileMessenger?.Invoke();
    }
    
    /// <summary>
    /// Opens a file dialog for the user, than the opened file is stored in the <see cref="OpenedFile"/> property.
    /// The file should always exist on this call.
    /// If no file have been opened, the string is empty
    /// </summary>
    public void SaveFile()
    {
        SaveFileMessenger?.Invoke();
    }
    
    /// <summary>
    /// Shows a message to the user
    /// </summary>
    /// <param name="args">The arguments containing the information to show</param>
    /// <seealso cref="ShowMessageEventArgs"/>
    public void ShowMessage(ShowMessageEventArgs args)
    {
        ShowMessageMessenger?.Invoke(this, args);
    }
}