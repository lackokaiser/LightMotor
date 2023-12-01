using LightMotor.ViewModel.ViewModel;

namespace LightMotor.ViewModel.Play;

public class EntityViewModel : ViewModelBase
{
    private int _col;
    private int _row;
    private string _image;
    
    public int Row
    {
        get => _row;
        set => Set(ref _row, value);
    }

    public int Col
    {
        get => _col;
        set => Set(ref _col, value);
    }

    public string Image
    {
        get => _image;
        private set => Set(ref _image, value);
    }

    private EntityViewModel(string image, int row, int col)
    {
        _image = image;
        _row = row;
        _col = col;
    }

    public void Update(string image, int row, int col)
    {
        Image = image; 
        Row = row;
        Col = col;
    }

    public static EntityViewModel Empty(int row, int col)
    {
        return new EntityViewModel("", row, col);
    }
}