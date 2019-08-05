using SimpleSudokuSolver.Model;
using System;
using System.ComponentModel;

namespace SimpleSudokuSolver.UI.ViewModel
{
  public class CellViewModel : INotifyPropertyChanged
  {
    private bool _isSelected;
    public bool IsSelected
    {
      get { return _isSelected; }
      set
      {
        _isSelected = value;
        OnPropertyChanged(nameof(IsSelected));
      }
    }

    private bool _isActive;
    public bool IsActive
    {
      get { return _isActive; }
      set
      {
        _isActive = value;
        OnPropertyChanged(nameof(IsActive));
      }
    }

    public string Tooltip => Cell.CanBe.Count > 0 ? string.Join(",", Cell.CanBe) : null;

    public Cell Cell { get; }

    public string Value
    {
      get { return Cell.Value != 0 ? Cell.Value.ToString() : string.Empty; }
    }

    public CellViewModel(Cell cell)
    {
      Cell = cell;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void NotifyCellValueChanged()
    {
      OnPropertyChanged(nameof(Value));
    }
  }
}
