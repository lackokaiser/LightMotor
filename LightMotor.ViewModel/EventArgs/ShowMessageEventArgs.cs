namespace LightMotorViewModel.EventArgs;

public class ShowMessageEventArgs
{
    public string Text { get; }
    public string Caption { get; }

    public ShowMessageEventArgs(string text, string caption)
    {
        Text = text;
        Caption = caption;
    }
}