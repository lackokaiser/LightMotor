using System.Windows.Input;

namespace WPFLightMotor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Loaded += (_, _) => MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }
    }
}