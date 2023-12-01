using System.Windows.Input;

namespace LightMotor.WPF.Controls;

public partial class GameControl
{
    public GameControl()
    {
        InitializeComponent();

        Focus();

        Loaded += (_, _) => MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    }
}