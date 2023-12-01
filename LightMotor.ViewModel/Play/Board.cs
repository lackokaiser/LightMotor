using System.Collections.ObjectModel;
using LightMotor.ViewModel.ViewModel;

namespace LightMotor.ViewModel.Play;

public class Board : ViewModelBase
{
    private int _size;
    private ObservableCollection<EntityViewModel> _entities = new ();

    public int Size
    {
        get => _size;
        set => Set(ref _size, value);
    }
    
    public ObservableCollection<EntityViewModel> Entities
    {
        get => _entities;
        set => Set(ref _entities, value);
    }

    public Board(int size)
    {
        _size = size;
    }
}