using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.UI.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace SimpleSudokuSolver.UI
{
  public partial class SudokuBoard
  {
    private readonly List<CellViewModel> _cellViewModels = new List<CellViewModel>();
    private CellViewModel _activeCell;

    #region Dependency properties

    public static readonly DependencyProperty SudokuPuzzleProperty =
      DependencyProperty.Register("SudokuPuzzle", typeof(SudokuPuzzle), typeof(SudokuBoard));

    public SudokuPuzzle SudokuPuzzle
    {
      get { return (SudokuPuzzle)GetValue(SudokuPuzzleProperty); }
      set { SetValue(SudokuPuzzleProperty, value); }
    }

    public static readonly DependencyProperty SolvedSudokuPuzzleProperty =
      DependencyProperty.Register("SolvedSudokuPuzzle", typeof(SudokuPuzzle), typeof(SudokuBoard));

    public SudokuPuzzle SolvedSudokuPuzzle
    {
      get { return (SudokuPuzzle)GetValue(SolvedSudokuPuzzleProperty); }
      set { SetValue(SolvedSudokuPuzzleProperty, value); }
    }

    #endregion

    public SudokuBoard()
    {
      InitializeComponent();

      Loaded += OnLoaded;
      MouseLeftButtonDown += OnMouseLeftButtonDown;
    }

    #region Event handlers

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      Loaded -= OnLoaded;

      if (DesignerProperties.GetIsInDesignMode(this))
        return;

      var descriptor = DependencyPropertyDescriptor.FromProperty(SudokuPuzzleProperty, typeof(SudokuBoard));
      descriptor.AddValueChanged(this, new System.EventHandler((s, args) => { BindPuzzle(); }));

      var window = Window.GetWindow(this);
      window.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      // if there is no active cell or active cell already has value
      if (_activeCell == null || _activeCell.Cell.HasValue)
        return;

      // if a non-valid number was entered
      var enteredValue = GetKeyValue(e.Key);
      if (enteredValue <= 0)
        return;

      // if value is not correct or solver cannot solve the puzzle (so we do not know if the value is correct), we do not allow it
      var mainViewModel = (MainViewModel)DataContext;
      var cellIndex = SudokuPuzzle.GetCellIndex(_activeCell.Cell);
      var solvedValue = SolvedSudokuPuzzle.Cells[cellIndex.RowIndex, cellIndex.ColumnIndex].Value;

      if(solvedValue == 0)
      {
        mainViewModel.AppendMessage("Cannot enter value because solver cannot solve the puzzle");
        return;
      }
      if (enteredValue != solvedValue)
      {
        mainViewModel.AppendMessage($"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {enteredValue} [Wrong value entered by user]");
        return;
      }

      _activeCell.Value = enteredValue.ToString();
      SetSelection(_activeCell);
      mainViewModel.AppendMessage($"Row {cellIndex.RowIndex + 1} Column {cellIndex.ColumnIndex + 1} Value {enteredValue} [Entered by user]");
      mainViewModel.UpdateStatusMessage();
    }

    private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      var frameworkElement = e.OriginalSource as FrameworkElement;
      if (frameworkElement.DataContext is CellViewModel cellViewModel)
      {
        if (_activeCell != null)
          _activeCell.IsActive = false;

        _activeCell = cellViewModel;
        _activeCell.IsActive = true;

        SetSelection(cellViewModel);
      }
    }

    #endregion

    private void BindPuzzle()
    {
      _cellViewModels.Clear();
      _activeCell = null;

      SetCellViewModelsForBlock(block1, SudokuPuzzle.Blocks[0, 0]);
      SetCellViewModelsForBlock(block2, SudokuPuzzle.Blocks[0, 1]);
      SetCellViewModelsForBlock(block3, SudokuPuzzle.Blocks[0, 2]);
      SetCellViewModelsForBlock(block4, SudokuPuzzle.Blocks[1, 0]);
      SetCellViewModelsForBlock(block5, SudokuPuzzle.Blocks[1, 1]);
      SetCellViewModelsForBlock(block6, SudokuPuzzle.Blocks[1, 2]);
      SetCellViewModelsForBlock(block7, SudokuPuzzle.Blocks[2, 0]);
      SetCellViewModelsForBlock(block8, SudokuPuzzle.Blocks[2, 1]);
      SetCellViewModelsForBlock(block9, SudokuPuzzle.Blocks[2, 2]);

      var lastUpdatedCellIndex = ((MainViewModel)DataContext).LastUpdatedCellIndex;
      if(lastUpdatedCellIndex != null)
      {
        if (_activeCell != null)
          _activeCell.IsActive = false;

        var lastUpdatedCell = SudokuPuzzle.Cells[lastUpdatedCellIndex.Item1, lastUpdatedCellIndex.Item2];
        _activeCell = _cellViewModels.Single(x => x.Cell == lastUpdatedCell);
        _activeCell.IsActive = true;
        SetSelection(_activeCell);
      }
    }

    private void SetCellViewModelsForBlock(SudokuBlock sudokuBlock, Block block)
    {
      var cellsViewModels = block.Cells.OfType<Cell>().Select(x => new CellViewModel(x)).ToArray();
      _cellViewModels.AddRange(cellsViewModels);
      sudokuBlock.DataContext = cellsViewModels;
    }

    private int GetKeyValue(System.Windows.Input.Key key)
    {
      int keyValue = (int)key;
      int value = -1;
      if ((keyValue >= (int)System.Windows.Input.Key.D0) && (keyValue <= (int)System.Windows.Input.Key.D9))
      {
        value = (int)key - (int)System.Windows.Input.Key.D0;
      }
      else if (keyValue >= (int)System.Windows.Input.Key.NumPad0 && keyValue <= (int)System.Windows.Input.Key.NumPad9)
      {
        value = (int)key - (int)System.Windows.Input.Key.NumPad0;
      }

      return value;
    }

    private void SetSelection(CellViewModel selectedCellViewModel)
    {
      if (SudokuPuzzle == null)
        return;

      var selectedCellIndex = SudokuPuzzle.GetCellIndex(selectedCellViewModel.Cell);

      foreach (var cellViewModel in _cellViewModels)
      {
        var index = SudokuPuzzle.GetCellIndex(cellViewModel.Cell);
        var hasValue = cellViewModel.Cell.HasValue;
        var hasSameValue = cellViewModel.Cell.Value == selectedCellViewModel.Cell.Value;
        var isInSameRow = selectedCellIndex.RowIndex == index.RowIndex;
        var isInSameColumn = selectedCellIndex.ColumnIndex == index.ColumnIndex;

        if (isInSameRow || isInSameColumn || (hasValue && hasSameValue))
        {
          cellViewModel.IsSelected = true;
        }
        else
        {
          cellViewModel.IsSelected = false;
        }
      }
    }
  }
}
