using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFLightMotor.Controls;

public partial class NumberText : TextBox
{
    private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

    public int Min { get; set; } = 6;
    public int Max { get; set; } = 64;
    
    public NumberText()
    {
        InitializeComponent();
    }
    
    private static bool IsTextAllowed(string text)
    {
        return !_regex.IsMatch(text);
    }

    private void NumberText_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextAllowed(e.Text);

        if (e.Handled)
        {
            Text = Math.Max(Math.Min(int.Parse(e.Text), Max), Min).ToString();
        }
    }
}