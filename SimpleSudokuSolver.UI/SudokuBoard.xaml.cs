using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SimpleSudokuSolver.UI
{
  public partial class SudokuBoard
  {
    private readonly List<CellViewModel> _cellViewModels = new List<CellViewModel>();
    private CellViewModel _activeCell;

    public SudokuBoard()
    {
      InitializeComponent();
      DataContextChanged += OnDataContextChanged;
      MouseLeftButtonDown += OnMouseLeftButtonDown;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var sudokuPuzzle = e.NewValue as SudokuPuzzle;
      if (sudokuPuzzle == null)
        return;

      _cellViewModels.Clear();

      SetCellViewModelsForBlock(block1, sudokuPuzzle.Blocks[0, 0]);
      SetCellViewModelsForBlock(block2, sudokuPuzzle.Blocks[0, 1]);
      SetCellViewModelsForBlock(block3, sudokuPuzzle.Blocks[0, 2]);
      SetCellViewModelsForBlock(block4, sudokuPuzzle.Blocks[1, 0]);
      SetCellViewModelsForBlock(block5, sudokuPuzzle.Blocks[1, 1]);
      SetCellViewModelsForBlock(block6, sudokuPuzzle.Blocks[1, 2]);
      SetCellViewModelsForBlock(block7, sudokuPuzzle.Blocks[2, 0]);
      SetCellViewModelsForBlock(block8, sudokuPuzzle.Blocks[2, 1]);
      SetCellViewModelsForBlock(block9, sudokuPuzzle.Blocks[2, 2]);
    }

    private void SetCellViewModelsForBlock(SudokuBlock sudokuBlock, Block block)
    {
      var cellsViewModels = block.Cells.OfType<Cell>().Select(x => new CellViewModel(x)).ToArray();
      _cellViewModels.AddRange(cellsViewModels);
      sudokuBlock.DataContext = cellsViewModels;
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

    private void SetSelection(CellViewModel selectedCellViewModel)
    {
      var sudokuPuzzle = DataContext as SudokuPuzzle;
      if (sudokuPuzzle == null)
        return;

      var selectedCellIndex = sudokuPuzzle.GetCellIndex(selectedCellViewModel.Cell);

      foreach (var cellViewModel in _cellViewModels)
      {
        var index = sudokuPuzzle.GetCellIndex(cellViewModel.Cell);
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
