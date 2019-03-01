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

      text1.DataContext = cellViewModels[0];
      text2.DataContext = cellViewModels[1];
      text3.DataContext = cellViewModels[2];
      text4.DataContext = cellViewModels[3];
      text5.DataContext = cellViewModels[4];
      text6.DataContext = cellViewModels[5];
      text7.DataContext = cellViewModels[6];
      text8.DataContext = cellViewModels[7];
      text9.DataContext = cellViewModels[8];
    }
  }
}
