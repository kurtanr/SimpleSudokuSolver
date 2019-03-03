using SimpleSudokuSolver.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

    public Tuple<int,int> LastUpdatedCellIndex { get; private set; }

    private SudokuPuzzle _solvedSudokuPuzzle;
    public SudokuPuzzle SolvedSudokuPuzzle
    {
      get
      {
        return _solvedSudokuPuzzle;
      }
      private set
      {
        _solvedSudokuPuzzle = value;
        OnPropertyChanged(nameof(SolvedSudokuPuzzle));
      }
    }

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

    private string _statusMessage;
    public string StatusMessage
    {
      get
      {
        return _statusMessage;
      }
      private set
      {
        _statusMessage = value;
        OnPropertyChanged(nameof(StatusMessage));
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

      StatusMessage = "Start new game or load and existing one";
    }

    private void ExecuteNewGameCommand()
    {
      var sudoku = _puzzleProvider.GetPuzzle();
      SudokuPuzzle = new SudokuPuzzle(sudoku);
      SolvedSudokuPuzzle = _solver.Solve(sudoku);
      LastUpdatedCellIndex = null;
      Message = string.Empty;
      UpdateStatusMessage();
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
          SolvedSudokuPuzzle = _solver.Solve(sudokuPuzzle.ToIntArray());
          LastUpdatedCellIndex = null;
          Message = string.Empty;
          UpdateStatusMessage();
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

      LastUpdatedCellIndex = null;
      UpdateStatusMessage();
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
        LastUpdatedCellIndex = new Tuple<int, int>(singleStepSolution.IndexOfRow, singleStepSolution.IndexOfColumn);
        SudokuPuzzle = new SudokuPuzzle(sudoku);
        AppendMessage(singleStepSolution.SolutionDescription);
      }

      UpdateStatusMessage();
    }

    private void ExecuteExitGameCommand()
    {
      ExitGame?.Invoke();
    }

    public void AppendMessage(string message)
    {
      Message += $"{message}{Environment.NewLine}";
    }

    public void UpdateStatusMessage()
    {
      var isSolved = _solver.IsSolved(SudokuPuzzle.ToIntArray());
      if (isSolved)
      {
        StatusMessage = "Sudoku is solved";
        return;
      }

      var sb = new StringBuilder("Values left: ");
      for(int i = 1; i <= SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; i++)
      {
        var numberOfValuesLeft = SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - SudokuPuzzle.Cells.OfType<Cell>().Where(x => x.Value == i).Count();
        var separator = (i == SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle) ? string.Empty : ",";
        if(numberOfValuesLeft > 0)
          sb.Append($"{i} x{numberOfValuesLeft}{separator} ");
      }

      StatusMessage = sb.ToString();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
