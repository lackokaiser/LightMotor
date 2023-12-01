namespace LightMotor.MAUI.Controls;

public partial class UniformGrid
{
    public static readonly BindableProperty RowsProperty =
        BindableProperty.Create ("Rows", typeof(int), typeof(UniformGrid), 6);
    public static readonly BindableProperty ColumnsProperty =
        BindableProperty.Create ("Columns", typeof(int), typeof(UniformGrid), 6);
    
    public int Rows
    {
        get => (int)GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int Columns
    {
        get => (int)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public new RowDefinitionCollection RowDefinitions => new (Enumerable.Repeat(new RowDefinition(GridLength.Auto), Rows).ToArray());
    public new ColumnDefinitionCollection ColumnDefinitions => new (Enumerable.Repeat(new ColumnDefinition(GridLength.Auto), Columns).ToArray());

    public UniformGrid()
    {
        InitializeComponent();
    }

    
}