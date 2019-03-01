using SimpleSudokuSolver.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace SimpleSudokuSolver.UI.ViewModel
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

    private SudokuPuzzle _sudokuPuzzle;
    public SudokuPuzzle SudokuPuzzle
    {
      get
      {
        return _sudokuPuzzle;
      }
      private set
      {
        _sudokuPuzzle = value;
        OnPropertyChanged(nameof(SudokuPuzzle));
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
      SaveGameCommand = new RelayCommand(ExecuteSaveGameCommand, () => SudokuPuzzle != null);
      SolveGameCommand = new RelayCommand(ExecuteSolveGameCommand, () => SudokuPuzzle != null);
      SolveGameStepCommand = new RelayCommand(ExecuteSolveGameStepCommand, () => SudokuPuzzle != null);
      ExitGameCommand = new RelayCommand(ExecuteExitGameCommand);
    }

    private void ExecuteNewGameCommand()
    {
      var sudoku = _puzzleProvider.GetPuzzle();
      SudokuPuzzle = new SudokuPuzzle(sudoku);
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
          SudokuPuzzle = sudokuPuzzle;
          Message = string.Empty;
        }
      }
    }

    private void ExecuteSaveGameCommand()
    {
      SaveGame?.Invoke(SudokuPuzzle);
    }

    private void ExecuteSolveGameCommand()
    {
      if (_solver.IsSolved(SudokuPuzzle.ToIntArray()))
        return;

      var puzzle = _solver.Solve(SudokuPuzzle.ToIntArray(), out SingleStepSolution[] steps);
      SudokuPuzzle = puzzle;

      foreach (var step in steps)
      {
        AppendMessage(step.SolutionDescription);
      }

      if (!_solver.IsSolved(SudokuPuzzle.ToIntArray()))
        AppendMessage("Cannot solve puzzle");
    }

    private void ExecuteSolveGameStepCommand()
    {
      var sudoku = SudokuPuzzle.ToIntArray();

      if (_solver.IsSolved(sudoku))
        return;

      var singleStepSolution = _solver.SolveSingleStep(sudoku);
      if (singleStepSolution == null)
      {
        AppendMessage("Cannot solve puzzle step");
      }
      else
      {
        sudoku[singleStepSolution.IndexOfRow, singleStepSolution.IndexOfColumn] = singleStepSolution.Value;
        SudokuPuzzle = new SudokuPuzzle(sudoku);
        AppendMessage(singleStepSolution.SolutionDescription);
      }
    }

    private void ExecuteExitGameCommand()
    {
      ExitGame?.Invoke();
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
