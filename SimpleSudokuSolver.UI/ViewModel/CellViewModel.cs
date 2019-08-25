using SimpleSudokuSolver.Model;
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

    public string Tooltip => ShowTooltip && Cell.CanBe.Count > 0 ? string.Join(",", Cell.CanBe) : null;

    private bool _showTooltip;
    public bool ShowTooltip
    {
      get => _showTooltip;
      set
      {
        _showTooltip = value;
        OnPropertyChanged(nameof(Tooltip));
      }
    }

    public Cell Cell { get; }

    public string Value
    {
      get { return Cell.Value != 0 ? Cell.Value.ToString() : string.Empty; }
    }

    public CellViewModel(Cell cell, bool showTooltip)
    {
      Cell = cell;
      ShowTooltip = showTooltip;
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
