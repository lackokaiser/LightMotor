using System.Globalization;

namespace LightMotor.MAUI.Converter;

public class ImagePathConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return String.Empty;
        string? str = value.ToString();
        if (string.IsNullOrEmpty(str))
            return String.Empty;
        return str.Substring(str.LastIndexOf('/')+1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return "Resources/Images/" + value;
    }
}