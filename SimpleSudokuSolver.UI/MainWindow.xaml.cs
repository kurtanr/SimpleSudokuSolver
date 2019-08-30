using Microsoft.Win32;
using SimpleSudokuSolver.Model;
using SimpleSudokuSolver.PuzzleProviders;
using SimpleSudokuSolver.UI.PuzzleProviders;
using SimpleSudokuSolver.UI.ViewModel;
using System.IO;
using System.Windows;

namespace SimpleSudokuSolver.UI
{
  public partial class MainWindow
  {
    private const string DialogFilter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

    public MainWindow()
    {
      InitializeComponent();
      TextBox.TextChanged += (s, e) => TextBox.ScrollToEnd();

      var puzzleProvider = new TrueMagicSudokuGeneratorPuzzleProvider();
      var solver = new DefaultSolver();
      var viewModel = new MainViewModel(puzzleProvider, solver);

      viewModel.LoadGame += OnLoadGame;
      viewModel.SaveGame += OnSaveGame;
      viewModel.ExitGame += OnExitGame;

      DataContext = viewModel;
    }

    private SudokuPuzzle OnLoadGame()
    {
      var openFileDialog = new OpenFileDialog
      {
        CheckFileExists = true,
        Filter = DialogFilter
      };
      if(openFileDialog.ShowDialog() == true)
      {
        var sudoku = new SudokuFilePuzzleProvider(openFileDialog.FileName).GetPuzzle();
        return new SudokuPuzzle(sudoku);
      }
      return null;
    }

    private void OnSaveGame(SudokuPuzzle sudokuPuzzle)
    {
      var saveFileDialog = new SaveFileDialog
      {
        Filter = DialogFilter
      };
      if (saveFileDialog.ShowDialog() == true)
      {
        File.WriteAllText(saveFileDialog.FileName, sudokuPuzzle.ToString());
      }
    }

    private void OnExitGame()
    {
      Application.Current.Shutdown(0);
    }
  }
}
