using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LightMotorViewModel;

public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected bool Set<T>(ref T field, T data, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, data)) return false;
        
        PropertyChangingInvoke(propertyName);
        field = data;
        PropertyChangedInvoke(propertyName);
        return true;
    }

    protected void PropertyChangingInvoke(string propertyName)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    protected void PropertyChangedInvoke(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}