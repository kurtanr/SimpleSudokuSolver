using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.UI.ViewModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

    public static readonly DependencyProperty ShowCandidatesProperty =
      DependencyProperty.Register("ShowCandidates", typeof(bool), typeof(SudokuBoard));

    public bool ShowCandidates
    {
      get { return (bool)GetValue(ShowCandidatesProperty); }
      set { SetValue(ShowCandidatesProperty, value); }
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

      var descriptor1 = DependencyPropertyDescriptor.FromProperty(SudokuPuzzleProperty, typeof(SudokuBoard));
      descriptor1.AddValueChanged(this, (s, args) => { BindPuzzle(); });

      var descriptor2 = DependencyPropertyDescriptor.FromProperty(ShowCandidatesProperty, typeof(SudokuBoard));
      descriptor2.AddValueChanged(this, (s, args) => { ToggleShowCandidates(); });

      var window = Window.GetWindow(this);
      if (window != null)
      {
        window.KeyDown += OnKeyDown;
      }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      // if arrow key was pressed, do navigation
      if (ProcessArrowKey(e.Key))
        return;

      // if there is no active cell or active cell already has value
      if (_activeCell == null || _activeCell.Cell.HasValue)
        return;

      // if a non-valid number was entered
      var enteredValue = GetKeyValue(e.Key);
      if (enteredValue <= 0)
        return;

      // if value is not correct or solver cannot solve the puzzle (so we do not know if the value is correct), we do not allow it
      var solvedValue = SolvedSudokuPuzzle.Cells[_activeCell.Cell.RowIndex, _activeCell.Cell.ColumnIndex].Value;

      var mainViewModel = (MainViewModel)DataContext;
      if (solvedValue == 0)
      {
        mainViewModel.AppendMessage("Cannot enter value because solver cannot solve the puzzle");
        return;
      }
      if (enteredValue != solvedValue)
      {
        mainViewModel.AppendMessage($"Row {_activeCell.Cell.RowIndex + 1} Column {_activeCell.Cell.ColumnIndex + 1} Value {enteredValue} [Wrong value entered by user]");
        return;
      }

      var solution = new SingleStepSolution(_activeCell.Cell.RowIndex, _activeCell.Cell.ColumnIndex, enteredValue, "Entered by user");
      SudokuPuzzle.ApplySingleStepSolution(solution);
      _activeCell.NotifyCellValueChanged();
      SetSelection(_activeCell);
      mainViewModel.AppendMessage(solution.SolutionDescription);
      mainViewModel.UpdateStatusMessage();
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if(e.OriginalSource is FrameworkElement frameworkElement && frameworkElement.DataContext is CellViewModel cellViewModel)
      {
        SetActiveCell(cellViewModel.Cell);

        // Fixes issue with arrow keys no longer working once user clicks inside the text box
        var window = Window.GetWindow(frameworkElement);
        if (window != null && Keyboard.FocusedElement is TextBox textBox)
        {
          DependencyObject scope = FocusManager.GetFocusScope(textBox);
          FocusManager.SetFocusedElement(scope, window);
        }
      }
    }

    #endregion

    private void BindPuzzle()
    {
      if (SudokuPuzzle == null)
        return;

      _cellViewModels.Clear();
      SetActiveCell(null);

      SetCellViewModelsForBlock(Block1, SudokuPuzzle.Blocks[0, 0]);
      SetCellViewModelsForBlock(Block2, SudokuPuzzle.Blocks[0, 1]);
      SetCellViewModelsForBlock(Block3, SudokuPuzzle.Blocks[0, 2]);
      SetCellViewModelsForBlock(Block4, SudokuPuzzle.Blocks[1, 0]);
      SetCellViewModelsForBlock(Block5, SudokuPuzzle.Blocks[1, 1]);
      SetCellViewModelsForBlock(Block6, SudokuPuzzle.Blocks[1, 2]);
      SetCellViewModelsForBlock(Block7, SudokuPuzzle.Blocks[2, 0]);
      SetCellViewModelsForBlock(Block8, SudokuPuzzle.Blocks[2, 1]);
      SetCellViewModelsForBlock(Block9, SudokuPuzzle.Blocks[2, 2]);

      var lastUpdatedCellIndex = ((MainViewModel)DataContext).LastUpdatedCellIndex;
      if (lastUpdatedCellIndex != null)
      {
        if (_activeCell != null)
          _activeCell.IsActive = false;

        var lastUpdatedCell = SudokuPuzzle.Cells[lastUpdatedCellIndex.Item1, lastUpdatedCellIndex.Item2];
        SetActiveCell(lastUpdatedCell);
      }
    }

    private void ToggleShowCandidates()
    {
      ToggleCellViewModelTooltip(Block1);
      ToggleCellViewModelTooltip(Block2);
      ToggleCellViewModelTooltip(Block3);
      ToggleCellViewModelTooltip(Block4);
      ToggleCellViewModelTooltip(Block5);
      ToggleCellViewModelTooltip(Block6);
      ToggleCellViewModelTooltip(Block7);
      ToggleCellViewModelTooltip(Block8);
      ToggleCellViewModelTooltip(Block9);
    }

    private void ToggleCellViewModelTooltip(SudokuBlock sudokuBlock)
    {
      var cellViewModels = sudokuBlock.DataContext as CellViewModel[];
      if (cellViewModels == null)
        return;

      foreach(var cellViewModel in cellViewModels)
      {
        cellViewModel.ShowTooltip = ShowCandidates;
      }
    }

    private void SetCellViewModelsForBlock(SudokuBlock sudokuBlock, Block block)
    {
      var cellsViewModels = block.Cells.OfType<Cell>().Select(x => new CellViewModel(x, ShowCandidates)).ToArray();
      _cellViewModels.AddRange(cellsViewModels);
      sudokuBlock.DataContext = cellsViewModels;
    }

    private bool ProcessArrowKey(Key key)
    {
      switch (key)
      {
        case Key.Down:
          ChangeActiveCell(1, 0);
          return true;
        case Key.Up:
          ChangeActiveCell(-1, 0);
          return true;
        case Key.Left:
          ChangeActiveCell(0, -1);
          return true;
        case Key.Right:
          ChangeActiveCell(0, 1);
          return true;
        default:
          return false;
      }
    }

    private void ChangeActiveCell(int rowOffset, int columnOffset)
    {
      if (SudokuPuzzle == null)
        return;

      var maxIndex = SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - 1;

      if (_activeCell == null)
      {
        var cell = SudokuPuzzle.Cells[rowOffset >= 0 ? 0 : maxIndex, columnOffset >= 0 ? 0 : maxIndex];
        SetActiveCell(cell);
      }
      else
      {
        var proposedRowIndex = _activeCell.Cell.RowIndex + rowOffset;
        var rowIndex = (proposedRowIndex < 0) ? maxIndex :
          proposedRowIndex % SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle;

        var proposedColumnIndex = _activeCell.Cell.ColumnIndex + columnOffset;
        var columnIndex = (proposedColumnIndex < 0) ? maxIndex :
          proposedColumnIndex % SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle;

        var cell = SudokuPuzzle.Cells[rowIndex, columnIndex];
        SetActiveCell(cell);
      }
    }

    private int GetKeyValue(Key key)
    {
      int keyValue = (int)key;
      int value = -1;
      if ((keyValue >= (int)Key.D0) && (keyValue <= (int)Key.D9))
      {
        value = (int)key - (int)Key.D0;
      }
      else if (keyValue >= (int)Key.NumPad0 && keyValue <= (int)Key.NumPad9)
      {
        value = (int)key - (int)Key.NumPad0;
      }

      return value;
    }

    private void SetActiveCell(Cell cell)
    {
      if (_activeCell != null)
      {
        _activeCell.IsActive = false;
        _activeCell = null;
      }

      if (cell != null)
      {
        _activeCell = _cellViewModels.Single(x => x.Cell == cell);
        _activeCell.IsActive = true;
        SetSelection(_activeCell);
      }
    }

    private void SetSelection(CellViewModel selectedCellViewModel)
    {
      if (SudokuPuzzle == null)
        return;

      foreach (var cellViewModel in _cellViewModels)
      {
        var hasValue = cellViewModel.Cell.HasValue;
        var hasSameValue = cellViewModel.Cell.Value == selectedCellViewModel.Cell.Value;
        var isInSameRow = selectedCellViewModel.Cell.RowIndex == cellViewModel.Cell.RowIndex;
        var isInSameColumn = selectedCellViewModel.Cell.ColumnIndex == cellViewModel.Cell.ColumnIndex;

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
