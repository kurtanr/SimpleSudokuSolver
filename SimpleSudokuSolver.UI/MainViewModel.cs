using SimpleSudokuSolver.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SimpleSudokuSolver.UI
{
  public class MainViewModel : INotifyPropertyChanged
  {
    public ICommand NewGameCommand { get; }
    public ICommand LoadGameCommand { get; }
    public ICommand SaveGameCommand { get; }
    public ICommand SolveGameCommand { get; }
    public ICommand SolveGameStepCommand { get; }
    public ICommand ExitGameCommand { get; }

    public event Func<SudokuPuzzle> LoadGame;
    public event Action<SudokuPuzzle> SaveGame;
    public event Action ExitGame;

    private int[,] _sudoku;
    public int[,] Sudoku
    {
      get
      {
        return _sudoku;
      }
      private set
      {
        _sudoku = value;
        SudokuForDisplay = ToSudokuForDisplay(_sudoku);
        OnPropertyChanged(nameof(Sudoku));
      }
    }

    private string[,] _sudokuForDisplay;
    public string[,] SudokuForDisplay
    {
      get
      {
        return _sudokuForDisplay;
      }
      private set
      {
        _sudokuForDisplay = value;
        OnPropertyChanged(nameof(SudokuForDisplay));
      }
    }

    private string _message;
    public string Message
    {
      get
      {
        return _message;
      }
      private set
      {
        _message = value;
        OnPropertyChanged(nameof(Message));
      }
    }

    private readonly ISudokuPuzzleProvider _puzzleProvider;
    private readonly ISudokuSolver _solver;

    public MainViewModel(
      ISudokuPuzzleProvider puzzleProvider,
      ISudokuSolver solver)
    {
      _puzzleProvider = puzzleProvider;
      _solver = solver;

      NewGameCommand = new RelayCommand(ExecuteNewGameCommand);
      LoadGameCommand = new RelayCommand(ExecuteLoadGameCommand);
      SaveGameCommand = new RelayCommand(ExecuteSaveGameCommand, () => Sudoku != null);
      SolveGameCommand = new RelayCommand(ExecuteSolveGameCommand, () => Sudoku != null);
      SolveGameStepCommand = new RelayCommand(ExecuteSolveGameStepCommand, () => Sudoku != null);
      ExitGameCommand = new RelayCommand(ExecuteExitGameCommand);
    }

    private void ExecuteNewGameCommand()
    {
      Sudoku = _puzzleProvider.GetPuzzle();
      Message = string.Empty;
    }

    private void ExecuteLoadGameCommand()
    {
      var loadGame = LoadGame;
      if (loadGame != null)
      {
        var sudokuPuzzle = loadGame();
        if (sudokuPuzzle != null)
        {
          Sudoku = sudokuPuzzle.ToIntArray();
          Message = string.Empty;
        }
      }
    }

    private void ExecuteSaveGameCommand()
    {
      SaveGame?.Invoke(new SudokuPuzzle(Sudoku));
    }

    private void ExecuteSolveGameCommand()
    {
      if (_solver.IsSolved(Sudoku))
        return;

      var puzzle = _solver.Solve(Sudoku, out SingleStepSolution[] steps);
      Sudoku = puzzle.ToIntArray();

      foreach (var step in steps)
      {
        AppendMessage(step.SolutionDescription);
      }

      if (!_solver.IsSolved(Sudoku))
        AppendMessage("Cannot solve puzzle");
    }

    private void ExecuteSolveGameStepCommand()
    {
      if (_solver.IsSolved(Sudoku))
        return;

      var singleStepSolution = _solver.SolveSingleStep(Sudoku);
      if (singleStepSolution == null)
      {
        AppendMessage("Cannot solve puzzle step");
      }
      else
      {
        Sudoku[singleStepSolution.IndexOfRow, singleStepSolution.IndexOfColumn] = singleStepSolution.Value;
        Sudoku = new SudokuPuzzle(Sudoku).ToIntArray();
        AppendMessage(singleStepSolution.SolutionDescription);
      }
    }

    private void ExecuteExitGameCommand()
    {
      ExitGame?.Invoke();
    }

    private string[,] ToSudokuForDisplay(int[,] sudoku)
    {
      int rowCount = sudoku.GetLength(0);
      int columnCount = sudoku.GetLength(0);
      string[,] result = new string[rowCount, columnCount];

      for (int i = 0; i < rowCount; i++)
      {
        for (int j = 0; j < columnCount; j++)
        {
          result[i, j] = sudoku[i, j] == 0 ? string.Empty : sudoku[i, j].ToString();
        }
      }

      return result;
    }

    private void AppendMessage(string message)
    {
      Message += $"{message}{Environment.NewLine}";
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
