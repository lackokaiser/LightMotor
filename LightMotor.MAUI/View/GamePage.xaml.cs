namespace LightMotor.MAUI.View;

public partial class GamePage
{
    public GamePage()
    {
        InitializeComponent();
    }

    private async void ExitButton_Click(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}