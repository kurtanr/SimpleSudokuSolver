using SimpleSudokuSolver.UI.ViewModel;
using System.Windows;

namespace SimpleSudokuSolver.UI
{
  public partial class SudokuBlock
  {
    public SudokuBlock()
    {
      InitializeComponent();
      DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var cellViewModels = e.NewValue as CellViewModel[];
      if (cellViewModels == null)
        return;

      Text1.DataContext = cellViewModels[0];
      Text2.DataContext = cellViewModels[1];
      Text3.DataContext = cellViewModels[2];
      Text4.DataContext = cellViewModels[3];
      Text5.DataContext = cellViewModels[4];
      Text6.DataContext = cellViewModels[5];
      Text7.DataContext = cellViewModels[6];
      Text8.DataContext = cellViewModels[7];
      Text9.DataContext = cellViewModels[8];
    }
  }
}
